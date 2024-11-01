using System.Net;

namespace GamifiedToDo.API.Exceptions;

public class HttpResponseException : Exception
{
    public HttpStatusCode Status { get; set; }
    public object Value { get; set; }

    public HttpResponseException(HttpStatusCode status)
    {
        this.Status = status;
    }

    public HttpResponseException(HttpStatusCode status, object value)
    {
        this.Status = status;
        this.Value = value;
    }
}