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

    private int _TargetSections; //How many sections are being watered

    private int _TargetLitres;

    private TimeSpan _TimeSpanOn;


    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.CropSprinklerController;

    public bool IsOn
    {
        get => _IsOn;
        set
        {
            _IsOn = value;
            PublishMessage(new DeviceMessage.CropSprinklerStateChanged(_IsOn));

        }
    }
    public int TargetSections
    {
        get => _TargetSections;
        set
        {
            _TargetSections = value;
            PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(_TargetSections));

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
        //Timespan = Litres * # of sections (It takes time for water to move)
        set => _TimeSpanOn = _TimeSpanOn;
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
    }

    public override void SendCommand(DeviceCommand command)

    {
        switch (command)
        {
            case DeviceCommand.IsOn:
                IsOn = (bool)command.Data!; break;
            case DeviceCommand.CurrentCropSection:
               TargetSections  = (int)command.Data!; break;
            case DeviceCommand.GetCurrentState:
                PublishMessage(new DeviceMessage.CropSprinklerStateChanged(IsOn));
                PublishMessage(new DeviceMessage.CropSprinklerSectionChanged(TargetSections));
                break;
        }
    }
    }

   