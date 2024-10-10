using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceMap : MonoBehaviour
{
	public List<ResourceData> biomeConfigs = new List<ResourceData>();

	public List<ResourceData> planetConfigs = new List<ResourceData>();

	public List<ResourceData> globalConfigs = new List<ResourceData>();

	public Dictionary<int, List<double>> seedsCache = new Dictionary<int, List<double>>();

	public int[] noiseSeed = new int[8];

	public NoiseGenerator spx = new NoiseGenerator();

	public List<DepletionData> _DepletionInfo;

	public List<BiomeLockData> _BiomeLockInfo;

	public List<PlanetScanData> _PlanetScanInfo;

	public static ResourceMap Instance { get; set; }

	public static bool Initialized => Instance != null;

	public List<DepletionData> DepletionInfo
	{
		get
		{
			if (_DepletionInfo == null)
			{
				_DepletionInfo = new List<DepletionData>();
			}
			if (ResourceScenario.Instance != null && _DepletionInfo.Count <= 0)
			{
				_DepletionInfo.AddRange(ResourceScenario.Instance.gameSettings.GetDepletionInfo());
			}
			return _DepletionInfo;
		}
	}

	public List<BiomeLockData> BiomeLockInfo
	{
		get
		{
			if (_BiomeLockInfo == null)
			{
				_BiomeLockInfo = new List<BiomeLockData>();
			}
			if (ResourceScenario.Instance != null && _BiomeLockInfo.Count <= 0)
			{
				_BiomeLockInfo.AddRange(ResourceScenario.Instance.gameSettings.GetBiomeLockInfo());
			}
			return _BiomeLockInfo;
		}
	}

	public List<PlanetScanData> PlanetScanInfo
	{
		get
		{
			if (_PlanetScanInfo == null)
			{
				_PlanetScanInfo = new List<PlanetScanData>();
			}
			if (ResourceScenario.Instance != null && _PlanetScanInfo.Count <= 0)
			{
				_PlanetScanInfo.AddRange(ResourceScenario.Instance.gameSettings.GetPlanetScanInfo());
			}
			return _PlanetScanInfo;
		}
	}

	public virtual void Awake()
	{
		if (Instance != null && Instance != this)
		{
			UnityEngine.Object.Destroy(Instance);
		}
		Instance = this;
		GameEvents.OnResourceMapLoaded.Fire();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void ResetCache()
	{
		_BiomeLockInfo = null;
		_DepletionInfo = null;
		_PlanetScanInfo = null;
		ResourceCache.Instance.ResetCache();
	}

	public DepletionNode GetDepletionNode(int planetId, string resource, int x, int y)
	{
		if (DepletionInfo.Count == 0)
		{
			return null;
		}
		DepletionData depletionData = null;
		int count = DepletionInfo.Count;
		for (int i = 0; i < count; i++)
		{
			DepletionData depletionData2 = DepletionInfo[i];
			if (depletionData2.PlanetId == planetId && depletionData2.ResourceName == resource)
			{
				depletionData = depletionData2;
				break;
			}
		}
		if (depletionData == null)
		{
			return null;
		}
		DepletionNode result = null;
		int count2 = depletionData.DepletionNodes.Count;
		for (int j = 0; j < count2; j++)
		{
			DepletionNode depletionNode = depletionData.DepletionNodes[j];
			if (depletionNode.Int32_0 == x && depletionNode.Int32_1 == y)
			{
				result = depletionNode;
				break;
			}
		}
		return result;
	}

	public float GetDepletionNodeValue(int planetId, string resource, int x, int y)
	{
		return GetDepletionNode(planetId, resource, x, y)?.Value ?? 1f;
	}

	public bool IsBiomeUnlocked(int planetId, string biomeName)
	{
		int count = BiomeLockInfo.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				BiomeLockData biomeLockData = BiomeLockInfo[num];
				if (biomeLockData.PlanetId == planetId && biomeLockData.BiomeName == biomeName)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool IsPlanetScanned(int planetId)
	{
		int count = PlanetScanInfo.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (PlanetScanInfo[num].PlanetId == planetId)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public void SetDepletionNodeValue(int planetId, string resource, int x, int y, float value)
	{
		DepletionData depletionData = null;
		int count = DepletionInfo.Count;
		for (int i = 0; i < count; i++)
		{
			DepletionData depletionData2 = DepletionInfo[i];
			if (depletionData2.PlanetId == planetId && depletionData2.ResourceName == resource)
			{
				depletionData = depletionData2;
				break;
			}
		}
		if (depletionData == null)
		{
			depletionData = new DepletionData();
			depletionData.PlanetId = planetId;
			depletionData.ResourceName = resource;
			DepletionInfo.Add(depletionData);
		}
		DepletionNode depletionNode = GetDepletionNode(planetId, resource, x, y);
		if (depletionNode == null)
		{
			depletionNode = new DepletionNode();
			depletionNode.Int32_0 = x;
			depletionNode.Int32_1 = y;
			depletionData.DepletionNodes.Add(depletionNode);
		}
		depletionNode.Value = value;
		depletionNode.LastUpdate = Planetarium.GetUniversalTime();
		ResourceScenario.Instance.gameSettings.SaveDepletionNodeValue(planetId, resource, x, y, value);
	}

	public void UnlockBiome(int planetId, string biomeName)
	{
		if (!IsBiomeUnlocked(planetId, biomeName))
		{
			BiomeLockData item = new BiomeLockData
			{
				PlanetId = planetId,
				BiomeName = biomeName
			};
			BiomeLockInfo.Add(item);
			ResourceScenario.Instance.gameSettings.SaveBiomeUnlockNode(planetId, biomeName);
		}
	}

	public void UnlockPlanet(int planetId)
	{
		if (!IsPlanetScanned(planetId))
		{
			PlanetScanData item = new PlanetScanData
			{
				PlanetId = planetId
			};
			PlanetScanInfo.Add(item);
			ResourceScenario.Instance.gameSettings.SavePlanetUnlockNode(planetId);
		}
	}

	public List<string> FetchAllResourceNames(HarvestTypes harvest)
	{
		List<string> list = new List<string>();
		list.AddRange(GetCacheNames(ResourceCache.Instance.PlanetaryResources, harvest));
		list.AddRange(GetCacheNames(ResourceCache.Instance.BiomeResources, harvest));
		list.AddRange(GetCacheNames(ResourceCache.Instance.GlobalResources, harvest));
		return DeDuplicate(list);
	}

	public static List<string> DeDuplicate(List<string> input)
	{
		int count = input.Count;
		List<string> list = new List<string>();
		for (int i = 0; i < count; i++)
		{
			if (!list.Contains(input[i]))
			{
				list.Add(input[i]);
			}
		}
		return list;
	}

	public IEnumerable<string> GetCacheNames(List<ResourceData> resList, HarvestTypes harvest)
	{
		List<string> list = new List<string>();
		int count = resList.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceData resourceData = resList[i];
			if (resourceData.ResourceType == (int)harvest)
			{
				list.Add(resourceData.ResourceName);
			}
		}
		return list;
	}

	public DistributionData GetBestResourceData(List<ResourceData> configs)
	{
		try
		{
			DistributionData distributionData = new DistributionData
			{
				Variance = configs[0].Distribution.Variance,
				Dispersal = configs[0].Distribution.Dispersal,
				PresenceChance = configs[0].Distribution.PresenceChance,
				MinAbundance = configs[0].Distribution.MinAbundance,
				MaxAbundance = configs[0].Distribution.MaxAbundance
			};
			int count = configs.Count;
			for (int i = 0; i < count; i++)
			{
				DistributionData distribution = configs[i].Distribution;
				if (distribution.PresenceChance > 0f && distribution.PresenceChance > distributionData.PresenceChance)
				{
					distributionData.PresenceChance = distribution.PresenceChance;
				}
				if (distribution.MinAbundance > 0f && distribution.MinAbundance < distributionData.MinAbundance)
				{
					distributionData.MinAbundance = distribution.MinAbundance;
				}
				if (distribution.MaxAbundance > 0f && distribution.MaxAbundance > distributionData.MaxAbundance)
				{
					distributionData.MaxAbundance = distribution.MaxAbundance;
				}
				if (distribution.Variance > 0f && distribution.Variance > distributionData.Variance)
				{
					distributionData.Variance = distribution.Variance;
				}
				if (distribution.Dispersal > 0f && distribution.Dispersal > distributionData.Dispersal)
				{
					distributionData.Dispersal = distribution.Dispersal;
				}
				if (distribution.MinAltitude > 0f && distribution.MinAltitude < distributionData.MinAltitude)
				{
					distributionData.MinAltitude = distribution.MinAltitude;
				}
				if (distribution.MaxAltitude > 0f && distribution.MaxAltitude > distributionData.MaxAltitude)
				{
					distributionData.MaxAltitude = distribution.MaxAltitude;
				}
				if (distribution.MinRange > 0f && distribution.MinRange < distributionData.MinRange)
				{
					distributionData.MinRange = distribution.MinRange;
				}
				if (distribution.MaxRange > 0f && distribution.MaxRange > distributionData.MaxRange)
				{
					distributionData.MaxRange = distribution.MaxRange;
				}
			}
			return distributionData;
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - ResourceMap_GetBestResourceData - " + ex);
			return new DistributionData();
		}
	}

	public int GenerateAbundanceSeed(AbundanceRequest request, string biomeName)
	{
		CelestialBody celestialBody = null;
		int count = FlightGlobals.Bodies.Count;
		for (int i = 0; i < count; i++)
		{
			if (FlightGlobals.Bodies[i].flightGlobalsIndex == request.BodyId)
			{
				celestialBody = FlightGlobals.Bodies[i];
				break;
			}
		}
		if (celestialBody == null)
		{
			return 0;
		}
		return ResourceScenario.Instance.gameSettings.Seed * (request.BodyId + 1) * (request.ResourceName.Length * ((request.ResourceName.Length > 1) ? request.ResourceName[1] : '\0')) * (celestialBody.bodyName.Length * ((celestialBody.bodyName.Length > 1) ? celestialBody.bodyName[1] : '\0')) + biomeName.Length * ((biomeName.Length > 1) ? biomeName[1] : '\0') * (int)(request.ResourceType + 1);
	}

	public string DetermineBiomeName(AbundanceRequest request)
	{
		double lat = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(request.Latitude));
		double lon = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(request.Longitude));
		CelestialBody body = null;
		int count = FlightGlobals.Bodies.Count;
		for (int i = 0; i < count; i++)
		{
			if (FlightGlobals.Bodies[i].flightGlobalsIndex == request.BodyId)
			{
				body = FlightGlobals.Bodies[i];
				break;
			}
		}
		if (!string.IsNullOrEmpty(request.BiomeName))
		{
			return request.BiomeName;
		}
		CBAttributeMapSO.MapAttribute biome = ResourceUtilities.GetBiome(lat, lon, body);
		if (biome != null)
		{
			return biome.name;
		}
		return GetDefaultSituation(request.ResourceType);
	}

	public float GetAbundanceFromCache(AbundanceRequest request, string biomeName)
	{
		ResourceCache.AbundanceSummary abundanceSummary = null;
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary2 = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary2.BodyId == request.BodyId && abundanceSummary2.ResourceName == request.ResourceName && abundanceSummary2.HarvestType == request.ResourceType && abundanceSummary2.BiomeName == biomeName)
			{
				abundanceSummary = abundanceSummary2;
				break;
			}
		}
		return abundanceSummary?.Abundance ?? 2f;
	}

	public float GetAbundance(AbundanceRequest request)
	{
		try
		{
			double num = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(request.Latitude));
			double num2 = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(request.Longitude));
			CelestialBody celestialBody = null;
			int count = FlightGlobals.Bodies.Count;
			for (int i = 0; i < count; i++)
			{
				if (FlightGlobals.Bodies[i].flightGlobalsIndex == request.BodyId)
				{
					celestialBody = FlightGlobals.Bodies[i];
					break;
				}
			}
			string text = GetDefaultSituation(request.ResourceType);
			bool flag = true;
			if (request.ResourceType == HarvestTypes.Planetary)
			{
				text = DetermineBiomeName(request);
				flag = Instance.IsBiomeUnlocked(request.BodyId, text);
				if (!request.CheckForLock)
				{
					flag = true;
				}
				if (request.ExcludeVariance)
				{
					flag = false;
				}
			}
			int num3 = GenerateAbundanceSeed(request, text);
			int count2 = ResourceCache.Instance.BiomeResources.Count;
			biomeConfigs.Clear();
			for (int j = 0; j < count2; j++)
			{
				ResourceData resourceData = ResourceCache.Instance.BiomeResources[j];
				if (resourceData.PlanetName == celestialBody.bodyName && resourceData.BiomeName == text && resourceData.ResourceName == request.ResourceName && resourceData.ResourceType == (int)request.ResourceType)
				{
					biomeConfigs.Add(resourceData);
				}
			}
			int count3 = ResourceCache.Instance.PlanetaryResources.Count;
			planetConfigs.Clear();
			for (int k = 0; k < count3; k++)
			{
				ResourceData resourceData2 = ResourceCache.Instance.PlanetaryResources[k];
				if (resourceData2.PlanetName == celestialBody.bodyName && resourceData2.ResourceName == request.ResourceName && resourceData2.ResourceType == (int)request.ResourceType)
				{
					planetConfigs.Add(resourceData2);
				}
			}
			int count4 = ResourceCache.Instance.GlobalResources.Count;
			globalConfigs.Clear();
			for (int l = 0; l < count4; l++)
			{
				ResourceData resourceData3 = ResourceCache.Instance.GlobalResources[l];
				if (resourceData3.ResourceName == request.ResourceName && resourceData3.ResourceType == (int)request.ResourceType)
				{
					globalConfigs.Add(resourceData3);
				}
			}
			DistributionData bestResourceData;
			if (biomeConfigs.Count > 0)
			{
				bestResourceData = GetBestResourceData(biomeConfigs);
				num3 *= 2;
			}
			else if (planetConfigs.Count > 0)
			{
				bestResourceData = GetBestResourceData(planetConfigs);
				num3 *= 3;
			}
			else
			{
				if (globalConfigs.Count <= 0)
				{
					return 0f;
				}
				bestResourceData = GetBestResourceData(globalConfigs);
				num3 *= 4;
			}
			if (!seedsCache.TryGetValue(num3, out var value))
			{
				KSPRandom kSPRandom = new KSPRandom(num3);
				value = new List<double>(5);
				value.Add(kSPRandom.NextDouble());
				value.Add(kSPRandom.NextDouble());
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.Next(100));
				value.Add(kSPRandom.NextDouble());
				value.Add(kSPRandom.NextDouble());
				seedsCache.Add(num3, value);
			}
			if (!((float)(int)(value[0] * 100.0) <= bestResourceData.PresenceChance * ResourceUtilities.GetDifficultyLevel()))
			{
				return 0f;
			}
			int num4 = (int)(bestResourceData.MinAbundance * 1000f);
			int num5 = (int)(bestResourceData.MaxAbundance * 1000f);
			if (num4 > num5)
			{
				num5 = num4 + 1;
			}
			int num6 = num5 - num4;
			float num7 = (float)((int)(value[1] * (double)num6) + num4) / 1000f * ResourceUtilities.GetDifficultyLevel();
			if (flag && !request.ExcludeVariance && request.ResourceType == HarvestTypes.Planetary)
			{
				for (int m = 0; m < 8; m++)
				{
					noiseSeed[m] = (int)value[2 + m];
				}
				spx.SetSeed(noiseSeed);
				float x = (float)num * bestResourceData.Dispersal;
				float y = (float)num2 * bestResourceData.Dispersal;
				float z = (float)(value[10] * 100.0) / 100f;
				float num8 = spx.noise(x, y, z);
				if (request.ResourceType != HarvestTypes.Exospheric)
				{
					float num9 = num8 * (bestResourceData.Variance / 100f);
					num7 += num9;
					if (num7 < 0f)
					{
						num7 = 0f;
					}
				}
			}
			else if (request.ResourceType == HarvestTypes.Planetary)
			{
				num7 /= 1.5f;
			}
			if ((request.ResourceType == HarvestTypes.Atmospheric || request.ResourceType == HarvestTypes.Exospheric) && bestResourceData.HasVariableAltitude())
			{
				double radius = celestialBody.Radius;
				double num10 = (radius * (double)bestResourceData.MinAltitude + radius * (double)bestResourceData.MaxAltitude) / 2.0;
				num6 = (int)(radius * (double)bestResourceData.MaxRange) - (int)(radius * (double)bestResourceData.MinRange);
				int num11 = (int)(value[11] * (double)num6) + (int)(radius * (double)bestResourceData.MinRange);
				double num12 = Math.Abs(num10 - request.Altitude) / (double)num11;
				double num13 = 1.0 - num12;
				num7 *= (float)num13;
			}
			if ((double)num7 <= 1E-09)
			{
				return 0f;
			}
			return num7 / 100f;
		}
		catch (Exception ex)
		{
			Debug.LogError("[RESOURCES] - Error in - ResourceMap_GetAbundance - " + ex);
			return 0f;
		}
	}

	public static string GetDefaultSituation(HarvestTypes type)
	{
		return type switch
		{
			HarvestTypes.Planetary => "LANDED", 
			HarvestTypes.Oceanic => "SPLASHED", 
			HarvestTypes.Atmospheric => "FLYING", 
			_ => "ORBITING", 
		};
	}

	public Vector2 GetDepletionNode(double latitude, double longitude)
	{
		double value = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(latitude));
		double value2 = ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(longitude));
		double num = Math.Round(value, 0);
		double num2 = Math.Round(value2, 0);
		return new Vector2((float)num, (float)num2);
	}

	public PlanetaryResource GetResourceByName(string resName, CelestialBody body, HarvestTypes harvest)
	{
		PlanetaryResource planetaryResource = new PlanetaryResource();
		if (!Instance.IsPlanetScanned(body.flightGlobalsIndex))
		{
			return planetaryResource;
		}
		List<ResourceCache.AbundanceSummary> list = new List<ResourceCache.AbundanceSummary>();
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary.BodyId == body.flightGlobalsIndex && abundanceSummary.HarvestType == harvest)
			{
				list.Add(abundanceSummary);
			}
		}
		List<ResourceCache.AbundanceSummary> list2 = new List<ResourceCache.AbundanceSummary>();
		int count2 = list.Count;
		for (int j = 0; j < count2; j++)
		{
			if (list[j].ResourceName == resName)
			{
				list2.Add(list[j]);
			}
		}
		if (list2.Count > 0)
		{
			float num = 0f;
			int count3 = list2.Count;
			for (int k = 0; k < count3; k++)
			{
				num += list2[k].Abundance;
			}
			float num2 = num / (float)count3;
			if (num2 > 0f)
			{
				PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(resName);
				planetaryResource.fraction = num2;
				planetaryResource.resourceID = definition.id;
				planetaryResource.resourceName = resName;
			}
		}
		return planetaryResource;
	}

	public List<PlanetaryResource> GetResourceItemList(HarvestTypes harvest, CelestialBody body)
	{
		List<PlanetaryResource> list = new List<PlanetaryResource>();
		if (!Instance.IsPlanetScanned(body.flightGlobalsIndex))
		{
			return list;
		}
		List<ResourceCache.AbundanceSummary> list2 = new List<ResourceCache.AbundanceSummary>();
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary.BodyId == body.flightGlobalsIndex && abundanceSummary.HarvestType == harvest)
			{
				list2.Add(abundanceSummary);
			}
		}
		List<string> list3 = FetchAllResourceNames(harvest);
		int count2 = list3.Count;
		for (int j = 0; j < count2; j++)
		{
			string text = list3[j];
			List<ResourceCache.AbundanceSummary> list4 = new List<ResourceCache.AbundanceSummary>();
			int count3 = list2.Count;
			for (int k = 0; k < count3; k++)
			{
				if (list2[k].ResourceName == text)
				{
					list4.Add(list2[k]);
				}
			}
			if (list4.Count > 0)
			{
				float num = 0f;
				int count4 = list4.Count;
				for (int l = 0; l < count4; l++)
				{
					num += list4[l].Abundance;
				}
				float num2 = num / (float)count4;
				if (num2 > 0f)
				{
					PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(text);
					PlanetaryResource planetaryResource = new PlanetaryResource();
					planetaryResource.fraction = num2;
					planetaryResource.resourceID = definition.id;
					planetaryResource.resourceName = text;
					list.Add(planetaryResource);
				}
			}
		}
		return list;
	}
}
