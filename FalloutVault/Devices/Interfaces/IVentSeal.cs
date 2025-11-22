using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutVault.Devices.Interfaces;

internal interface IVentSealController
{
    bool IsOpen { get;}
    bool LockState { get;}
}
