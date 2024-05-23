using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AcrylicWindow;

/// <summary>
/// ToolWindow.xaml 的交互逻辑
/// </summary>
public partial class ToolWindow : Window
{
    public ToolWindow()
    {
        InitializeComponent();
        Loaded += ToolWindow_Loaded;
        MouseLeftButtonDown += ToolWindow_MouseLeftButtonDown;
    }

    private void ToolWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            this.DragMove();
    }

    public WindowBlur.WindowAccentCompositor wac = null;

    private void ToolWindow_Loaded(object sender, RoutedEventArgs e)
    {
        wac = new(this, true);
        wac.Color = Color.FromArgb(180, 0, 0, 0);
        wac.IsEnabled = true;
    }
}