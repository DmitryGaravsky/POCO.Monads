namespace POCO.Monads.Internal {
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    static class RuntimeMethodHelper {
        readonly static int OFFSET = IntPtr.Size * 2;
        internal static IntPtr Replace(MethodBase source, MethodBase destination) {
            RuntimeHelpers.PrepareMethod(source.MethodHandle);
            RuntimeHelpers.PrepareMethod(destination.MethodHandle);
            return Replace(source.MethodHandle, destination.MethodHandle);
        }
        internal static void Restore(MethodBase destination, IntPtr originalValue) {
            Marshal.WriteIntPtr(destination.MethodHandle.Value, OFFSET, originalValue);
        }
        static IntPtr Replace(RuntimeMethodHandle source, RuntimeMethodHandle destination) {
            var originalValue = Marshal.ReadIntPtr(destination.Value, OFFSET);
            Marshal.WriteIntPtr(destination.Value, OFFSET, Marshal.ReadIntPtr(source.Value, OFFSET));
            return originalValue;
        }
    }
}