using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.Eventing;

public sealed class PowerEventBus : IEventBus<Watt>
{
    private readonly Dictionary<DeviceId, Watt> _devicePowerDraws = [];

    public event EventHandler<Watt>? Handler;

    public void Publish(object sender, Watt data)
    {
        if (sender is not IDevice device)
            return;

        _devicePowerDraws[device.Id] = data;

        var totalDraw = (Watt)_devicePowerDraws.Values.Sum(x => x.W);
        Handler?.Invoke(this, totalDraw);
    }
}