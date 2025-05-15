using System.Net;

namespace MinimalHttp;

/// <summary>
/// BaseModel of any exception happening in any external service provider which called through http call
/// </summary>
public class ExternalProviderException : Exception
{
    /// <summary>
    /// Default constructor for <see cref="ExternalProviderException"/>
    /// </summary>
    public ExternalProviderException(
        string errorCode,
        string errorTitle,
        string errorMessage, 
        HttpStatusCode httpStatusCode, 
        IReadOnlyDictionary<string, string[]> errors)
    {
        Code = errorCode;
        Title = errorTitle;
        Detail = errorMessage;
        HttpStatusCode = httpStatusCode;
        Errors = errors;
    }

    /// <summary>
    /// Identifier for error type of external service provider.
    /// This could help to handle different errors
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Title of error happened in external service provider.
    /// This could help to create an error message for clients.
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Detail of error happened in external service provider.
    /// This could help to create an error message for clients.
    /// </summary>
    public string Detail { get; private set; }

    /// <summary>
    /// Returned HttpStatusCode from external service provider.
    /// This could be useful to handle different errors
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; private set; }

    /// <summary>
    /// Detailed errors of external service provider.
    /// Usually in BadRequest(400) scenarios contains properties as key and validation errors on values. 
    /// </summary>
    public IReadOnlyDictionary<string, string[]> Errors { get; private set; }
}

/// <summary>
/// Wrapping ErrorModel of external service provider and also is a <see cref="ExternalProviderException"/>
/// </summary>
public class ExternalProviderException<TErrorModel> : ExternalProviderException
{
    /// <summary>
    /// Default constructor for <see cref="ExternalProviderException{TErrorModel}"/>
    /// </summary>
    public ExternalProviderException(
        string errorCode,
        string errorTitle,
        string errorMessage, 
        HttpStatusCode httpStatusCode, 
        IReadOnlyDictionary<string, string[]> errors,
        TErrorModel? errorModel) 
        : base(errorCode, errorTitle, errorMessage, httpStatusCode, errors)
    {
        ErrorModel = errorModel;
    }

    /// <summary>
    /// Wrapped External service provider's error model
    /// </summary>
    public TErrorModel? ErrorModel { get; private set; }
}
