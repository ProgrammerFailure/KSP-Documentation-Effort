using System;
using ns9;
using UnityEngine;

public static class DiscoverableObjectsUtil
{
	public static int specialNameChance = 20;

	public static string GenerateAsteroidName()
	{
		string text = "";
		text += randomLetter(uppercase: true);
		text += randomLetter(uppercase: true);
		text += randomLetter(uppercase: true);
		text += "-";
		text += randomNumber(3);
		return Localizer.Format("#autoLOC_6001923", text);
	}

	public static char randomLetter(bool uppercase)
	{
		int num = UnityEngine.Random.Range(0, 26);
		char c = (char)(97 + num);
		if (uppercase)
		{
			return char.ToUpper(c);
		}
		return c;
	}

	public static string randomNumber(int digits)
	{
		int max = (int)Math.Pow(10.0, digits);
		string text = "";
		for (int i = 0; i < digits; i++)
		{
			text += "0";
		}
		return UnityEngine.Random.Range(0, max).ToString(text);
	}

	public static ProtoVessel SpawnAsteroid(string asteroidName, Orbit o, uint seed, UntrackedObjectClass objClass, double lifeTime, double lifeTimeMax)
	{
		ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(asteroidName, VesselType.SpaceObject, o, 0, new ConfigNode[1] { ProtoVessel.CreatePartNode("PotatoRoid", seed) }, new ConfigNode("ACTIONGROUPS"), ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Presence, objClass, lifeTime, lifeTimeMax));
		ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
		GameEvents.onAsteroidSpawned.Fire(protoVessel.vesselRef);
		return protoVessel;
	}

	public static ProtoVessel SpawnComet(string cometName, Orbit o, CometDefinition cometDef, uint seed, UntrackedObjectClass objClass, double lifeTime, double lifeTimeMax, bool optimizedCollider, float fragmentDynamicPressureModifier)
	{
		ConfigNode configNode = cometDef.CreateVesselNode(optimizedCollider, fragmentDynamicPressureModifier, hasName: false);
		ConfigNode configNode2 = ProtoVessel.CreatePartNode("PotatoComet", seed);
		uint value = 0u;
		configNode2.TryGetValue("persistentId", ref value);
		configNode.AddValue("cometPartId", value);
		ConfigNode configNode3 = new ConfigNode("VESSELMODULES");
		configNode3.AddNode(configNode);
		ConfigNode protoVesselNode = ProtoVessel.CreateVesselNode(cometName, VesselType.SpaceObject, o, 0, new ConfigNode[1] { configNode2 }, new ConfigNode("ACTIONGROUPS"), ProtoVessel.CreateDiscoveryNode(DiscoveryLevels.Presence, objClass, lifeTime, lifeTimeMax), configNode3);
		ProtoVessel protoVessel = HighLogic.CurrentGame.AddVessel(protoVesselNode);
		GameEvents.onCometSpawned.Fire(protoVessel.vesselRef);
		return protoVessel;
	}

	public static string GenerateCometName(System.Random generator = null)
	{
		KSPRandom kSPRandom = ((generator != null) ? (generator as KSPRandom) : new KSPRandom());
		if (HighLogic.CurrentGame != null)
		{
			double num = 0.0;
			num = 1.0 - (double)HighLogic.CurrentGame.cometNames.Count / 10.0;
			Math.Max(num, 0.1);
			if (kSPRandom.NextDouble() > num)
			{
				int index = kSPRandom.Next(0, HighLogic.CurrentGame.cometNames.Count - 1);
				return HighLogic.CurrentGame.GetCometNumberedName(HighLogic.CurrentGame.cometNames.KeysList[index]);
			}
		}
		string[] array;
		string[] array2;
		if (kSPRandom.Next(0, 9) < 5)
		{
			array = CrewGenerator.SpecialNamesMale;
			array2 = CrewGenerator.KerbalNamesMale;
		}
		else
		{
			array = CrewGenerator.SpecialNamesFemale;
			array2 = CrewGenerator.KerbalNamesFemale;
		}
		string empty = string.Empty;
		empty = (((kSPRandom?.Next(specialNameChance) ?? generator.Next(specialNameChance)) != 0) ? array2[kSPRandom?.Next(array2.Length) ?? generator.Next(array2.Length)] : array[kSPRandom?.Next(array.Length) ?? generator.Next(array.Length)]);
		if (kSPRandom.Next(0, 100) < 24)
		{
			string text = (((kSPRandom?.Next(specialNameChance) ?? generator.Next(specialNameChance)) != 0) ? array2[kSPRandom?.Next(array2.Length) ?? generator.Next(array2.Length)] : array[kSPRandom?.Next(array.Length) ?? generator.Next(array.Length)]);
			empty = empty + "-" + text;
			if (kSPRandom.Next(0, 100) < 24)
			{
				string text2 = (((kSPRandom?.Next(specialNameChance) ?? generator.Next(specialNameChance)) != 0) ? array2[kSPRandom?.Next(array2.Length) ?? generator.Next(array2.Length)] : array[kSPRandom?.Next(array.Length) ?? generator.Next(array.Length)]);
				empty = empty + "-" + text2;
			}
		}
		if (HighLogic.CurrentGame != null)
		{
			return HighLogic.CurrentGame.GetCometNumberedName(empty);
		}
		return empty + " 1";
	}
}
