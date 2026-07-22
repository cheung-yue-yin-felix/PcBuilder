using Microsoft.EntityFrameworkCore;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Manufacturer> Manufacturers { get; set; }
    DbSet<Cpu> Cpus { get; set; }
    DbSet<Motherboard> Motherboards { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}