using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Serenity.RobotArmFX;

[Serializable]
public abstract class RobotArmScannerFX : ScriptableObject, IConfigNode
{
	public string className;

	public float effectStartTime;

	public float effectStopTime;

	public bool IsReady
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RobotArmScannerFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnStart(Part part)
	{
		throw null;
	}

	public abstract void OnEffectStart();

	public abstract void OnUpdate(float animationTime, float distanceFromSurface, Vector3 instrumentTargetPosition);

	public abstract void OnEffectStop();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RobotArmScannerFX CreateInstanceOfRobotArmScannerFX(string className)
	{
		throw null;
	}
}
