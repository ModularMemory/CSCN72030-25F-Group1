using FalloutVault.Models;

namespace FalloutVault.Devices.Interfaces;

public interface IPowerController
{
    Watt PowerGeneration { get; }

    Watt StandardGeneration { get; }

    double Efficiency { get; }

    Watt AvailablePower { get; }
}
