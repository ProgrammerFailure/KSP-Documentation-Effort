using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DeltaVGlobals : MonoBehaviour
{
	public static bool ready;

	public DeltaVAppValues deltaVAppValues;

	private static DeltaVGlobals _fetch;

	private static bool _doStockSimulations;

	[SerializeField]
	private static List<int> propellantsToIgnore;

	public static DeltaVAppValues DeltaVAppValues
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static DeltaVGlobals fetch
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool DoStockSimulations
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static List<int> PropellantsToIgnore
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DeltaVGlobals()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitDeltaVGlobal(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> scn)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void InitInfoLines()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool EnableStockSimluations()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool DisableStockSimluations()
	{
		throw null;
	}
}
