using System;
using System.Collections.Generic;
using ns9;
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
	}

	public class ResourceInfo
	{
		public string resourceName;

		public string displayName;

		public string info;

		public string primaryInfo;
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
	public int _entryCost;

	public static APEntryCostGetDelegate EntryCostGetter = _GetEntryCost;

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

	public Renderer[] _iconRenderers;

	public float iconScale;

	public Part partPrefab;

	[Persistent]
	public bool mapActionsToSymmetryParts;

	public float minimumMass = 0.0001f;

	public float minimumRBMass = 0.001f;

	public float minimumCost;

	[Persistent]
	public PreferredInitialStage preferredStage = PreferredInitialStage.DEFAULT;

	public ConfigNode internalConfig;

	public ConfigNode partConfig;

	public string configFileFullName;

	public UrlDir.UrlConfig partUrlConfig;

	public List<string> fileTimes = new List<string>();

	public float partSize;

	public bool costsFunds = true;

	public List<ModuleInfo> moduleInfos;

	public List<ResourceInfo> resourceInfos;

	[Persistent]
	public int entryCost => EntryCostGetter(this);

	public Renderer[] iconRenderers
	{
		get
		{
			if (_iconRenderers == null)
			{
				_iconRenderers = iconPrefab.GetComponentsInChildren<Renderer>();
			}
			return _iconRenderers;
		}
	}

	public float MinimumMass
	{
		get
		{
			return minimumMass;
		}
		set
		{
			minimumMass = Mathf.Max(0.0001f, value);
		}
	}

	public float MinimumRBMass
	{
		get
		{
			return minimumRBMass;
		}
		set
		{
			minimumRBMass = Mathf.Max(0.001f, value);
		}
	}

	public string partPath { get; set; }

	public List<PartVariant> Variants
	{
		get
		{
			if (partPrefab.variants != null)
			{
				return partPrefab.variants.variantList;
			}
			return null;
		}
	}

	public AvailablePart()
	{
		Init();
	}

	public AvailablePart(string path)
	{
		Init();
		partPath = path;
	}

	public AvailablePart(AvailablePart ap)
	{
		name = ap.name;
		title = ap.title;
		manufacturer = ap.manufacturer;
		author = ap.author;
		description = ap.description;
		typeDescription = ap.typeDescription;
		moduleInfo = ap.moduleInfo;
		resourceInfo = ap.resourceInfo;
		category = ap.category;
		TechHidden = ap.TechHidden;
		_entryCost = ap._entryCost;
		amountAvailable = ap.amountAvailable;
		cost = ap.cost;
		bulkheadProfiles = ap.bulkheadProfiles;
		tags = ap.tags;
		partUrl = ap.partUrl;
		iconPrefab = ap.iconPrefab;
		iconScale = ap.iconScale;
		partPrefab = ap.partPrefab;
		internalConfig = ap.internalConfig;
		partConfig = ap.partConfig;
		configFileFullName = ap.configFileFullName;
		partUrlConfig = ap.partUrlConfig;
		fileTimes = new List<string>(ap.fileTimes);
		partSize = ap.partSize;
		TechRequired = ap.TechRequired;
		identicalParts = ap.identicalParts;
		moduleInfos = new List<ModuleInfo>(ap.moduleInfos);
		resourceInfos = new List<ResourceInfo>(ap.resourceInfos);
		variant = ap.variant;
		showVesselNaming = ap.showVesselNaming;
		mapActionsToSymmetryParts = ap.mapActionsToSymmetryParts;
		MinimumMass = ap.MinimumMass;
		minimumCost = ap.minimumCost;
		preferredStage = ap.preferredStage;
	}

	public static int _GetEntryCost(AvailablePart ap)
	{
		return ap._entryCost;
	}

	public void SetEntryCost(int cost)
	{
		_entryCost = cost;
	}

	public void AddFileTime(UrlDir.UrlFile file)
	{
		if (file == null)
		{
			Debug.Log("AddFileTime: file is null!");
		}
		else
		{
			fileTimes.Add(file.fileTime.ToBinary().ToString());
		}
	}

	public PartVariant GetVariant(string variantName)
	{
		if (Variants == null)
		{
			return null;
		}
		int num = 0;
		while (true)
		{
			if (num < Variants.Count)
			{
				if (Variants[num].Name == variantName)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return Variants[num];
	}

	public void Init()
	{
		if (Localizer.Instance == null)
		{
			author = "Unknown";
			title = "Unknown Mystery Component";
			manufacturer = "Found lying by the side of the road";
			description = "Nothing is really known about this thing. Use it at your own risk.";
		}
		else
		{
			author = Localizer.Format("#autoLOC_168872");
			title = Localizer.Format("#autoLOC_168874");
			manufacturer = Localizer.Format("#autoLOC_168875");
			description = Localizer.Format("#autoLOC_168876");
		}
		name = "unknownPart";
		amountAvailable = 999;
		internalConfig = new ConfigNode();
		TechRequired = string.Empty;
		identicalParts = string.Empty;
		_entryCost = 0;
		moduleInfos = new List<ModuleInfo>();
		resourceInfos = new List<ResourceInfo>();
		tags = "*";
		mapActionsToSymmetryParts = true;
	}
}
