using PcBuilderBackend.Domain.Enums;

namespace PcBuilderBackend.Domain.ValueObjects;

public readonly record struct MotherboardM2GroupKey(
    M2Key Key,
    PcieGeneration PcieGeneration,
    bool SupportsSata,
    string FormFactors)
{
    public static MotherboardM2GroupKey From(
        M2Key key,
        PcieGeneration pcieGeneration,
        bool supportsSata,
        IEnumerable<M2FormFactor> formFactors) =>
        new(key, pcieGeneration, supportsSata, string.Join(',', formFactors.Distinct().OrderBy(x => x)));
}
