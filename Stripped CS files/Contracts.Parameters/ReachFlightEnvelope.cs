using System;
using System.Collections;
using ns9;
using UnityEngine;

namespace Contracts.Parameters;

[Serializable]
public class ReachFlightEnvelope : ContractParameter
{
	public CelestialBody Destination;

	public Vessel.Situations Situation;

	public string BiomeName;

	public bool useAltLimits;

	public bool useSpdLimits;

	public double maxAltitude;

	public double minAltitude;

	public double maxSpeed;

	public double minSpeed;

	public string title = "";

	public Vessel tgtVessel;

	public bool trackerActive;

	public ReachFlightEnvelope()
	{
	}

	public ReachFlightEnvelope(CelestialBody dest, Vessel.Situations sit, string biomeName, float maxAlt, float minAlt, float maxSpd, float minSpd, string title)
	{
		Destination = dest;
		Situation = sit;
		BiomeName = biomeName ?? "";
		maxAltitude = maxAlt;
		minAltitude = minAlt;
		useAltLimits = maxAltitude != 3.4028234663852886E+38 && minAltitude != 0.0;
		maxSpeed = maxSpd;
		minSpeed = minSpd;
		useSpdLimits = maxSpeed != 3.4028234663852886E+38 && minSpeed != 0.0;
		this.title = title;
	}

	public override string GetTitle()
	{
		title += Localizer.Format("#autoLOC_270222", title, GetSituationStringShort(Situation, Destination, BiomeName));
		if (useAltLimits)
		{
			title += Localizer.Format("#autoLOC_270224", minAltitude.ToString("N0"), maxAltitude.ToString("N0"));
		}
		if (useSpdLimits)
		{
			title += Localizer.Format("#autoLOC_270225", minAltitude.ToString("N1"), maxAltitude.ToString("N1"));
		}
		return title;
	}

