using System;

namespace DidSayIt.Data
{
    public class AppSettings
    {
        private T Get<T>(string variableName)
        {
            if (Environment.GetEnvironmentVariable(variableName) is null)
            {
                throw new Exception($"The environment variable '{variableName}' was not found.");
            }

            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(variableName)))
            {
                throw new Exception($"The environment variable '{variableName}' is empty.");
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
