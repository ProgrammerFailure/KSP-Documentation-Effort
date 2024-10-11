using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens.Flight;

public class IVAEVACollapseGroups : MonoBehaviour
{
	public UIPanelTransition[] IVACollapseGroups;

	public UIPanelTransition[] EVACollapseGroups;

	[SerializeField]
	private int expandedIndex;

	[SerializeField]
	private int collapsedIndex;

	[HideInInspector]
	public Action finishedAllTransitions;

	public bool isIVA;

	public bool isEVA;

	private List<UIPanelTransition> transitionList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public IVAEVACollapseGroups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FinishedAllTransitionsCallback()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnCameraChange(CameraManager.CameraMode m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onVesselWasModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdatePanels()
	{
		throw null;
	}
}
