using FalloutVault.Devices.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class LogViewModel : ViewModelBase
{
    public DeviceId Sender { get; }
    public string Message { get; }
    public string? DataString { get; }

    public LogViewModel(DeviceId sender, string message, string? dataString)
    {
        Sender = sender;
        Message = message;
        DataString = dataString;
    }
}