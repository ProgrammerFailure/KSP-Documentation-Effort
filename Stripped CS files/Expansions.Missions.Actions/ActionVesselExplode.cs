using System.Collections;
using System.Collections.Generic;
using Expansions.Missions.Editor;
using ns9;
using UnityEngine;

namespace Expansions.Missions.Actions;

public class ActionVesselExplode : ActionVessel
{
	[MEGUI_InputField(ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8000041", Tooltip = "#autoLOC_8000042")]
	public float partsExplodeTime;

	public WaitForSeconds waitForPartsExplode;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8000040");
		restartOnSceneLoad = true;
		useActiveVessel = true;
	}

	public override IEnumerator Fire()
	{
		isRunning = true;
		if (base.vessel != null)
		{
			waitForPartsExplode = new WaitForSeconds(partsExplodeTime / 1000f);
			List<Part> parts = new List<Part>();
			int count = base.vessel.parts.Count;
			while (count-- > 0)
			{
				parts.Add(base.vessel.parts[count]);
			}
			int i = parts.Count;
			while (i-- > 0)
			{
				parts[i].explode();
				yield return waitForPartsExplode;
			}
			if (base.vessel != null)
			{
				base.vessel.Die();
			}
		}
		isRunning = false;
	}

	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		if (field.name == "partsExplodeTime")
		{
			return Localizer.Format("#autoLOC_8006045", partsExplodeTime);
		}
		return base.GetNodeBodyParameterString(field);
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004001");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("explodeTime", partsExplodeTime);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetValue("explodeTime", ref partsExplodeTime);
	}
}
