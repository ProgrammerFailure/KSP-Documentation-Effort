using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using Experience.Effects;
using ns9;
using UnityEngine;

public class KerbalSeat : PartModule, ICommNetControlSource, IConstruction
{
	[KSPField]
	public string seatPivotName;

	[KSPField]
	public string controlTransformName = "";

	[KSPField]
	public string seatName = "";

	public Transform seatPivot;

	public Transform controlTransform;

	public uint seatOccupantID;

	public Part occupant;

	[KSPField]
	public Vector3 ejectDirection = Vector3.up;

	[KSPField(guiActiveEditor = true, guiFormat = "F0", isPersistant = true, guiActive = true, advancedTweakable = true, guiName = "#autoLOC_8003340")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 100f, minValue = 0f)]
	public float ejectionForcePercentage;

	[KSPField]
	public float ejectionForceMax = 100f;

	[KSPField]
	public Vector3 ejectionForceDirection = Vector3.up;

	public static string cacheAutoLOC_222214;

	public Transform SeatPivot => seatPivot ?? base.transform;

	public Transform ControlTransform => controlTransform ?? controlTransform;

	public Part Occupant => occupant;

	public float EjectionForce => ejectionForceMax * ejectionForcePercentage * 0.01f;

	string ICommNetControlSource.name => base.name;

	public override void OnStart(StartState state)
	{
		seatPivot = base.part.FindModelTransform(seatPivotName);
		if (string.IsNullOrEmpty(seatName))
		{
			seatName = base.part.partInfo.title;
		}
		if (seatPivot == null)
		{
			Debug.LogWarning("[KerbalSeat Warning]: No transform found for seatPivot with name: " + seatPivotName + ".", base.gameObject);
		}
		if (controlTransformName == string.Empty)
		{
			controlTransform = base.part.transform;
		}
		else
		{
			controlTransform = base.part.FindModelTransform(controlTransformName);
			if (!controlTransform)
			{
				Debug.LogWarning("[KerbalSeat Warning]: WARNING - No control transform found with name " + controlTransformName, base.part.gameObject);
				controlTransform = base.part.transform;
			}
		}
		ejectDirection.Normalize();
		base.Events["BoardSeat"].guiName = Localizer.Format("#autoLOC_6001832", seatName);
		if (HighLogic.LoadedSceneIsFlight)
		{
			occupant = base.vessel[seatOccupantID];
		}
		base.Events["BoardSeat"].active = occupant == null;
		base.part.crewTransferAvailable = false;
	}

	public override void OnInventoryModeEnable()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			occupant = base.vessel[seatOccupantID];
		}
	}

	public override void OnStartFinished(StartState state)
	{
		base.OnStartFinished(state);
		if (base.part.protoModuleCrew != null && base.part.protoModuleCrew.Count > 0)
		{
			StartCoroutine(SpawnBoardingKerbal());
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("occupantID"))
		{
			seatOccupantID = uint.Parse(node.GetValue("occupantID"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		if (occupant != null)
		{
			node.AddValue("occupantID", occupant.flightID);
		}
	}

	public IEnumerator SpawnBoardingKerbal()
	{
		if (base.part.protoModuleCrew == null || base.part.protoModuleCrew.Count == 0)
		{
			yield break;
		}
		ProtoCrewMember protoCrewMember = base.part.protoModuleCrew[0];
		KerbalEVA boardingKerbal = FlightEVA.Spawn(protoCrewMember);
		if (boardingKerbal == null)
		{
			yield break;
		}
		CollisionEnhancer.bypass = true;
		boardingKerbal.gameObject.SetActive(value: true);
		boardingKerbal.part.vessel = boardingKerbal.gameObject.AddComponent<Vessel>();
		boardingKerbal.vessel.Initialize();
		boardingKerbal.vessel.id = Guid.NewGuid();
		boardingKerbal.vessel.SetPosition(base.part.partTransform.position);
		boardingKerbal.part.AddCrewmember(protoCrewMember);
		base.part.RemoveCrewmember(protoCrewMember);
		boardingKerbal.gameObject.name = protoCrewMember.GetKerbalEVAPartName() + " (" + protoCrewMember.name + ")";
		boardingKerbal.vessel.vesselName = protoCrewMember.name;
		boardingKerbal.vessel.vesselType = VesselType.const_11;
		boardingKerbal.vessel.launchedFrom = base.vessel.launchedFrom;
		boardingKerbal.vessel.orbit.referenceBody = base.vessel.orbit.referenceBody;
		boardingKerbal.part.flagURL = base.part.flagURL;
		boardingKerbal.part.flightID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		boardingKerbal.part.missionID = ShipConstruction.GetUniqueFlightID(HighLogic.CurrentGame.flightState);
		boardingKerbal.part.launchID = base.part.launchID;
		while (!boardingKerbal.Ready || !boardingKerbal.part.started)
		{
			if (boardingKerbal != null)
			{
				boardingKerbal.EnableCharacterAndLadderColliders(enable: false);
			}
			yield return null;
		}
		boardingKerbal.EnableCharacterAndLadderColliders(enable: true);
		if (boardingKerbal.BoardSeat(this))
		{
			Debug.Log("[KerbalSeat]: Boarded by " + boardingKerbal.vessel.vesselName, base.gameObject);
			occupant = boardingKerbal.part;
			base.Events["BoardSeat"].active = false;
		}
		if (boardingKerbal.part.attachJoint != null)
		{
			boardingKerbal.part.attachJoint.SetUnbreakable(unbreakable: true, forceRigid: true);
		}
		base.vessel.ResumeStaging();
	}

	[KSPAction("#autoLOC_6001447")]
	public void MakeReferenceToggle(KSPActionParam act)
	{
		MakeReferenceTransform();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001447")]
	public void MakeReferenceTransform()
	{
		base.part.SetReferenceTransform(controlTransform);
		base.vessel.SetReferenceTransform(base.part);
	}

	[KSPEvent(guiActiveUnfocused = true, externalToEVAOnly = true, unfocusedRange = 2f, guiName = "#autoLOC_900820")]
	public void BoardSeat()
	{
		if (occupant != null)
		{
			Debug.LogError("[KerbalSeat Error]: seat is occupied by " + occupant.name, base.gameObject);
			return;
		}
		KerbalEVA component = FlightGlobals.ActiveVessel.GetComponent<KerbalEVA>();
		if (component == null)
		{
			Debug.LogError("[KerbalSeat Error]: Cannot call this method from a non-EVA vessel", base.gameObject);
			return;
		}
		Debug.Log("[KerbalSeat]: Being Boarded by " + component.vessel.vesselName, base.gameObject);
		if (SeatBoundsAreClear(component.part))
		{
			if (component.BoardSeat(this))
			{
				Debug.Log("[KerbalSeat]: Boarded by " + component.vessel.vesselName, base.gameObject);
				occupant = component.part;
				base.Events["BoardSeat"].active = false;
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage(cacheAutoLOC_222214, 2f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	[KSPAction(guiName = "#autoLOC_6001810")]
	public void LeaveSeat(KSPActionParam act)
	{
		KerbalEVA kerbalEVA = null;
		if (occupant != null)
		{
			kerbalEVA = occupant.GetComponent<KerbalEVA>();
			if (kerbalEVA != null)
			{
				kerbalEVA.OnDeboardSeat();
			}
		}
	}

	public void OnChildRemove(Part child)
	{
		if (child == occupant)
		{
			occupant = null;
			base.Events["BoardSeat"].active = true;
		}
	}

	public bool SeatBoundsAreClear(Part candidate)
	{
		List<PhysicsUtil.SphereHit> list = PhysicsUtil.SphereSweepTestWhere(seatPivot.position, seatPivot.up * 0.5f, 0.5f, 0.25f, 0.2f, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000, (PhysicsUtil.SphereHit hit) => !base.vessel.ContainsCollider(hit.collider) && !candidate.vessel.ContainsCollider(hit.collider));
		DebugDrawUtil.DrawCrosshairs(seatPivot.position, 0.25f, Color.magenta, 5f);
		DebugDrawUtil.DrawCrosshairs(seatPivot.position + base.transform.up * 0.5f, 0.25f, Color.yellow, 5f);
		if (list.Count > 0)
		{
			Debug.Log("KerbalSeat: Bounds not clear: " + list.Count + " objects on it", list[0].collider.gameObject);
			return false;
		}
		return true;
	}

	public bool EditorSeatBoundsAreClear()
	{
		List<PhysicsUtil.SphereHit> list = PhysicsUtil.SphereSweepTest(seatPivot.position, seatPivot.up * 0.5f, 0.5f, 0.25f, 0.2f, LayerUtil.DefaultEquivalent | 0x8000 | 0x80000);
		DebugDrawUtil.DrawCrosshairs(seatPivot.position, 0.25f, Color.magenta, 5f);
		DebugDrawUtil.DrawCrosshairs(seatPivot.position + base.transform.up * 0.5f, 0.25f, Color.yellow, 5f);
		Collider[] partColliders = base.part.GetPartColliders();
		int count = list.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (partColliders.IndexOf(list[count].collider) >= 0);
		Debug.Log("KerbalSeat: Bounds not clear: " + list.Count + " objects on it", list[count].collider.gameObject);
		return false;
	}

	public virtual VesselControlState GetControlSourceState()
	{
		if (occupant != null && !occupant.protoModuleCrew[0].inactive)
		{
			if (Occupant.protoModuleCrew[0].HasEffect<FullVesselControlSkill>())
			{
				return VesselControlState.KerbalFull;
			}
			return VesselControlState.KerbalPartial;
		}
		return VesselControlState.None;
	}

	public virtual bool IsCommCapable()
	{
		if (occupant != null && occupant.protoModuleCrew.Count > 0 && !occupant.protoModuleCrew[0].inactive)
		{
			return true;
		}
		return false;
	}

	public virtual void UpdateNetwork()
	{
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_222214 = Localizer.Format("#autoLOC_222214");
	}

	public bool CanBeDetached()
	{
		return occupant == null;
	}

	public bool CanBeOffset()
	{
		return occupant == null;
	}

	public bool CanBeRotated()
	{
		return occupant == null;
	}
}
