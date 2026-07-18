using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.Entities;

public class MotherboardM2 : BaseEntity
{
    public Guid MotherboardId { get; set; }
    
    public PcieGeneration PcieGeneration { get; set; }

    public List<M2FormFactor> FormFactors { get; set; } = new List<M2FormFactor>();
    
    public int SlotCount { get; set; }
}