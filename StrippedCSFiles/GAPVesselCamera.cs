using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GAPVesselCamera : MonoBehaviour
{
	public enum CameraMode
	{
		VesselMode,
		ObjectMode
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GAPVesselCamera _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CStart_003Ed__44(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public float initialPitch;

	public float initialHeading;

	public float initialHeight;

	public float camPitch;

	public float camHdg;

	public Vector3 offset;

	public float maxDisplaceX;

	public float maxDisplaceZ;

	public float offsetSensitivityAtMinDist;

	public float offsetSensitivityAtMaxDist;

	public float minZoom;

	public float maxZoom;

	public float minPitch;

	public float maxPitch;

	public float orbitSensitivity;

	public float zoomSensitivity;

	public float sharpness;

	public Transform vesselRoot;

	[SerializeField]
	private Quaternion endRot;

	[SerializeField]
	private Vector3 endPos;

	[SerializeField]
	private Transform pivotTransform;

	[SerializeField]
	private Transform transformCache;

	[SerializeField]
	private Camera cameraCache;

	private float distance;

	public float scrollHeight;

	public float startDistance;

	public float maxDistance;

	public float minDistance;

	public float minHeight;

	public float maxHeight;

	private float clampedScrollHeight;

	public float mouseZoomSensitivity;

	public float offsetSensitivity;

	private ShipConstruct shipCache;

	public EditorBounds editorBounds;

	[SerializeField]
	private Vector3 SPHeditorBoundsExtents;

	[SerializeField]
	private Vector3 VABeditorBoundsExtents;

	private bool setUp;

	public CameraMode cameraMode;

	public bool Camera_VAB_Controls;

	[SerializeField]
	private Light vesselLight;

	public float Distance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPVesselCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__44))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateCameraControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVABCameraControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVABCamera(bool smooth = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSPHCameraControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateSPHCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Rotate(Vector2 delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Zoom(float delta)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusVessel(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlaceCamera(Vector3 focusPoint, float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusPoint(Vector3 point, float minSize, float maxSize)
	{
		throw null;
	}
}
