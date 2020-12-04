<p align="center">
  <a target="_blank" align="center">
    <img src="https://github.com/lucas-zimerman/ContribSentry.UWP/raw/master/.assets/contrib-nuget.png" width="280">
  </a>
  <br />
</p>

# ContribSentry.UWP
Enhanced experience for Legacy UWP Projects.

|      Integrations             |    Downloads     |    NuGet Stable     |    NuGet Preview     |
| ----------------------------- | :-------------------: | :-------------------: | :-------------------: |
|         **ContribSentry.UWP**            | [![Downloads](https://img.shields.io/nuget/dt/ContribSentry.UWP.svg)](https://www.nuget.org/packages/ContribSentry.UWP) | [![NuGet](https://img.shields.io/nuget/v/ContribSentry.UWP.svg)](https://www.nuget.org/packages/ContribSentry.UWP)   |    [![NuGet](https://img.shields.io/nuget/vpre/ContribSentry.UWP.svg)](https://www.nuget.org/packages/ContribSentry.UWP)   |


## How to use

All you need to do is to include UWPSentryIntegration on SentryOptions

```C#
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //You must initialize after the InitializeComponent();
            SentrySdk.Init(o =>
            {
                o.Dsn = new Dsn("your DSN");
                o.AddIntegration(new UwpSentryIntegration());
            });
            ...
        }
     ...
```

And that's all that you need to setup :D

## NOTE

V1.0.1 is not compatible with Sentry SDK 3.0.0, there'll be a specific release to support 3.0.0
