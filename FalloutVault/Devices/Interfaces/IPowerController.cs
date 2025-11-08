using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FalloutVault.Eventing.Models;

namespace FalloutVault.Devices.Interfaces;
public interface IPowerController
{
    Watt PowerGeneration { get; }
    Watt StandardGeneration { get; }
    double Efficiency { get; }
}
