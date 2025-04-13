namespace MinimalHttp.Sample;

public class TargetErrorModel : IExternalProviderError
{
    public string ErrorCode { get; set; }
    public string Message { get; set; }

    public Dictionary<string, string[]> GetErrors() => [];

    public string GetTitle() => "Mocky Error";

    public string GetMessage() => Message;

    public string GetCode() => $"MockyError_{ErrorCode}";
}
