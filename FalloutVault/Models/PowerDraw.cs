namespace FalloutVault.Models;
public class PowerDraw
{
    public Watt TotalDraw { get; init; }
    public Watt Available { get; init; }

    public PowerDraw(Watt totalDraw, Watt available)
    {
        TotalDraw = totalDraw;
        Available = available;
    }
}
