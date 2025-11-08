using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FalloutVault.Devices.Interfaces;
public interface IFanController
{
    bool IsOn { get; set; }

    double TargetRpm { get; set; }

    double SpeedRpm { get; }
}
