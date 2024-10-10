using System;
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

	public float textureSpeed = 2f;

	public float textureScale = 1f;

	public float falloff1 = 1f;

	public float falloff2 = 1f;

	public float falloff3 = 1f;

	public float wobble = 1f;

	public float intensity = 0.11f;

	public int shaderLOD;

	public float cameraSqrDistance;

	public int occlusionMapResolution = 512;

	public Camera velocityCam;

	public RenderTexture velocityDepth;

	public Shader depthDebugShader;

	public bool debugShader;

	public float projectionBias = 0.001f;

	public float projectionRange = 50f;

	public float cameraDistance = 10f;

	public Camera _camera;

	public float startFarClipPlane = 300f;

	public float farClipPlaneBuffer = 100f;

	public bool applyObliqueness = true;

	public static FXCamera Instance { get; set; }

	public void Awake()
	{
		Instance = this;
	}

	public void Start()
	{
		GameEvents.onGameSceneLoadRequested.Add(OnSceneSwitch);
		GameEvents.OnCameraChange.Add(OnCameraChange);
		GameEvents.OnGameSettingsApplied.Add(OnSettingsUpdate);
		GameEvents.OnCameraDistanceAdjustedToFitVessel.Add(OnCameraDistanceAdjustedToFitVessel);
		if (debugShader)
		{
			this.GetComponentCached(ref _camera).SetReplacementShader(depthDebugShader, "RenderType");
		}
		else if (ReplacementShaders.Length != 0)
		{
			this.GetComponentCached(ref _camera).SetReplacementShader(ReplacementShaders[shaderLOD], "RenderType");
		}
		startFarClipPlane = this.GetComponentCached(ref _camera).farClipPlane;
		if (!Application.isPlaying)
		{
			return;
		}
		if ((bool)GameObject.Find("velocity camera"))
		{
			UnityEngine.Object.DestroyImmediate(GameObject.Find("velocity camera"));
		}
		if (velocityDepth != null)
		{
			UnityEngine.Object.DestroyImmediate(velocityDepth);
			velocityDepth = null;
		}
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
		{
			velocityCam = new GameObject("velocity camera").AddComponent<Camera>();
			velocityCam.orthographic = true;
			velocityCam.cullingMask = 1;
			velocityCam.orthographicSize = 3f;
			velocityCam.nearClipPlane = 0f;
			velocityCam.farClipPlane = projectionRange;
			velocityCam.clearFlags = CameraClearFlags.Color;
			velocityCam.backgroundColor = Color.white;
			switch (GameSettings.AERO_FX_QUALITY)
			{
			case 0:
				occlusionMapResolution = 256;
				break;
			default:
				occlusionMapResolution = 512;
				break;
			case 2:
				occlusionMapResolution = 1024;
				break;
			case 3:
				occlusionMapResolution = 2048;
				break;
			}
			velocityDepth = new RenderTexture(occlusionMapResolution, occlusionMapResolution, 32, RenderTextureFormat.Depth);
			velocityCam.targetTexture = velocityDepth;
			velocityCam.depthTextureMode = DepthTextureMode.Depth;
			velocityCam.transform.parent = FlightCamera.fetch.transform.parent;
			velocityCam.transform.localPosition = Vector3.zero;
		}
	}

	public void OnSceneSwitch(GameScenes scene)
	{
		_camera.targetTexture = null;
		velocityCam.targetTexture = null;
		UnityEngine.Object.DestroyImmediate(base.gameObject);
		UnityEngine.Object.DestroyImmediate(velocityCam);
		UnityEngine.Object.DestroyImmediate(velocityDepth);
		velocityDepth = null;
	}

	public void OnSettingsUpdate()
	{
		if (velocityCam != null)
		{
			velocityCam.targetTexture = null;
		}
		if (velocityDepth != null)
		{
			UnityEngine.Object.DestroyImmediate(velocityDepth);
			velocityDepth = null;
		}
		if (velocityCam != null)
		{
			switch (GameSettings.AERO_FX_QUALITY)
			{
			case 0:
				occlusionMapResolution = 256;
				break;
			default:
				occlusionMapResolution = 512;
				break;
			case 2:
				occlusionMapResolution = 1024;
				break;
			case 3:
				occlusionMapResolution = 2048;
				break;
			}
			velocityDepth = new RenderTexture(occlusionMapResolution, occlusionMapResolution, 32, RenderTextureFormat.Depth);
			velocityCam.targetTexture = velocityDepth;
		}
	}

	public void OnCameraChange(CameraManager.CameraMode cameraMode)
	{
		if (cameraMode != CameraManager.CameraMode.const_3 && cameraMode != CameraManager.CameraMode.Internal)
		{
			if (_camera != null)
			{
				_camera.depth = 3f;
			}
		}
		else if (_camera != null)
		{
			_camera.depth = 2f;
		}
	}

	public void LateUpdate()
	{
		if (!this.GetComponentCached(ref _camera).enabled)
		{
			if ((bool)velocityCam && velocityCam.enabled)
			{
				velocityCam.enabled = false;
			}
			return;
		}
		if ((bool)velocityCam && !velocityCam.enabled)
		{
			velocityCam.enabled = true;
		}
		if (!(FlightCamera.fetch != null))
		{
			return;
		}
		this.GetComponentCached(ref _camera).fieldOfView = FlightCamera.fetch.FieldOfView;
		this.GetComponentCached(ref _camera).farClipPlane = Mathf.Max(startFarClipPlane, FlightCamera.fetch.Distance + farClipPlaneBuffer);
		if (FlightCamera.fetch.Target != null)
		{
			cameraDistance = FlightCamera.fetch.Distance;
			Vector3 vesselSize = FlightGlobals.ActiveVessel.vesselSize;
			float num = Mathf.Sqrt(vesselSize.x * vesselSize.x + vesselSize.y * vesselSize.y + vesselSize.z * vesselSize.z);
			cameraDistance = Mathf.Max(cameraDistance, num);
			if ((bool)velocityCam)
			{
				velocityCam.transform.position = velocityCam.transform.parent.position - effectDirection * cameraDistance;
				velocityCam.orthographicSize = cameraDistance;
				velocityCam.farClipPlane = Mathf.Max(projectionRange, cameraDistance + num);
				velocityCam.transform.forward = effectDirection;
			}
		}
	}

	public void OnGUI()
	{
		if ((bool)velocityDepth && debugShader)
		{
			GUI.DrawTexture(new Rect(0f, 0f, Math.Min(occlusionMapResolution, 256f), Math.Min(occlusionMapResolution, 256f)), velocityDepth);
		}
	}

	public void OnPreRender()
	{
		if (this.GetComponentCached(ref _camera).enabled)
		{
			if (debugShader)
			{
				this.GetComponentCached(ref _camera).SetReplacementShader(depthDebugShader, "RenderType");
			}
			else if (ReplacementShaders.Length != 0)
			{
				shaderLOD = Mathf.Clamp(shaderLOD, 0, ReplacementShaders.Length - 1);
				this.GetComponentCached(ref _camera).SetReplacementShader(ReplacementShaders[shaderLOD], "RenderType");
			}
			Shader.SetGlobalVector("_LightDirection0", effectDirection);
			Shader.SetGlobalColor("_FXColor", new Color(1f, 1f, 1f, intensity * (float)(shaderLOD + 1)));
			Shader.SetGlobalColor("_MyLightColor0", color);
			Shader.SetGlobalFloat("_FxLength", length);
			Shader.SetGlobalFloat("_EdgeFade", edgeFade);
			Shader.SetGlobalFloat("_Obliqueness", obliqueness);
			Shader.SetGlobalFloat("_FXTextureSpeed", textureSpeed);
			Shader.SetGlobalFloat("_FXTextureScale", textureScale);
			Shader.SetGlobalTexture("_FXMainTex", texture);
			if ((bool)velocityCam)
			{
				Shader.SetGlobalTexture("_FXDepthMap", velocityDepth);
				Shader.SetGlobalMatrix("_FXDepthCamMatrix", velocityCam.worldToCameraMatrix);
				Shader.SetGlobalMatrix("_FXDepthProjMatrix", velocityCam.projectionMatrix);
				Shader.SetGlobalVector("_FXCameraPos", new Vector4(velocityCam.transform.position.x, velocityCam.transform.position.y, velocityCam.transform.position.z, 0f));
				Shader.SetGlobalFloat("_FXProjectionNear", velocityCam.nearClipPlane);
				Shader.SetGlobalFloat("_FXProjectionFar", velocityCam.farClipPlane);
				Shader.SetGlobalFloat("_FXProjectionBias", projectionBias);
			}
			Shader.SetGlobalMatrix("_FXWorld2Velocity", Matrix4x4.TRS(Vector3.zero, Quaternion.FromToRotation(Vector3.up, effectDirection), Vector3.one));
			Shader.SetGlobalFloat("_FXFalloff", falloff1);
			Shader.SetGlobalFloat("_FXFalloff2", falloff2);
			Shader.SetGlobalFloat("_FXFalloff3", falloff3);
			Shader.SetGlobalFloat("_FXWobble", wobble);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneSwitch);
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		GameEvents.OnGameSettingsApplied.Remove(OnSettingsUpdate);
		GameEvents.OnCameraDistanceAdjustedToFitVessel.Remove(OnCameraDistanceAdjustedToFitVessel);
		UnityEngine.Object.DestroyImmediate(velocityDepth);
		velocityDepth = null;
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnCameraDistanceAdjustedToFitVessel(Vessel v, float newDistance, Vector3 newBounds)
	{
		farClipPlaneBuffer = Mathf.Max(Mathf.Max(newBounds.x, newBounds.y), newBounds.z);
	}

	public void ApplyObliqueness(float ob)
	{
	}
}
