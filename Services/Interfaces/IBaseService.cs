using ArtBiathlon.Interfaces;


namespace ArtBiathlon.Services.Interfaces
{
    public interface IBaseService<T>
    {
        Task<IBaseResponse<T>> Create(T model);

        Task<IBaseResponse<IEnumerable<T>>> GetAll();

        Task<IBaseResponse<bool>> Delete(long id);
    }
}
