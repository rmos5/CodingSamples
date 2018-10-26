using System;
using System.Text;

namespace GraphLibTestApp
{
    class ErrorStringBuilder
    {
        public static string BuildErrorString(string message, Exception error)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(message);
            if (error != null)
            {
                sb.Append(Environment.NewLine);
                sb.Append("Details:");
                BuildErrorString(error, sb);
            }

            return sb.ToString();
        }

        private static void BuildErrorString(Exception error, StringBuilder sb)
        {
            sb.Append(Environment.NewLine);
            sb.Append(error.Message);
            if (error.InnerException != null)
                BuildErrorString(error.InnerException, sb);
        }
    }
}
