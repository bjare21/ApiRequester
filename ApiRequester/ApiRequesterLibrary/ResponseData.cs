using Newtonsoft.Json;

namespace ApiRequesterLibrary;
public class ResponseData<T>
{
    public T Data { get; set; }
    public string Error { get; set; }

}

public class ResponseData<T, P>
{
    public T Data { get; set; }
    public string Error { get; set; }
    public P Pagination { get; set; }
}

public class PaginationResponseData<T> : ResponseData<T>, IResponseData<T>
{
    public Pagination Pagination { get; set; }
}

public class Pagination
{
    public bool Has_More { get; set; }
}

public interface IResponseData<T>
{
    T Data { get; set; }
    string Error { get; set; }
}