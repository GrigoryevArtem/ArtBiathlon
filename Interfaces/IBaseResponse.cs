using ArtBiathlon.Enums;
using Microsoft.AspNetCore.Http;


namespace ArtBiathlon.Interfaces
{
    public interface IBaseResponse<T>
    {
        string Description { get; }
        StatusCode StatusCode { get; }
        T Data { get; }
    }
}
