namespace ApiRequesterLibrary;
public interface IApiRequesterClientFactory
{
    ApiRequesterClient Create(HttpClient client);
}
