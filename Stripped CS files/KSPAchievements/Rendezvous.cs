using System.Collections;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace KSPAchievements;

public class Rendezvous : ProgressNode
{
	public CelestialBody body;

	public double maxClosingSpeed = 50.0;

	public Rendezvous(CelestialBody cb)
		: base("Rendezvous", startReached: false)
	{
		body = cb;
		OnDeploy = delegate
		{
			GameEvents.onVesselLoaded.Add(OnVesselLoaded);
		};
		OnStow = delegate
		{
			GameEvents.onVesselLoaded.Remove(OnVesselLoaded);
		};
	}

	public void OnVesselLoaded(Vessel v)
	{
		if (!(v == FlightGlobals.ActiveVessel) && !(v.mainBody != body) && !v.isEVA && !FlightGlobals.ActiveVessel.isEVA && (v.situation == Vessel.Situations.ORBITING || v.situation == Vessel.Situations.ESCAPING) && v.rootPart.missionID != FlightGlobals.ActiveVessel.rootPart.missionID)
		{
			v.StartCoroutine(TrackCandidateVessel(v, FlightGlobals.ActiveVessel));
		}
	}

	public IEnumerator TrackCandidateVessel(Vessel candidate, Vessel reference)
	{
		Debug.Log("[Rendezvous]: Now tracking " + candidate.GetDisplayName() + " vs. " + reference.GetDisplayName());
		while (!base.IsComplete && candidate != null && candidate.loaded && reference != null && reference.loaded)
		{
			if ((candidate.obt_velocity - reference.obt_velocity).sqrMagnitude < maxClosingSpeed * maxClosingSpeed)
			{
				CrewSensitiveComplete(reference);
				if (!base.IsComplete)
				{
					Complete();
					AwardProgressStandard(Localizer.Format("#autoLOC_298266", body.displayName), ProgressType.RENDEZVOUS, body);
				}
				Achieve();
			}
			yield return null;
		}
		Debug.Log("[Rendezvous]: Tracking " + candidate.GetDisplayName() + " vs. " + reference.GetDisplayName() + " ended.");
	}
}
