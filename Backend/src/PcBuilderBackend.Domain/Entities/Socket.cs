namespace PcBuilderBackend.Domain.Entities;

public class Socket : ProductEntity
{
    public ICollection<Motherboard> Motherboards { get; set; } = new List<Motherboard>();
    public ICollection<Cpu> Cpus { get; set; } = new List<Cpu>();
    public ICollection<CpuCoolerSocket> CpuCoolerSockets { get; set; } = new List<CpuCoolerSocket>();
    
    protected Socket() {}

    public Socket(Guid manufacturerId, string name)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
    }
}
