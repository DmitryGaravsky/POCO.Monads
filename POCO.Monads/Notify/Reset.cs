namespace POCO.Monads.Notify {
    using System;
    using System.Linq;
    using Monads.Object;

    partial class @Notify {
        public static void Reset(object @this = null) {
            Type type = (@this as Type) ?? @this.@Get(x => x.GetType());
            if(type == null) {
                raisePropertyCache.Clear();
                raiseCanExecuteCache.Clear();
                getPropCache.Clear();
            }
            else {
                raisePropertyCache.Remove(type);
                raiseCanExecuteCache.Remove(type);
                var keys = getPropCache.Keys
                    .Where(k => k.StartsWith(type.FullName))
                    .ToArray();
                for(int i = 0; i < keys.Length; i++)
                    getPropCache.Remove(keys[i]);
            }
        }
    }
}