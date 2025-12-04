using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using static FalloutVault.Commands.DeviceCommand;

namespace FalloutVault.Devices;

public class CropSprinklerController : PoweredDevice, ICropSprinklerController
{
    private bool _IsOn;
    private int _TargetSection;
    private int _TargetLitres;
    private TimeSpan _TimeSpanOn;
    public Watt sprinklerWattage { get; }


    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.CropSprinklerController;

    public bool IsOn
    {
        get => _IsOn;
        set
        {
            _IsOn = value;
            PublishMessage(new DeviceMessage.CropSprinklerOnOffChanged(_IsOn));
        }
    }

    public int TargetSection
    {
        get => _TargetSection;
        set
        {
            _TargetSection = value;
            PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(_TargetSection));
        }
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


    public CropSprinklerController(DeviceId id, Watt SprinklerWattage)
    {
        Id = id;
        sprinklerWattage = SprinklerWattage;
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
        {
            PublishMessage(new DeviceMessage.CropSprinklerWattChanged(sprinklerWattage));
            return Watt.Zero;
        }

        return (Watt)50;
    }

    public override void Update() { }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case SetOn setOn:
                IsOn = setOn.IsOn;
                break;
            case CurrentCropSection currentCropSection:
                TargetSection = currentCropSection.Section;
                break;
            case GetCurrentState:
                PublishMessage(new DeviceMessage.CropSprinklerOnOffChanged(IsOn));
                PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(TargetSection));
                break;
        }
    }
}