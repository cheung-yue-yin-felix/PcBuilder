using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Catalog.Motherboards;

public interface IMotherboardRepository
{
    Task<Motherboard?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Motherboard?> GetWithChildrenAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Motherboard>> GetByIdsAsync(IReadOnlyCollection<Guid> ids, CancellationToken cancellationToken);
    void Add(Motherboard motherboard);
    void DeletePcieSlot(MotherboardPcie pcieSlot);
    void DeleteM2Slot(MotherboardM2 m2Slot);
    void DeleteUsbPort(MotherboardUsb usbPort);
    void DeleteM2FormFactor(MotherboardM2FormFactor m2FormFactor);
}