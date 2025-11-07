using FalloutVault.Eventing.Interfaces;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Eventing;

public sealed class PowerEventBus : IEventBus<WattHours>
{
    private readonly Dictionary<object, WattHours> _devicePowerDraws = [];

    public event EventHandler<WattHours>? Handler;

    public void Publish(object sender, WattHours data)
    {
        _devicePowerDraws[sender] = data;

        var totalDraw = (WattHours)_devicePowerDraws.Values.Sum(x => x.Wh);
        Handler?.Invoke(this, totalDraw);
    }
}