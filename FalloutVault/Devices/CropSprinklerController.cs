using System.Diagnostics;
using FalloutVault.Commands;
using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;
using static FalloutVault.Commands.DeviceCommand;

namespace FalloutVault.Devices;

public class CropSprinklerController : PoweredDevice, ICropSprinklerController
{
    private long _timeTurnedOn;

    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.CropSprinklerController;

    public Watt Wattage { get; }

    public bool IsOn
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            _timeTurnedOn = Stopwatch.GetTimestamp();
            PublishMessage(new DeviceMessage.CropSprinklerOnOffChanged(field));
        }
    }

    public SprinklerSection TargetSection
    {
        get;
        private set
        {
            if (!SetFieldEnum(ref field, value)) return;

            PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(field));
        }
    }


    public int TargetLitres
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.CropSprinklerTargetLitresChanged(field));
        }
    }

    public TimeSpan TimeOn
    {
        get;
        private set
        {
            if (!SetField(ref field, value)) return;

            PublishMessage(new DeviceMessage.CropSprinklerTimeOnChanged(field));
        }
    }


    public CropSprinklerController(DeviceId id, Watt wattage)
    {
        Id = id;
        Wattage = wattage;
    }

    protected override Watt ComputePowerDraw()
    {
        if (!IsOn)
            return Watt.Zero;

        return Wattage;
    }

    public override void Update()
    {
        TimeOn = IsOn
            ? Stopwatch.GetElapsedTime(_timeTurnedOn)
            : TimeSpan.Zero;
    }

    public override void SendCommand(DeviceCommand command)
    {
        switch (command)
        {
            case SetOn setOn:
                IsOn = setOn.IsOn;
                break;
            case SetCropSection setCropSection:
                TargetSection = setCropSection.Section;
                break;
            case SetCropTargetLitres setCropTargetLitres:
                TargetLitres = setCropTargetLitres.Litres;
                break;
            case GetCurrentState:
                PublishMessage(new DeviceMessage.CropSprinklerWattage(Wattage));
                PublishMessage(new DeviceMessage.CropSprinklerOnOffChanged(IsOn));
                PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(TargetSection));
                PublishMessage(new DeviceMessage.CropSprinklerTargetLitresChanged(TargetLitres));
                break;
        }
    }
}