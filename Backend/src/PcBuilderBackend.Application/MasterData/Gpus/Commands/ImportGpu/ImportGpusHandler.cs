using AutoMapper;
using MediatR;
using PcBuilderBackend.Application.MasterData.Gpus.Dto;
using PcBuilderBackend.Application.Common.Interfaces;
using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.MasterData.Gpus.Commands.ImportGpu;

public class ImportGpusHandler(IApplicationDbContext context, IExcelImportService excel, IMapper mapper)
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

        return [.. result.Select(mapper.Map<GpuDto>)];
    }
}
