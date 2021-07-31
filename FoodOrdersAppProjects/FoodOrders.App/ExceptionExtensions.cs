using System;
using System.Linq;
using System.Text;

namespace FoodOrders
{
    //todo: localize
    public static class ExceptionExtensions
    {
        static string CreateRecord(string data, string separator, bool insertSeparator)
        {
            if (insertSeparator)
                return $"{separator}{data}";
            else
                return data;
        }
        
        public static string GetAllMessages(this Exception exception, string separator = null, bool addStackTrace = false, bool innerExceptionsFirst = false)
        {
            string sep = separator ?? Environment.NewLine;
            StringBuilder sb = new StringBuilder();
            Exception error = exception;
            string message;
            bool insertSeparator = false;

            while (error != null)
            {
                if (error is AggregateException)
                {
                    message = string.Join(sep, ((AggregateException)error).InnerExceptions.Select(o => o.Message));
                }
                else
                {
                    message = string.IsNullOrWhiteSpace(error.Message) ? "Virheen tarkemmat tiedot puuttuu." : error.Message;
                }

                sb.Append(CreateRecord(message, sep, insertSeparator));
                insertSeparator = true;

                // AggregateException sets InnerException value for some reason, we use InnerExceptions collection to retrieve messages
                // keep last visited error for stack trace
                if (error is AggregateException
                    || error.InnerException == null)
                    break;

                error = error.InnerException;
            }

            string result = sb.ToString();

            if (innerExceptionsFirst)
                result = string.Join(sep, result.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries).Reverse());

            if (addStackTrace)
            {
                sb.Clear();
                sb.Append(result);
                sb.Append(CreateRecord(error?.StackTrace ?? "No stack trace information.", sep + sep, true));
                result = sb.ToString();
            }

            return result;
        }

        public static T FindFirst<T>(this Exception source)
            where T : Exception
        {
            T result = null;

            Exception error = source;

            while (error != null)
            {
                if (error is T)
                {
                    result = (T)error;
                    break;
                }

                //todo: AggregateException omitted for this implementation

                error = error.InnerException;
            }

            return result;
        }
    }
}
