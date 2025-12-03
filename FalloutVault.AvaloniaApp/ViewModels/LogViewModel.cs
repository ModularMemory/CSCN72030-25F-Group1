using FalloutVault.Devices.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class LogViewModel : ViewModelBase
{
    public DeviceId Sender { get; }
    public string Message { get; }

    public LogViewModel(DeviceId sender, string message)
    {
        Sender = sender;
        Message = message;
    }
}