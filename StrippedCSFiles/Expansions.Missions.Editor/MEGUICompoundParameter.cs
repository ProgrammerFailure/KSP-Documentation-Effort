using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUICompoundParameter : MEGUIParameter
{
	public bool subParametersSelectable;

	public bool subParametersPinneable;

	protected Dictionary<string, MEGUIParameter> subParameters;

	private List<MEGUIParameterGroup> supParametersGroups;

	private List<MEGUIParameter> parametersToSort;

	protected bool isSubParameterMouseOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUICompoundParameter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Setup(string name, object value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Setup(string name, object value, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private MEGUIParameter AddSubParameter(BaseAPField field, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Display()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SortParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetSubParameter(string fieldId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int SortParametersByOrder(MEGUIParameter p1, MEGUIParameter p2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}
}
