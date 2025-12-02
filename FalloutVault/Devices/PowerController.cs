using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using System.Linq;
using FalloutVault.Interfaces;

namespace FalloutVault.Devices;

public class PowerController : Device, IPowerController
{
    // Fields
    private Watt _totalPowerDraw;
    private readonly Dictionary<DeviceId, Watt> _devicePowerDraws = new();
    private readonly Lock _drawsLock = new();
    private readonly IDeviceController? _deviceController;

    // Properties
    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.PowerController;
    public Watt StandardGeneration { get; }

    public Watt PowerGeneration
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;
            PublishMessage(new DeviceMessage.PowerGenerationChanged(field));
        }
    }

    public bool IsOn
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;
            PublishMessage(new DeviceMessage.PowerOnOffChanged(field));
        }
    }

    public double Efficiency
    {
        get
        {
            if (StandardGeneration.W <= 0)
                return 0;
            return Math.Clamp((double)(PowerGeneration / StandardGeneration), 0, 1);
        }
    }

    public Watt AvailablePower
    {
        get
        {
            return new Watt(Math.Max(0, PowerGeneration.W - _totalPowerDraw.W));
        }
    }

    // Constructors
    public PowerController(DeviceId id, Watt standardGeneration)
    {
        Id = id;
        StandardGeneration = standardGeneration;
        PowerGeneration = standardGeneration;
        _totalPowerDraw = Watt.Zero;
        IsOn = true;
        _deviceController = _deviceController;
    }

    // Methods
    public override void SetEventBus(IEventBus<Watt> eventBus)
    {
        eventBus.Handler += PowerMessageReceived;
        base.SetEventBus(eventBus);
    }

    private void PowerMessageReceived(object? sender, Watt totalPowerDraw)
    {
        if (sender is not IDevice device) return;

        lock(_drawsLock)
        {
            _devicePowerDraws[device.Id] = totalPowerDraw;
            _totalPowerDraw = new Watt(_devicePowerDraws.Values.Sum(x => x.W));
        }

        PublishMessage(new DeviceMessage.TotalPowerDrawChanged(
           new PowerDraw(_totalPowerDraw, AvailablePower)
        ));

        if (_totalPowerDraw.W > PowerGeneration.W && sender is IDevice triggeringDevice)
        {
            try
            {
                triggeringDevice.SendCommand(new DeviceCommand.SetOn(false));
            }
            catch
            {
            }
        }
    }

    public override void Update()
    {
        PowerGeneration = ComputePowerGeneration();
    }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case DeviceCommand.SetOn setOn:
                if (setOn.IsOn)
                    TurnOn();
                else
                    TurnOff();
                break;
        }
    }

    private void TurnOff()
    {
        IsOn = false;
        PowerGeneration = Watt.Zero;

        //still have to decide how to turn off all devices
        _deviceController?.SendBroadcastCommand(new DeviceCommand.SetOn(false));
    }

    private void TurnOn()
    {
        IsOn = true;
        PowerGeneration = StandardGeneration;
    }

    private Watt ComputePowerGeneration()
    {
        if (!IsOn)
            return Watt.Zero;

        return StandardGeneration;
    }
}