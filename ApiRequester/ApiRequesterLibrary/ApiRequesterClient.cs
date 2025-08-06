using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;

namespace ApiRequesterLibrary;
public class ApiRequesterClient : IApiRequesterClient
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

    /// <summary>
    /// Get ssynchronously collection of items and serialize them to given ResponseData with given Pagination object
    /// </summary>
    /// <typeparam name="T">Return type of requested items.</typeparam>
    /// <typeparam name="P">Return type of pagination data object.</typeparam>
    /// <param name="uri">Endpoint uri.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Response data with requested data and pagination object.</returns>
    public async Task<ResponseData<IReadOnlyCollection<T>, P>> GetListAsync<T, P>(string uri, CancellationToken cancellationToken = default)
    {
        var responseData = new ResponseData<IReadOnlyCollection<T>, P>();

        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using (var result = await this.httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken))
        {
            using (var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer
                        .Deserialize<ResponseData<IReadOnlyCollection<T>, P>>(jsonTextReader);

                }
            }
        }

        return responseData;
    }


    public async Task<string> ReadAsync(string uri, CancellationToken cancellationToken)
    {
        string response = string.Empty;
        var request = new HttpRequestMessage(HttpMethod.Get, uri);
        using (var result = await this.httpClient.SendAsync(request, cancellationToken))
        {
            using (var responseStream = await result.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                response = await streamReader.ReadToEndAsync();
            }
        }

        return response;
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

    public async Task<ResponseData<U>> UpdateAsync<T,U>(T item, string uri, CancellationToken cancellationToken)
    {
        var responseData = new ResponseData<U>();
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
                    responseData = this.serializer.Deserialize<ResponseData<U>>(jsonTextReader);
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

    public async Task<ResponseData<U>> PostAsync<T,U>(T item, string uri, CancellationToken cancellationToken)
    {
        var responseData = new ResponseData<U>();
        var request = new HttpRequestMessage(HttpMethod.Post, uri);

        var jsonData = JsonConvert.SerializeObject(item);
        var stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

        using (var response = await this.httpClient.PostAsync(uri, stringContent, cancellationToken))
        {
            using (var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken))
            {
                using (var streamReader = new StreamReader(responseStream))
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    responseData = this.serializer.Deserialize<ResponseData<U>>(jsonTextReader);
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

    public Uri BaseAddress
    {
        get
        {
            return this.httpClient.BaseAddress;
        }
    }
}
