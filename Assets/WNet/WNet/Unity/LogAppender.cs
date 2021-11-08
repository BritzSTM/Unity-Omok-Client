using System;
using System.Xml;
using UnityEngine;
using log4net.Core;
using log4net.Appender;
using log4net.Config;

namespace WNet.Unity
{
    public class LogAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            string message = RenderLoggingEvent(loggingEvent);

            if (Level.Compare(loggingEvent.Level, Level.Error) >= 0)
                Debug.LogError(message);
            else if (Level.Compare(loggingEvent.Level, Level.Warn) >= 0)
                Debug.LogWarning(message);
            else
                Debug.Log(message);
        }
    }

    public static class LoggerConfigure
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            var textAsset = Resources.Load<TextAsset>("log4net");
            if (textAsset == null)
                throw new InvalidOperationException("Not found log4net config file.");

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(textAsset.text);

            XmlConfigurator.Configure(xmlDoc.DocumentElement);
        }
    }
}