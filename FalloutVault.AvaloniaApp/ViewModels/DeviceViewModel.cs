using FalloutVault.Commands;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.AvaloniaApp.ViewModels;

public abstract class DeviceViewModel : ViewModelBase, IDeviceViewModel
{
    private readonly IDeviceController _deviceController;
    private readonly IEventBus<DeviceMessage> _messageBus;

    protected DeviceViewModel(IDeviceController deviceController, IEventBus<DeviceMessage> messageBus)
    {
        _deviceController = deviceController;
        _messageBus = messageBus;
        _messageBus.Handler += OnDeviceMessage;
    }

    public DeviceId Id
    {
        get;
        set
        {
            if (field != default) throw new InvalidOperationException();

            field = value;
        }
    }

    public void ForceUpdateCurrentState()
    {
        _deviceController.SendCommand(Id, new DeviceCommand.GetCurrentState());
    }

    public DeviceType Type
    {
        get;
        set
        {
            if (field != DeviceType.Unknown) throw new InvalidOperationException();

            field = value;
        }
    }

    protected abstract void OnDeviceMessage(object? sender, DeviceMessage message);

    ~DeviceViewModel()
    {
        _messageBus.Handler -= OnDeviceMessage;
    }
}