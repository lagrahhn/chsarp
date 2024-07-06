using System.Collections.ObjectModel;
using System.Windows;

namespace EmbeddedEverythingWPF;

public partial class MainWindow : Window
{
    // 最大搜索限制
    // const int MAX_RESULTS = 1000;
    public ObservableCollection<Result> SearchResults { get; set; }


    public MainWindow()
    {
        InitializeComponent();
        SearchResults = new ObservableCollection<Result>();
        DataContext = this; // 设置数据上下文

        var qry = "everything";
        var results = Everything.Search(qry);

        // var resultCount = 0;
        foreach (var result in results)
        {
            // resultCount++;
            // if (resultCount > MAX_RESULTS)
            // break;
            var searchResult = new Result
            {
                Filename = result.Filename,
                Size = result.Size,
                Path = result.Path,
                DateModified = result.DateModified
            };
            SearchResults.Add(searchResult); // 添加结果到集合
        }
    }
}