	public static string GetSituationStringShort(Vessel.Situations sit, CelestialBody body, string biomeName)
	{
		if (string.IsNullOrEmpty(biomeName))
		{
			return sit switch
			{
				Vessel.Situations.FLYING => Localizer.Format("#autoLOC_270235", body.displayName), 
				Vessel.Situations.LANDED => Localizer.Format("#autoLOC_270236", body.displayName), 
				Vessel.Situations.SPLASHED => Localizer.Format("#autoLOC_270238", body.displayName), 
				Vessel.Situations.PRELAUNCH => Localizer.Format("#autoLOC_270234", body.displayName), 
				Vessel.Situations.ESCAPING => Localizer.Format("#autoLOC_270240", body.displayName), 
				Vessel.Situations.ORBITING => Localizer.Format("#autoLOC_270237", body.displayName), 
				Vessel.Situations.SUB_ORBITAL => Localizer.Format("#autoLOC_270239", body.displayName), 
				_ => Localizer.Format("#autoLOC_270241"), 
			};
		}
		return sit switch
		{
			Vessel.Situations.FLYING => Localizer.Format("#autoLOC_270249", body.name, biomeName), 
			Vessel.Situations.LANDED => Localizer.Format("#autoLOC_270250", body.name, biomeName), 
			Vessel.Situations.SPLASHED => Localizer.Format("#autoLOC_270252", body.name, biomeName), 
			Vessel.Situations.PRELAUNCH => Localizer.Format("#autoLOC_270248", body.name, biomeName), 
			Vessel.Situations.ESCAPING => Localizer.Format("#autoLOC_270254", body.name, biomeName), 
			Vessel.Situations.ORBITING => Localizer.Format("#autoLOC_270251", body.name, biomeName), 
			Vessel.Situations.SUB_ORBITAL => Localizer.Format("#autoLOC_270253", body.name, biomeName), 
			_ => Localizer.Format("#autoLOC_270255"), 
		};
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("dest"))
		{
			Destination = FlightGlobals.Bodies[int.Parse(node.GetValue("dest"))];
		}
		if (node.HasValue("sit"))
		{
			Situation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), node.GetValue("sit"));
		}
		if (node.HasValue("biome"))
		{
			BiomeName = node.GetValue("biome");
		}
		if (node.HasValue("maxAlt"))
		{
			maxAltitude = double.Parse(node.GetValue("maxAlt"));
		}
		if (node.HasValue("minAlt"))
		{
			minAltitude = double.Parse(node.GetValue("minAlt"));
		}
		if (node.HasValue("maxSpd"))
		{
			maxSpeed = double.Parse(node.GetValue("maxSpd"));
		}
		if (node.HasValue("minSpd"))
		{
			minSpeed = double.Parse(node.GetValue("minSpd"));
		}
		if (node.HasValue("useAltLimit"))
		{
			useAltLimits = bool.Parse(node.GetValue("useAltLimit"));
		}
		if (node.HasValue("useSpdLimit"))
		{
			useSpdLimits = bool.Parse(node.GetValue("useSpdLimit"));
		}
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("dest", FlightGlobals.Bodies.IndexOf(Destination));
		node.AddValue("sit", Situation.ToString());
		node.AddValue("biome", BiomeName);
		node.AddValue("maxAlt", maxAltitude);
		node.AddValue("minAlt", minAltitude);
		node.AddValue("maxSpd", maxSpeed);
		node.AddValue("minSpd", minSpeed);
		node.AddValue("useAltLimit", useAltLimits);
		node.AddValue("useSpdLimit", useSpdLimits);
		node.AddValue("title", title);
	}

	public override void OnRegister()
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			GameEvents.onFlightReady.Add(OnFlightReady);
			GameEvents.onVesselChange.Add(OnVesselChange);
			GameEvents.onVesselSituationChange.Add(OnVesselSituationChange);
			trackerActive = true;
		}
		else if (base.State == ParameterState.Complete && base.Root.ContractState != Contract.State.Completed)
		{
			SetIncomplete();
		}
	}

	public override void OnUnregister()
	{
		if (trackerActive)
		{
			trackerActive = false;
			GameEvents.onFlightReady.Remove(OnFlightReady);
			GameEvents.onVesselChange.Remove(OnVesselChange);
			GameEvents.onVesselSituationChange.Remove(OnVesselSituationChange);
		}
	}

	public void OnFlightReady()
	{
		tgtVessel = FlightGlobals.ActiveVessel;
		ContractSystem.Instance.StartCoroutine(TrackVessel(tgtVessel));
	}

	public void OnVesselChange(Vessel v)
	{
		tgtVessel = v;
		ContractSystem.Instance.StartCoroutine(TrackVessel(tgtVessel));
	}

	public void OnVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> v)
	{
		if (v.host == tgtVessel)
		{
			ContractSystem.Instance.StartCoroutine(TrackVessel(tgtVessel));
		}
	}

	public IEnumerator TrackVessel(Vessel v)
	{
		Debug.Log("[Parameter.ReachFlightEnvelope]: Tracking Started for " + v.GetDisplayName());
		while (v == tgtVessel && v.situation == Situation && base.Root.ContractState == Contract.State.Active)
		{
			if (base.State == ParameterState.Incomplete && checkVesselWithinFlightEnvelope(tgtVessel))
			{
				if (checkVesselWithinBiome(tgtVessel))
				{
					SetComplete();
				}
			}
			else if ((base.State == ParameterState.Complete && !checkVesselWithinFlightEnvelope(tgtVessel)) || !checkVesselWithinBiome(tgtVessel))
			{
				SetIncomplete();
			}
			yield return null;
		}
		if (base.State == ParameterState.Complete && base.Root.ContractState != Contract.State.Completed)
		{
			SetIncomplete();
		}
	}

	public bool checkVesselWithinBiome(Vessel v)
	{
		if (!string.IsNullOrEmpty(BiomeName))
		{
			if (v.mainBody.BiomeMap != null)
			{
				return v.mainBody.BiomeMap.GetAtt(v.latitude, v.longitude).name == BiomeName;
			}
			return false;
		}
		return true;
	}

	public bool checkVesselWithinFlightEnvelope(Vessel v)
	{
		if (v.situation != Situation)
		{
			return false;
		}
		switch (Situation)
		{
		case Vessel.Situations.FLYING:
			if (v.srfSpeed < maxSpeed && v.srfSpeed >= minSpeed && v.altitude < maxAltitude && v.altitude >= minAltitude)
			{
				return true;
			}
			break;
		case Vessel.Situations.LANDED:
		case Vessel.Situations.SPLASHED:
		case Vessel.Situations.PRELAUNCH:
			if (v.srfSpeed < maxSpeed && v.srfSpeed >= minSpeed)
			{
				return true;
			}
			break;
		case Vessel.Situations.SUB_ORBITAL:
		case Vessel.Situations.ORBITING:
		case Vessel.Situations.ESCAPING:
			if (v.obt_speed < maxSpeed && v.obt_speed >= minSpeed && v.altitude < maxAltitude && v.altitude >= minAltitude)
			{
				return true;
			}
			break;
		}
		return false;
	}
}
