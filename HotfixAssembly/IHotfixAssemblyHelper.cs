
namespace GameFramework.HotfixAssembly
{
    /// <summary>
    /// hotfix assembly helper interface
    /// </summary>
    public interface IHotfixAssemblyHelper
    {
        void Load(GameFrameworkAction assembliesLoadSuccessCallback, GameFrameworkAction assembliesLoadFailureCallback);
        void Run(GameFrameworkAction assembliesRunSuccessCallback, GameFrameworkAction assembliesRunFailureCallback);
        void Stop(GameFrameworkAction assembliesStopSuccessCallback, GameFrameworkAction assembliesStopFailureCallback);
    }
}