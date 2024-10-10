using System;
using System.Collections.Generic;
using UnityEngine;

namespace KSPAchievements;

[Serializable]
public class CelestialBodySubtree : ProgressNode
{
	public CelestialBody Body;

	public CelestialBodyFlyby flyBy;

	public CelestialBodyOrbit orbit;

	public CelestialBodyEscape escape;

	public CelestialBodySuborbit suborbit;

	public CelestialBodyFlight flight;

	public CelestialBodyLanding landing;

	public CelestialBodySplashdown splashdown;

	public CelestialBodyTransfer crewTransfer;

	public CelestialBodyScience science;

	public Rendezvous rendezvous;

	public Docking docking;

	public Spacewalk spacewalk;

	public SurfaceEVA surfaceEVA;

	public FlagPlant flagPlant;

	public BaseConstruction baseConstruction;

	public StationConstruction stationConstruction;

	public CelestialBodyReturn returnFromFlyby;

	public CelestialBodyReturn returnFromOrbit;

	public CelestialBodyReturn returnFromSurface;

	[NonSerialized]
	public CelestialBodySubtree parentTree;

	[NonSerialized]
	public CelestialBodySubtree[] childTrees;

	public CelestialBodySubtree(CelestialBody body)
		: base(body.name, startReached: false)
	{
		Body = body;
		body.progressTree = this;
		orbit = new CelestialBodyOrbit(body);
		base.Subtree.AddNode(orbit);
		escape = new CelestialBodyEscape(body);
		base.Subtree.AddNode(escape);
		landing = new CelestialBodyLanding(body);
		base.Subtree.AddNode(landing);
		splashdown = new CelestialBodySplashdown(body);
		base.Subtree.AddNode(splashdown);
		science = new CelestialBodyScience(body);
		base.Subtree.AddNode(science);
		rendezvous = new Rendezvous(body);
		base.Subtree.AddNode(rendezvous);
		docking = new Docking(body);
		base.Subtree.AddNode(docking);
		spacewalk = new Spacewalk(body);
		base.Subtree.AddNode(spacewalk);
		surfaceEVA = new SurfaceEVA(body);
		base.Subtree.AddNode(surfaceEVA);
		flagPlant = new FlagPlant(body);
		base.Subtree.AddNode(flagPlant);
		baseConstruction = new BaseConstruction(body);
		base.Subtree.AddNode(baseConstruction);
		stationConstruction = new StationConstruction(body);
		base.Subtree.AddNode(stationConstruction);
		crewTransfer = new CelestialBodyTransfer(body);
		base.Subtree.AddNode(crewTransfer);
		returnFromOrbit = new CelestialBodyReturn(body, ReturnFrom.Orbit);
		base.Subtree.AddNode(returnFromOrbit);
		if (!body.isHomeWorld)
		{
			flight = new CelestialBodyFlight(body);
			base.Subtree.AddNode(flight);
			suborbit = new CelestialBodySuborbit(body);
			base.Subtree.AddNode(suborbit);
			flyBy = new CelestialBodyFlyby(body);
			base.Subtree.AddNode(flyBy);
			returnFromFlyby = new CelestialBodyReturn(body, ReturnFrom.FlyBy);
			base.Subtree.AddNode(returnFromFlyby);
			returnFromSurface = new CelestialBodyReturn(body, ReturnFrom.Surface);
			base.Subtree.AddNode(returnFromSurface);
		}
		OnDeploy = delegate
		{
			GameEvents.OnProgressReached.Add(OnAchievementAchieve);
			base.Subtree.Deploy();
		};
		OnIterateVessels = delegate(Vessel v)
		{
			base.Subtree.IterateVessels(v);
		};
		OnStow = delegate
		{
			GameEvents.OnProgressReached.Remove(OnAchievementAchieve);
			base.Subtree.Stow();
		};
		OnGenerateSummary = generateSubtreeSummary;
	}

	public void OnAchievementAchieve(ProgressNode node)
	{
		if (!base.IsReached && base.Subtree.Contains(node))
		{
			Reach();
		}
		if (base.Subtree.AllComplete())
		{
			if (!base.IsComplete)
			{
				Complete();
			}
			Achieve();
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.Subtree.Save(node);
	}

	public override void OnLoad(ConfigNode node)
	{
		base.Subtree.Load(node);
	}

	public string generateSubtreeSummary(string baseID)
	{
		return base.Subtree.GetTreeSummary(Body.name);
	}

	public void LinkBodyHome(CelestialBodySubtree[] trees)
	{
		int num = Body.orbitingBodies.Count;
		if (Body.isHomeWorld)
		{
			num++;
		}
		if (num == 0)
		{
			childTrees = new CelestialBodySubtree[0];
			return;
		}
		List<CelestialBodySubtree> list = new List<CelestialBodySubtree>();
		for (int i = 0; i < Body.orbitingBodies.Count; i++)
		{
			CelestialBody celestialBody = Body.orbitingBodies[i];
			if (celestialBody == Body || celestialBody.isHomeWorld)
			{
				continue;
			}
			int j = 0;
			for (int num2 = trees.Length; j < num2; j++)
			{
				trees[j] = trees[j];
				if (trees[j].Body == celestialBody)
				{
					list.Add(trees[j]);
					trees[j].parentTree = this;
					trees[j].LinkBodyHome(trees);
					break;
				}
			}
		}
		if (Body.isHomeWorld)
		{
			int k = 0;
			for (int num3 = trees.Length; k < num3; k++)
			{
				if (trees[k].Body == Body.referenceBody)
				{
					list.Add(trees[k]);
					trees[k].parentTree = this;
					trees[k].LinkBodyHome(trees);
					break;
				}
			}
		}
		childTrees = list.ToArray();
		Array.Sort(childTrees, (CelestialBodySubtree a, CelestialBodySubtree b) => a.Body.scienceValues.RecoveryValue.CompareTo(b.Body.scienceValues.RecoveryValue));
	}

	public void DebugBodyTree()
	{
		Debug.Log(Body.bodyName + " " + base.IsReached);
		int i = 0;
		for (int num = childTrees.Length; i < num; i++)
		{
			childTrees[i].DebugBodyTree();
		}
	}
}
