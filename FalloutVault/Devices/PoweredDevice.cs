using FalloutVault.Devices.Interfaces;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public abstract class PoweredDevice : Device, IPoweredDevice
{
    public Watt PowerDraw
    {
        get;
        protected set
        {
            if (!SetField(ref field, value)) return;

            PublishPowerUsage(field);
        }
    }

    protected abstract Watt ComputePowerDraw();


    private void PublishPowerUsage(Watt power) => PowerBus?.Publish(this, power);
}