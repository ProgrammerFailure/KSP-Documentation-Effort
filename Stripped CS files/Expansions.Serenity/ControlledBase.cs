using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public abstract class ControlledBase : IConfigNode
{
	[SerializeField]
	public uint partId;

	[SerializeField]
	public List<uint> SymmetryPartIDs;

	[SerializeField]
	public string partNickName = "";

	[SerializeField]
	public int rowIndex = -1;

	[SerializeField]
	public uint moduleId;

	[SerializeField]
	public uint moduleActionsId;

	public uint persistentActionId;

	public Part Part { get; set; }

	public ModuleRoboticController Controller { get; set; }

	public uint PartPersistentId
	{
		get
		{
			if (!(Part == null))
			{
				return Part.persistentId;
			}
			return 0u;
		}
	}

	public List<Part> SymmetryParts
	{
		get
		{
			if (!(Part != null))
			{
				return null;
			}
			return Part.symmetryCounterparts;
		}
	}

	public string PartNickName => partNickName;

	[SerializeField]
	public PartModule Module { get; set; }

	public uint PersistentActionId => persistentActionId;

	public abstract string BaseName { get; }

	public ControlledBase(Part part, PartModule module, ModuleRoboticController controller)
	{
		partId = part.persistentId;
		Part = part;
		Module = module;
		if (module != null)
		{
			moduleId = module.GetPersistentId();
			persistentActionId = module.GetPersistenActiontId();
		}
		Controller = controller;
	}

	public ControlledBase()
	{
		partId = 0u;
		Part = null;
		moduleId = 0u;
		Module = null;
		Controller = null;
	}

	public void SetPartNickName(string newNickName)
	{
		partNickName = newNickName;
	}

	public abstract bool OnAssignReferenceVars();

	public abstract void ClearSymmetryLists();

	public abstract void AddSymmetryPart(Part part);

	public abstract bool OnChangeSymmetryMaster(Part part, out uint oldPartId);

	public bool AssignReferenceVars()
	{
		if (!FlightGlobals.FindLoadedPart(partId, out var partout))
		{
			Debug.LogWarningFormat("[ModuleRoboticController]: Unable to find Controlled Part in vessel: {0}", partId);
			return false;
		}
		Part = partout;
		if (moduleId != 0)
		{
			Module = partout.Modules[moduleId];
		}
		if (partNickName == "")
		{
			partNickName = Part.partInfo.title;
		}
		bool result = OnAssignReferenceVars();
		RebuildSymmetryList();
		return result;
	}

	public void RebuildSymmetryList(params uint[] excludedPartIds)
	{
		ClearSymmetryLists();
		if (SymmetryParts == null)
		{
			return;
		}
		for (int i = 0; i < SymmetryParts.Count; i++)
		{
			if (excludedPartIds.IndexOf(SymmetryParts[i].persistentId) <= -1)
			{
				AddSymmetryPart(SymmetryParts[i]);
			}
		}
	}

	public void ChangeSymmetryMaster(Part newPart)
	{
		uint oldPartId;
		if (!SymmetryParts.Contains(newPart))
		{
			Debug.LogErrorFormat("[ControlledAxis]: Cannot change Symmetry Master to {0} as its not listed in the SymmetryParts for {1}", newPart.persistentId, PartPersistentId);
		}
		else if (OnChangeSymmetryMaster(newPart, out oldPartId))
		{
			Part = newPart;
			partId = newPart.persistentId;
			RebuildSymmetryList(oldPartId);
		}
	}

	public abstract void OnLoad(ConfigNode node);

	public abstract void OnSave(ConfigNode node);

	public void Load(ConfigNode node)
	{
		node.TryGetValue("persistentId", ref partId);
		node.TryGetValue("moduleId", ref moduleId);
		node.TryGetValue("partNickName", ref partNickName);
		node.TryGetValue("rowIndex", ref rowIndex);
		OnLoad(node);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("persistentId", partId);
		node.AddValue("moduleId", moduleId);
		node.AddValue("partNickName", partNickName);
		node.AddValue("rowIndex", rowIndex);
		if (SymmetryParts != null && SymmetryParts.Count > 0)
		{
			ConfigNode configNode = node.AddNode("SYMPARTS");
			for (int i = 0; i < SymmetryParts.Count; i++)
			{
				configNode.AddValue("symPersistentId", SymmetryParts[i].persistentId);
			}
		}
		OnSave(node);
	}
}
