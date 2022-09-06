namespace ASPNET6.ScriptMarker.ViewModels
{
  public class MarkPdfViewModel
  {
    public string Name { get; set; } = default!;
    public IFormFile? PostedFile { get; set; }
  }
}
