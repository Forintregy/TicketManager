using Microsoft.Extensions.Logging;
using System;

namespace TicketsWebApp.Helpers
{
    public static class HelperMethods
    {
        public static Exception LogAndThrow(string message, Exception ex, ILogger logger)
        {
            logger.LogError(message + $" {ex.Message}");
            throw ex;
        }
    }
}
