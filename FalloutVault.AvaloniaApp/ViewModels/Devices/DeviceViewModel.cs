using FalloutVault.Commands;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Interfaces;
using FalloutVault.Models;
using Serilog;

namespace FalloutVault.AvaloniaApp.ViewModels.Devices;

public abstract class DeviceViewModel : ViewModelBase, IDeviceViewModel
{
    protected readonly IDeviceController DeviceController;
    protected readonly ILogger Logger;
    private readonly IEventBus<DeviceMessage> _messageBus;

    protected DeviceViewModel(IDeviceController deviceController, IEventBus<DeviceMessage> messageBus, ILogger logger)
    {
        DeviceController = deviceController;
        _messageBus = messageBus;
        Logger = logger;
        _messageBus.Handler += OnDeviceMessage;
    }

    public DeviceId Id
    {
        get;
        set
        {
            if (field != default) throw new InvalidOperationException($"{nameof(Id)} can only be set once.");

            field = value;
        }
    }

    public DeviceType Type
    {
        get;
        set
        {
            if (field != DeviceType.Unknown) throw new InvalidOperationException($"{nameof(Type)} can only be set once.");

            field = value;
        }
    }

    public void ForceUpdateCurrentState()
    {
        DeviceController.SendCommand(Id, new DeviceCommand.GetCurrentState());
    }

    protected abstract void OnDeviceMessage(object? sender, DeviceMessage message);

    ~DeviceViewModel()
    {
        _messageBus.Handler -= OnDeviceMessage;
    }
}