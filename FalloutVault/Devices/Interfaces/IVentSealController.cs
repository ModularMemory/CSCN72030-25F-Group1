namespace FalloutVault.Devices.Interfaces;

internal interface IVentSealController
{
    bool IsOpen { get;}
    bool LockState { get;}
}
