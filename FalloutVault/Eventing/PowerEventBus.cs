using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Eventing;

public sealed class PowerEventBus : IEventBus<WattHours>
{
    private readonly Dictionary<DeviceId, WattHours> _devicePowerDraws = [];

    public event EventHandler<WattHours>? Handler;

    public void Publish(object sender, WattHours data)
    {
        if (sender is not IDevice device)
            return;

        _devicePowerDraws[device.Id] = data;

        var totalDraw = (WattHours)_devicePowerDraws.Values.Sum(x => x.Wh);
        Handler?.Invoke(this, totalDraw);
    }
}