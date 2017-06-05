namespace POCO.Monads.Internal {
    using System;
    using System.Reflection;

    static class MethodInfoHelper {
        internal static MethodInfo GetMethodInfo(Type sourceType, string methodName) {
            return GetMethodInfo(sourceType, methodName, Type.EmptyTypes);
        }
        internal static MethodInfo GetMethodInfo(Type sourceType, string methodName, Type[] types,
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance) {
            return GetMemberInfo(sourceType, (type) => type.GetMethod(methodName, flags, null, types, null));
        }
        static TMemberInfo GetMemberInfo<TMemberInfo>(Type sourceType, Func<Type, TMemberInfo> getMember)
            where TMemberInfo : MemberInfo {
            Type[] types = GetTypes(sourceType, sourceType.GetInterfaces());
            for(int i = 0; i < types.Length; i++) {
                var memberInfo = getMember(types[i]);
                if(memberInfo != null)
                    return memberInfo;
            }
            return null;
        }
        static Type[] GetTypes(Type sourceType, Type[] interfaces) {
            Type[] types = new Type[interfaces.Length + 1];
            Array.Copy(interfaces, types, interfaces.Length);
            types[interfaces.Length] = sourceType;
            return types;
        }
    }
}