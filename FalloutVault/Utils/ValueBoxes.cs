namespace FalloutVault.Utils;

/// <summary>
/// Pre-boxed values for reduced GC pressure.
/// </summary>
internal static class ValueBoxes
{
    public static readonly object True = true;
    public static readonly object False = false;

    public static object BooleanBox(bool value) => value ? True : False;
}