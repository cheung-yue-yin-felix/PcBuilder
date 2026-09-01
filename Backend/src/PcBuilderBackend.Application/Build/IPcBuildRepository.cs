using PcBuilderBackend.Domain.Entities;

namespace PcBuilderBackend.Application.Build;

public interface IPcBuildRepository
{
    Task<PcBuild?> GetByIdAsync(Guid id);
    Task<PcBuild?> GetWithChildrenAsync(Guid id);
    void Add(PcBuild pcBuild);
    void AddUser(PcBuildUser pcBuildUser);
    void DeletePart(PcBuildPart pcBuildPart);
}