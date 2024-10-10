using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class OverlayGenerator : MonoBehaviour
{
	public List<PartResourceDefinition> _resourceList;

	public PartResourceDefinition _displayResource;

	public CelestialBody _displayBody;

	public KSPRandom _rng;

	public static readonly Dictionary<int, Texture2D> textureDict = new Dictionary<int, Texture2D>(3);

	public static readonly Dictionary<int, Color32[]> resetDict = new Dictionary<int, Color32[]>(3);

	public static OverlayGenerator instance;

	public PartResourceDefinition DisplayResource
	{
		get
		{
			if (_displayResource != null)
			{
				return _displayResource;
			}
			if (ResourceList.Count == 0)
			{
				return null;
			}
			_displayResource = ResourceList[0];
			return _displayResource;
		}
		set
		{
			_displayResource = value;
		}
	}

	public bool IsActive { get; set; }

	public MapDisplayTypes DisplayMode { get; set; }

	public int Cutoff { get; set; }

	public HarvestTypes DisplayResourceType { get; set; }

	public int OverlayStyle { get; set; }

	public List<PartResourceDefinition> ResourceList => _resourceList ?? (_resourceList = LoadResourceList());

	public bool IsMapView { get; set; }

	public CelestialBody DisplayBody
	{
		get
		{
			return _displayBody;
		}
		set
		{
			_displayBody = value;
		}
	}

	public KSPRandom KSPRandom_0 => _rng ?? (_rng = new KSPRandom(ResourceScenario.Instance.gameSettings.Seed));

	public static OverlayGenerator Instance => instance ?? (instance = new GameObject("OverlayGenerator").AddComponent<OverlayGenerator>());

	public static Texture2D GetTexture(int width, int height)
	{
		Color32[] value2;
		if (!textureDict.TryGetValue(height, out var value))
		{
			value = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
			textureDict.Add(height, value);
			resetDict.Add(height, value.GetPixels32());
		}
		else if (resetDict.TryGetValue(height, out value2))
		{
			value.SetPixels32(value2);
		}
		return value;
	}

	public void ResetRNG()
	{
		_rng = new KSPRandom(ResourceScenario.Instance.gameSettings.Seed);
	}

	public void Awake()
	{
		GameEvents.onGameStateLoad.Add(OnGameLoaded);
		GameEvents.onPlanetariumTargetChanged.Add(OnMapFocusChange);
		MapView.OnEnterMapView = (Callback)Delegate.Combine(MapView.OnEnterMapView, new Callback(OnEnterMapView));
		MapView.OnExitMapView = (Callback)Delegate.Combine(MapView.OnExitMapView, new Callback(OnExitMapView));
	}

	public void OnDestroy()
	{
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void OnExitMapView()
	{
		IsMapView = false;
	}

	public void OnEnterMapView()
	{
		IsMapView = true;
	}

	public void OnGameLoaded(ConfigNode data)
	{
		int num = 0;
		if (Instance.DisplayBody != null)
		{
			num = Instance.DisplayBody.flightGlobalsIndex;
		}
		int num2 = 0;
		if (FlightGlobals.currentMainBody != null)
		{
			num2 = FlightGlobals.currentMainBody.flightGlobalsIndex;
		}
		if (num != num2)
		{
			Instance.ClearDisplay();
			Instance.DisplayBody = FlightGlobals.currentMainBody;
		}
		if (Instance.OverlayStyle == 0)
		{
			Instance.OverlayStyle = ResourceSetup.Instance.ResConfig.OverlayStyle;
		}
	}

	public void OnMapFocusChange(MapObject target)
	{
		if (!(target == null) && (target.type == MapObject.ObjectType.Vessel || target.type == MapObject.ObjectType.CelestialBody))
		{
			int num = 0;
			if (Instance.DisplayBody != null)
			{
				num = Instance.DisplayBody.flightGlobalsIndex;
			}
			CelestialBody celestialBody = target.celestialBody ?? target.vessel.mainBody;
			int num2 = 0;
			if (celestialBody != null)
			{
				num2 = celestialBody.flightGlobalsIndex;
			}
			if (num != num2)
			{
				Instance.ClearDisplay();
				Instance.DisplayBody = celestialBody;
			}
			if (Instance.OverlayStyle == 0)
			{
				Instance.OverlayStyle = ResourceSetup.Instance.ResConfig.OverlayStyle;
			}
		}
	}

	public Vector2 GenerateScanPoint(float lon, float lat)
	{
		Vector2 result = new Vector2(lon, lat);
		result.y = lat - 90f;
		if (lon <= 180f)
		{
			result.x = 180f - lon;
		}
		else
		{
			result.x = (lon - 180f) * -1f;
		}
		result.x -= 90f;
		if (result.x < -180f)
		{
			result.x += 360f;
		}
		result.y = (float)ResourceUtilities.Deg2Rad(ResourceUtilities.clampLat(result.y));
		result.x = (float)ResourceUtilities.Deg2Rad(ResourceUtilities.clampLon(result.x));
		return result;
	}

	public Texture2D CreateTexture(int height, bool checkForLock)
	{
		ResetRNG();
		int num = height * 2;
		int num2 = (int)Math.Pow(2.0, OverlaySetup.Instance.OverlayConfig.InterpolationLevel);
		float num3 = 360f / (float)num;
		float num4 = 180f / (float)height;
		Color color = FetchOverlayColor();
		float num5 = GetMaxAbundance();
		float num6 = GetMinAbundance();
		if ((double)Math.Abs(num5 - num6) < 1E-09)
		{
			num5 *= 1.1f;
			num6 *= 0.9f;
		}
		Texture2D texture = GetTexture(num, height);
		texture.filterMode = (FilterMode)OverlaySetup.Instance.OverlayConfig.FilterMode;
		float loColor = OverlaySetup.Instance.OverlayConfig.LoColor;
		float hiColor = OverlaySetup.Instance.OverlayConfig.HiColor;
		Color startColor = new Color(color.r * loColor, color.g * loColor, color.b * loColor, OverlaySetup.Instance.OverlayConfig.LoOpacity);
		Color endColor = new Color(color.r * hiColor, color.g * hiColor, color.b * hiColor, OverlaySetup.Instance.OverlayConfig.HiOpacity);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < num; j++)
			{
				float num7 = 0f;
				Color color2 = new Color(0f, 0f, 0f, 0f);
				if (j % num2 == 0 && i % num2 == 0)
				{
					Vector2 vector = GenerateScanPoint(num3 * (float)j, num4 * (float)i);
					float y = vector.y;
					float x = vector.x;
					num7 = GetPoint(new Vector3(x, y, 0f), checkForLock, num5, num6);
					color2 = GetColor(num7, startColor, endColor);
				}
				texture.SetPixel(j, i, color2);
			}
		}
		Vector2 vector2 = GenerateScanPoint(89f, 89f);
		float point = GetPoint(new Vector3(vector2.x, vector2.y, 0f), checkForLock, num5, num6, bypassCache: true);
		for (int k = 0; k < OverlaySetup.Instance.OverlayConfig.CapPixels; k++)
		{
			for (int l = 0; l < texture.width; l++)
			{
				Color color3 = GetColor(point, startColor, endColor);
				texture.SetPixel(l, k, color3);
				texture.SetPixel(l, texture.height - k, color3);
			}
		}
		for (int num8 = num2 / 2; num8 > 1; num8 /= 2)
		{
			Interpolate(texture, fuzzyEdges: true, num8, num8, num8);
			Interpolate(texture, fuzzyEdges: true, 0, num8, num8);
			Interpolate(texture, fuzzyEdges: true, num8, 0, num8);
		}
		if (Instance.OverlayStyle == 1)
		{
			Interpolate(texture, fuzzyEdges: true, 1, 0, 1, isEmpty: false, xOnly: true);
			AverageLine(texture);
		}
		if (Instance.OverlayStyle == 2)
		{
			AverageDots(texture);
		}
		if (Instance.OverlayStyle == 3)
		{
			Interpolate(texture, fuzzyEdges: true, 1, 1, 1);
			Interpolate(texture, fuzzyEdges: true, 0, 1, 1);
			Interpolate(texture, fuzzyEdges: true, 1, 0, 1);
		}
		texture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		return texture;
	}

	public Color GetColor(float point, Color startColor, Color endColor)
	{
		Color result = new Color(0f, 0f, 0f, 0f);
		if ((double)point > 1E-09)
		{
			float a = (OverlaySetup.Instance.OverlayConfig.HiOpacity + OverlaySetup.Instance.OverlayConfig.HiOpacity) / 2f;
			result = Color.Lerp(startColor, endColor, point);
			if (DisplayMode == MapDisplayTypes.HeatMapBlue)
			{
				result = new Color(2f * point, 0f, 2f * (1f - point), a);
			}
			if (DisplayMode == MapDisplayTypes.HeatMapGreen)
			{
				result = new Color(2f * point, 2f * (1f - point), 0f, a);
			}
		}
		return result;
	}

	public void AverageLine(Texture2D tex)
	{
		float gridWeight = OverlaySetup.Instance.OverlayConfig.GridWeight;
		for (int i = 0; i < tex.height; i++)
		{
			for (int j = 0; j < tex.width; j++)
			{
				if (i % 2 != 0)
				{
					Color pixel = tex.GetPixel(j, i + 1);
					tex.SetPixel(j, i, new Color(pixel.r * gridWeight, pixel.g * gridWeight, pixel.b * gridWeight, pixel.a * gridWeight));
				}
			}
		}
	}

	public void AverageDots(Texture2D tex)
	{
		float gridWeight = OverlaySetup.Instance.OverlayConfig.GridWeight;
		for (int i = 0; i < tex.height; i++)
		{
			for (int j = 0; j < tex.width; j++)
			{
				if (i % 2 != 0 || j % 2 != 0)
				{
					Color pixel = tex.GetPixel(j, i + 1);
					tex.SetPixel(j, i, new Color(pixel.r * gridWeight, pixel.g * gridWeight, pixel.b * gridWeight, pixel.a * gridWeight));
				}
			}
		}
	}

	public void Interpolate(Texture2D tex, bool fuzzyEdges, int xOff, int yOff, int step, bool isEmpty = false, bool xOnly = false)
	{
		for (int i = yOff; i < tex.height + yOff; i += 2 * step)
		{
			for (int j = xOff; j < tex.width + xOff; j += 2 * step)
			{
				int num = j - step;
				if (num < 0)
				{
					num += tex.width;
				}
				int num2 = j + step;
				if (num2 >= tex.width)
				{
					num2 -= tex.width;
				}
				int num3 = i - step;
				if (num3 < 0)
				{
					num3 = 0;
				}
				int num4 = i + step;
				if (num4 >= tex.height)
				{
					num4 = tex.height - 1;
				}
				int lerpStep = 0;
				if (fuzzyEdges)
				{
					lerpStep = step * 2;
				}
				Color a = Color.Lerp(tex.GetPixel(j, num3), tex.GetPixel(j, num4), GetLerp(lerpStep));
				Color color = Color.Lerp(tex.GetPixel(num2, i), tex.GetPixel(num, i), GetLerp(lerpStep));
				if (xOff == yOff)
				{
					a = Color.Lerp(tex.GetPixel(num, num3), tex.GetPixel(num2, num4), GetLerp(lerpStep));
					color = Color.Lerp(tex.GetPixel(num2, num3), tex.GetPixel(num, num4), GetLerp(lerpStep));
				}
				if (xOnly)
				{
					a = color;
				}
				Color color2 = Color.Lerp(a, color, GetLerp(lerpStep));
				if (isEmpty)
				{
					color2 = new Color(0f, 0f, 0f, 0f);
				}
				tex.SetPixel(j, i, color2);
			}
		}
	}

	public List<PartResourceDefinition> LoadResourceList()
	{
		List<string> list = ResourceMap.Instance.FetchAllResourceNames(DisplayResourceType);
		List<PartResourceDefinition> list2 = new List<PartResourceDefinition>();
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			PartResourceDefinition definition = PartResourceLibrary.Instance.GetDefinition(list[i]);
			list2.Add(definition);
		}
		return list2;
	}

	public float GetLerp(int lerpStep)
	{
		if (lerpStep == 0)
		{
			return 0.5f;
		}
		return (float)lerpStep / 100f + (float)KSPRandom_0.Next(100 - lerpStep * 2) / 100f;
	}

	public void GenerateOverlay(bool checkForLock)
	{
		if (DisplayBody != null)
		{
			if (ResourceMap.Instance.IsPlanetScanned(_displayBody.flightGlobalsIndex))
			{
				int mapResolution = OverlaySetup.Instance.OverlayConfig.MapResolution;
				Texture2D resourceMap = CreateTexture(mapResolution, checkForLock);
				DisplayBody.SetResourceMap(resourceMap);
				IsActive = true;
			}
			else
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001070", Instance.DisplayBody.displayName), 5f, ScreenMessageStyle.UPPER_CENTER);
				ClearDisplay();
			}
		}
	}

	public void ClearDisplay()
	{
		IsActive = false;
		if (DisplayBody != null)
		{
			DisplayBody.SetResourceMap(null);
		}
	}

	public float GetMaxAbundance(PartResourceDefinition resource, int planetID, HarvestTypes harvestType)
	{
		if (resource == null)
		{
			return 0f;
		}
		List<ResourceCache.AbundanceSummary> list = new List<ResourceCache.AbundanceSummary>();
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary.BodyId == planetID && abundanceSummary.ResourceName == resource.name && abundanceSummary.HarvestType == harvestType)
			{
				list.Add(abundanceSummary);
			}
		}
		float num = 0f;
		int count2 = list.Count;
		for (int j = 0; j < count2; j++)
		{
			ResourceCache.AbundanceSummary abundanceSummary2 = list[j];
			if (abundanceSummary2.Abundance > num)
			{
				num = abundanceSummary2.Abundance;
			}
		}
		return num;
	}

	public float GetMinAbundance(PartResourceDefinition resource, int planetID, HarvestTypes harvestType)
	{
		if (resource == null)
		{
			return 0f;
		}
		List<ResourceCache.AbundanceSummary> list = new List<ResourceCache.AbundanceSummary>();
		int count = ResourceCache.Instance.AbundanceCache.Count;
		for (int i = 0; i < count; i++)
		{
			ResourceCache.AbundanceSummary abundanceSummary = ResourceCache.Instance.AbundanceCache[i];
			if (abundanceSummary.BodyId == planetID && abundanceSummary.ResourceName == resource.name && abundanceSummary.HarvestType == harvestType && (double)abundanceSummary.Abundance > 1E-09)
			{
				list.Add(abundanceSummary);
			}
		}
		if (list.Count == 0)
		{
			return 0f;
		}
		float num = 1f;
		int count2 = list.Count;
		for (int j = 0; j < count2; j++)
		{
			ResourceCache.AbundanceSummary abundanceSummary2 = list[j];
			if (abundanceSummary2.Abundance < num)
			{
				num = abundanceSummary2.Abundance;
			}
		}
		return num;
	}

	public float GetMaxAbundance()
	{
		return GetMaxAbundance(Instance.DisplayResource, Instance.DisplayBody.flightGlobalsIndex, Instance.DisplayResourceType);
	}

	public float GetMinAbundance()
	{
		return GetMinAbundance(Instance.DisplayResource, Instance.DisplayBody.flightGlobalsIndex, Instance.DisplayResourceType);
	}

	public float GetPoint(Vector3 radial, bool checkForLock, float maxAbundance, float minAbundance, bool bypassCache = false)
	{
		AbundanceRequest request = default(AbundanceRequest);
		request.Altitude = 0.0;
		request.BodyId = DisplayBody.flightGlobalsIndex;
		request.CheckForLock = checkForLock;
		request.ResourceName = _displayResource.name;
		request.ResourceType = DisplayResourceType;
		request.Longitude = ResourceUtilities.Rad2Lon(radial.x);
		request.Latitude = ResourceUtilities.Rad2Lat(radial.y);
		request.ExcludeVariance = bypassCache;
		float num = minAbundance;
		num += maxAbundance * ((float)Cutoff / 100f);
		float abundance = ResourceMap.Instance.GetAbundance(request);
		if (abundance <= num)
		{
			return 0f;
		}
		float num2 = maxAbundance - num;
		abundance -= num;
		return abundance / num2;
	}

	public Color FetchOverlayColor()
	{
		if (DisplayMode == MapDisplayTypes.Monochrome)
		{
			return DisplayResource.color;
		}
		Color color = DisplayResource.color;
		return InvertColor(color);
	}

	public Color InvertColor(Color color)
	{
		if (color == Color.white)
		{
			return Color.magenta;
		}
		Color result = color;
		result.r = 1f - result.r;
		result.g = 1f - result.g;
		result.b = 1f - result.b;
		return result;
	}

	public void FetchNextResource()
	{
		if (Instance.DisplayResource == null && Instance.ResourceList.Count > 0)
		{
			Instance.DisplayResource = Instance.ResourceList[0];
		}
		int num = ResourceList.IndexOf(Instance.DisplayResource) + 1;
		if (num == ResourceList.Count)
		{
			num = 0;
		}
		Instance.DisplayResource = ResourceList[num];
	}
}
