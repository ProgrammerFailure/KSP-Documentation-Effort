using System;
using System.IO;

namespace Expansions.Missions;

[Serializable]
public class MissionCraft
{
	public string craftFile;

	public string craftFolder;

	public string facility = "";

	public bool mappedItem;

	public string mappedVesselName = "";

	public uint craftPersistentId;

	public uint rootPartPersistentId;

	public ConfigNode _configNodeCache;

	public ConfigNode CraftNode
	{
		get
		{
			if (_configNodeCache == null)
			{
				if (File.Exists(craftFolder.TrimEnd('/') + "/" + craftFile))
				{
					_configNodeCache = ConfigNode.Load(craftFolder.TrimEnd('/') + "/" + craftFile);
				}
				else if (File.Exists(craftFolder.TrimEnd('/') + "/VAB/" + craftFile))
				{
					_configNodeCache = ConfigNode.Load(craftFolder.TrimEnd('/') + "/VAB/" + craftFile);
					facility = "VAB";
				}
				else if (File.Exists(craftFolder.TrimEnd('/') + "/SPH/" + craftFile))
				{
					_configNodeCache = ConfigNode.Load(craftFolder.TrimEnd('/') + "/SPH/" + craftFile);
					facility = "SPH";
				}
			}
			return _configNodeCache;
		}
	}

	public string VesselName
	{
		get
		{
			string value = "";
			if (CraftNode != null)
			{
				CraftNode.TryGetValue("ship", ref value);
			}
			return value;
		}
	}

	public uint persistentId
	{
		get
		{
			uint value = 0u;
			CraftNode.TryGetValue("persistentId", ref value);
			return value;
		}
	}

	public MissionCraft(string craftFolder, string craftFile)
	{
		_configNodeCache = null;
		this.craftFolder = craftFolder;
		this.craftFile = craftFile;
		string[] array = craftFolder.TrimEnd('/').Split('/');
		if (array[array.Length - 1] == "VAB")
		{
			facility = "VAB";
		}
		else if (array[array.Length - 1] == "SPH")
		{
			facility = "SPH";
		}
		checkPersistentIds();
	}

	public string GetPartNameInVessel(uint persistentId)
	{
		if (CraftNode == null)
		{
			return "No part selected";
		}
		ConfigNode[] nodes = CraftNode.GetNodes("PART");
		int num = nodes.Length;
		do
		{
			if (num-- <= 0)
			{
				return "No part selected";
			}
		}
		while (persistentId != GetPartPersistentId(nodes[num]));
		return GetPartName(nodes[num]);
	}

	public uint GetPartPersistentId(ConfigNode node)
	{
		if (node == null)
		{
			return 0u;
		}
		string value = null;
		if (node.TryGetValue("persistentId", ref value))
		{
			return uint.Parse(value);
		}
		uint uniquepersistentId = FlightGlobals.GetUniquepersistentId();
		node.AddValue("persistentId", uniquepersistentId.ToString());
		return uniquepersistentId;
	}

	public string GetPartName(ConfigNode node)
	{
		if (node == null)
		{
			return null;
		}
		string value = null;
		if (node.TryGetValue("part", ref value))
		{
			value = KSPUtil.GetPartName(value);
			value = PartLoader.getPartInfoByName(value).title;
		}
		return value;
	}

	public void checkPersistentIds()
	{
		if (CraftNode == null)
		{
			return;
		}
		bool flag = false;
		ConfigNode configNode = _configNodeCache.CreateCopy();
		if (!configNode.HasValue("persistentId"))
		{
			configNode.AddValue("persistentId", FlightGlobals.GetUniquepersistentId());
			flag = true;
		}
		configNode.TryGetValue("persistentId", ref craftPersistentId);
		ConfigNode[] nodes = configNode.GetNodes("PART");
		for (int i = 0; i < nodes.Length; i++)
		{
			if (!nodes[i].HasValue("persistentId"))
			{
				nodes[i].AddValue("persistentId", FlightGlobals.GetUniquepersistentId());
				flag = true;
			}
			if (i == 0)
			{
				nodes[i].TryGetValue("persistentId", ref rootPartPersistentId);
			}
		}
		if (flag)
		{
			configNode.Save(craftFolder + craftFile);
			_configNodeCache = null;
		}
	}

	public void Clear()
	{
		if (_configNodeCache != null)
		{
			_configNodeCache.ClearData();
			_configNodeCache = null;
		}
	}
}
