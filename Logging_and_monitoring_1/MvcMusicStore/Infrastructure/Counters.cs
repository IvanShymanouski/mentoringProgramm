using PerformanceCounterHelper;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory("MvcMusicStor",System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, "MvcMusicStor")]
    public enum Counters
    {
        [PerformanceCounter("Success login counter", "Success login",System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessLogIn,
        [PerformanceCounter("Success logout counter", "Success logout", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessLogOut,
        [PerformanceCounter("Go to Home count", "Go to home", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        GoToHome
    }
}