using System.Net;

namespace Hair.Core.Results
{
    public class DataResult<T>
    {
        public T? Data { get; set; }
        public string? Message { get; set; }
        public bool Succeeded { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        //public T? Data2 { get; set; }

        public DataResult()
        {
            Succeeded = false;
        }

        //public DataResult(T? data)
        //{
        //    Succeeded = true;
        //    Data = data;
        //}

        //public DataResult(bool succeeded, T? data)
        //{
        //    Data = data;
        //    Succeeded = succeeded;
        //}

        //public DataResult(string errorCode, string errorDefination)
        //{
        //    Succeeded = false;
        //    ErrorCode = errorCode;
        //    ErrorDefination = errorDefination;
        //}

        //public DataResult(bool succeeded, T? data, string errorCode, string errorDefination)
        //{
        //    if (!string.IsNullOrEmpty(errorCode) || !string.IsNullOrEmpty(errorDefination))
        //    {
        //        Succeeded = false;
        //        ErrorCode = errorCode;
        //        ErrorDefination = errorDefination;
        //        Data = data;
        //    }
        //    else
        //    {
        //        Succeeded = true;
        //        Data = data;
        //    }
        //}

        //public DataResult(bool succeeded, T? data, int errorCode, string errorDefination)
        //{
        //    Succeeded = false;
        //    ErrorCodeInt = errorCode;
        //    ErrorDefination = errorDefination;
        //    Data = data;
        //}


        //for resullt builder
        public DataResult(HttpStatusCode HttpStatusCode, string message)
        {
            StatusCode = HttpStatusCode;
            Message = message;
        }

        public DataResult(HttpStatusCode statusCode, T? data)
        {
            StatusCode = statusCode;
            Data = data;
        }
    }
}
