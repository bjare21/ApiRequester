using Newtonsoft.Json;
using System.Text;

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

    public async Task<ResponseData<IReadOnlyCollection<T>>> GetListAsync<T>(string uri, CancellationToken cancellationToken = default)
    {
        var responseData = new ResponseData<IReadOnlyCollection<T>>();

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using (var result = await this.httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            using (var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer
                        .Deserialize<ResponseData<IReadOnlyCollection<T>>>(jsonTextReader);
                }
            }
        }

        return responseData;
    }

    public async Task<ResponseData<T>> GetItemAsync<T>(string uri, CancellationToken cancellationToken)
    {
        var responseData = new ResponseData<T>();

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using (var result = await this.httpClient.SendAsync(request, cancellationToken))
        {
            using (var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer
                        .Deserialize<ResponseData<T>>(jsonTextReader);
                }
            }
        }

        return responseData;
    }

    public async Task<ResponseData<T>> UpdateAsync<T>(T item, string uri, CancellationToken cancellationToken)
    {
        var responseData = new ResponseData<T>();

        var request = new HttpRequestMessage(HttpMethod.Put, uri);
        var jsonData = JsonConvert.SerializeObject(item);
        var stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

        using (var response = await this.httpClient.PutAsync(uri, stringContent, cancellationToken))
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer.Deserialize<ResponseData<T>>(jsonTextReader);
                }
            }
        }

        return responseData;
    }

    public async Task<ResponseData<T>> PostAsync<T>(T item, string uri, CancellationToken cancellationToken)
    {
        var responseData = new ResponseData<T>();

        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var jsonData = JsonConvert.SerializeObject(item);
        var stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

        using (var response = await this.httpClient.PostAsync(uri, stringContent, cancellationToken))
        {
            using (var responsStream = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responsStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer.Deserialize<ResponseData<T>>(jsonTextReader);
                }
            }
        }

        return responseData;
    }
}
