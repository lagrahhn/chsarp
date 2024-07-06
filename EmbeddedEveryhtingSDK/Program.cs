namespace EmbeddedEveryhtingSDK;

class Program
{
    /// <summary>
    /// 此 SDK 是一个基础的 IPC 包装器。
    /// 需要Everything正在运行。
    /// ANSI/Unicode 支持。
    /// 安全线程。
    /// 支持阻挡和非阻挡模式。
    /// x86 和 x64 支持。
    /// </summary>
    const int MAX_RESULTS = 20;

    static void Main(string[] args)
    {
        var qry = "everything";
        var results = Everything.Search(qry);

        var resultCount = 0;
        foreach (var result in results)
        {
            resultCount++;
            if (resultCount > MAX_RESULTS)
                break;

            Console.WriteLine($"文件名称{result.Filename}");
            Console.WriteLine($"文件大小{result.Size}");
            Console.WriteLine($"文件路径{result.Path}");
            Console.WriteLine($"修改时间{result.DateModified}");
        }
    }
}