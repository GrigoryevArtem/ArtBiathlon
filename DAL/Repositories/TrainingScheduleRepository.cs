using ArtBiathlon.DAL.Interfaces;
using ArtBiathlon.DataEntity;

namespace ArtBiathlon.DAL.Repositories;

public class TrainingScheduleRepository : IBaseRepository<TrainingSchedule>
{
    private readonly ApplicationDbContext _db;

    public TrainingScheduleRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public IQueryable<TrainingSchedule> GetAll()
    {
        return _db.TrainingsSchedules;
    }

    public async Task Delete(TrainingSchedule entity)
    {
        _db.TrainingsSchedules.Remove(entity);
        await _db.SaveChangesAsync();
    }

    public async Task Create(TrainingSchedule entity)
    {
        await _db.TrainingsSchedules.AddAsync(entity);
        await _db.SaveChangesAsync();
    }

    public async Task<TrainingSchedule> Update(TrainingSchedule entity)
    {
        _db.TrainingsSchedules.Update(entity);
        await _db.SaveChangesAsync();

        return entity;
    }
}