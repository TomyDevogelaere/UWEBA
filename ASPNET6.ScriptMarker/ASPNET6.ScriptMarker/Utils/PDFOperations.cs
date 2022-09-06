using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Extgstate;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace ASPNET6.ScriptMarker.Utils
{
  public static class PDFOperations
  {
    public static Stream WriteWatermarkOnPdf(this Stream output, string text, Stream input)
    {
      using (var pdfDoc = new PdfDocument(new PdfReader(input), new PdfWriter(output)))
      {
        var document = new Document(pdfDoc);
        Rectangle pageSize;
        PdfCanvas canvas;
        int n = pdfDoc.GetNumberOfPages();
        for (int i = 1; i <= n; i++)
        {
          PdfPage page = pdfDoc.GetPage(i);
          pageSize = page.GetPageSize();
          canvas = new PdfCanvas(page);
          //Draw header text
          Paragraph p = new Paragraph(text).SetFontSize(60);
          canvas.SaveState();
          PdfExtGState gs1 = new PdfExtGState().SetFillOpacity(0.2f);
          canvas.SetExtGState(gs1);
          document.ShowTextAligned(p, pageSize.GetWidth() / 2, pageSize.GetHeight() / 2, pdfDoc.GetPageNumber(page), TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45);
          canvas.RestoreState();
        }
      }
      return output;
    }
  }
}
