using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class CropSprinklerController : PoweredDevice, ICropSprinklerController
{
    private bool _IsOn;

    private int _TargetSection;

    private int _TargetLitres;

    private TimeSpan _TimeSpanOn;


    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.CropSprinklerController;

    public bool IsOn
    {
        get => _IsOn;
        set => _IsOn = value;
    }
    public int TargetSection
    {
        get => _TargetSection;
        set => _TargetSection = value;
    }

    public int TargetLitres
    {
        get => _TargetLitres;
        set => _TargetLitres = value;
    }

    public TimeSpan TimeSpanOn
    {
        get => _TimeSpanOn;

        set => _TimeSpanOn = value;
    }


    public CropSprinklerController(DeviceId id)
    {
        Id = id;
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;
            
        return (Watt)50;
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public override void SendCommand(DeviceCommand command)
    {
        throw new NotImplementedException();
    }
}

   