using System.Media;
using System.Windows;
using System.Windows.Input;

namespace WindowPet;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MouseDown += delegate
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                DragMove();
        };
    }

    private void CloseWindow_OnClick(object sender, RoutedEventArgs e)
    {
        this.Close();
    }

    private void Audio_OnClick(object sender, RoutedEventArgs e)
    {
        SoundPlayer player = new SoundPlayer();
        player.SoundLocation = "F:\\项目\\C#项目\\控制台应用\\chsarp\\WindowPet\\assests\\001_ずんだもん（ノーマル）_任務完了です ma….wav";
        player.Play();
    }
}