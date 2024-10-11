using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlightCamera : MonoBehaviour, IKSPCamera
{
	public enum TargetMode
	{
		None,
		Vessel,
		Part,
		Transform
	}

	public enum Modes
	{
		[Description("#autoLOC_6003102")]
		AUTO,
		[Description("#autoLOC_6003103")]
		FREE,
		[Description("#autoLOC_6002359")]
		ORBITAL,
		[Description("#autoLOC_6003104")]
		CHASE,
		[Description("#autoLOC_6003105")]
		LOCKED
	}

	protected enum ChaseMode
	{
		Parked,
		Velocity,
		Target_SrfUp,
		Target_YUp,
		Null
	}

	[CompilerGenerated]
	private sealed class _003CStartup_003Ed__67 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public FlightCamera _003C_003E4__this;

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
		public _003CStartup_003Ed__67(int _003C_003E1__state)
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

	public float minPitch;

	public float maxPitch;

	public float startDistance;

	public float maxDistance;

	public float minDistance;

	public float minDistOnDestroy;

	public float maxDistOnDestroy;

	public float minHeightAtMaxDist;

	public float minHeightAtMinDist;

	public float minHeight;

	[SerializeField]
	private float distance;

	[SerializeField]
	private Transform pivot;

	[SerializeField]
	private Vector3 endPos;

	private Quaternion frameOfReference;

	private Quaternion tgtFoR;

	private Quaternion lastFoR;

	public float orbitSensitivity;

	public float zoomScaleFactor;

	public float sharpness;

	public float pivotTranslateSharpness;

	public float orientationSharpness;

	public float cameraWobbleSensitivity;

	public float camPitch;

	public float camHdg;

	public float cameraAlt;

	[SerializeField]
	private float camera00IVANearClipPlane;

	[SerializeField]
	private float camera00DefaultNearClipPlane;

	[SerializeField]
	private float terrainPitch;

	[SerializeField]
	private float meshPitch;

	private RaycastHit hit;

	private Transform target;

	public Vessel vesselTarget;

	public Part partTarget;

	public TargetMode targetMode;

	public Vector3 upAxis;

	public Vector3 tgtUpAxis;

	public Camera[] cameras;

	public Camera mainCamera;

	public GameObject AudioListenerGameObject;

	private AudioListener listener;

	private Rigidbody _rigidbody;

	public Modes mode;

	public Modes autoMode;

	public FoRModes FoRMode;

	private ScreenMessage cameraModeReadout;

	public static FlightCamera fetch;

	public FlightReflectionProbe reflectionProbe;

	[SerializeField]
	protected float endPitch;

	[SerializeField]
	protected float localPitch;

	[SerializeField]
	protected float lastLocalPitch;

	protected float offsetPitch;

	protected float offsetHdg;

	protected float tIRpitch;

	protected float tIRyaw;

	protected float tIRroll;

	protected Vector3 camFXPos;

	protected Quaternion camFXRot;

	protected Vector3 nextMove;

	public Vector3 targetDirection;

	public Vector3 endDirection;

	protected float FoRlerp;

	protected Vector3 chaseFwd;

	protected Vector3 chaseFUp;

	protected Vector3 chaseOrt;

	protected Quaternion chaseBaseRot;

	protected ChaseMode chaseMode;

	protected ChaseMode chaseModePrev;

	protected Modes chaseBaseMode;

	public bool updateActive;

	public float fovDefault;

	public float fovMin;

	public float fovMax;

	public float FieldOfView;

	public float vesselSwitchBackoffFOVFactor;

	public float vesselSwitchBackoffPadding;

	public Callback AbortExternalControl;

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

	public static float CamPitch
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

	public static float CamHdg
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

	public static int CamMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static FoRModes FrameOfReferenceMode
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform Target
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStartup_003Ed__67))]
	protected virtual IEnumerator Startup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnLevelLoaded(GameScenes level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearTarget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTarget(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTarget(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetTarget(Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTargetNone()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTargetVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTargetPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTargetTransform(Transform tgt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTarget(Transform tgt, TargetMode targetMode = TargetMode.Transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetTarget(Transform tgt, bool keepWorldPos, TargetMode targetMode = TargetMode.Transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDistance(float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDistanceImmediate(float dist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetMode(Modes m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void setMode(Modes m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetModeImmediate(Modes m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void setModeImmediate(Modes m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNextMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool TrackIRisActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void UpdateCameraTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion GetCameraFoR(FoRModes mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void updateFoR(Quaternion FoR, float lerpT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Modes GetAutoModeForVessel(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Quaternion GetChaseFoR(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void TargetActiveVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnTargetDestroyed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DeactivateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ActivateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void EnableCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableCamera(bool disableAudioListener)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void CycleCameraHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void DisableCameraHighlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetDefaultFoV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ResumeFoV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetFoV(float fov)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ResetFoV()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnSceneSwitch(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnVesselChange(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void AdjustDistanceToFit(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void MinDistanceBackaway()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Transform GetPivot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Transform GetCameraTransform()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetCamCoordsFromPosition(Vector3 wPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool OnNavigatorRequestControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Func<bool> OnNavigatorTakeOver(Callback RequestControl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnNavigatorHandoff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Quaternion getRotation(float pitch, float hdg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Quaternion getReferenceFrame()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float getPitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float getYaw()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraChange(CameraManager.CameraMode newMode)
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
