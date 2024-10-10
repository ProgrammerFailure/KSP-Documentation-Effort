using System;
using System.ComponentModel;
using Expansions.Missions.Editor;
using UnityEngine;

namespace Expansions.Missions;

[Serializable]
public class Asteroid : IConfigNode
{
	public enum AsteroidType
	{
		[Description("#autoLOC_8100110")]
		Regular,
		[Description("#autoLOC_8100111")]
		Glimmeroid
	}

	[MEGUI_InputField(tabStop = true, order = 1, guiName = "#autoLOC_8100112")]
	public string name;

	public uint seed;

	[MEGUI_Dropdown(order = 2, gapDisplay = true, guiName = "#autoLOC_8100113")]
	public AsteroidType asteroidType;

	[MEGUI_Dropdown(order = 3, gapDisplay = true, guiName = "#autoLOC_8100114")]
	public UntrackedObjectClass asteroidClass;

	public uint persistentId;

	public Asteroid()
	{
		name = "Asteroid";
		asteroidClass = UntrackedObjectClass.const_2;
		asteroidType = AsteroidType.Regular;
		persistentId = 0u;
		seed = 0u;
	}

	public void Randomize()
	{
		name = DiscoverableObjectsUtil.GenerateAsteroidName();
		persistentId = FlightGlobals.GetUniquepersistentId();
		seed = GetAsteroidSeed();
	}

	public void Load(ConfigNode node)
	{
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "persistentId":
				uint.TryParse(value.value, out persistentId);
				break;
			case "asteroidType":
				asteroidType = (AsteroidType)Enum.Parse(typeof(AsteroidType), value.value);
				break;
			case "asteroidClass":
				asteroidClass = (UntrackedObjectClass)Enum.Parse(typeof(UntrackedObjectClass), value.value);
				break;
			case "asteroidSeed":
				uint.TryParse(value.value, out seed);
				break;
			case "asteroidName":
				name = value.value;
				break;
			}
		}
	}

	public GameObject SetGAPAsteroid()
	{
		return MissionsUtils.MEPrefab("Prefabs/GAP_ProceduralAsteroid.prefab");
	}

	public void OnGAPPrefabInstantiated(GameObject prefabInstance)
	{
		prefabInstance.GetComponent<GAPProceduralAsteroid>().Setup((int)seed, asteroidType == AsteroidType.Glimmeroid, asteroidClass);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("asteroidName", name);
		node.AddValue("asteroidSeed", seed);
		node.AddValue("asteroidClass", asteroidClass);
		node.AddValue("asteroidType", asteroidType);
		node.AddValue("persistentId", persistentId);
	}

	public static uint GetAsteroidSeed()
	{
		return (uint)UnityEngine.Random.Range(0, int.MaxValue);
	}
}
