export const DDR_GENERATIONS = ["Ddr4", "Ddr5"] as const;
export type DdrGeneration = (typeof DDR_GENERATIONS)[number];

export const RAM_RANKS = ["SingleRank", "DualRank"] as const;
export type RamRank = (typeof RAM_RANKS)[number];

export const PCIE_GENERATIONS = ["Gen3", "Gen4", "Gen5", "Gen6"] as const;
export type PcieGeneration = (typeof PCIE_GENERATIONS)[number];

export const PSU_CABLE_TYPES = [
  "Motherboard24Pin",
  "Cpu4Plus4Pin",
  "Pcie6Plus2Pin",
  "Pcie12VHighPower",
  "Pcie12V2X6",
  "Sata",
  "Molex",
  "Floppy",
] as const;
export type PsuCableType = (typeof PSU_CABLE_TYPES)[number];

export const RAM_FORM_FACTORS = ["UDimm", "SoDimm"] as const;
export type RamFormFactor = (typeof RAM_FORM_FACTORS)[number];

export const VIDEO_MEMORY_GB = [4, 6, 8, 12, 16, 24, 32, 64] as const;
export type VideoMemoryGb = (typeof VIDEO_MEMORY_GB)[number];

export const PCIE_SLOTS_USED = [1, 2, 3, 4] as const;
export type PcieSlotsUsed = (typeof PCIE_SLOTS_USED)[number];
