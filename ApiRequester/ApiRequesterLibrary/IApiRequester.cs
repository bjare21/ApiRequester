namespace ApiRequesterLibrary;
public interface IApiRequesterClient
{
    Task<ResponseData<IReadOnlyCollection<T>>> GetListAsync<T>(string uri, CancellationToken cancellationToken);
    Task<ResponseData<IReadOnlyCollection<T>,P>> GetListAsync<T,P>(string uri, CancellationToken cancellationToken);
    Task<ResponseData<T>> GetItemAsync<T>(string uri, CancellationToken cancellationToken);
    Task<ResponseData<T>> PostAsync<T>(T item, string uri, CancellationToken cancellationToken);
    Task<ResponseData<U>> PostAsync<T, U>(T item, string uri, CancellationToken cancellationToken);
    Task<ResponseData<T>> UpdateAsync<T>(T item, string uri, CancellationToken cancellationToken);
    Task<ResponseData<U>> UpdateAsync<T,U>(T item, string uri, CancellationToken cancellationToken);

    public Uri BaseAddress { get;}
}