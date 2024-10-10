using System.Collections;
using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

public class ModuleDecouple : ModuleDecouplerBase
{
	public class Jettisonning
	{
		public Part part;

		public Vector3 ejectionVector;

		public Jettisonning(Part p, Vector3 vec)
		{
			part = p;
			ejectionVector = vec;
		}
	}

	[KSPField]
	public string menuName = "";

	[KSPField]
	public bool automaticDir = true;

	[KSPField]
	public Vector3 explosiveDir = Vector3.down;

	public override void OnAwake()
	{
		base.OnAwake();
		if (menuName != string.Empty)
		{
			base.Events["Decouple"].guiName = menuName;
			base.Actions["DecoupleAction"].guiName = menuName;
		}
	}

	public override void OnStart(StartState state)
	{
		base.OnStart(state);
		if (base.part.stagingIcon == string.Empty && overrideStagingIconIfBlank)
		{
			base.part.stagingIcon = "DECOUPLER_VERT";
		}
		if (isDecoupled)
		{
			base.Events["Decouple"].active = false;
			base.part.findFxGroup("activate")?.setActive(value: false);
		}
	}

	public override void OnActive()
	{
		if (staged)
		{
			OnDecouple();
		}
	}

	public override void OnDecouple()
	{
		base.OnDecouple();
		if (fx != null)
		{
			fx.Burst();
		}
		Part parent = base.part.parent;
		Part partUpwardsCached = FlightGlobals.GetPartUpwardsCached(base.vessel.ReferenceTransform.gameObject);
		Vessel oldVessel = base.vessel;
		List<Jettisonning> list = new List<Jettisonning>();
		if (isOmniDecoupler)
		{
			List<Part> list2 = new List<Part>(base.part.children);
			int num = base.part.children.Count + 1;
			float num2 = ejectionForce * (ejectionForcePercent * 0.01f) / (float)num;
			int i = 0;
			for (int count = list2.Count; i < count; i++)
			{
				Part part = list2[i];
				part.decouple();
				Vector3 vector = ((!automaticDir) ? base.part.partTransform.TransformDirection(explosiveDir) : Vector3.Normalize(base.part.transform.position - part.transform.position));
				vector *= num2;
				list.Add(new Jettisonning(part, -vector));
				list.Add(new Jettisonning(base.part, vector));
			}
			if (base.part.parent != null)
			{
				base.part.decouple();
				num2 = ejectionForce * (ejectionForcePercent * 0.01f) * 0.5f;
				Vector3 vector2 = ((!automaticDir) ? base.part.partTransform.TransformDirection(explosiveDir) : Vector3.Normalize(base.part.transform.position - parent.transform.position));
				vector2 *= num2;
				list.Add(new Jettisonning(parent, -vector2));
				list.Add(new Jettisonning(base.part, vector2));
			}
		}
		else if (explosiveNode.attachedPart != null)
		{
			parent = explosiveNode.attachedPart;
			if (parent == base.part.parent)
			{
				base.part.decouple();
			}
			else
			{
				parent.decouple();
			}
			float num3 = ejectionForce * (ejectionForcePercent * 0.01f) * 0.5f;
			Vector3 vector3 = ((!automaticDir) ? base.part.partTransform.TransformDirection(explosiveDir) : Vector3.Normalize(base.part.transform.position - parent.transform.position));
			vector3 *= num3;
			list.Add(new Jettisonning(parent, -vector3));
			list.Add(new Jettisonning(base.part, vector3));
		}
		StartCoroutine(ApplyEjectionForces(2, list));
		GameEvents.onStageSeparation.Fire(new EventReport(FlightEvents.STAGESEPARATION, base.part, null, null, StageManager.CurrentStage));
		isDecoupled = true;
		refreshStaging = true;
		base.Events["Decouple"].active = false;
		StartCoroutine(WaitAndSwitchFocus(partUpwardsCached, oldVessel));
	}

	public IEnumerator WaitAndSwitchFocus(Part p, Vessel oldVessel)
	{
		yield return null;
		if (p.vessel != oldVessel)
		{
			oldVessel.vesselType = VesselType.Debris;
			oldVessel.vesselType = oldVessel.FindDefaultVesselType();
			if (!oldVessel.UpdateVesselNaming())
			{
				oldVessel.vesselName = Vessel.AutoRename(oldVessel, oldVessel.vesselName);
			}
			p.vessel.ctrlState.CopyFrom(oldVessel.ctrlState);
			p.vessel.ActionGroups.CopyFrom(oldVessel.ActionGroups);
			p.vessel.currentStage = oldVessel.currentStage;
			p.vessel.SetReferenceTransform(p);
			FlightGlobals.ForceSetActiveVessel(p.vessel);
		}
	}

	public IEnumerator ApplyEjectionForces(int frameDelay, List<Jettisonning> jettisonObjs)
	{
		int i = frameDelay;
		while (i-- > 0)
		{
			yield return null;
		}
		int count = jettisonObjs.Count;
		while (count-- > 0)
		{
			Jettisonning jettisonning = jettisonObjs[count];
			if (jettisonning.part != null)
			{
				Part part = jettisonning.part;
				if (part != null)
				{
					part.AddForce(jettisonning.ejectionVector);
				}
			}
		}
	}

	public override string GetInfo()
	{
		return "" + Localizer.Format("#autoLOC_240611", ejectionForce.ToString("0.0###"));
	}

	public override string GetModuleDisplayName()
	{
		if (isOmniDecoupler)
		{
			return Localizer.Format("#autoLOC_6001039");
		}
		return Localizer.Format("#autoLOC_6001040");
	}
}
