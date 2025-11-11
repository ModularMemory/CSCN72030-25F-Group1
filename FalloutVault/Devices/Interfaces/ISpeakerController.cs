namespace FalloutVault.Devices.Interfaces;

public interface ISpeakerController : ITemporaryOn, ITemporaryOff
{
    bool IsOn { get; set; }
}
