using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class AvailablePart
{
	public delegate int APEntryCostGetDelegate(AvailablePart ap);

	public class ModuleInfo
	{
		public string moduleName;

		public string moduleDisplayName;

		public string info;

		public Callback<Rect> onDrawWidget;

		public string primaryInfo;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModuleInfo()
		{
			throw null;
		}
	}

	public class ResourceInfo
	{
		public string resourceName;

		public string displayName;

		public string info;

		public string primaryInfo;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceInfo()
		{
			throw null;
		}
	}

	[Persistent]
	public string name;

	[Persistent]
	public string title;

	[Persistent]
	public string manufacturer;

	[Persistent]
	public string author;

	[Persistent]
	public string description;

	[Persistent]
	public string typeDescription;

	[Persistent]
	public string moduleInfo;

	[Persistent]
	public string resourceInfo;

	[Persistent]
	public PartCategories category;

	[Persistent]
	public string TechRequired;

	[Persistent]
	public bool TechHidden;

	[Persistent]
	protected int _entryCost;

	public static APEntryCostGetDelegate EntryCostGetter;

	[Persistent]
	public string identicalParts;

	[Persistent]
	public int amountAvailable;

	[Persistent]
	public float cost;

	[Persistent]
	public string bulkheadProfiles;

	[Persistent]
	public string tags;

	[Persistent]
	public PartVariant variant;

	[Persistent]
	public bool showVesselNaming;

	public string partUrl;

	public GameObject iconPrefab;

	private Renderer[] _iconRenderers;

	public float iconScale;

	public Part partPrefab;

	[Persistent]
	public bool mapActionsToSymmetryParts;

	private float minimumMass;

	private float minimumRBMass;

	public float minimumCost;

	[Persistent]
	public PreferredInitialStage preferredStage;

	public ConfigNode internalConfig;

	public ConfigNode partConfig;

	public string configFileFullName;

	public UrlDir.UrlConfig partUrlConfig;

	public List<string> fileTimes;

	public float partSize;

	public bool costsFunds;

	public List<ModuleInfo> moduleInfos;

	public List<ResourceInfo> resourceInfos;

	[Persistent]
	public int entryCost
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Renderer[] iconRenderers
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float MinimumMass
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

	public float MinimumRBMass
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

	public string partPath
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public List<PartVariant> Variants
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AvailablePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AvailablePart(string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AvailablePart(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static AvailablePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int _GetEntryCost(AvailablePart ap)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEntryCost(int cost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFileTime(UrlDir.UrlFile file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartVariant GetVariant(string variantName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Init()
	{
		throw null;
	}
}
