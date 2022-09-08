using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders;
using System.Net;

namespace ScriptMarker.Utils
{
  public class FileBase64ContentProvider
  {
    private const string VersionKey = "v";
    private static readonly char[] QueryStringAndFragmentTokens = new[] { '?', '#' };
    private readonly IFileProvider fileProvider;
    private readonly IMemoryCache cache;
    private readonly PathString requestPathBase;

    public FileBase64ContentProvider(
        IFileProvider fileProvider,
        IMemoryCache cache,
        PathString requestPathBase)
    {
      this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
      this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
      this.requestPathBase = requestPathBase;
    }

    public string RetrieveBase64Data(string path)
    {
      string? resolvedPath = path ?? throw new ArgumentNullException(nameof(path));
      int queryStringOrFragmentStartIndex = path.IndexOfAny(QueryStringAndFragmentTokens);
      if (queryStringOrFragmentStartIndex != -1)
      {
        resolvedPath = path.Substring(0, queryStringOrFragmentStartIndex);
      }
      if (!this.cache.TryGetValue(path, out string value))
      {
        var cacheEntryOptions = new MemoryCacheEntryOptions();
        cacheEntryOptions.AddExpirationToken(this.fileProvider.Watch(resolvedPath));
        IFileInfo? fileInfo = this.fileProvider.GetFileInfo(resolvedPath);
        if (!fileInfo.Exists &&
            this.requestPathBase.HasValue &&
            resolvedPath.StartsWith(this.requestPathBase.Value, StringComparison.OrdinalIgnoreCase))
        {
          string? requestPathBaseRelativePath = resolvedPath.Substring(this.requestPathBase.Value.Length);
          cacheEntryOptions.AddExpirationToken(this.fileProvider.Watch(requestPathBaseRelativePath));
          fileInfo = this.fileProvider.GetFileInfo(requestPathBaseRelativePath);
        }
        if (fileInfo.Exists)
        {
          value = string.Format("{0}{1}", RetrieveBase64Prefix(fileInfo.Name), GetContentsForFile(fileInfo));
        }
        else
        {
          // if the file is not in the current server. try to get from internet
          byte[]? bytes = TryRetrieveFileFromInternet(path, out bool existsFileOnInternet);
          if (existsFileOnInternet && bytes is not null)
          {
            string? image = Convert.ToBase64String(bytes);
            value = string.Format("{0}{1}", RetrieveBase64Prefix(path), image);
          }
          else
          {
            value = path;
          }
        }
        value = this.cache.Set(path, value, cacheEntryOptions);
      }
      return value;
    }

    private static byte[]? TryRetrieveFileFromInternet(string path, out bool existsFileOnInternet)
    {
      existsFileOnInternet = false;
      try
      {
        using (var webClient = new WebClient())
        {
          byte[] data = webClient.DownloadData(path);
          existsFileOnInternet = true;
          return data;
        }
      }
      // the path is not a valid url
      catch (ArgumentException)
      {
        return null;
      }
      // the path is not a valid file on internet
      catch (WebException)
      {
        return null;
      }
    }

    private static string GetContentsForFile(IFileInfo fileInfo)
    {
      long numberBytes = fileInfo.Length;
      int iNumberBytes = (int)numberBytes;
      byte[]? contentFile = new byte[numberBytes];
      using (Stream? readStream = fileInfo.CreateReadStream())
      {
        int bytes = readStream.Read(contentFile, 0, (int)numberBytes);
        return Convert.ToBase64String(contentFile);
      }
    }

    private static string RetrieveBase64Prefix(string fileName)
    {
      string extension = Path.GetExtension(fileName);
      if (!extension.StartsWith("."))
      {
        throw new NotSupportedException(fileName);
      }
      extension = extension.Substring(1).ToLowerInvariant();//remove .
      return string.Format("data:image/{0};base64,", extension);
    }
  }
}
