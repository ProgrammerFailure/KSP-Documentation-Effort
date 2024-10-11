using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class KnowledgeBase : MonoBehaviour
{
	public enum KbTargetType
	{
		Null,
		Vessel,
		CelestialBody,
		Unowned
	}

	public ApplicationLauncherButton appLauncherButtonPrefab;

	public KbApp[] appPrefabs;

	private bool showing;

	private UIList applauncherList;

	private List<KbApp> apps;

	public static KnowledgeBase Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KnowledgeBase()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Cleanup(GameEvents.FromToAction<GameScenes, GameScenes> action)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAppLauncherReady()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Show()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ActivateApps(KbTargetType targetType, MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMapFocusChange(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static double GetUnloadedVesselMass(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>> GetVesselCrewByAvailablePart(Vessel v)
	{
		throw null;
	}
}
