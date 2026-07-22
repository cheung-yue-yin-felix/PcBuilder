using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class Gpu : ProductEntity
{
    public Guid SeriesId { get; set; }
    
    protected Gpu() {}
    
    public Gpu(string name, Guid manufacturerId, Guid seriesId)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
        SetSeries(seriesId);
    }

    public void UpdateSeries(Guid seriesId)
    {
        SetSeries(seriesId);
        UpdatedAtUtc = DateTime.UtcNow;
    }
    
    private void SetSeries(Guid seriesId)
    {
        if (seriesId == Guid.Empty)
            throw new ArgumentException("Series id can't be empty");
        
        SeriesId = seriesId;
    }
}