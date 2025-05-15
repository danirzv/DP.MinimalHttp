using System.Collections.ObjectModel;

namespace MinimalHttp;

/// <summary>
/// Abstraction of an External service provider's error
/// </summary>
public interface IExternalProviderError
{
    /// <summary>
    /// Detailed errors of external service provider.
    /// Usually in BadRequest(400) scenarios contains properties as key and validation errors on values.
    /// If your service provider doesn't return such thing an empty dictionary should be fine
    /// </summary>
    public virtual IReadOnlyDictionary<string, string[]> GetErrors()
        => new Dictionary<string, string[]>(0);

    /// <summary>
    /// Title of error happened in external service provider.
    /// This could help to create an error message for clients.
    /// If your service provider doesn't return such thing <see cref="string.Empty"/> should be fine
    /// </summary>
    public virtual string GetTitle()
        => String.Empty;

    /// <summary>
    /// Detailed message of error happened in external service provider.
    /// This could help to create an error message for clients.
    /// If your service provider doesn't return such thing <see cref="string.Empty"/> should be fine
    /// </summary>
    public virtual string GetMessage()
        => String.Empty;

    /// <summary>
    /// Identifier for error type of external service provider.
    /// This could help to handle different errors
    /// If you like to let client know that error is because of an external service provider you can add provider name in errorCode like 'FooError_{ErrorCode}'
    /// If your service provider doesn't return such thing put some name on that provider errors like 'FooError'
    /// </summary>
    public string GetCode();
}
