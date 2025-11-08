using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Devices.Interfaces;

public interface ILightController : ITemporaryOn, ITemporaryOff
{
    bool IsOn { get; set; }

    [Range(0, 1)]
    double DimmerLevel { get; set; }
}