﻿//**********************************************************
//
//      Author:     Vassilis Salis
//      Summary:    My Speech to Text implementation using Azure Congnitive Services.
//                  If needed, this can be replaced by "other" speech to text Services
//
//      Started:    Winter 2021
//      Last mod:   Spring 2021
//
//      License:    Beerware
//**********************************************************


using Microsoft.Graph.Communications.Common.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

namespace RecordingBot.Services.Util
{
    public class MyGraphLogger : IObserver<LogEvent>
    {
        private readonly LogEventFormatter formatter = new LogEventFormatter();
        private TelemetryClient mTelemetryClient;

        public MyGraphLogger(TelemetryClient aTelemetryClient)
        {
            mTelemetryClient = aTelemetryClient;
        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="logEvent">The current notification information.</param>
        public void OnNext(LogEvent logEvent)
        {
            // Log event.
            // Event Severity: logEvent.Level
            // Http trace: logEvent.EventType == LogEventType.HttpTrace
            // Log trace: logEvent.EventType == LogEventType.Trace
            var logString = this.formatter.Format(logEvent);

            if (logEvent.Level == System.Diagnostics.TraceLevel.Error)
            {
                ExceptionTelemetry lExTel = new ExceptionTelemetry(new Exception(logString));
                mTelemetryClient.TrackException(lExTel);                
            }
            else
            {
                mTelemetryClient.TrackTrace(logString);
            }
        }

        /// <summary>
        /// Notifies the observer that the provider has experienced an error condition.
        /// </summary>
        /// <param name="error">An object that provides additional information about the error.</param>
        public void OnError(Exception error)
        {
            // Error occurred with the logger, not with the SDK.
        }

        /// <summary>
        /// Notifies the observer that the provider has finished sending push-based notifications.
        /// </summary>
        public void OnCompleted()
        {
            // Graph Logger has completed logging (shutdown).
        }
    }

    //public static class MyAppInsightsLogger
    //{
    //    private static Lazy<TelemetryClient> mLogger = new Lazy<TelemetryClient>(() => {
    //        var lConfig = new TelemetryConfiguration("e8e4be66-3875-4cc2-8fd1-3f392ba56455");            

    //        var mLogger = new TelemetryClient(lConfig);
    //        return mLogger;
    //    });

    //    public static TelemetryClient Logger
    //    {
    //        get 
    //        {
    //            return mLogger.Value;
    //        }

    //        private set { }
    //    }
    //}
}
