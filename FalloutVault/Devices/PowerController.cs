using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class PowerController : Device, IPowerController
{
    // Fields
    private Watt _totalPowerDraw;
    private readonly Dictionary<DeviceId, Watt> _devicePowerDraws = new();
    private readonly Lock _drawsLock = new();

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

    public bool IsShutdown
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.PowerOnChanged(field));
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
        IsShutdown = false;
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
            triggeringDevice.SendCommand(new DeviceCommand.SetOn(false));
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
                    Shutdown();
                break;
        }
    }

    private void Shutdown()
    {
        if (IsShutdown)
            return;

        IsShutdown = true;
        PowerGeneration = ComputePowerGeneration();
    }

    private void TurnOn()
    {
        if (!IsShutdown)
            return;

        IsShutdown = false;
        PowerGeneration = ComputePowerGeneration();
    }

    private Watt ComputePowerGeneration()
    {
        if (IsShutdown)
            return Watt.Zero;

        return StandardGeneration;
    }
}