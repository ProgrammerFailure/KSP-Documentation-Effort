using Expansions;
using Expansions.Missions;
using UnityEngine;

public class ScaledMovement : MapObject
{
	public bool firstUpdate;

	public static ScaledMovement Create(string name, Vessel vessel)
	{
		ScaledMovement scaledMovement = new GameObject(name).AddComponent<ScaledMovement>();
		scaledMovement.tgtRef = vessel.transform;
		scaledMovement.type = ObjectType.Vessel;
		scaledMovement.vessel = FlightGlobals.ActiveVessel;
		scaledMovement.missionsNode = null;
		scaledMovement.celestialBody = null;
		scaledMovement.maneuverNode = null;
		scaledMovement.orbit = vessel.orbit;
		return scaledMovement;
	}

	public static ScaledMovement Create(string name, CelestialBody cb, GameObject scaledObject)
	{
		ScaledMovement scaledMovement = scaledObject.AddComponent<ScaledMovement>();
		scaledMovement.tgtRef = cb.transform;
		scaledMovement.type = ObjectType.CelestialBody;
		scaledMovement.vessel = null;
		scaledMovement.missionsNode = null;
		scaledMovement.maneuverNode = null;
		scaledMovement.celestialBody = cb;
		scaledMovement.orbit = cb.orbit;
		return scaledMovement;
	}

	public static ScaledMovement Create(string name, MENode newNode)
	{
		byte[] hashBytes = null;
		string signature = null;
		if (newNode != null && newNode.mission != null)
		{
			hashBytes = newNode.mission.HashBytes;
			signature = newNode.mission.Signature;
		}
		if (!ExpansionsLoader.IsExpansionInstalled("MakingHistory", hashBytes, signature))
		{
			return null;
		}
		ScaledMovement scaledMovement = new GameObject(name).AddComponent<ScaledMovement>();
		scaledMovement.tgtRef = newNode.transform;
		scaledMovement.type = ObjectType.MENode;
		scaledMovement.vessel = null;
		scaledMovement.celestialBody = null;
		scaledMovement.maneuverNode = null;
		scaledMovement.orbit = newNode.orbitDriver.orbit;
		scaledMovement.missionsNode = newNode;
		return scaledMovement;
	}

	public override void OnLateUpdate()
	{
		if (!tgtRef)
		{
			return;
		}
		switch (type)
		{
		case ObjectType.MENode:
			ScaledSpace.LocalToScaledSpace(missionsNode.GetNodeLocationInWorld());
			break;
		case ObjectType.Generic:
			base.transform.position = ScaledSpace.LocalToScaledSpace(tgtRef.position);
			break;
		case ObjectType.CelestialBody:
			base.transform.position = ScaledSpace.LocalToScaledSpace(celestialBody.position);
			break;
		case ObjectType.Vessel:
			if (vessel != null && vessel.state == Vessel.State.DEAD)
			{
				tgtRef = null;
				return;
			}
			base.transform.position = ScaledSpace.LocalToScaledSpace(vessel.GetWorldPos3D());
			break;
		}
		base.transform.localRotation = tgtRef.rotation;
		firstUpdate = true;
	}
}
