using System.Collections.Generic;
using UnityEngine;

public class ResourceCache : MonoBehaviour
{
	public class AbundanceSummary
	{
		public int BodyId { get; set; }

		public string BiomeName { get; set; }

		public string ResourceName { get; set; }

		public float Abundance { get; set; }

		public HarvestTypes HarvestType { get; set; }
	}

	public static ResourceCache instance;

	public static List<AbundanceSummary> _abundanceCache;

	public static List<ResourceData> _globalResources;

	public static List<ResourceData> _planetaryResources;

	public static List<ResourceData> _biomeResources;

	public static ResourceCache Instance => instance ?? (instance = new GameObject("ResourceCache").AddComponent<ResourceCache>());

	public List<AbundanceSummary> AbundanceCache => _abundanceCache ?? (_abundanceCache = LoadAbundanceCache());

	public List<ResourceData> BiomeResources => _biomeResources ?? (_biomeResources = LoadResourceInfo("BIOME_RESOURCE"));

	public List<ResourceData> GlobalResources => _globalResources ?? (_globalResources = LoadResourceInfo("GLOBAL_RESOURCE"));

	public List<ResourceData> PlanetaryResources => _planetaryResources ?? (_planetaryResources = LoadResourceInfo("PLANETARY_RESOURCE"));

	public void OnDestroy()
	{
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void ResetCache()
	{
		_abundanceCache = null;
		_globalResources = null;
		_planetaryResources = null;
		_biomeResources = null;
	}

	public AbundanceSummary RequestSummary(CelestialBody body, string biome, string resourceName, HarvestTypes type)
	{
		AbundanceSummary abundanceSummary = new AbundanceSummary();
		AbundanceRequest request = new AbundanceRequest
		{
			BodyId = body.flightGlobalsIndex,
			CheckForLock = false,
			ExcludeVariance = true,
			ResourceName = resourceName,
			BiomeName = biome,
			ResourceType = type
		};
		abundanceSummary.Abundance = ResourceMap.Instance.GetAbundance(request);
		abundanceSummary.BodyId = body.flightGlobalsIndex;
		abundanceSummary.BiomeName = biome;
		abundanceSummary.ResourceName = resourceName;
		abundanceSummary.HarvestType = type;
		return abundanceSummary;
	}

	public List<AbundanceSummary> LoadAbundanceCache()
	{
		List<AbundanceSummary> list = new List<AbundanceSummary>();
		int count = FlightGlobals.Bodies.Count;
		for (int i = 0; i < count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.Bodies[i];
			List<string> list2 = ResourceMap.Instance.FetchAllResourceNames(HarvestTypes.Exospheric);
			int count2 = list2.Count;
			string defaultSituation = ResourceMap.GetDefaultSituation(HarvestTypes.Exospheric);
			for (int j = 0; j < count2; j++)
			{
				list.Add(RequestSummary(celestialBody, defaultSituation, list2[j], HarvestTypes.Exospheric));
			}
			List<string> list3 = ResourceMap.Instance.FetchAllResourceNames(HarvestTypes.Atmospheric);
			int count3 = list3.Count;
			string defaultSituation2 = ResourceMap.GetDefaultSituation(HarvestTypes.Atmospheric);
			for (int k = 0; k < count3; k++)
			{
				list.Add(RequestSummary(celestialBody, defaultSituation2, list3[k], HarvestTypes.Atmospheric));
			}
			List<string> list4 = ResourceMap.Instance.FetchAllResourceNames(HarvestTypes.Oceanic);
			int count4 = list4.Count;
			string defaultSituation3 = ResourceMap.GetDefaultSituation(HarvestTypes.Oceanic);
			for (int l = 0; l < count4; l++)
			{
				list.Add(RequestSummary(celestialBody, defaultSituation3, list4[l], HarvestTypes.Oceanic));
			}
			List<string> list5 = ResourceMap.Instance.FetchAllResourceNames(HarvestTypes.Planetary);
			int count5 = list5.Count;
			if (!(celestialBody.BiomeMap != null))
			{
				continue;
			}
			int num = celestialBody.BiomeMap.Attributes.Length;
			for (int m = 0; m < num; m++)
			{
				CBAttributeMapSO.MapAttribute mapAttribute = celestialBody.BiomeMap.Attributes[m];
				for (int n = 0; n < count5; n++)
				{
					list.Add(RequestSummary(celestialBody, mapAttribute.name, list5[n], HarvestTypes.Planetary));
				}
			}
		}
		return list;
	}

	public List<ResourceData> LoadResourceInfo(string node)
	{
		return ResourceUtilities.ImportConfigNodeList(GameDatabase.Instance.GetConfigNodes(node));
	}
}
