namespace FalloutVault.Utils;

public static class MathUtils
{
    public static double Remap(double value, double low1, double high1, double low2, double high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    public static double RemapClamped(double value, double low1, double high1, double low2, double high2)
    {
        return Math.Clamp(Remap(value, low1, high1, low2, high2), low2, high2);
    }
}