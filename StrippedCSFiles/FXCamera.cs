using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FXCamera : MonoBehaviour
{
	public Shader[] ReplacementShaders;

	public float length;

	public float edgeFade;

	public float obliqueness;

	public Vector3 effectDirection;

	public Color color;

	public Texture2D texture;

	public float textureSpeed;

	public float textureScale;

	public float falloff1;

	public float falloff2;

	public float falloff3;

	public float wobble;

	public float intensity;

	public int shaderLOD;

	public float cameraSqrDistance;

	public int occlusionMapResolution;

	private Camera velocityCam;

	private RenderTexture velocityDepth;

	public Shader depthDebugShader;

	public bool debugShader;

	public float projectionBias;

	public float projectionRange;

	public float cameraDistance;

	private Camera _camera;

	private float startFarClipPlane;

	private float farClipPlaneBuffer;

	public bool applyObliqueness;

	public static FXCamera Instance
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
	public FXCamera()
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
	private void OnSceneSwitch(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSettingsUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraChange(CameraManager.CameraMode cameraMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPreRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCameraDistanceAdjustedToFitVessel(Vessel v, float newDistance, Vector3 newBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplyObliqueness(float ob)
	{
		throw null;
	}
}
