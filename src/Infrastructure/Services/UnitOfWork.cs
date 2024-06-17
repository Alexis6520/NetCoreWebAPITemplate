using Domain.Entities;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Services;
using Services.Repositories;

namespace Infrastructure.Services
{
    public class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext = dbContext;
        private IRepository<Donut>? _donuts;

        public IRepository<Donut> Donuts => _donuts ??= new Repository<Donut>(_dbContext.Donuts);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
