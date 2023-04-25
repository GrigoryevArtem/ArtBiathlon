using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories;

public class CampPeriodRepository : IBaseRepository<CampPeriod>
{
    private readonly ApplicationDbContext _db;

    public CampPeriodRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<CampPeriod> GetAll()
    {
        return _db.CampsPeriods;
    }

    public async Task Delete(CampPeriod entity)
    {
        _db.CampsPeriods.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(CampPeriod entity)
    {
        await _db.CampsPeriods.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<CampPeriod> Update(CampPeriod entity)
    {
        _db.CampsPeriods.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}