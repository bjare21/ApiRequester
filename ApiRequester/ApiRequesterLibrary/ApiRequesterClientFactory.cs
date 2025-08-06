using Newtonsoft.Json;

namespace ApiRequesterLibrary;
public class ApiRequesterClientFactory:IApiRequesterClientFactory
{
    private readonly JsonSerializer jsonSerializer;

    public ApiRequesterClientFactory(JsonSerializer jsonSerializer)
    {
        this.jsonSerializer = jsonSerializer;
    }

    public ApiRequesterClient Create(HttpClient client)
    {
        return new ApiRequesterClient(client, jsonSerializer);
    }
}
