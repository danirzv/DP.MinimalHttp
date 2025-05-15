using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MinimalHttp;

/// <summary>
/// Options of an HttpClient
/// </summary>
public abstract class HttpClientOptions
{
    private Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>? _certificateValidationCallback;
    private static readonly Uri EmptyUri = new("", UriKind.Relative);

    /// <summary>
    /// BaseUri of target server
    /// </summary>
    public Uri BaseUri { get; init; } = null!;

    /// <summary>
    /// RelativeUri of HealthCheck path on target server (default is root path)
    /// </summary>
    public Uri HealthCheckPath { get; init; } = EmptyUri;

    /// <summary>
    /// Timeout of target server (default is 100 seconds)
    /// </summary>
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(100);

    /// <summary>
    /// CertificateThumbprint(Ssl) of target server if you want to accept an invalid one (default is null)
    /// </summary>
    public string? CertificateThumbprint { get; init; }

    /// <summary>
    /// Bypass CertificateThumbprint(Ssl) validation of target server (default is false)
    /// </summary>
    public bool IgnoreCertificate { get; init; }

    /// <summary>
    /// Sets LoggingMode of HttpClient to (default is 'None') 
    /// </summary>
    public HttpClientLoggingMode HttpClientLoggingMode { get; init; } = HttpClientLoggingMode.None;

    /// <summary>
    /// Sets ServerCertificateCustomValidationCallback in <see cref="T:System.Net.Http.HttpClientHandler" /> 
    /// </summary>
    public Func<HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors, bool>? CertificateValidationCallback
    {
        get
        {
            // IgnoreCertificate means in any scenario Ssl certificate is valid
            if (IgnoreCertificate)
                return (_, _, _, _) => true;

            // If no Thumbprint provided it requires a real valid Ssl certificate
            if (string.IsNullOrEmpty(CertificateThumbprint))
            {
                return null;
            }

            // Check if CertificateThumbprint is same as server provided thumbprint
            return _certificateValidationCallback ??= (_, certificate, _, _) => certificate.Thumbprint.Equals(CertificateThumbprint, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

/// <summary>
/// HttpClient loggingMode as Flag
/// </summary>
[Flags]
public enum HttpClientLoggingMode
{
    /// <summary>
    /// Won't log sending request or response body
    /// </summary>
    None = 0,

    /// <summary>
    /// just logs sending request body
    /// </summary>
    RequestBody = 1,

    /// <summary>
    /// just logs received response body
    /// </summary>
    ResponseBody = 2,

    /// <summary>
    /// logs both request and response body
    /// </summary>
    RequestAndResponseBody = 3,
}