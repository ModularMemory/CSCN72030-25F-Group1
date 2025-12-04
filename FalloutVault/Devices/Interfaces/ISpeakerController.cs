using FalloutVault.Models;
using System.ComponentModel.DataAnnotations;

namespace FalloutVault.Devices.Interfaces;

public interface ISpeakerController : IOnOff, ITemporaryOn, ITemporaryOff
{
    [Range(0, 1)]
    double Volume { get; set; }

    Watt SpeakerWattage { get; }

    Watt PowerGeneration {  get; }
}
