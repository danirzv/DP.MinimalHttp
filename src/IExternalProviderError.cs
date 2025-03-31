namespace MinimalHttp;

public interface IExternalProviderError
{
    public Dictionary<string, string[]> GetErrors();

    public string GetTitle();
    
    public string GetMessage();

    public string GetCode();
}
