using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace PDF_Parser;

class Program
{
    static void Main(string[] args)
    {
    }


    /// <summary>
    /// 添加字体样式
    /// </summary>
    /// <param name="path">创建的文件路径名</param>
    public static void AddFOntStyleExample(string path)
    {
        PdfWriter writer = new PdfWriter(path);
        Document document = new Document(new PdfDocument(writer));

        // StandardFonts 字体enum
        // 字体样式的enum
        // FontStyles.BOLD
        PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        // NOTE:可复用式样式
        Style style1 = new Style().SetFont(font).SetFontSize(14).SetFontColor(ColorConstants.RED)
            .SetBackgroundColor(ColorConstants.LIGHT_GRAY);

        Paragraph paragraph = new Paragraph().Add("In this example, named")
            .Add(new Text("HelloWorldStyles").AddStyle(style1))
            .Add(", we experiment with some text in ").Add(new Text("faf").AddStyle(style1));

        document.Add(paragraph);

        document.Close();
    }

    // 创建一个PDF文件
    public static void CreatePdfDocument(string path)
    {
        PdfWriter writer = new PdfWriter(path);
        Document document = new Document(new PdfDocument(writer));
        document.Close();
    }

    // 打开PDF并返回新的PDF对象，在sourcePath的文件的基础上，控制outputPath
    public static PdfDocument OpenPdfDocument(string sourcePath)
    {
        PdfReader reader = new PdfReader(sourcePath);
        string temppath = null;
        string replacement = sourcePath.Split("\\")[sourcePath.Split("\\").Length - 1].Split(".")[0];
        temppath = sourcePath.Replace(replacement, "temp");
        CreatePdfDocument(temppath);
        PdfWriter writer = new PdfWriter(temppath);
        return new PdfDocument(reader, writer);
    }

    public static void SwapPDF(string path)
    {
        File.Delete(path);
        string replacement = path.Split("\\")[path.Split("\\").Length - 1].Split(".")[0];
        File.Move(path.Replace(replacement, "temp"), path);
    }

    /// <summary>
    /// 从已有的PDF文件上添加新的内容
    /// <remarks>
    /// 先复制一个目标文件的副本，叫temp.pdf，然后在这个的基础上添加新的内容，保存。
    /// 将原有的PDF删除，然后将temp.pdf修改为原先文件的名字。
    /// </remarks>
    /// </summary>
    public static void TestCase1()
    {
        PdfDocument pdfDocument = OpenPdfDocument("F:\\项目\\C#项目\\控制台应用\\PDF_Parser\\PDF_Parser\\test\\test2.pdf");
        Document document = new Document(pdfDocument);
        Paragraph paragraph = new Paragraph("22222");
        document.Add(paragraph);
        pdfDocument.AddNewPage(); // 在末尾添加一页
        pdfDocument.RemovePage(4); // 删除指定页，从1开始
        document.Close();

        SwapPDF("F:\\项目\\C#项目\\控制台应用\\PDF_Parser\\PDF_Parser\\test\\test2.pdf");
    }

    /// <summary>
    /// 添加水印
    /// </summary>
    /// <seealso cref="WaterMarkingEventHandler"/>
    /// <example>
    /// WaterMark("F:\\项目\\C#项目\\控制台应用\\PDF_Parser\\PDF_Parser\\test\\watermark.pdf");
    /// </example>
    /// <param name="sourcePath"></param>
    public static void WaterMark(string sourcePath)
    {
        PdfWriter writer = new PdfWriter(sourcePath);
        PdfDocument pdfDocument = new PdfDocument(writer);
        Document document = new Document(pdfDocument);

        document.Add(new Paragraph("dfaf").Add(new Text("faf").SetFontSize(26).SetFontColor(ColorConstants.RED)));

        pdfDocument.AddEventHandler(PdfDocumentEvent.END_PAGE, new WaterMarkingEventHandler());

        document.Close();
    }


