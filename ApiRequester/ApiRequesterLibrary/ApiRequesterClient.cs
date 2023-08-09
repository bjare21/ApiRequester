using Newtonsoft.Json;

namespace ApiRequesterLibrary;
public class ApiRequesterClient
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializer serializer;

    public ApiRequesterClient(HttpClient client, JsonSerializer serializer)
    {
        this.httpClient = client ?? throw new ArgumentNullException(nameof(client));
        this.serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public async Task<IReadOnlyCollection<T>> GetListAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using (var result = await this.httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            using (var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var res = this.serializer.Deserialize<ResponseData<List<T>>>(jsonTextReader);
                    return res.Data;
                }
            }
        }
    }
}
