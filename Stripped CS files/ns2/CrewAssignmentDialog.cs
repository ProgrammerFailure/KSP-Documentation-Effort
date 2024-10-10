using System.Collections;
using PreFlightTests;
using UnityEngine;

namespace ns2;

public class CrewAssignmentDialog : BaseCrewAssignmentDialog
{
	public static CrewAssignmentDialog Instance;

	public override void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("CrewAssignmentDialog: Instance already exists.");
			base.gameObject.DestroyGameObject();
			return;
		}
		Instance = this;
		if (!HighLogic.LoadedSceneIsEditor || (HighLogic.LoadedSceneIsEditor && HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			base.CurrentCrewRoster = HighLogic.CurrentGame.CrewRoster;
		}
		base.Awake();
		GameEvents.onGUIAstronautComplexDespawn.Add(Refresh);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onGUIAstronautComplexDespawn.Remove(Refresh);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
		HighLogic.CurrentGame.CrewRoster = newRoster;
	}

	public override KerbalRoster GetCurrentCrewRoster()
	{
		return HighLogic.CurrentGame.CrewRoster;
	}

	public void ButtonAstronautComplex()
	{
		onOpenAstronautComplex();
	}

	public void onOpenAstronautComplex()
	{
		InputLockManager.SetControlLock("ACoperationalCheck");
		PreFlightCheck preFlightCheck = new PreFlightCheck(onOpenACProceed, onOpenACDismiss);
		preFlightCheck.AddTest(new FacilityOperational("AstronautComplex", "Astronaut Complex"));
		preFlightCheck.RunTests();
	}

	public void onOpenACProceed()
	{
		onOpenACDismiss();
		GameEvents.onGUIAstronautComplexSpawn.Fire();
	}

	public void onOpenACDismiss()
	{
		InputLockManager.RemoveControlLock("ACoperationalCheck");
	}

	public override void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		base.MoveCrewToEmptySeat(fromlist, tolist, itemToMove, index);
		EditorLogicUpdateCrew();
	}

	public override void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		base.MoveCrewToAvail(fromlist, tolist, itemToMove);
		EditorLogicUpdateCrew();
	}

	public override void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		base.DropOnCrewList(fromList, insertItem, insertIndex);
		StartCoroutine(RefreshCrewOnDrop());
	}

	public override void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		base.DropOnAvailList(fromList, insertItem, insertIndex);
		StartCoroutine(RefreshCrewOnDrop());
	}

	public IEnumerator RefreshCrewOnDrop()
	{
		yield return null;
		EditorLogicUpdateCrew();
	}

	public void EditorLogicUpdateCrew()
	{
		if (EditorLogic.fetch != null)
		{
			EditorLogic.fetch.UpdateCrewManifest();
		}
		Refresh();
	}
}
