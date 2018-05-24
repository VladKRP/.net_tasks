using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace MvcMusicStore.Infrastructure
{
    [PerformanceCounterCategory("UserLogin",PerformanceCounterCategoryType.SingleInstance, "Count users success log in and log off actions.")]
    public enum UserLoginCounter
    {
        [PerformanceCounter("log in", "Count success user log in", PerformanceCounterType.NumberOfItems32)]
        LogIn,
        [PerformanceCounter("log off", "Count success user log off", PerformanceCounterType.NumberOfItems32)]
        LogOff,
        [PerformanceCounter("log in/log off error", "Count log in/log off errors", PerformanceCounterType.NumberOfItems32)]
        Error
    }
}