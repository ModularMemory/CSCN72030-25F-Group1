using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class CropController : PoweredDevice, ICropSprinklerController
{
    private bool _IsOn;

    private int _TargetSection;

    private int _TargetLitres;

    private double _MinutesOn;


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

    public double MinutesOn
    {
        get => _TargetSection;
        get => _TargetLitres;
        get => _MinutesOn;

        set => _MinutesOn = _TargetSection * _TargetLitres;
    }



    public CropSprinklerController(DeviceId id)
    {
        Id = id;
    }
}

   