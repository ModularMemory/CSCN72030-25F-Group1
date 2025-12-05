namespace FalloutVault.Utils;

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

    public static bool SetFieldEnum<T>(ref T field, T value) where T : Enum
    {
        if (Comparer<T>.Default.Compare(value,field) == 0)
        {
            return false;
        }

        field = value;
        return true;
    }
}