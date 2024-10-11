using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class ROCDefinition
{
	[SerializeField]
	public string type;

	[SerializeField]
	public string displayName;

	[SerializeField]
	public string prefabName;

	[SerializeField]
	public string modelName;

	[SerializeField]
	public bool orientateUp;

	[SerializeField]
	public float depth;

	[SerializeField]
	public bool canBeTaken;

	[SerializeField]
	public float frequency;

	[SerializeField]
	public bool castShadows;

	[SerializeField]
	public bool receiveShadows;

	[SerializeField]
	public float collisionThreshold;

	[SerializeField]
	public bool smallRoc;

	[SerializeField]
	public bool randomDepth;

	[SerializeField]
	public bool randomOrientation;

	[SerializeField]
	public bool randomRotation;

	[SerializeField]
	public float burstEmitterMinWait;

	[SerializeField]
	public float burstEmitterMaxWait;

	[SerializeField]
	public float sfxVolume;

	[SerializeField]
	public string idleClipPath;

	[SerializeField]
	public string burstClipPath;

	public List<RocCBDefinition> myCelestialBodies;

	[SerializeField]
	public List<Vector3> localSpaceScanPoints;

	[SerializeField]
	public float scale;

	public float vfxBaseForce;

	public FloatCurve vfxCurveForce;

	public bool applyForces;

	public Vector2 vfxForceRadius;

	public Vector3 forceDirection;

	public Vector3 radiusCenter;

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal ROCDefinition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROCDefinition(string type, string displayName, string prefabName, string modelName, bool orientateUp, float depth, bool canBeTaken, float frequency, bool castShadows, bool receiveShadows, float collisionThreshold, bool smallRoc, bool randomDepth, bool randomOrientation, List<Vector3> localSpaceScanPoints, float burstEmitterMinWait, float burstEmitterMaxWait, bool randomRotation, float scale, float sfxVolume, string idleClipPath, string burstClipPath, FloatCurve vfxCurveForce, float vfxBaseForce, Vector2 vfxForceRadius, Vector3 forceDirection, Vector3 radiusCenter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ContainsCBBiome(string cbName, string biomeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
