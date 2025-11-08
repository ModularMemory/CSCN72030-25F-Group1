namespace FalloutVault;

internal static class Utils
{
    public static bool SetField<T>(ref T field, T value) where T : IEquatable<T>
    {
        if (field.Equals(value))
        {
            return false;
        }

        field = value;
        return true;
    }
}