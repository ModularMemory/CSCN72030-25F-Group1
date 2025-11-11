using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Devices.Interfaces;

public interface ILightController : IOnOff, ITemporaryOn, ITemporaryOff
{
    [Range(0, 1)]
    double DimmerLevel { get; set; }
}