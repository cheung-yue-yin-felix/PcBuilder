namespace PcBuilderBackend.Domain.Entities;

public class Socket : ProductEntity
{
    private readonly List<Motherboard> _motherboards = [];
    public IReadOnlyCollection<Motherboard> Motherboards => _motherboards;

    private readonly List<Cpu> _cpus = [];
    public IReadOnlyCollection<Cpu> Cpus => _cpus;

    private readonly List<CpuCoolerSocket> _cpuCoolerSockets = [];
    public IReadOnlyCollection<CpuCoolerSocket> CpuCoolerSockets => _cpuCoolerSockets; 
    
    protected Socket() {}

    public Socket(Guid manufacturerId, string name)
    {
        SetName(name);
        SetManufacturer(manufacturerId);
    }
}
