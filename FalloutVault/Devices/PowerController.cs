using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class PowerController : Device, IPowerController
{
    // Fields
    private Watt _powerGeneration;
    private Watt _standardGeneration;

    // Properties
    public override DeviceId Id { get; }

    public Watt StandardGeneration => _standardGeneration;

    public Watt PowerGeneration
    {
        get => _powerGeneration;
        private set
        {
            if (!SetField(ref _powerGeneration, value)) return;

            PublishMessage(new DeviceMessage("Power generation changed", _powerGeneration));
        }
    }

    public double Efficiency
    {
        get
        {
            if (_standardGeneration.W <= 0)
                return 0;

            return Math.Clamp((double)(_powerGeneration / _standardGeneration), 0, 1);
        }
    }

    // Constructors
    public PowerController(DeviceId id, Watt standardGeneration)
    {
        Id = id;
        _standardGeneration = standardGeneration;
        _powerGeneration = standardGeneration;
    }

    // Methods
    public override void Update()
    {
        PowerGeneration = ComputePowerGeneration();
    }

    protected override Watt ComputePowerDraw()
    {
        return Watt.Zero;
        // this system doesnt consume any power, why does it need this?
    }

    private Watt ComputePowerGeneration()
    {
        // possible: calculate power consumption from different devices
        // possible: calculate power consumption based on datafile room data

        // for now hard code for demo purposes
        return _standardGeneration;
    }
}