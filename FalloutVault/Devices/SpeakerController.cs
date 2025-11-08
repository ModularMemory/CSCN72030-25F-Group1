using FalloutVault.Devices.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class SpeakerController : Device, ISpeakerController
{
    // Fields

    private bool _activated;

    // Properties

    public override string Name { get; }
    public override string Zone { get; }
    public override EventHandler<DeviceMessage>? OnDeviceMessage { get; }

    public bool Activated
    {
        get => _activated;
        set => _activated = value;
    }

    // Constructors

    public SpeakerController(string name, string zone)
    {
        Name = name;
        Zone = zone;
    }

    // Methods

    public override void Update()
    {
        throw new NotImplementedException();
    }

    public void TurnOnFor(TimeSpan time)
    {
        throw new NotImplementedException();
    }

    public void TurnOffFor(TimeSpan time)
    {
        throw new NotImplementedException();
    }
}