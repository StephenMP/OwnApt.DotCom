using Microsoft.Extensions.Logging;
using OwnApt.RestfulProxy.Interface;
using System;
using System.Text;

namespace OwnApt.DotCom.Domain.Exceptions
{
    public static class ExceptionUtility
    {
        #region Public Methods

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

        public static Exception RaiseException<TResponseDto>(IProxyResponse<TResponseDto> proxyResponse, ILogger logger)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Recieved unsuccessful status code from proxy");
            builder.AppendLine($"RequestHeaders: {proxyResponse.RequestHeaders?.ToString()}");
            builder.AppendLine($"ResponseHeaders: {proxyResponse.ResponseHeaders?.ToString()}");
            builder.AppendLine($"RequestUri: {proxyResponse.RequestUri?.ToString()}");
            builder.AppendLine($"ResponseMessage: {proxyResponse.ResponseMessage}");
            builder.AppendLine($"StatusCode: {proxyResponse.StatusCode}");

            var message = proxyResponse.ResponseMessage ?? $": {proxyResponse.StatusCode.ToString()}";
            return HandleException(new Exception(builder.ToString()), logger, LogLevel.Critical);
        }

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}
