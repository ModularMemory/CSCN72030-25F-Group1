using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutVault.Devices.Interfaces
{
    internal class IVentSealController
    {
        int _Section { get; set; }
        bool _IsOpen { get; set; }
        bool _LockState { get; set; }
    }
}
