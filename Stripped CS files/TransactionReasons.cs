using System;

[Flags]
public enum TransactionReasons
{
	None = 0,
	ContractAdvance = 2,
	ContractReward = 4,
	ContractPenalty = 8,
	Contracts = 0x4000E,
	VesselRollout = 0x10,
	VesselRecovery = 0x20,
	VesselLoss = 0x40,
	Vessels = 0x70,
	StrategyInput = 0x80,
	StrategyOutput = 0x100,
	StrategySetup = 0x200,
	Strategies = 0x380,
	ScienceTransmission = 0x400,
	StructureRepair = 0x800,
	StructureCollapse = 0x1000,
	StructureConstruction = 0x2000,
	Structures = 0x3800,
	RnDTechResearch = 0x4000,
	RnDPartPurchase = 0x8000,
	RnDs = 0xC000,
	Cheating = 0x10000,
	CrewRecruited = 0x20000,
	ContractDecline = 0x40000,
	Progression = 0x80000,
	Mission = 0x100000,
	Any = -1
}
