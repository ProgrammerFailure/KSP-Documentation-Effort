using System.Collections;
using ns11;
using ns9;
using UnityEngine;

public class ModuleAnchoredDecoupler : ModuleDecouplerBase
{
	[KSPField]
	public string anchorName = "anchor";

	[KSPField]
	public Vector3 explosiveDir = Vector3.right;

	public Part otherPart;

	public Transform anchor;

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "DECOUPLER_HOR";
		}
		anchor = base.part.FindModelTransform(anchorName);
		if (isDecoupled)
		{
			anchor.gameObject.SetActive(value: false);
			base.Events["Decouple"].active = false;
		}
	}

	public override void OnActive()
	{
		if (staged && stagingEnabled)
		{
			OnDecouple();
		}
	}

	public override void OnDecouple()
	{
		base.OnDecouple();
		if (explosiveNode != null && explosiveNode.attachedPart != null)
		{
			if (fx != null)
			{
				fx.Burst();
			}
			otherPart = explosiveNode.attachedPart;
			if (anchor != null)
			{
				anchor.parent = otherPart.transform;
			}
			if (otherPart == base.part.parent)
			{
				base.part.decouple();
			}
			else
			{
				otherPart.decouple();
			}
		}
		StartCoroutine(ApplyEjectionForces(2, base.part, otherPart, base.part.partTransform.TransformDirection(explosiveDir) * ejectionForce * 0.5f * (ejectionForcePercent / 100f), base.part));
		GameEvents.onStageSeparation.Fire(new EventReport(FlightEvents.STAGESEPARATION, base.part, null, null, StageManager.CurrentStage));
		isDecoupled = true;
		refreshStaging = true;
		base.Events["Decouple"].active = false;
	}

	public IEnumerator ApplyEjectionForces(int frameDelay, Part rb1, Part rb2, Vector3 force, Part part)
	{
		int i = frameDelay;
		while (i-- > 0)
		{
			yield return null;
		}
		if (part != null)
		{
			if (rb1 != null)
			{
				rb1.AddForce(-force);
			}
			if (rb2 != null)
			{
				rb2.AddForceAtPosition(force, part.transform.position);
			}
		}
	}

	public override string GetInfo()
	{
		return "" + Localizer.Format("#autoLOC_240283", ejectionForce.ToString("0.0###"));
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_6001040");
	}
}
