using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ROC : MonoBehaviour
{
	private class ScanPoint : IComparable<ScanPoint>
	{
		public Vector3 point;

		public Vector3 traceDirection;

		public float distance;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ScanPoint()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int CompareTo(ScanPoint otherPoint)
		{
			throw null;
		}
	}

	public string type;

	public string displayName;

	public string prefabName;

	public string modelName;

	public Transform modelTransform;

	public bool orientateup;

	public float depth;

	public bool canbetaken;

	public float frequency;

	public List<RocCBDefinition> myCelestialBodies;

	public bool castShadows;

	public bool recieveShadows;

	public float collisionThreshold;

	public bool smallROC;

	public int rocID;

	public bool randomDepth;

	public bool randomOrientation;

	public bool randomRotation;

	public List<Vector3> localSpaceScanPoints;

	public float burstEmitterMinWait;

	public float burstEmitterMaxWait;

	public ROCEmitter rocEmitter;

	[SerializeField]
	public float scale;

	public float sfxVolume;

	public string idleClipPath;

	public string burstClipPath;

	public Vector3 upDirection;

	public float vfxBaseForce;

	public FloatCurve vfxCurveForce;

	public bool applyForces;

	public Vector2 vfxForceRadius;

	public Vector3 forceDirection;

	public Vector3 radiusCenter;

	public float rocArrowLength;

	public float rocArrowXZScale;

	public ArrowPointer rocArrow;

	[SerializeField]
	private Color rocArrowColor;

	public float rocTransformLength;

	public float rocScanpointLength;

	public List<ArrowPointer> arrowScanPoints;

	[SerializeField]
	private Color rocTransformColor;

	[SerializeField]
	private Color rocScanColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ROC()
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
	private void SetRocEmitters(Transform objectTransform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStats(string type, string displayName, string prefabName, string modelName, bool orientateup, float depth, bool canbetaken, float frequency, List<RocCBDefinition> myCelestialBodies, bool castShadows, bool recieveShadows, float collisionthreshold, bool smallroc, bool randomdepth, bool randomorientation, List<Vector3> localSpaceScanPoints, float burstEmitterMinWait, float burstEmitterMaxWait, bool randomRotation, float scale, float sfxVolume, string idleClipPath, string burstClipPath, FloatCurve vfxCurveForce, float vfxBaseForce, bool applyForces, Vector2 vfxForceRadius, Vector3 forceDirection, Vector3 radiusCenter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetClosestScanPosition(Vector3 testPosition, out float bestDistanceToPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector3 GetClosestScanPositionWithRaycasts(Vector3 testPosition, out float distanceToPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CheckCollision(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PickUpROC()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool PerformExperiment(ModuleScienceExperiment moduleScienceExperiment)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateModelTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateRocArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyRocFinderArrow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void DestroyScanPointArrows()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetVFXForceScale(float burstClipTime)
	{
		throw null;
	}
}
