using System.Collections;
using ns11;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionVesselStage : ActionModule
{
	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000043");
	}

	public override IEnumerator Fire()
	{
		if (FlightGlobals.ActiveVessel != null)
		{
			yield return new WaitForSeconds(0.1f);
			StageManager.ActivateNextStage();
			FlightGlobals.ActiveVessel.ActionGroups.ToggleGroup(KSPActionGroup.Stage);
		}
		yield return null;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004005");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
	}
}
