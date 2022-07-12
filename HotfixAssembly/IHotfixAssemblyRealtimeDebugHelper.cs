namespace GameFramework.HotfixAssembly
{
    public interface IHotfixAssemblyRealtimeDebugHelper
    {
        void CheckRemoteDllInfo(GameFrameworkAction<object> dllCacheUpdateCallback);
    }
}