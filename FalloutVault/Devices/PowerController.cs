using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class PowerController : Device, IPowerController
{
    // Fields
    private Watt _totalPowerDraw;

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

    public double Efficiency
    {
        get
        {
            if (StandardGeneration.W <= 0)
                return 0;

            return Math.Clamp((double)(PowerGeneration / StandardGeneration), 0, 1);
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
        _totalPowerDraw = totalPowerDraw;

        PublishMessage(new DeviceMessage.TotalPowerDrawChanged(
            new { TotalDraw = totalPowerDraw, Available = PowerGeneration - totalPowerDraw }
        ));
    }

    public override void Update()
    {
        PowerGeneration = ComputePowerGeneration();
    }

    public override void SendCommand(DeviceCommand command)
    {
        throw new NotImplementedException();
    }

    private Watt ComputePowerGeneration()
    {
        // possible: calculate power consumption from different devices
        // possible: calculate power consumption based on datafile room data

        // for now hard code for demo purposes
        return StandardGeneration;
    }
}