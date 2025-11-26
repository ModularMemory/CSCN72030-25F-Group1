using System.Runtime.CompilerServices;

namespace FalloutVault.Tests.Utils;

public static class TestUtils
{
    public static int LineNumber([CallerLineNumber] int lineNumber = 0) => lineNumber;
}