    /// <summary>
    /// 水印处理时间
    /// <seealso cref="Program.WaterMark"/>
    /// </summary>
    private class WaterMarkingEventHandler : IEventHandler
    {
        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent pdfDocumentEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDocument = pdfDocumentEvent.GetDocument();
            PdfPage page = pdfDocumentEvent.GetPage();
            PdfFont font = null;

            try
            {
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e.Message);
            }

            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDocument);
            new Canvas(canvas, page.GetPageSize()).SetFontColor(ColorConstants.LIGHT_GRAY).SetFontSize(60)
                .SetFont(font).ShowTextAligned(new Paragraph("WATERMARK"), 298, 421, pdfDocument.GetPageNumber(page),
                    TextAlignment.CENTER, VerticalAlignment.MIDDLE, 45).Close();
        }
    }


    public static void Test()
    {
        PdfWriter writer = new PdfWriter("F:\\项目\\C#项目\\控制台应用\\PDF_Parser\\PDF_Parser\\test\\test.pdf");
        PdfDocument pdfDocument = new PdfDocument(writer);
        Document document = new Document(pdfDocument);
        Paragraph header = new Paragraph("HEADER CONTENT").SetTextAlignment(TextAlignment.CENTER).SetFontSize(20);
        // NOTE:分割线
        LineSeparator ls = new LineSeparator(new SolidLine());
        // NOTE:图像
        Image img = new Image(ImageDataFactory
                .Create(@"F:\项目\C#项目\控制台应用\PDF_Parser\PDF_Parser\images\0.jpg"))
            .SetTextAlignment(TextAlignment.CENTER);

        document.Add(header);
        document.Add(ls);
        document.Add(img);

        // NOTE:表格
        Table table = new Table(2, false);
        Cell cell11 = new Cell(1, 1)
            .SetBackgroundColor(ColorConstants.GRAY)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("State"));
        Cell cell12 = new Cell(1, 1)
            .SetBackgroundColor(ColorConstants.GRAY)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("Capital"));

        Cell cell21 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("New York"));
        Cell cell22 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("Albany"));

        Cell cell31 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("New Jersey"));
        Cell cell32 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("Trenton"));

        Cell cell41 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("California"));
        Cell cell42 = new Cell(1, 1)
            .SetTextAlignment(TextAlignment.CENTER)
            .Add(new Paragraph("Sacramento"));

        table.AddCell(cell11);
        table.AddCell(cell12);
        table.AddCell(cell21);
        table.AddCell(cell22);
        table.AddCell(cell31);
        table.AddCell(cell32);
        table.AddCell(cell41);
        table.AddCell(cell42);
        document.Add(table);

        //NOTE:超链接
        Link link = new Link("click here",
            PdfAction.CreateURI("https://www.google.com"));
        Paragraph hyperLink = new Paragraph("Please ")
            .Add(link.SetBold().SetUnderline()
                .SetItalic().SetFontColor(ColorConstants.BLUE))
            .Add(" to go www.google.com.");

        document.Add(hyperLink);

        // NOTE:右上角添加页码
        int n = pdfDocument.GetNumberOfPages();
        for (int i = 1; i <= n; i++)
        {
            document.ShowTextAligned(new Paragraph(String
                    .Format("page" + i + " of " + n)),
                559, 806, i, TextAlignment.RIGHT,
                VerticalAlignment.TOP, 0);
        }

        // NOTE:添加中文字体支持
        PdfFont sysFont =
            PdfFontFactory.CreateFont("C:/Windows/Fonts/simsun.ttc,1", iText.IO.Font.PdfEncodings.IDENTITY_H);

        Paragraph paragraph = new Paragraph("Hello world!你好").SetFontSize(12).SetFont(sysFont);
        Text text = new Text("Hell,我是谁?").SetFontSize(15);
        paragraph.Add(text);
        document.Add(paragraph);

        document.Close();
    }
}