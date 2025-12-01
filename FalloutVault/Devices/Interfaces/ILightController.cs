using System.ComponentModel.DataAnnotations;
using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface ILightController : IOnOff, ITemporaryOn, ITemporaryOff
{
    Watt BulbWattage { get; }

    [Range(0, 1)]
    double DimmerLevel { get; }
}