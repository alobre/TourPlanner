﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TourPlanner.BL.Services.Logging
{
    public class Log
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void LogError(string message)
        {
            log.Error(message);
        }
        public static void LogInfo(string message)
        {
            log.Info(message);
        }
        public static void LogFatal(string message)
        {
            log.Fatal(message);
        }
        public static void LogDebug(string message)
        {
            log.Debug(message);
        }

    }
}
