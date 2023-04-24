using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;

namespace ArtBiathlon.Services.Interfaces;

public interface IHrvIndicatorService : IBaseService<HrvIndicator>
{
    Task<IBaseResponse<bool>> Delete(DateTime date, long id);
}