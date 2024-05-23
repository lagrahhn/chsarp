using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PaddleOCRSharp;
using PlayWrightLibrary;

namespace ExportPDF;

class Program
{
    static async Task Main(string[] args)
    {
        // await Export();
        await GenerateMarkdown();
    }

    public static async Task GenerateMarkdown()
    {
        // NOTE:首先将所有的图片复制到obsidian中，然后观察图片的序号，在下面填写好开始和结束，生成即可。然后可以从obsidian导出pdf。
        for (int i = 1; i < 200; i++)
        {
            File.AppendAllText("F:\\文档\\Obsidian笔记\\稿子\\未命名.md", $"![[{i}.jpg]]");
        }
    }


    public static async Task OCR(string path)
    {
        Bitmap bitmap = new Bitmap(path);

        OCRModelConfig config = null;
        OCRParameter parameter = new StructureParameter();
        OCRResult result = new OCRResult();
        PaddleOCREngine engine = new PaddleOCREngine(config, parameter);
        result = engine.DetectText(bitmap);
        Console.WriteLine(result);
    }

    public static async Task Export()
    {
        bool isNextPage = true;


        IPage page =
            await ConnectCurrentChrome.GetCurrentPage("600");
        int a = 0;
        do
        {
            var articles = await page.QuerySelectorAllAsync("canvas");
            ElementHandleScreenshotOptions options = new ElementHandleScreenshotOptions();
            foreach (var article in articles)
            {
                options.Path = $"F:\\项目\\C#项目\\控制台应用\\ExportPDF\\english\\{++a}.jpg";
                await article.ScreenshotAsync(options);
            }

            var nextpageButton = await page.QuerySelectorAsync("button.readerFooter_button");
            if (nextpageButton != null)
            {
                await nextpageButton.ClickAsync();
                await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
                // NOTE:连续15次鼠标下滑事件，以加载下一页界面
                for (int i = 0; i < 15; i++)
                {
                    await page.Mouse.WheelAsync(0, 200);
                    Thread.Sleep(350);
                    await Task.Delay(2000); // NOTE:等待页面完全加载
                }

                await Task.Delay(10000); // NOTE:等待页面加载完毕
            }
            else isNextPage = false;
        } while (isNextPage);
    }
}