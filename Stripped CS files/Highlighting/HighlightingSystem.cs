using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace Highlighting;

[RequireComponent(typeof(Camera))]
public class HighlightingSystem : MonoBehaviour
{
	public static bool ppfxEnabled = true;

	public static readonly Color colorClear = new Color(0f, 0f, 0f, 0f);

	public static readonly string renderBufferName = "HighlightingSystem";

	public static readonly Matrix4x4 identityMatrix = Matrix4x4.identity;

	public const CameraEvent queue = CameraEvent.BeforeImageEffectsOpaque;

	public static RenderTargetIdentifier cameraTargetID;

	public static Mesh quad;

	public const int int_0 = 0;

	public const int D3D9 = 1;

	public const int D3D11 = 2;

	public static int graphicsDeviceVersion = 1;

	public float offsetFactor;

	public float offsetUnits;

	public CommandBuffer renderBuffer;

	public bool isDirty = true;

	public int cachedWidth = -1;

	public int cachedHeight = -1;

	public int cachedAA = -1;

	[SerializeField]
	public int _downsampleFactor = 4;

	[SerializeField]
	public int _iterations = 2;

	[SerializeField]
	public float _blurMinSpread = 0.65f;

	[SerializeField]
	public float _blurSpread = 0.25f;

	[SerializeField]
	public float _blurIntensity = 0.3f;

	public RenderTargetIdentifier highlightingBufferID;

	public RenderTexture highlightingBuffer;

	public Camera cam;

	public bool isSupported;

	public bool isDepthAvailable = true;

	public const int BLUR = 0;

	public const int int_1 = 1;

	public const int COMP = 2;

	public static readonly string[] shaderPaths = new string[3] { "Hidden/Highlighted/Blur", "Hidden/Highlighted/Cut", "Hidden/Highlighted/Composite" };

	public static Shader[] shaders;

	public static Material[] materials;

	public static Material cutMaterial;

	public static Material compMaterial;

	public Material blurMaterial;

	public static bool initialized = false;

	public static bool FxEnabled
	{
		get
		{
			return ppfxEnabled;
		}
		set
		{
			ppfxEnabled = value;
			if (ppfxEnabled)
			{
				HighlightingSystem[] array = Object.FindObjectsOfType<HighlightingSystem>();
				int num = array.Length;
				while (num-- > 0)
				{
					array[num].enabled = true;
				}
			}
		}
	}

	public int downsampleFactor
	{
		get
		{
			return _downsampleFactor;
		}
		set
		{
			if (_downsampleFactor != value)
			{
				if (value != 0 && (value & (value - 1)) == 0)
				{
					_downsampleFactor = value;
					isDirty = true;
				}
				else
				{
					Debug.LogWarning("HighlightingSystem : Prevented attempt to set incorrect downsample factor value.");
				}
			}
		}
	}

	public int iterations
	{
		get
		{
			return _iterations;
		}
		set
		{
			if (_iterations != value)
			{
				_iterations = value;
				isDirty = true;
			}
		}
	}

	public float blurMinSpread
	{
		get
		{
			return _blurMinSpread;
		}
		set
		{
			if (_blurMinSpread != value)
			{
				_blurMinSpread = value;
				isDirty = true;
			}
		}
	}

	public float blurSpread
	{
		get
		{
			return _blurSpread;
		}
		set
		{
			if (_blurSpread != value)
			{
				_blurSpread = value;
				isDirty = true;
			}
		}
	}

	public float blurIntensity
	{
		get
		{
			return _blurIntensity;
		}
		set
		{
			if (_blurIntensity != value)
			{
				_blurIntensity = value;
				if (Application.isPlaying)
				{
					blurMaterial.SetFloat(Highlighter._Intensity, _blurIntensity);
				}
			}
		}
	}

	public bool IsDirty
	{
		get
		{
			return isDirty;
		}
		set
		{
			isDirty = value;
		}
	}

