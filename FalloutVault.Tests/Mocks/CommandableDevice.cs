using FalloutVault.Commands;
using FalloutVault.Devices;
using FalloutVault.Devices.Models;
using FalloutVault.Models;

namespace FalloutVault.Tests.Mocks;

public class CommandableDevice : Device
{
    private readonly List<DeviceCommand> _commands = [];

    public override DeviceId Id { get; }
    public override DeviceType Type { get; }
    public IReadOnlyList<DeviceCommand> Commands => _commands;

    public CommandableDevice(DeviceId id, DeviceType type)
    {
        Id = id;
        Type = type;
    }

    public override void Update() { }

    public override void SendCommand(DeviceCommand command)
    {
        _commands.Add(command);
    }

    public void ClearCommands()
    {
        _commands.Clear();
    }
}