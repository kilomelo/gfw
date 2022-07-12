using System;

namespace GameFramework.HotfixAssembly
{
    public class HotfixAssemblyDebugCacheUpdatedEventArgs : GameFrameworkEventArgs
    {
        public object UpdatedAssemblyInfo
        {
            get;
            private set;
        }
        public HotfixAssemblyDebugCacheUpdatedEventArgs()
        {
        }
        
        public static HotfixAssemblyDebugCacheUpdatedEventArgs Create(object updatedAssemblyInfo)
        {
            var hotfixAssemblyUpdatedEventArgs = ReferencePool.Acquire<HotfixAssemblyDebugCacheUpdatedEventArgs>();
            hotfixAssemblyUpdatedEventArgs.UpdatedAssemblyInfo = updatedAssemblyInfo;
            return hotfixAssemblyUpdatedEventArgs;
        }
        
        public override void Clear()
        {
            UpdatedAssemblyInfo = null;
        }
    }
}
