using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Adjusters;
using UnityEngine;

public class ProtoPartSnapshot
{
	public Dictionary<string, KSPParseable> partStateValues;

	public Part partRef;

	public Part rootPartPrefab;

	public string partName;

	public uint craftID;

	public uint flightID;

	public uint missionID;

	public uint launchID;

	public uint persistentId;

	public bool sameVesselCollision;

	public Vector3d position;

	public Quaternion rotation;

	public Vector3 mirror;

	public SymmetryMethod symMethod;

	public AvailablePart partInfo;

	public ProtoPartSnapshot parent;

	public int parentIdx;

	public List<ProtoPartSnapshot> symLinks;

	public List<int> symLinkIdxs;

	public List<AttachNodeSnapshot> attachNodes;

	public AttachNodeSnapshot srfAttachNode;

	public int stageIndex;

	public int inverseStageIndex;

	public int resourcePriorityOffset;

	public int defaultInverseStage;

	public int seqOverride;

	public int inStageIndex;

	public int separationIndex;

	public string customPartData;

	public int attachMode;

	public ProtoVessel pVesselRef;

	public List<ProtoCrewMember> protoModuleCrew;

	public List<int> protoCrewIndicesBackup;

	public List<string> protoCrewNames;

	public float mass;

	public double temperature;

	public double skinTemperature;

	public double skinUnexposedTemperature;

	public double staticPressureAtm;

	public float explosionPotential;

	public int state;

	public int PreFailState;

	public bool shielded;

	public bool attached;

	public Part.AutoStrutMode autostrutMode;

	public bool rigidAttachment;

	public List<ProtoPartModuleSnapshot> modules;

	public List<ProtoPartResourceSnapshot> resources;

	public List<string> partRendererBoundsIgnore;

	public ConfigNode partEvents;

	public ConfigNode partActions;

	public ConfigNode partEffects;

	public ConfigNode partData;

	public string flagURL;

	public string refTransformName;

	public float moduleCosts;

	public float moduleMass;

	public string moduleVariantName;

	public int moduleCargoStackableQuantity;

	public VesselNaming vesselNaming;

	public Part partPrefab
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartSnapshot(Part PartRef, ProtoVessel protoVessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartSnapshot(Part PartRef, ProtoVessel protoVessel, bool preCreate)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartSnapshot(ConfigNode node, ProtoVessel protoVessel, Game st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void storePartRefs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ConfigurePart(Vessel vesselRef, Part partToConfigure, AvailablePart PartInfo, bool setPosition)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part CreatePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Part Load(Vessel vesselRef, bool loadAsRootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AvailablePart LoadEVAInfoBySuitGender(ProtoCrewMember protoCrew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(Vessel vesselRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AttachNode FindAttachNode(string name, List<AttachNode> nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private AttachNodeSnapshot FindAttachNodeSnapShot(string name, List<AttachNodeSnapshot> nodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasCrew(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember GetCrew(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember GetCrew(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetCrewIndex(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrew(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrew(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveCrew(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartModuleSnapshot FindModule(string moduleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartModuleSnapshot FindModule(PartModule pm, int moduleIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddProtoPartModuleAdjusters(List<AdjusterPartModuleBase> ModuleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveProtoPartModuleAdjusters(List<AdjusterPartModuleBase> ModuleAdjusters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ProtoPartRepair()
	{
		throw null;
	}
}
