using FalloutVault.Devices.Interfaces;
using FalloutVault.Devices.Models;
using FalloutVault.Eventing.Commands;
using FalloutVault.Eventing.Models;
using FalloutVault.Models;

namespace FalloutVault.Devices;

public class VentSealController : PoweredDevice, IVentSealController
{
    private int _Section;

    private bool _IsOpen;

    private bool _LockState;


    public override DeviceId Id { get; }
    public override DeviceType Type => DeviceType.VentSealController;


    public int Section
    {
        get => _Section;
        set => _Section = value;
    }

    public bool LockState
    {
        get => _LockState;
        set => _LockState = value;
    }

    public bool IsOpen
    {
        get => _IsOpen;
        set
        {
            if (LockState == true) {
                set => _IsOpen = _IsOpen;
            }
            else
            {
                set => _IsOpen = value;
            }
        }
    }

    public VentSealController(DeviceId id)
    {
        Id = id;
    }
}

