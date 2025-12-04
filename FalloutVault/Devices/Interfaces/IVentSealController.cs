namespace FalloutVault.Devices.Interfaces;

internal interface IVentSealController : IOpenClose
{
    bool LockState { get;}
}
