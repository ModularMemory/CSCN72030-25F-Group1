using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace FalloutVault.Models;

public readonly record struct Watt(double W) : IFormattable
{
    // Constants
    public static readonly Watt Zero = new(0);

    // Cast operators
    public static explicit operator Watt(double d) => new(d);
    public static explicit operator double(Watt w) => w.W;

    // Watt-double operators
    public static Watt operator *(Watt a, double d) => new(a.W * d);
    public static Watt operator /(Watt a, double d) => new(a.W / d);

    // Watt-Watt operators
    public static Watt operator +(Watt a, Watt b) => new(a.W + b.W);
    public static Watt operator -(Watt a, Watt b) => new(a.W - b.W);
    public static Watt operator *(Watt a, Watt b) => a * b.W;
    public static Watt operator /(Watt a, Watt b) => a / b.W;
    public static bool operator <(Watt a, Watt b) => a.W < b.W;
    public static bool operator >(Watt a, Watt b) => a.W > b.W;

    public override string ToString() => ToString(null, null);

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var handler = new DefaultInterpolatedStringHandler(1, 1, formatProvider);
        handler.AppendFormatted(W, format);
        handler.AppendLiteral("W");
        return handler.ToStringAndClear();
    }
}