using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Expansions.Missions.Editor;
using UnityEngine;

public static class ResourceUtilities
{
	public const double FLOAT_TOLERANCE = 1E-09;

	public const double SECONDS_PER_TICK = 0.02;

	public static CBAttributeMapSO.MapAttribute GetBiome(double lat, double lon, CelestialBody body)
	{
		try
		{
			return body.BiomeMap.GetAtt(lat, lon);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static T LoadNodeProperties<T>(ConfigNode node) where T : new()
	{
		Type typeFromHandle = typeof(T);
		T val = new T();
		int count = node.values.Count;
		for (int i = 0; i < count; i++)
		{
			ConfigNode.Value value = node.values[i];
			PropertyInfo property = typeFromHandle.GetProperty(value.name);
			if (property != null)
			{
				property.SetValue(val, Convert.ChangeType(value.value, property.PropertyType), null);
			}
		}
		return val;
	}

	public static List<ResourceData> ImportConfigNodeList(ConfigNode[] nodes)
	{
		List<ResourceData> list = new List<ResourceData>();
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			ResourceData resourceData = LoadNodeProperties<ResourceData>(nodes[i]);
			ConfigNode node = nodes[i].GetNode("Distribution");
			if (node != null)
			{
				DistributionData distribution = LoadNodeProperties<DistributionData>(node);
				resourceData.Distribution = distribution;
			}
			list.Add(resourceData);
		}
		return list;
	}

	public static List<DepletionData> ImportDepletionNodeList(ConfigNode[] nodes)
	{
		List<DepletionData> list = new List<DepletionData>();
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			DepletionData depletionData = LoadNodeProperties<DepletionData>(nodes[i]);
			ConfigNode[] nodes2 = nodes[i].GetNodes("DEPLETION_NODE");
			if (nodes2 != null)
			{
				int num2 = nodes2.Length;
				for (int j = 0; j < num2; j++)
				{
					DepletionNode item = LoadNodeProperties<DepletionNode>(nodes2[j]);
					depletionData.DepletionNodes.Add(item);
				}
			}
			list.Add(depletionData);
		}
		return list;
	}

	public static List<BiomeLockData> ImportBiomeLockNodeList(ConfigNode[] nodes)
	{
		List<BiomeLockData> list = new List<BiomeLockData>();
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			BiomeLockData item = LoadNodeProperties<BiomeLockData>(nodes[i]);
			list.Add(item);
		}
		return list;
	}

	public static List<PlanetScanData> ImportPlanetScanNodeList(ConfigNode[] nodes)
	{
		List<PlanetScanData> list = new List<PlanetScanData>();
		int num = nodes.Length;
		for (int i = 0; i < num; i++)
		{
			PlanetScanData item = LoadNodeProperties<PlanetScanData>(nodes[i]);
			list.Add(item);
		}
		return list;
	}

	public static double GetValue(ConfigNode node, string name, double curVal)
	{
		if (node.HasValue(name) && double.TryParse(node.GetValue(name), out var result))
		{
			return result;
		}
		return curVal;
	}

	public static double GetMaxDeltaTime()
	{
		if (ResourceScenario.Instance == null)
		{
			return ResourceGameSettings.defaultMaxDeltaTime;
		}
		return ResourceScenario.Instance.gameSettings.MaxDeltaTime;
	}

	public static double GetAltitude(Vessel v)
	{
		return v.radarAltitude;
	}

	public static T DeepClone<T>(T obj)
	{
		using MemoryStream memoryStream = new MemoryStream();
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(memoryStream, obj);
		memoryStream.Position = 0L;
		return (T)binaryFormatter.Deserialize(memoryStream);
	}

	public static double Deg2Rad(double degrees)
	{
		return degrees * Math.PI / 180.0;
	}

	public static double Rad2Lat(double radians)
	{
		double num = radians % (Math.PI * 2.0);
		if (num < 0.0)
		{
			num = Math.PI * 2.0 + num;
		}
		double num2 = num % Math.PI;
		if (num2 > Math.PI / 2.0)
		{
			num2 = Math.PI - num2;
		}
		num = ((!(num > Math.PI)) ? num2 : (0.0 - num2));
		return num / Math.PI * 180.0;
	}

	public static double Rad2Lon(double radians)
	{
		double num = radians % (Math.PI * 2.0);
		if (num < 0.0)
		{
			num = Math.PI * 2.0 + num;
		}
		double num2 = num % (Math.PI * 2.0);
		if (num2 > Math.PI)
		{
			num2 = Math.PI * 2.0 - num2;
		}
		num = ((!(num > Math.PI)) ? num2 : (0.0 - num2));
		return num / Math.PI * 180.0;
	}

	public static Color HSL2RGB(double h, double sl, double l, float alpha)
	{
		double num = l;
		double num2 = l;
		double num3 = l;
		double num4 = ((l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl));
		if (num4 > 0.0)
		{
			double num5 = l + l - num4;
			double num6 = (num4 - num5) / num4;
			h *= 6.0;
			int num7 = (int)h;
			double num8 = h - (double)num7;
			double num9 = num4 * num6 * num8;
			double num10 = num5 + num9;
			double num11 = num4 - num9;
			switch (num7)
			{
			case 0:
				num = num4;
				num2 = num10;
				num3 = num5;
				break;
			case 1:
				num = num11;
				num2 = num4;
				num3 = num5;
				break;
			case 2:
				num = num5;
				num2 = num4;
				num3 = num10;
				break;
			case 3:
				num = num5;
				num2 = num11;
				num3 = num4;
				break;
			case 4:
				num = num10;
				num2 = num5;
				num3 = num4;
				break;
			case 5:
				num = num4;
				num2 = num5;
				num3 = num11;
				break;
			}
		}
		return new Color((float)num, (float)num2, (float)num3, alpha);
	}

	public static double clampLat(double lat)
	{
		return (lat + 180.0 + 90.0) % 180.0 - 90.0;
	}

	public static double clampLon(double lon)
	{
		return (lon + 360.0 + 180.0) % 360.0 - 180.0;
	}

	public static float GetDifficultyLevel()
	{
		if (MissionEditorLogic.Instance != null)
		{
			return MissionEditorLogic.Instance.EditorMission.situation.gameParameters.Difficulty.ResourceAbundance;
		}
		return HighLogic.CurrentGame.Parameters.Difficulty.ResourceAbundance;
	}
}
