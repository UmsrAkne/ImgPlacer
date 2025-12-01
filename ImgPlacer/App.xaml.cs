using System.Windows;
using ImgPlacer.Utils;
using ImgPlacer.Views;
using Prism.Ioc;

namespace ImgPlacer;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public static AppSettings Settings { get; private set; } = null!;

    protected override Window CreateShell()
    {
        return Container.Resolve<MainWindow>();
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
        // アプリ起動時に設定ファイルがあれば読み込み、なければデフォルト
        Settings = AppSettings.LoadOrDefault();

        // DI からも参照できるようにシングルトン登録
        containerRegistry.RegisterInstance(Settings);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        try
        {
            // アプリ終了時に設定を保存
            Settings?.Save();
        }
        catch
        {
            // 失敗してもアプリ終了は続行
        }

        base.OnExit(e);
    }
}