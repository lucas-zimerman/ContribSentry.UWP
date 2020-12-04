using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContribSentry.UWP;
using Sentry;
using Sentry.Extensibility;
using System.Threading;
using System;
using Windows.UI.Xaml;

namespace ContribSentry.UWP.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void UwpPlatformEventProcessor_Register_Device_Fields()
        {
            var options = new SentryOptions();
            var eventProcessor = new UwpPlatformEventProcessor(options);
            var @event = eventProcessor.Process(new SentryEvent());

            Assert.IsNotNull(@event);
            Assert.IsNotNull(@event.Contexts.Device.Family);
            Assert.IsNotNull(@event.Contexts.Device.Manufacturer);
            Assert.IsNotNull(@event.Contexts.Device.Model);
            Assert.IsNotNull(@event.Contexts.Device.Name);
            Assert.IsNotNull(@event.Contexts.OperatingSystem.Name);
            Assert.IsNotNull(@event.Contexts.OperatingSystem.Version);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void UwpSentryIntegration_Register_Unhandled_Exception()
        {
            var integration = new UwpSentryIntegration();
            var eventProcessorCalled = new ManualResetEvent(false);
            var eventProcessor = new MockEventProcessor(eventProcessorCalled);
            SentrySdk.Init(o =>
            {
                o.Dsn = new Dsn("https://80aed643f81249d4bed3e30687b310ab@o447951.ingest.sentry.io/5428537");
                o.AddEventProcessor(eventProcessor);
                o.AddIntegration(integration);
            });
//            integration.Handle(default, new Windows.UI.Xaml.UnhandledExceptionEventArgs());
            throw new Exception("Boom");
            eventProcessorCalled.WaitOne(7000);
        }

        private class MockEventProcessor : ISentryEventProcessor
        {
            private ManualResetEvent _resetEvent;
            public MockEventProcessor(ManualResetEvent resetEvent) => _resetEvent = resetEvent;
            public SentryEvent Process(SentryEvent @event)
            {
                _resetEvent.Set();
                Assert.IsTrue(true);
                return null;
            }
        }
    }
}
