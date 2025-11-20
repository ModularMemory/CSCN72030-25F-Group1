using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Devices.Interfaces;

public interface ISpeakerController : IOnOff, ITemporaryOn, ITemporaryOff
{
    [Range(0, 100)]
    double Volume { get; set; }
}
