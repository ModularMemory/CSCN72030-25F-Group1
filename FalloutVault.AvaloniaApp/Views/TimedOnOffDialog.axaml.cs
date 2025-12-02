using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FalloutVault.AvaloniaApp.Views;

public partial class TimedOnOffDialog : Window
{
    public TimedOnOffDialog()
    {
        InitializeComponent();
    }

    private void AcceptButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close(DataContext);
    }

    private void CancelButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}