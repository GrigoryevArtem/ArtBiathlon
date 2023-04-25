using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories;

public class TrainingTypeRepository: IBaseRepository<TrainingType>
{
    private readonly ApplicationDbContext _db;

    public TrainingTypeRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<TrainingType> GetAll()
    {
        return _db.TrainingTypes;
    }

    public async Task Delete(TrainingType entity)
    {
        _db.TrainingTypes.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(TrainingType entity)
    {
        await _db.TrainingTypes.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<TrainingType> Update(TrainingType entity)
    {
        _db.TrainingTypes.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}