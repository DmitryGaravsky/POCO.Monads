namespace POCO.Monads.Internal {
    using System;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    static class RuntimeMethodHelper {
        internal static IntPtr Replace(MethodBase source, MethodBase destination) {
            RuntimeHelpers.PrepareMethod(source.MethodHandle);
            RuntimeHelpers.PrepareMethod(destination.MethodHandle);
            return Replace(GetMethodPtr(source), GetMethodPtr(destination));
        }
        internal static void Restore(MethodBase destination, IntPtr originalValue) {
            Marshal.WriteIntPtr(GetMethodPtr(destination), originalValue);
        }
        static IntPtr Replace(IntPtr sourcePtr, IntPtr destinationPtr) {
            IntPtr originalValue = Marshal.ReadIntPtr(destinationPtr);
            IntPtr value = Marshal.ReadIntPtr(sourcePtr);
            Marshal.WriteIntPtr(destinationPtr, value);
            return originalValue;
        }
        static IntPtr GetMethodPtr(MethodBase method) {
            if(method.IsVirtual)
                return GetVirtualMethodPtr(method);
            return GetInstanceOrStaticMethodPtr(method);
        }
        static IntPtr GetInstanceOrStaticMethodPtr(MethodBase method) {
            IntPtr methodHandle = method.MethodHandle.Value;
            const int METHOD_PTR_OFFSET = 8;
            return (IntPtr.Size == 4) ?
                new IntPtr(methodHandle.ToInt32() + METHOD_PTR_OFFSET) :
                new IntPtr(methodHandle.ToInt64() + METHOD_PTR_OFFSET);
        }
        static IntPtr GetVirtualMethodPtr(MethodBase method) {
            const int METHOD_INDEX_OFFSET = 4;
            int methodIndex = Marshal.ReadByte(method.MethodHandle.Value, METHOD_INDEX_OFFSET);
            IntPtr typeHandle = method.DeclaringType.TypeHandle.Value;
            const int TYPE_METHODTABLE_OFFSET_X86 = 10 * sizeof(int);
            const int TYPE_METHODTABLE_OFFSET_X64 = 8 * sizeof(int);
            return (IntPtr.Size == 4) ?
                new IntPtr(Marshal.ReadInt32(typeHandle, TYPE_METHODTABLE_OFFSET_X86) + methodIndex * IntPtr.Size) :
                new IntPtr(Marshal.ReadInt64(typeHandle, TYPE_METHODTABLE_OFFSET_X64) + methodIndex * IntPtr.Size);
        }
    }
}