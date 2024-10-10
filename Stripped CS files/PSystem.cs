using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KSP/SolarSystem/PSystem")]
public class PSystem : MonoBehaviour
{
	[HideInInspector]
	public int mainToolbarSelected;

	public double systemScale;

	public double systemTimeScale;

	public string systemName;

	public PSystemBody rootBody;

	public void Reset()
	{
		systemName = "Unnamed";
		systemScale = 1.0;
		systemTimeScale = 1.0;
		rootBody = AddBody(null);
	}

	public PSystemBody AddBody(PSystemBody parent)
	{
		GameObject gameObject = new GameObject();
		gameObject.transform.parent = base.transform;
		gameObject.name = "PSystemBody";
		PSystemBody pSystemBody = gameObject.AddComponent<PSystemBody>();
		if (parent != null)
		{
			parent.children.Add(pSystemBody);
		}
		GameObject gameObject2 = new GameObject();
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.name = "Unnamed Object";
		pSystemBody.celestialBody = gameObject2.AddComponent<CelestialBody>();
		if (parent != null)
		{
			pSystemBody.orbitDriver = gameObject2.AddComponent<OrbitDriver>();
			pSystemBody.orbitDriver.referenceBody = parent.celestialBody;
			pSystemBody.orbitRenderer = gameObject2.AddComponent<OrbitRenderer>();
		}
		return pSystemBody;
	}

	public void OnDrawGizmos()
	{
		OnDrawGizmosBody(null, rootBody);
	}

	public void OnDrawGizmosBody(PSystemBody parent, PSystemBody body)
	{
	}

	[ContextMenu("Load Celestial Bodies Database")]
	public void LoadDatabase()
	{
		Debug.Log("PSystem: Attempting to load CelestialBodies.cfg");
		ConfigNode configNode = ConfigNode.Load("CelestialBodies.cfg");
		if (configNode == null)
		{
			Debug.LogError("PSystem: Database could not be loaded.");
			return;
		}
		ConfigNode[] nodes = configNode.GetNodes();
		Debug.Log("PSystem: Attempting to load " + nodes.Length + " nodes.");
		foreach (ConfigNode configNode2 in nodes)
		{
			PSystemBody pSystemBody = FindBody(configNode2.name, rootBody);
			if (pSystemBody == null)
			{
				Debug.LogError("PSystem: Cannot find celestial body '" + configNode2.name + "'.");
			}
			else
			{
				LoadBody(pSystemBody, configNode2);
			}
		}
	}

	public void LoadBody(PSystemBody body, ConfigNode node)
	{
		Debug.Log("PSystem: LOADING '" + node.name + "'.");
		string value = node.GetValue("atmosphereDepth");
		if (value != null)
		{
			body.celestialBody.atmosphereDepth = double.Parse(value);
		}
		value = node.GetValue("atmosphereAdiabaticIndex");
		if (value != null)
		{
			body.celestialBody.atmosphereAdiabaticIndex = double.Parse(value);
		}
		value = node.GetValue("atmosphereMolarMass");
		if (value != null)
		{
			body.celestialBody.atmosphereMolarMass = double.Parse(value);
		}
		value = node.GetValue("atmospherePressureSeaLevel");
		if (value != null)
		{
			body.celestialBody.atmospherePressureSeaLevel = double.Parse(value);
		}
		value = node.GetValue("atmosphereTemperatureSeaLevel");
		if (value != null)
		{
			body.celestialBody.atmosphereTemperatureSeaLevel = double.Parse(value);
		}
		value = node.GetValue("albedo");
		if (value != null)
		{
			body.celestialBody.albedo = double.Parse(value);
		}
		value = node.GetValue("emissivity");
		if (value != null)
		{
			body.celestialBody.emissivity = double.Parse(value);
		}
		value = node.GetValue("coreTemperatureOffset");
		if (value != null)
		{
			body.celestialBody.coreTemperatureOffset = double.Parse(value);
		}
		value = node.GetValue("shockTemperatureMultiplier");
		if (value != null)
		{
			body.celestialBody.shockTemperatureMultiplier = double.Parse(value);
		}
		ConfigNode node2 = node.GetNode("atmosphereTemperatureCurve");
		if (node2 != null && node2.values.Count > 0)
		{
			body.celestialBody.atmosphereTemperatureCurve.Load(node2);
			body.celestialBody.atmosphereUseTemperatureCurve = true;
			body.celestialBody.atmosphereTemperatureCurveIsNormalized = body.celestialBody.atmosphereTemperatureCurve.maxTime == 1f;
		}
		else
		{
			body.celestialBody.atmosphereUseTemperatureCurve = false;
		}
		node2 = node.GetNode("atmospherePressureCurve");
		if (node2 != null && node2.values.Count > 0)
		{
			body.celestialBody.atmospherePressureCurve.Load(node2);
			body.celestialBody.atmosphereUsePressureCurve = true;
			body.celestialBody.atmospherePressureCurveIsNormalized = body.celestialBody.atmospherePressureCurve.maxTime == 1f;
		}
		else
		{
			body.celestialBody.atmosphereUsePressureCurve = false;
		}
		node2 = node.GetNode("latitudeTemperatureBiasCurve");
		if (node2 != null)
		{
			body.celestialBody.latitudeTemperatureBiasCurve.Load(node2);
		}
		node2 = node.GetNode("latitudeTemperatureSunMultCurve");
		if (node2 != null)
		{
			body.celestialBody.latitudeTemperatureSunMultCurve.Load(node2);
		}
		node2 = node.GetNode("axialTemperatureSunMultCurve");
		if (node2 != null)
		{
			body.celestialBody.axialTemperatureSunMultCurve.Load(node2);
		}
		node2 = node.GetNode("atmosphereTemperatureSunMultCurve");
		if (node2 != null)
		{
			body.celestialBody.atmosphereTemperatureSunMultCurve.Load(node2);
		}
	}

