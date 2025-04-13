namespace MinimalHttp.Sample;

public class TargetClient(
    HttpClient client,
    ILogger<TargetClient> logger)
{
    public async Task<string> CoolMethodCall(string url)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            TargetSuccessModel response = await client.SendAsync<TargetSuccessModel, TargetErrorModel>(request, logger);

            return $"We got response name of '{response.Name}'";
        }
        catch (ExternalProviderException<TargetErrorModel> ex) 
            when (ex.ErrorModel?.ErrorCode == "1234")
        {
            return $"{ex.Code}: it's interesting!";
        }
        catch (ExternalProviderException ex) 
        {
            return $"{ex.Code}: some uninteresting error occured!";
        }
    }
}