using FalloutVault.AvaloniaApp.Models;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public class LogViewModel : ViewModelBase
{
    public DeviceId Sender { get; }
    public string Message { get; }
    public string? DataString { get; }
    public bool IsAlert { get; }

    public LogViewModel(DeviceLog log)
    {
        Sender = log.Sender;
        Message = log.Message.Message;
        DataString = log.DataString;
        IsAlert = log.Message is DeviceMessage.Alert;
    }
}