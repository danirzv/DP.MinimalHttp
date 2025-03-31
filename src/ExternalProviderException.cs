using System.Net;

namespace MinimalHttp;

public class ExternalProviderException : Exception
{
    public ExternalProviderException(string errorCode, string errorTitle, string errorMessage, HttpStatusCode httpStatusCode, Dictionary<string, string[]> errors)
    {
        Code = errorCode;
        Title = errorTitle;
        Detail = errorMessage;
        HttpStatusCode = httpStatusCode;
        Errors = errors;

    }

    public string Code { get; private set; }

    public string Title { get; private set; }

    public string Detail { get; private set; }

    public HttpStatusCode HttpStatusCode { get; private set; }

    public Dictionary<string, string[]> Errors { get; private set; }
}

public class ExternalProviderException<TErrorModel> : ExternalProviderException
{
    public ExternalProviderException(string errorCode,string errorTitle, string errorMessage, HttpStatusCode httpStatusCode, Dictionary<string, string[]> errors, TErrorModel? errorModel) : base(errorCode, errorTitle, errorMessage, httpStatusCode, errors)
    {
        ErrorModel = errorModel;
    }

    public TErrorModel? ErrorModel { get; private set; }
}
