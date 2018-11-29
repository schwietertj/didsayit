using System;

namespace DidSayIt.Data
{
    public static class AppSettings
    {
        public static T Get<T>(string variableName, bool throwIfEmpty = false)
        {
            if (Environment.GetEnvironmentVariable(variableName) is null)
            {
                throw new Exception($"The environment variable '{variableName}' was not found.");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(variableName)))
            {
                return throwIfEmpty
                    ? throw new Exception($"The environment variable '{variableName}' is empty.")
                    : default(T);
            }

            try
            {
                return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(variableName), typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
