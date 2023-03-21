using ArtBiathlon.DataEntity;
using ArtBiathlon.Interfaces;
using ArtBiathlon.Models;

namespace ArtBiathlon.Services.Interfaces
{
    public interface IAdminService
    {
        Task<IBaseResponse<List<HelpViewModel>>> GetQuestions();

        Task<IBaseResponse<bool>> CreateAnswer(uint id, string answer);

        Task<IBaseResponse<Dictionary<string, (int, int, int)>>> GetTopicsInformation();
    }
}
