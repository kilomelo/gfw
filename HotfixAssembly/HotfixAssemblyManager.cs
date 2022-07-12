using System;

namespace GameFramework.HotfixAssembly
{
    internal class HotfixAssemblyManager : GameFrameworkModule, IHotfixAssemblyManager
    {
        private enum EHotfixAssemblyState : byte
        {
            Invalid = 0,
            Loaded,
            Running,
        }
        private EHotfixAssemblyState _hotfixAssemblyState;
        
        private IHotfixAssemblyHelper _hotfixAssemblyHelper;
        private IHotfixAssemblyRealtimeDebugHelper _realtimeDebugHelper;
        private GameFrameworkAction _assembliesLoadSuccessCallback;
        private GameFrameworkAction _assembliesLoadFailureCallback;

        private EventHandler<HotfixAssemblyDebugCacheUpdatedEventArgs> _debugCacheUpdatedHandler;
        
        /// <summary>
        /// realtime debug only works when interval is greater then 0
        /// </summary>
        private int _realtimeDebugPollingInterval;
        private DateTime _nextRequestTime = DateTime.MaxValue;
        
        public event EventHandler<HotfixAssemblyDebugCacheUpdatedEventArgs> DebugCacheUpdatedHandler
        {
            add => _debugCacheUpdatedHandler += value;
            remove => _debugCacheUpdatedHandler -= value;
        }
        
        /// <summary>
        /// 全局配置管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (null != _realtimeDebugHelper && DateTime.Now > _nextRequestTime)
            {
                _realtimeDebugHelper.CheckRemoteDllInfo(DllCacheUpdated);
                if (0 < _realtimeDebugPollingInterval) _nextRequestTime = _nextRequestTime.AddMilliseconds(_realtimeDebugPollingInterval);
            }
        }

        /// <summary>
        /// 关闭并清理全局配置管理器。
        /// </summary>
        internal override void Shutdown()
        {
            Stop(null, null);
        }

        public void SetRealtimeDebugPollingInterval(int intervalInMilliseconds)
        {
            _realtimeDebugPollingInterval = intervalInMilliseconds;
            if (0 >= _realtimeDebugPollingInterval)
            {
                _realtimeDebugPollingInterval = 0;
                _nextRequestTime = DateTime.MaxValue;
                return;
            }
            _nextRequestTime = DateTime.Now.AddMilliseconds(_realtimeDebugPollingInterval);
        }

        public void SetHotfixAssemblyRealtimeDebugHelper(IHotfixAssemblyRealtimeDebugHelper realtimeDebugHelperHelper)
        {
            _realtimeDebugHelper = realtimeDebugHelperHelper;
        }
        public void SetHotfixAssemblyHelper(IHotfixAssemblyHelper helper)
        {
            _hotfixAssemblyHelper = helper ?? throw new GameFrameworkException("Hotfix assembly helper is invalid.");
        }
        
        public void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback)
        {
            _assembliesLoadSuccessCallback = assembliesLoadSuccessCallback;
            _assembliesLoadFailureCallback = assembliesLoadFailureCallback;
            _hotfixAssemblyHelper.Load(AssembliesLoadSuccessCallbackWrap, AssembliesLoadFailureCallbackWrap);
        }
        
        private void AssembliesLoadSuccessCallbackWrap()
        {
            _hotfixAssemblyState = EHotfixAssemblyState.Loaded;
            _assembliesLoadSuccessCallback?.Invoke();
        }
        private void AssembliesLoadFailureCallbackWrap()
        {
            _hotfixAssemblyState = EHotfixAssemblyState.Invalid;
            _assembliesLoadFailureCallback?.Invoke();
        }
        
        public void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback)
        {
            if (EHotfixAssemblyState.Loaded != _hotfixAssemblyState) return;
            _hotfixAssemblyState = EHotfixAssemblyState.Running;
            _hotfixAssemblyHelper.Run(assembliesRunSuccessCallback, assembliesRunFailureCallback);
        }
        public void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback)
        {
            if (EHotfixAssemblyState.Running != _hotfixAssemblyState) return;
            _hotfixAssemblyState = EHotfixAssemblyState.Loaded;
            _hotfixAssemblyHelper.Stop(assembliesStopSuccessCallback, assembliesStopFailureCallback);
        }

        private void DllCacheUpdated(object updateInfo)
        {
            if (_debugCacheUpdatedHandler != null)
            {
                _debugCacheUpdatedHandler(this, HotfixAssemblyDebugCacheUpdatedEventArgs.Create(updateInfo));
            }
        }
    }
}
