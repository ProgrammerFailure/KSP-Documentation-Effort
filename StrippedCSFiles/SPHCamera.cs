using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class SPHCamera : MonoBehaviour, IKSPCamera
{
	[CompilerGenerated]
	private sealed class _003CStart_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public SPHCamera _003C_003E4__this;

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
		public _003CStart_003Ed__35(int _003C_003E1__state)
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

	public float minHeight;

	public float maxHeight;

	public float minPitch;

	public float maxPitch;

	public float startDistance;

	public float maxDistance;

	public float minDistance;

	private float distance;

	private GameObject pivot;

	private Vector3 endPos;

	private Quaternion endRot;

	public float orbitSensitivity;

	public float mouseZoomSensitivity;

	public float offsetSensitivityAtMinDist;

	public float offsetSensitivityAtMaxDist;

	private float offsetSensitivity;

	public float sharpness;

	public float scrollHeight;

	private float clampedScrollHeight;

	public float camPitch;

	public float camHdg;

	public float initialHeight;

	public float initialPitch;

	public float initialHeading;

	public float camInitialMinDistance;

	public Vector3 offset;

	public float maxDisplaceX;

	public float maxDisplaceZ;

	public Cubemap SPHReflection;

	public float Distance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion pivotRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 pivotPosition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SPHCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__35))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorShipLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetInitialCameraPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PlaceCamera(Vector3 focusPoint, float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Transform GetCameraTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCamCoordsFromPosition(Vector3 wPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Func<bool> OnNavigatorTakeOver(Callback onRequestControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnNavigatorHandoff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion getReferenceFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float getPitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float getYaw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool OnNavigatorRequestControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[SpecialName]
	bool IKSPCamera.get_enabled()
	{
		throw null;
	}
}
