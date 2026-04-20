// Polyfill required for `init` accessors and C# record types on netstandard2.0/2.1
#if !NET5_0_OR_GREATER
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
#endif
