using System.Net;

namespace FinMind.Application.Contract.Responses;

public class ApplicationResponse<T>
{
    public HttpStatusCode StatusCode { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
}