	public bool IsSupported => isSupported;

	public bool IsDepthAvailable => isDepthAvailable;

	public virtual void Awake()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	public virtual void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	public virtual void OnSceneUnloaded(Scene scene)
	{
		base.enabled = false;
	}

	public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (GameSettings.HIGHLIGHT_FX && (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor))
		{
			if (base.gameObject.activeInHierarchy)
			{
				StartCoroutine(RefreshCoroutine());
			}
		}
		else
		{
			base.enabled = false;
		}
	}

	public virtual IEnumerator RefreshCoroutine(int n = 10)
	{
		while (n-- > 0)
		{
			yield return null;
		}
		base.enabled = false;
		base.enabled = true;
	}

	public virtual void Cycle()
	{
		OnDisable();
		OnEnable();
	}

	public virtual void Disable()
	{
		OnDisable();
	}

	public virtual void OnEnable()
	{
		Initialize();
		isSupported = CheckSupported();
		if (isSupported && GameSettings.HIGHLIGHT_FX && (HighLogic.LoadedSceneIsFlight || HighLogic.LoadedSceneIsEditor) && (!EVAConstructionModeController.Instance || !EVAConstructionModeController.Instance.IsOpen || EVAConstructionModeController.Instance.panelMode != 0))
		{
			ppfxEnabled = true;
			blurMaterial = new Material(materials[0]);
			blurMaterial.SetFloat(Highlighter._Intensity, _blurIntensity);
			renderBuffer = new CommandBuffer();
			renderBuffer.name = renderBufferName;
			cam = GetComponent<Camera>();
			UpdateHighlightingBuffer();
			isDirty = true;
			base.enabled = true;
			cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, renderBuffer);
		}
		else
		{
			base.enabled = false;
			ppfxEnabled = false;
		}
	}

	public virtual void OnDisable()
	{
		if (renderBuffer != null)
		{
			cam.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, renderBuffer);
			renderBuffer = null;
		}
		if (highlightingBuffer != null && highlightingBuffer.IsCreated())
		{
			highlightingBuffer.Release();
			highlightingBuffer = null;
		}
		base.enabled = false;
	}

	public virtual void OnPreRender()
	{
		UpdateHighlightingBuffer();
		int aA = GetAA();
		bool flag = aA == 1;
		if (aA > 1 && (cam.actualRenderingPath == RenderingPath.Forward || cam.actualRenderingPath == RenderingPath.VertexLit))
		{
			flag = false;
		}
		if (isDepthAvailable != flag)
		{
			isDepthAvailable = flag;
			Highlighter.SetZWrite(isDepthAvailable ? 0f : 1f);
			if (isDepthAvailable)
			{
				Debug.LogWarning("HighlightingSystem : Framebuffer depth data is available back again and will be used to occlude highlighting. Highlighting occluders disabled.");
			}
			else
			{
				Debug.LogWarning("HighlightingSystem : Framebuffer depth data is not available and can't be used to occlude highlighting. Highlighting occluders enabled.");
			}
			isDirty = true;
		}
		Highlighter.SetOffsetFactor(offsetFactor);
		Highlighter.SetOffsetUnits(offsetUnits);
		isDirty |= Highlighter.HighlightDirty;
		isDirty |= HighlightersChanged();
		if (isDirty)
		{
			RebuildCommandBuffer();
			isDirty = false;
		}
	}

	public virtual void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		Graphics.Blit(src, dst, compMaterial);
	}

	public static void Initialize()
	{
		if (!initialized)
		{
			string text = SystemInfo.graphicsDeviceVersion.ToLower();
			if (!text.Contains("direct3d") && !text.Contains("directx"))
			{
				graphicsDeviceVersion = 0;
			}
			else if (!text.Contains("direct3d 11") && !text.Contains("directx 11"))
			{
				graphicsDeviceVersion = 1;
			}
			else
			{
				graphicsDeviceVersion = 2;
			}
			Highlighter.Initialize();
			int num = shaderPaths.Length;
			shaders = new Shader[num];
			materials = new Material[num];
			for (int i = 0; i < num; i++)
			{
				Shader shader = Shader.Find(shaderPaths[i]);
				shaders[i] = shader;
				Material material = new Material(shader);
				materials[i] = material;
			}
			cutMaterial = materials[1];
			compMaterial = materials[2];
			cameraTargetID = new RenderTargetIdentifier(BuiltinRenderTextureType.CameraTarget);
			CreateQuad();
			initialized = true;
		}
	}

	public static void CreateQuad()
	{
		if (quad == null)
		{
			quad = new Mesh();
		}
		else
		{
			quad.Clear();
		}
		float y = 1f;
		float y2 = -1f;
		if (graphicsDeviceVersion == 0)
		{
			y = -1f;
			y2 = 1f;
		}
		quad.vertices = new Vector3[4]
		{
			new Vector3(-1f, y, 0f),
			new Vector3(-1f, y2, 0f),
			new Vector3(1f, y2, 0f),
			new Vector3(1f, y, 0f)
		};
		quad.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		quad.colors = new Color[4] { colorClear, colorClear, colorClear, colorClear };
		quad.triangles = new int[6] { 0, 1, 2, 2, 3, 0 };
	}

	public virtual int GetAA()
	{
		int num = QualitySettings.antiAliasing;
		if (num == 0)
		{
			num = 1;
		}
		if (cam.actualRenderingPath == RenderingPath.DeferredLighting || cam.actualRenderingPath == RenderingPath.DeferredShading)
		{
			num = 1;
		}
		return num;
	}

	public virtual void UpdateHighlightingBuffer()
	{
		int aA = GetAA();
		if (cam.pixelWidth != cachedWidth || cam.pixelHeight != cachedHeight || aA != cachedAA || !(highlightingBuffer != null))
		{
			cachedWidth = cam.pixelWidth;
			cachedHeight = cam.pixelHeight;
			cachedAA = aA;
			if (highlightingBuffer != null && highlightingBuffer.IsCreated())
			{
				highlightingBuffer.Release();
			}
			highlightingBuffer = new RenderTexture(cachedWidth, cachedHeight, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
			highlightingBuffer.antiAliasing = cachedAA;
			highlightingBuffer.filterMode = FilterMode.Point;
			highlightingBuffer.useMipMap = false;
			highlightingBuffer.wrapMode = TextureWrapMode.Clamp;
			if (!highlightingBuffer.Create())
			{
				Debug.LogError("HighlightingSystem : UpdateHighlightingBuffer() : Failed to create highlightingBuffer RenderTexture!");
			}
			highlightingBufferID = new RenderTargetIdentifier(highlightingBuffer);
			Shader.SetGlobalTexture(Highlighter._HighlightingBuffer, highlightingBuffer);
			Vector4 value = new Vector4(((graphicsDeviceVersion == 0) ? 1f : (-1f)) / (float)highlightingBuffer.width, 1f / (float)highlightingBuffer.height, 0f, 0f);
			Shader.SetGlobalVector(Highlighter._HighlightingBufferTexelSize, value);
			isDirty = true;
		}
	}

	public virtual bool CheckInstance()
	{
		HighlightingSystem[] components = GetComponents<HighlightingSystem>();
		if (components.Length > 1 && components[0] != this)
		{
			base.enabled = false;
			string arg = GetType().ToString();
			Debug.LogWarning($"HighlightingSystem : Only single instance of the HighlightingRenderer component is allowed on a single Gameobject! {arg} has been disabled on GameObject with name '{base.name}'.");
			return false;
		}
		return true;
	}

	public static bool CheckSupported()
	{
		Initialize();
		if (!SystemInfo.supportsImageEffects)
		{
			Debug.LogWarning("HighlightingSystem : Image effects is not supported on this platform!");
			return false;
		}
		if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB32))
		{
			Debug.LogWarning("HighlightingSystem : RenderTextureFormat.ARGB32 is not supported on this platform!");
			return false;
		}
		if (!Highlighter.opaqueShader.isSupported)
		{
			Debug.LogWarning("HighlightingSystem : HighlightingOpaque shader is not supported on this platform!");
			return false;
		}
		if (!Highlighter.transparentShader.isSupported)
		{
			Debug.LogWarning("HighlightingSystem : HighlightingTransparent shader is not supported on this platform!");
			return false;
		}
		int num = 0;
		Shader shader;
		while (true)
		{
			if (num < shaders.Length)
			{
				shader = shaders[num];
				if (!shader.isSupported)
				{
					break;
				}
				num++;
				continue;
			}
			if (QualitySettings.antiAliasing == 0)
			{
				Debug.LogWarning("HighlightingSystem : Edge Highlighting requires AA to work!");
				return false;
			}
			return true;
		}
		Debug.LogWarning("HighlightingSystem : Shader '" + shader.name + "' is not supported on this platform!");
		return false;
	}

	public virtual bool HighlightersChanged()
	{
		bool flag = false;
		HashSet<Highlighter>.Enumerator enumerator = Highlighter.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Highlighter current = enumerator.Current;
			flag |= current.UpdateHighlighting(isDepthAvailable);
		}
		return flag;
	}

	public virtual void RebuildCommandBuffer()
	{
		renderBuffer.Clear();
		RenderTargetIdentifier depth = (isDepthAvailable ? cameraTargetID : highlightingBufferID);
		renderBuffer.SetRenderTarget(highlightingBufferID, depth);
		renderBuffer.ClearRenderTarget(!isDepthAvailable, clearColor: true, colorClear);
		FillBuffer(renderBuffer, 0);
		FillBuffer(renderBuffer, 1);
		FillBuffer(renderBuffer, 2);
		RenderTargetIdentifier renderTargetIdentifier = new RenderTargetIdentifier(Highlighter._HighlightingBlur1);
		RenderTargetIdentifier renderTargetIdentifier2 = new RenderTargetIdentifier(Highlighter._HighlightingBlur2);
		int width = highlightingBuffer.width / _downsampleFactor;
		int height = highlightingBuffer.height / _downsampleFactor;
		renderBuffer.GetTemporaryRT(Highlighter._HighlightingBlur1, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderBuffer.GetTemporaryRT(Highlighter._HighlightingBlur2, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
		renderBuffer.Blit(highlightingBufferID, renderTargetIdentifier);
		bool flag = true;
		for (int i = 0; i < _iterations; i++)
		{
			float value = _blurMinSpread + _blurSpread * (float)i;
			renderBuffer.SetGlobalFloat(Highlighter._HighlightingBlurOffset, value);
			if (flag)
			{
				renderBuffer.Blit(renderTargetIdentifier, renderTargetIdentifier2, blurMaterial);
			}
			else
			{
				renderBuffer.Blit(renderTargetIdentifier2, renderTargetIdentifier, blurMaterial);
			}
			flag = !flag;
		}
		renderBuffer.SetGlobalTexture(Highlighter._HighlightingBlurred, flag ? renderTargetIdentifier : renderTargetIdentifier2);
		renderBuffer.SetRenderTarget(highlightingBufferID, depth);
		renderBuffer.DrawMesh(quad, identityMatrix, cutMaterial);
		renderBuffer.ReleaseTemporaryRT(Highlighter._HighlightingBlur1);
		renderBuffer.ReleaseTemporaryRT(Highlighter._HighlightingBlur2);
	}

	public virtual void FillBuffer(CommandBuffer buffer, int renderQueue)
	{
		HashSet<Highlighter>.Enumerator enumerator = Highlighter.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.FillBuffer(renderBuffer, renderQueue);
		}
	}

	public void ReloadSettings()
	{
		isSupported = CheckSupported();
	}
}
