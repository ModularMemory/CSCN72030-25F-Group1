using FalloutVault.Devices.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class LogViewModel : ViewModelBase
{
    public DeviceId Sender { get; }
    public string Message { get; }
    public string? LogValue { get; }

    public LogViewModel(DeviceId sender, string message, string? logValue)
    {
        Sender = sender;
        Message = message;
        LogValue = logValue;
    }
}