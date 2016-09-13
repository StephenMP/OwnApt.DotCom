using Microsoft.Extensions.Logging;
using System;

namespace DotCom.Domain.Exceptions
{
    public static class ExceptionUtility
    {
        #region Methods

        public static Exception HandleException(Exception exception, ILogger logger, LogLevel logLevel)
        {
            var exceptionId = Guid.NewGuid().ToString("N");
            var message = $"{exceptionId} - {exception.Message}";

            LogException(message, logger, logLevel);
            exception.Data.Add("ExceptionId", exceptionId);

            return exception;
        }

        public static Exception RaiseException(string message, ILogger logger, LogLevel logLevel)
        {
            return HandleException(new Exception(message), logger, logLevel);
        }

        private static void LogException(string message, ILogger logger, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Warning:
                    logger.LogWarning(message);
                    break;

                case LogLevel.Error:
                    logger.LogError(message);
                    break;

                case LogLevel.Critical:
                    logger.LogCritical(message);
                    break;

                default:
                    logger.LogWarning($"[DEFAULT] {message}");
                    break;
            }
        }

        #endregion Methods
    }
}
