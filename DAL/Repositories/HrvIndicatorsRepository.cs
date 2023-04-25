using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories;

public class HrvIndicatorsRepository: IBaseRepository<HrvIndicator>
{
    private readonly ApplicationDbContext _db;

    public HrvIndicatorsRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<HrvIndicator> GetAll()
    {
        return _db.HrvIndicators;
    }

    public async Task Delete(HrvIndicator entity)
    {
        _db.HrvIndicators.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(HrvIndicator entity)
    {
        await _db.HrvIndicators.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<HrvIndicator> Update(HrvIndicator entity)
    {
        _db.HrvIndicators.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}