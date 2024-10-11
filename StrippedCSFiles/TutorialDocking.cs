using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialDocking : TutorialScenario
{
	public string stateName;

	public bool complete;

	private Texture2D navBallVectors;

	private Texture2D maneuverVectors;

	private TutorialPage welcome;

	private TutorialPage grappleDeploy;

	private TutorialPage asteroidCatched;

	private TutorialPage alignment;

	private TutorialPage attitude3;

	private TutorialPage lowerPe3;

	private TutorialPage closeToAsteroid;

	private TutorialPage conclusion;

	private TutorialPage conclusion2;

	private TutorialPage conclusion3;

	private TutorialPage conclusion4;

	private TutorialPage precision;

	private TutorialPage beginApproach;

	private TutorialPage deathBonusPage;

	private KFSMEvent fsmEvent;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialDocking()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}
}
