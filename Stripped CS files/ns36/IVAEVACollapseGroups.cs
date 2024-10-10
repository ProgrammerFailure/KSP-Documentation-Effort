using System;
using System.Collections.Generic;
using ns11;
using ns2;
using UnityEngine;

namespace ns36;

public class IVAEVACollapseGroups : MonoBehaviour
{
	public UIPanelTransition[] IVACollapseGroups;

	public UIPanelTransition[] EVACollapseGroups;

	[SerializeField]
	public int expandedIndex;

	[SerializeField]
	public int collapsedIndex = 1;

	[HideInInspector]
	public Action finishedAllTransitions;

	public bool isIVA;

	public bool isEVA;

	public List<UIPanelTransition> transitionList = new List<UIPanelTransition>();

	public void Awake()
	{
		GameEvents.OnCameraChange.Add(OnCameraChange);
		GameEvents.onVesselChange.Add(onVesselChange);
		GameEvents.onVesselWasModified.Add(onVesselWasModified);
		finishedAllTransitions = (Action)Delegate.Combine(finishedAllTransitions, new Action(FinishedAllTransitionsCallback));
	}

	public void OnDestroy()
	{
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		GameEvents.onVesselChange.Remove(onVesselChange);
		GameEvents.onVesselWasModified.Remove(onVesselWasModified);
		finishedAllTransitions = (Action)Delegate.Remove(finishedAllTransitions, new Action(FinishedAllTransitionsCallback));
	}

	public void FinishedAllTransitionsCallback()
	{
		GameEvents.OnFlightUIModeChanged.Fire(FlightUIModeController.Instance.Mode);
	}

	public void OnCameraChange(CameraManager.CameraMode m)
	{
		switch (m)
		{
		case CameraManager.CameraMode.const_3:
		case CameraManager.CameraMode.Internal:
			isIVA = true;
			break;
		case CameraManager.CameraMode.Flight:
		case CameraManager.CameraMode.Map:
		case CameraManager.CameraMode.External:
			isIVA = false;
			break;
		}
		UpdatePanels();
	}

	public void onVesselChange(Vessel v)
	{
		isEVA = v.isEVA;
		UpdatePanels();
	}

	public void onVesselWasModified(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel && (EditorActionGroups.Instance == null || !EditorActionGroups.Instance.interfaceActive))
		{
			onVesselChange(v);
		}
	}

	public void UpdatePanels()
	{
		transitionList.Clear();
		if (isIVA)
		{
			int num = IVACollapseGroups.Length;
			while (num-- > 0)
			{
				transitionList.AddUnique(IVACollapseGroups[num]);
			}
		}
		if (isEVA)
		{
			int num2 = EVACollapseGroups.Length;
			while (num2-- > 0)
			{
				transitionList.AddUnique(EVACollapseGroups[num2]);
			}
		}
		int count = transitionList.Count;
		while (count-- > 0)
		{
			transitionList[count].Transition(collapsedIndex);
		}
		int num3 = IVACollapseGroups.Length;
		while (num3-- > 0)
		{
			UIPanelTransition uIPanelTransition = IVACollapseGroups[num3];
			if (!transitionList.Contains(uIPanelTransition))
			{
				transitionList.Add(uIPanelTransition);
				uIPanelTransition.Transition(expandedIndex);
			}
		}
		int num4 = EVACollapseGroups.Length;
		while (num4-- > 0)
		{
			UIPanelTransition uIPanelTransition = EVACollapseGroups[num4];
			if (!transitionList.Contains(uIPanelTransition))
			{
				transitionList.Add(uIPanelTransition);
				if (num4 == 0)
				{
					uIPanelTransition.Transition(expandedIndex, finishedAllTransitions);
				}
				else
				{
					uIPanelTransition.Transition(expandedIndex);
				}
			}
		}
	}
}