	public PSystemBody FindBody(string name, PSystemBody parent)
	{
		if (parent.celestialBody.name == name)
		{
			return parent;
		}
		int num = 0;
		PSystemBody pSystemBody;
		while (true)
		{
			if (num < parent.children.Count)
			{
				pSystemBody = FindBody(name, parent.children[num]);
				if (pSystemBody != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return pSystemBody;
	}

	public List<PSystemBody> GetBodies(PSystemBody body)
	{
		List<PSystemBody> list = new List<PSystemBody>();
		GetBodies(list, body);
		return list;
	}

	public void GetBodies(List<PSystemBody> list, PSystemBody body)
	{
		list.Add(body);
		for (int i = 0; i < body.children.Count; i++)
		{
			GetBodies(list, body.children[i]);
		}
	}

	[ContextMenu("Save Celestial Bodies Database")]
	public void SaveDatabase()
	{
		ConfigNode configNode = new ConfigNode();
		ConfigNode configNode2 = configNode.AddNode("CELESTIALBODIES");
		List<PSystemBody> bodies = GetBodies(rootBody);
		for (int i = 0; i < bodies.Count; i++)
		{
			SaveBody(bodies[i], configNode2.AddNode(bodies[i].celestialBody.name));
		}
		configNode.Save("CelestialBodies.cfg");
	}

	public void SaveBody(PSystemBody body, ConfigNode node)
	{
		node.AddValue("atmosphereDepth", body.celestialBody.atmosphereDepth);
		node.AddValue("atmosphereAdiabaticIndex", body.celestialBody.atmosphereAdiabaticIndex);
		node.AddValue("atmosphereMolarMass", body.celestialBody.atmosphereMolarMass);
		node.AddValue("atmospherePressureSeaLevel", body.celestialBody.atmospherePressureSeaLevel);
		node.AddValue("atmosphereTemperatureSeaLevel", body.celestialBody.atmosphereTemperatureSeaLevel);
		node.AddValue("atmospherePressureSeaLevel", body.celestialBody.atmospherePressureSeaLevel);
		node.AddValue("atmosphereTemperatureSeaLevel", body.celestialBody.atmosphereTemperatureSeaLevel);
		node.AddValue("albedo", body.celestialBody.albedo);
		node.AddValue("emissivity", body.celestialBody.emissivity);
		node.AddValue("coreTemperatureOffset", body.celestialBody.coreTemperatureOffset);
		node.AddValue("shockTemperatureMultiplier", body.celestialBody.shockTemperatureMultiplier);
		body.celestialBody.atmosphereTemperatureCurve.Save(node.AddNode("atmosphereTemperatureCurve"));
		body.celestialBody.atmospherePressureCurve.Save(node.AddNode("atmospherePressureCurve"));
		body.celestialBody.axialTemperatureSunMultCurve.Save(node.AddNode("axialTemperatureSunMultCurve"));
		body.celestialBody.atmosphereTemperatureSunMultCurve.Save(node.AddNode("atmosphereTemperatureSunMultCurve"));
	}
}
