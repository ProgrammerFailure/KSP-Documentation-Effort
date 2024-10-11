using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Expansions.Missions.Editor;

public class GAPPrefabDisplay : ActionPaneDisplay
{
	protected GAPVesselCamera vesselCamera;

	protected GameObject vesselCameraSetup;

	protected GameObject targetObject;

	protected bool isReady;

	public GameObject PrefabInstance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPPrefabDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Setup(Camera displayCamera, int layerMask)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(GameObject prefabTargetObject, float camDistance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LoadTarget(GameObject newTarget)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Clean()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Destroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDisplayDrag(PointerEventData.InputButton button, Vector2 delta)
	{
		throw null;
	}
}
