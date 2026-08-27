namespace PcBuilderBackend.Domain.Entities;

public class CpuCoolerSocket : BaseEntity
{
    public Guid CpuCoolerId { get; private set; }
    public Guid SocketId { get; private set; }
    public CpuCooler CpuCooler { get; private set; } = null!;
    public Socket Socket { get; private set; } = null!;

    protected CpuCoolerSocket()
    {
    }

    public CpuCoolerSocket(Guid cpuCoolerId, Guid socketId)
    {
        SetSpecs(cpuCoolerId, socketId);
    }

    public void UpdateSpecs(Guid cpuCoolerId, Guid socketId)
    {
        SetSpecs(cpuCoolerId, socketId);
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private void SetSpecs(Guid cpuCoolerId, Guid socketId)
    {
        if (cpuCoolerId == Guid.Empty)
            throw new ArgumentException("CPU Cooler ID cannot be empty");

        if (socketId == Guid.Empty)
            throw new ArgumentException("Socket ID cannot be empty");

        CpuCoolerId = cpuCoolerId;
        SocketId = socketId;
    }
}
