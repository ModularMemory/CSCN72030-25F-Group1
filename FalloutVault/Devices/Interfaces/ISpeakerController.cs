namespace FalloutVault.Devices.Interfaces;

interface ISpeakerController : ITemporaryOn, ITemporaryOff
{
    bool Activated { get; set; }
}
