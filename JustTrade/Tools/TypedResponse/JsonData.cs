using System;

namespace JustTrade.Tools
{
    public static class JsonData
    {
        public static object Create(bool success)
        {
            return new { result = GetSuccess(success) , message = string.Empty, details=string.Empty };
        }

        public static object Create(bool success, string message)
        {
            return new { result = GetSuccess(success), message = message, details = string.Empty };
        }

        public static object Create(bool success, string message, string details)
        {
            return new { result = GetSuccess(success), message = message, details = details };
        }

        public static object Create(Exception ex)
        {
            return new { result = GetSuccess(false), message = ex.Message, details = ex.StackTrace };
        }

        private static string GetSuccess(bool success){
            return (success ? "success" : "error");
        }
    }
}

