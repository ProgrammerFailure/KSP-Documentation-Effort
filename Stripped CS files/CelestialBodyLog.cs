using System.Collections.Generic;

public class CelestialBodyLog : IConfigNode
{
	public List<string> bodies;

	public CelestialBodyLog()
	{
		bodies = new List<string>();
	}

	public void AddNewBody(CelestialBody body)
	{
		if (!bodies.Contains(body.name))
		{
			bodies.Add(body.name);
		}
	}

	public bool HasBody(string cbName)
	{
		return bodies.Contains(cbName);
	}

	public bool At(CelestialBody cb)
	{
		return HasBody(cb.name);
	}

	public void Load(ConfigNode node)
	{
		string[] values = node.GetValues("at");
		foreach (string item in values)
		{
			bodies.Add(item);
		}
	}

	public void Save(ConfigNode node)
	{
		foreach (string body in bodies)
		{
			node.AddValue("at", body);
		}
	}

	public void MergeWith(CelestialBodyLog cbl)
	{
		int count = cbl.bodies.Count;
		for (int i = 0; i < count; i++)
		{
			bodies.AddUnique(cbl.bodies[0]);
		}
	}

	public List<string> GetBodies()
	{
		return bodies;
	}
}
