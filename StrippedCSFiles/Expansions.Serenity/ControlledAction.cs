using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity;

[Serializable]
public class ControlledAction : ControlledBase
{
	[SerializeField]
	internal string actionName;

	[SerializeField]
	public List<float> times;

	[SerializeField]
	internal List<BaseAction> SymmetryActions;

	private float tempTime;

	public BaseAction Action
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	internal override string BaseName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAction(ControlledAction sourceAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAction(Part part, PartModule module, BaseAction action, ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ControlledAction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReverseTimes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RescaleTimes(float adjustmentRatio, float minSpace = 0.01f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnChangeSymmetryMaster(Part newPart, out uint oldPartId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAssignReferenceVars()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ClearSymmetryLists()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void AddSymmetryPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnSave(ConfigNode node)
	{
		throw null;
	}
}
