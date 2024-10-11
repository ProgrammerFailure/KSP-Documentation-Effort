using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_DynamicModule]
public class MEGUIParameterDynamicModule : MEGUICompoundParameter
{
	public Button buttonExpand;

	public GameObject arrowExpand;

	public Button removeButton;

	public GameObject container;

	public Transform childRoot;

	protected MEGUIParameterDynamicModuleList parentParameter;

	public DynamicModule dynamicModule
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterDynamicModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterDynamicModule Create(DynamicModule dynamicModule, MEGUIParameterDynamicModuleList parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name, object value, Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleExpand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveButton()
	{
		throw null;
	}
}
