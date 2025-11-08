namespace FalloutVault.Eventing.Models;

public readonly record struct Watt(double W)
{
    // Constants
    public static readonly Watt Zero = new(0);

    // Cast operators
    public static explicit operator Watt(double d) => new(d);
    public static explicit operator double(Watt w) => w.W;

    // WattHours-double operators
    public static Watt operator *(Watt a, double d) => new(a.W * d);
    public static Watt operator /(Watt a, double d) => new(a.W / d);

    // WattHours-WattHours operators
    public static Watt operator +(Watt a, Watt b) => new(a.W + b.W);
    public static Watt operator -(Watt a, Watt b) => new(a.W - b.W);
    public static Watt operator *(Watt a, Watt b) => a * b.W;
    public static Watt operator /(Watt a, Watt b) => a / b.W;
}