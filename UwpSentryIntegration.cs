using Sentry;
using Sentry.Infrastructure;
using Sentry.Integrations;
using Sentry.Protocol;
using System;
using System.Runtime.ExceptionServices;
using System.Security;
using Windows.ApplicationModel;
using Windows.UI.Xaml;

namespace ContribSentry.UWP
{
    public class UwpSentryIntegration : ISdkIntegration
    {
        private IHub _hub;
        private Application _application;

        public void Register(IHub hub, SentryOptions options)
        {
            _hub = hub;

            options.AddEventProcessor(new UwpPlatformEventProcessor(options));
            options.DiagnosticLogger = new DebugDiagnosticLogger(options.DiagnosticsLevel);

            _application = Application.Current;
            _application.UnhandledException += NativeHandle;
            _application.EnteredBackground += OnSleep;
            _application.LeavingBackground += OnResume;
        }

        public void Unregister()
        {
            _application.UnhandledException -= NativeHandle;
            _application.EnteredBackground -= OnSleep;
            _application.LeavingBackground -= OnResume;

            _hub = null;
        }

        #region Events

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        internal void NativeHandle(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            //We need to backup the reference, because the Exception reference last for one access.
            //After that, a new  Exception reference is going to be set into e.Exception.
            var exception = e.Exception;
            Unregister();
            Handle(exception);
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        internal void Handle(Exception exception)
        {
            if (exception != null)
            {
                exception.Data[Mechanism.HandledKey] = false;
                exception.Data[Mechanism.MechanismKey] = "Application.UnhandledException";
                _ = SentrySdk.CaptureException(exception);
                (_hub as IDisposable)?.Dispose();
            }
        }


        private void OnResume(object sender, LeavingBackgroundEventArgs e)
            => SentrySdk.AddBreadcrumb("OnResume", "app.lifecycle", "event");

        private void OnSleep(object sender, EnteredBackgroundEventArgs e)
            => SentrySdk.AddBreadcrumb("OnSleep", "app.lifecycle", "event");

        #endregion
    }
}