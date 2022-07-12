using System;

namespace GameFramework.HotfixAssembly
{
    public interface IHotfixAssemblyManager
    {
        event EventHandler<HotfixAssemblyDebugCacheUpdatedEventArgs> DebugCacheUpdatedHandler;
        
        void SetRealtimeDebugPollingInterval(int intervalInMilliseconds);
        void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback);
        void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback);
        void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback);
        void SetHotfixAssemblyHelper(IHotfixAssemblyHelper helper);
        void SetHotfixAssemblyRealtimeDebugHelper(IHotfixAssemblyRealtimeDebugHelper realtimeDebugHelperHelper);
    }
}
