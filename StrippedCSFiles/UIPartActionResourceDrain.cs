using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[UI_Resources]
public class UIPartActionResourceDrain : UIPartActionFieldItem
{
	public UI_Resources resourcesControl;

	public VerticalLayoutGroup verticalLayout;

	public GameObject resourceTogglePrefab;

	public int fieldValue;

	private ModuleResourceDrain moduleResourceDrain;

	public List<UIPartActionResourceToggle> uiPartActionResourcetoggle;

	private static string cacheAutoLOC_6006009;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceDrain()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetFieldValue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control, BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdatePartResourceStatus(PartResource p, bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourcesDrainRate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateResourcesDrainRateAndStatus(string resourceName, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitializeResourceFieldsDrainStatus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
