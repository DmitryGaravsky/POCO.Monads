namespace POCO.Monads.Internal {
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    static class RuntimeMethodHelper {
        readonly static int METHOD_OFFSET = 8;
        internal static IntPtr Replace(MethodBase source, MethodBase destination) {
            RuntimeHelpers.PrepareMethod(source.MethodHandle);
            RuntimeHelpers.PrepareMethod(destination.MethodHandle);
            return Replace(source.MethodHandle, destination.MethodHandle);
        }
        internal static void Restore(MethodBase destination, IntPtr originalValue) {
            Marshal.WriteIntPtr(destination.MethodHandle.Value, METHOD_OFFSET, originalValue);
        }
        static IntPtr Replace(RuntimeMethodHandle source, RuntimeMethodHandle destination) {
            IntPtr originalValue = Marshal.ReadIntPtr(destination.Value, METHOD_OFFSET);
            IntPtr value = Marshal.ReadIntPtr(source.Value, METHOD_OFFSET);
            Marshal.WriteIntPtr(destination.Value, METHOD_OFFSET, value);
            return originalValue;
        }
    }
}