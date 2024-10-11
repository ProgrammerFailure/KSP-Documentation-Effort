using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Experience;
using UnityEngine;

[Serializable]
public class ProtoCrewMember
{
	public enum RosterStatus
	{
		[Description("#autoLOC_901072")]
		Available,
		[Description("#autoLOC_901073")]
		Assigned,
		[Description("#autoLOC_901074")]
		Dead,
		[Description("#autoLOC_901075")]
		Missing
	}

	public enum KerbalType
	{
		[Description("#autoLoc_6002189")]
		Crew,
		[Description("#autoLOC_900290")]
		Applicant,
		[Description("#autoLOC_901070")]
		Unowned,
		[Description("#autoLOC_476080")]
		Tourist
	}

	public enum Gender
	{
		Male,
		Female
	}

	public enum KerbalSuit
	{
		Default,
		Vintage,
		Future,
		Slim
	}

	public delegate void voidDelegate_PCM_Part(ProtoCrewMember pcm, Part p);

	public delegate void voidDelegate_PCM(ProtoCrewMember pcm);

	public delegate double doubleDelegatePCM(ProtoCrewMember pcm);

	public delegate void voidDelegate_PCM_UT(ProtoCrewMember pcm, double UT);

	public delegate Kerbal SpawnKerbalDelegate(ProtoCrewMember pcm);

	[SerializeField]
	private string _name;

	public float stupidity;

	public float courage;

	public bool isBadass;

	public bool hasHelmetOn;

	public bool hasNeckRingOn;

	public bool hasVisorDown;

	public float lightR;

	public float lightG;

	public float lightB;

	public bool completedFirstEVA;

	protected bool _inactive;

	public double inactiveTimeEnd;

	public bool veteran;

	public bool hasToured;

	public double UTaR;

	public Kerbal KerbalRef;

	private RosterStatus _rosterStatus;

	private KerbalType _type;

	private Gender _gender;

	private KerbalSuit _suit;

	private string suitTexturePath;

	private string normalTexturePath;

	private string spritePath;

	private bool useStockTexture;

	private string comboId;

	public uint persistentID;

	private ConfigNode chuteNode;

	private ModuleInventoryPart cachedPrefabModule;

	private ModuleInventoryPart instanceKerbalModule;

	private ConfigNode inventoryNode;

	private Part cachedPart;

	public int seatIdx;

	public InternalSeat seat;

	public static voidDelegate_PCM_Part CallbackActiveFixedUpdate;

	public static voidDelegate_PCM_Part CallbackActiveUpdate;

	public static voidDelegate_PCM_Part CallbackOnPartPack;

	public static voidDelegate_PCM_Part CallbackOnPartUnpack;

	public static voidDelegate_PCM_UT CallbackUpdate;

	public static voidDelegate_PCM_Part ApplyGCallback;

	public double geeForce;

	public double gExperienced;

	public double gIncrement;

	public bool outDueToG;

	public static doubleDelegatePCM GToleranceMult;

	public static doubleDelegatePCM MaxSustainedG;

	public static bool doStockGCalcs;

	public static SpawnKerbalDelegate Spawn;

	public FlightLog careerLog;

	public FlightLog flightLog;

	public float experience;

	private float extraExperience;

	public int experienceLevel;

	public ExperienceTrait experienceTrait;

	public string trait;

	public string name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string displayName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string nameWithGender
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool inactive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool isHero
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		internal set
		{
			throw null;
		}
	}

	public RosterStatus rosterStatus
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public KerbalType type
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Gender gender
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public KerbalSuit suit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string SuitTexturePath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string NormalTexturePath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string SpritePath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool UseStockTexture
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string ComboId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public ConfigNode ChuteNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	private ModuleInventoryPart kerbalModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleInventoryPart KerbalInventoryModule
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ConfigNode InventoryNode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public double GExperiencedNormalized
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ExtraExperience
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public float ExperienceLevelDelta
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember(KerbalType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember(KerbalType type, string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember(ProtoCrewMember copyOf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember(Game.Modes mode, ConfigNode node, KerbalType crewType = KerbalType.Crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ProtoCrewMember()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SetInactive(double time, bool additive = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CheckActive(double ut)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ChangeName(string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLocalizedType()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLocalizedStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLocalizedTrait()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameStateCreated(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveEVAChute(ModuleEvaChute chute)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float InventoryCosts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float InventoryMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ResourceMass()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefaultInventory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SaveInventory(ModuleInventoryPart inventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartUnpack(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPartPack(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double _GToleranceMult(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double _MaxSustainedG(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActiveFixedUpdate(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ActiveUpdate(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update(double UT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Kerbal _Spawn(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetKerbalEVAPartName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetKerbalIVAName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetKerbalIconSuitSuffix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetKerbalIconSuitPath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckRespawnTimer(double UT, GameParameters gameParameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartRespawnPeriod(double timeToRespawn = -1.0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTimeForRespawn(double UTforRespawn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ArchiveFlightLog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateExperience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float CalculateExperiencePoints(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float CalculateExperiencePoints(GameParameters parameters, Game.Modes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RegisterExperienceTraits(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnregisterExperienceTraits(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEffect<T>() where T : ExperienceEffect
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEffect(string effect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T GetEffect<T>() where T : ExperienceEffect
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperienceEffect GetEffect(string effect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}
}
