using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public abstract class ControlledBase : IConfigNode
{
	[SerializeField]
	internal uint partId;

	[SerializeField]
	internal List<uint> SymmetryPartIDs;

	[SerializeField]
	private string partNickName;

	[SerializeField]
	internal int rowIndex;

	[SerializeField]
	internal uint moduleId;

	[SerializeField]
	internal uint moduleActionsId;

	internal uint persistentActionId;

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public ModuleRoboticController Controller
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		internal set
		{
			throw null;
		}
	}

	public uint PartPersistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public List<Part> SymmetryParts
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string PartNickName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[SerializeField]
	public PartModule Module
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public uint PersistentActionId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal abstract string BaseName { get; }

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal ControlledBase(Part part, PartModule module, ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal ControlledBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartNickName(string newNickName)
	{
		throw null;
	}

	protected abstract bool OnAssignReferenceVars();

	protected abstract void ClearSymmetryLists();

	protected abstract void AddSymmetryPart(Part part);

	protected abstract bool OnChangeSymmetryMaster(Part part, out uint oldPartId);

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool AssignReferenceVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void RebuildSymmetryList(params uint[] excludedPartIds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeSymmetryMaster(Part newPart)
	{
		throw null;
	}

	protected abstract void OnLoad(ConfigNode node);

	protected abstract void OnSave(ConfigNode node);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
