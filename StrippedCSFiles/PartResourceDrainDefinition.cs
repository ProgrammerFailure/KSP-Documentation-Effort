using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class PartResourceDrainDefinition
{
	[SerializeField]
	private bool _isDrainable;

	[SerializeField]
	private bool _showDrainFX;

	[SerializeField]
	private int _drainFXPriority;

	[SerializeField]
	private float _drainForceISP;

	[SerializeField]
	private string _drainFXDefinition;

	public bool isDrainable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool showDrainFX
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int drainFXPriority
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float drainForceISP
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string drainFXDefinition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDrainDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDrainDefinition(bool drainable, bool showFX, int priority, float drainISP, string fxName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
