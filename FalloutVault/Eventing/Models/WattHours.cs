namespace FalloutVault.Eventing.Models;

public readonly record struct WattHours(double Wh)
{
    // Constants
    public static readonly WattHours Zero = new(0);

    // Cast operators
    public static explicit operator WattHours(double d) => new(d);
    public static explicit operator double(WattHours w) => w.Wh;

    // WattHours-double operators
    public static WattHours operator *(WattHours a, double d) => new(a.Wh * d);
    public static WattHours operator /(WattHours a, double d) => new(a.Wh / d);

    // WattHours-WattHours operators
    public static WattHours operator +(WattHours a, WattHours b) => new(a.Wh + b.Wh);
    public static WattHours operator -(WattHours a, WattHours b) => new(a.Wh - b.Wh);
    public static WattHours operator *(WattHours a, WattHours b) => a * b.Wh;
    public static WattHours operator /(WattHours a, WattHours b) => a / b.Wh;
}