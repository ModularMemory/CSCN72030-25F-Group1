using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using System.Linq;

namespace FalloutVault.Devices;

public class PowerController : Device, IPowerController
{
    // Fields
    private Watt _totalPowerDraw;
    private Watt _powerGeneration;
    private readonly Dictionary<DeviceId, Watt> _allocations = new();
    private readonly Lock _allocationsLock = new();
    private readonly Dictionary<DeviceId, Watt> _devicePowerDraws = new();
    private readonly Lock _drawsLock = new();

    // Properties
    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.PowerController;

    public Watt StandardGeneration { get; } 

    public Watt PowerGeneration
    {
        get => _powerGeneration;
        private set
        {
            if (!SetField(ref _powerGeneration, value)) return;

            PublishMessage(new DeviceMessage.PowerGenerationChanged(_powerGeneration));
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
            // available = generation - current draw
            return new Watt(Math.Max(0, PowerGeneration.W - _totalPowerDraw.W));
        }
    }

    public bool RequestPower(DeviceId deviceId, Watt amount)
    {
        lock (_allocationsLock)
        {
            var currentAllocated = _allocations.Values.Sum(x => x.W);
            if (currentAllocated + amount.W <= PowerGeneration.W)
            {
                _allocations[deviceId] = amount;
                return true;
            }

            return false;
        }
    }

    public void ReleasePower(DeviceId deviceId)
    {
        lock (_allocationsLock)
        {
            _allocations.Remove(deviceId);
        }
    }

    // Constructors
    public PowerController(DeviceId id, Watt standardGeneration)
    {
        Id = id;
        StandardGeneration = standardGeneration;
        PowerGeneration = standardGeneration;
        _totalPowerDraw = Watt.Zero;
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

        // if usage exceeds generation, forcibly shut down the device that just reported
        if (_totalPowerDraw.W > PowerGeneration.W && sender is IDevice triggeringDevice)
        {
            try
            {
                triggeringDevice.SendCommand(new DeviceCommand.SetOn(false));
            }
            catch
            {
                //not much else i can do
            }

            ReleasePower(triggeringDevice.Id);
        }
    }

    public override void Update()
    {
        PowerGeneration = ComputePowerGeneration();
    }

    public override void SendCommand(DeviceCommand command)
    {
        // todo commands: maybe shut off all power generation or adjust the max power amount
        throw new NotImplementedException();
    }

    private Watt ComputePowerGeneration()
    {
        return StandardGeneration;
    }
}