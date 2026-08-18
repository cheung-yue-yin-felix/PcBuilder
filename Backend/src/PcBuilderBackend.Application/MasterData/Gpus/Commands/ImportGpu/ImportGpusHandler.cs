using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.Common.Caching;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;

public class ImportGpusHandler(
    IApplicationDbContext context,
    IExcelImportService excel,
    IMapper mapper,
    ICacheService cache)
    : IRequestHandler<ImportGpusCommand, List<GpuDto>>
{
    public async Task<List<GpuDto>> Handle(ImportGpusCommand request, CancellationToken cancellationToken)
    {
        var gpus = await excel.ParseGpuImportAsync(request.Stream, cancellationToken);
        var result = new List<Gpu>();

        foreach (var gpu in gpus)
        {
            var entity = new Gpu(gpu.Name, gpu.ManufacturerId, gpu.SeriesId);
            context.Gpus.Add(entity);
            result.Add(entity);
        }

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByPrefixAsync(MasterDataCacheKeys.Gpus.Prefix, cancellationToken);

        return [.. result.Select(mapper.Map<GpuDto>)];
    }
}
