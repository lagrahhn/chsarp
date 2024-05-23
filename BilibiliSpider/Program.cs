using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace BilibiliSpider;

class Program
{
    static async Task Main(string[] args)
    {
        // string example = "BV1xi421X72x";
        // string example = "BV1kJ4m1n79d";
        // await 根据BV号下载视频封面(example);

        // await 根据关键词批量下载视频封面("春日影");
    }

    /// <summary>
    /// 如果有视频链接号，则优先按照视频链接
    /// </summary>
    /// <param name="BV">视频的BV号</param>
    /// <param name="URL">视频的链接</param>
    public static async Task 根据BV号下载视频封面(string URL, string BV = null)
    {
        // NOTE:后面跟着AV号或者BV号
        string url = "https://www.bilibili.com/video/";

        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // 无头模式
            Channel = "chrome",
        });

        var page = await browser.NewPageAsync();
        if (URL != null) await page.GotoAsync(URL);
        else await page.GotoAsync(url + BV);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var imageElement =
            await page.QuerySelectorAsync(
                ".video-capture-img.b-img>picture.b-img__inner>source[type='image/avif']");
        string imageUrl = await imageElement.GetAttributeAsync("srcset");
        string savePath = "F:/项目/C#项目/控制台应用/csharp/BilibiliSpider/images/2.avif";

        // NOTE:以下两个方法都可以

        // NOTE:使用WebClient下载文件
        // WebClient client = new WebClient();
        // client.DownloadFile("https:" + imageUrl, savePath);
        // client.Dispose();
        // NOTE:使用HttpClient下载文件
        HttpClient client = new HttpClient();
        byte[] imageBytes = await client.GetByteArrayAsync("https:" + imageUrl);
        File.WriteAllBytes(savePath, imageBytes);

        if (File.Exists(savePath))
            Console.WriteLine("下载完成");
        else
            Console.WriteLine("error");
    }


    public static async Task 根据关键词批量下载视频封面(string keyword)
    {
        // NOTE:后面跟着AV号或者BV号
        string url = "https://www.bilibili.com/video/";
        // NOTE:后面跟着搜索关键词
        string searchUrl = "https://search.bilibili.com/all?keyword=";
        var playwright = await Playwright.CreateAsync();
        var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = false, // 无头模式
            Channel = "chrome",
        });
        var page = await browser.NewPageAsync();
        page.GotoAsync(searchUrl + keyword);
        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var listElement = await page.QuerySelectorAllAsync(".video-list.row>div>div>div:nth-child(2)>a");
        List<string> lists = new List<string>();
        foreach (var element in listElement)
        {
            string temp = await element.GetAttributeAsync("href");
            lists.Add(temp.Replace("//", ""));
        }

        int count = 3;
        foreach (var i in lists)
        {
            await page.GotoAsync("https://" + i);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            var imageElement =
                await page.QuerySelectorAsync(
                    ".video-capture-img.b-img>picture.b-img__inner>source[type='image/avif']");
            string imageUrl = await imageElement.GetAttributeAsync("srcset");
            string savePath = $"F:/项目/C#项目/控制台应用/csharp/BilibiliSpider/images/{count++}.avif";
            // NOTE:以下两个方法都可以
            // NOTE:使用WebClient下载文件
            WebClient client = new WebClient();
            client.DownloadFile("https:" + imageUrl, savePath);
            client.Dispose();
            if (File.Exists(savePath)) Console.WriteLine(savePath + "保存成功");
        }
    }
}