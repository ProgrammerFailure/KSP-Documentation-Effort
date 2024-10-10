using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Highlighting;

public class Highlighter : MonoBehaviour
{
	[Serializable]
	public class RendererCache
	{
		public struct Data
		{
			public Material material;

			public int submeshIndex;

			public bool transparent;
		}

		public static readonly string sRenderType = "RenderType";

		public static readonly string sOpaque = "Opaque";

		public static readonly string sTransparent = "Transparent";

		public static readonly string sTransparentCutout = "TransparentCutout";

		public static readonly string sMainTex = "_MainTex";

		public static readonly string sBackTex = "_BackTex";

		public const int opaquePassID = 0;

		public const int transparentPassID = 1;

		public GameObject go;

		public Renderer renderer;

		public List<Data> data;

		public bool visible { get; set; }

		public RendererCache(Renderer r, Material sharedOpaqueMaterial, float zTest, float stencilRef)
		{
			data = new List<Data>();
			renderer = r;
			go = r.gameObject;
			Material[] sharedMaterials = r.sharedMaterials;
			if (sharedMaterials != null)
			{
				for (int i = 0; i < sharedMaterials.Length; i++)
				{
					Material material = sharedMaterials[i];
					if (material == null)
					{
						continue;
					}
					Data item = default(Data);
					string tag = material.GetTag(sRenderType, searchFallbacks: true, sOpaque);
					if (!(tag == sTransparent) && !(tag == sTransparentCutout))
					{
						item.material = sharedOpaqueMaterial;
						item.transparent = false;
					}
					else
					{
						Material material2 = ((!material.HasProperty(_BackTex)) ? new Material(transparentShader) : new Material(transparentBackgroundShader));
						material2.SetFloat(_ZTest, zTest);
						material2.SetFloat(_StencilRef, stencilRef);
						if (r is SpriteRenderer)
						{
							material2.SetFloat(_Cull, 0f);
						}
						if (material.HasProperty(_MainTex))
						{
							material2.SetTexture(_MainTex, material.mainTexture);
							material2.SetTextureOffset(sMainTex, material.mainTextureOffset);
							material2.SetTextureScale(sMainTex, material.mainTextureScale);
						}
						if (material.HasProperty(_BackTex))
						{
							material2.SetTexture(_BackTex, material.GetTexture(sBackTex));
							material2.SetTextureOffset(sBackTex, material.GetTextureOffset(sBackTex));
							material2.SetTextureScale(sBackTex, material.GetTextureScale(sBackTex));
						}
						int cutoff = _Cutoff;
						material2.SetFloat(cutoff, material.HasProperty(cutoff) ? material.GetFloat(cutoff) : transparentCutoff);
						item.material = material2;
						item.transparent = true;
					}
					item.submeshIndex = i;
					data.Add(item);
				}
			}
			visible = !IsDestroyed() && IsVisible();
		}

		public bool UpdateVisibility()
		{
			bool flag = !IsDestroyed() && IsVisible();
			if (visible != flag)
			{
				visible = flag;
				return true;
			}
			return false;
		}

		public bool FillBuffer(CommandBuffer buffer)
		{
			if (IsDestroyed())
			{
				return false;
			}
			if (IsVisible())
			{
				int i = 0;
				for (int count = this.data.Count; i < count; i++)
				{
					Data data = this.data[i];
					buffer.DrawRenderer(renderer, data.material, data.submeshIndex);
				}
			}
			return true;
		}

		public void SetColorForTransparent(Color clr)
		{
			int i = 0;
			for (int count = this.data.Count; i < count; i++)
			{
				Data data = this.data[i];
				if (data.transparent)
				{
					data.material.SetColor(_Outline, clr);
				}
			}
		}

		public void SetZTestForTransparent(float zTest)
		{
			int i = 0;
			for (int count = this.data.Count; i < count; i++)
			{
				Data data = this.data[i];
				if (data.transparent)
				{
					data.material.SetFloat(_ZTest, zTest);
				}
			}
		}

		public void SetStencilRefForTransparent(float stencilRef)
		{
			int i = 0;
			for (int count = this.data.Count; i < count; i++)
			{
				Data data = this.data[i];
				if (data.transparent)
				{
					data.material.SetFloat(_StencilRef, stencilRef);
				}
			}
		}

		public bool IsVisible()
		{
			if (renderer.enabled)
			{
				return renderer.isVisible;
			}
			return false;
		}

		public bool IsDestroyed()
		{
			if (!(go == null))
			{
				return renderer == null;
			}
			return true;
		}

		public void CleanUp()
		{
			for (int i = 0; i < data.Count; i++)
			{
				if (data[i].material != null)
				{
					UnityEngine.Object.Destroy(data[i].material);
				}
			}
			data.Clear();
		}
	}

	public static float constantOnSpeed = 4.5f;

	public static float constantOffSpeed = 4f;

	public static float transparentCutoff = 0.5f;

	public const int highlightingLayer = 7;

	public static readonly List<Type> types = new List<Type>
	{
		typeof(MeshRenderer),
		typeof(SkinnedMeshRenderer)
	};

	public const float doublePI = (float)Math.PI * 2f;

	public readonly Color occluderColor = new Color(0f, 0f, 0f, 0f);

	public const float zTestLessEqual = 4f;

	public const float zTestAlways = 8f;

	public const float cullOff = 0f;

	public static float zWrite = -1f;

	public static float offsetFactor = float.NaN;

	public static float offsetUnits = float.NaN;

	public static Color colorPartHighlightDefault = new Color(0f, 1f, 0f, 1f);

	public static Color colorPartEditorAttached = new Color(0f, 1f, 0f, 1f);

	public static Color colorPartEditorDetached = new Color(1f, 0f, 0f, 1f);

	public static Color colorPartEditorActionSelected = new Color(0f, 0f, 1f, 1f);

	public static Color colorPartEditorActionHighlight = new Color(0f, 1f, 1f, 1f);

	public static Color colorPartEditorAxisSelected = new Color(0f, 0f, 1f, 1f);

	public static Color colorPartEditorAxisHighlight = new Color(0f, 1f, 1f, 1f);

	public static Color colorPartRootToolHighlight = new Color(0.03921569f, 0.5333334f, 46f / 85f, 0.3f);

	public static Color colorPartRootToolHighlightEdge = new Color(0f, 1f, 1f, 0.5f);

	public static Color colorPartRootToolHover = new Color(0.254902f, 0.9921569f, 0.9960784f, 1f);

	public static Color colorPartRootToolHoverEdge = new Color(0.04313726f, 83f / 85f, 0.9176471f, 1f);

	public static Color colorPartEngineerAppHighlight = new Color(1f, 0f, 0f, 1f);

	public static Color colorPartTransferSourceHighlight = new Color(66f / 85f, 0.3176471f, 0.007843138f, 1f);

	public static Color colorPartTransferSourceHover = new Color(1f, 0.694f, 0f, 1f);

	public static Color colorPartTransferDestHighlight = new Color(0.5490196f, 1f, 0.8588235f, 1f);

	public static Color colorPartTransferDestHover = new Color(0.04313726f, 83f / 85f, 0.9176471f, 1f);

	public static Color colorPartInventoryContainer = new Color(0.254902f, 0.9921569f, 0.9960784f, 1f);

	public static Color colorPartInventoryUnavailableSpace = Color.yellow;

	public static Color colorPartConstructionValid = new Color(0f, 1f, 0f, 1f);

	public static Color colorConstructionPartDropAsNewVessel = new Color(66f / 85f, 0.3176471f, 0.007843138f, 1f);

	public Transform tr;

	public List<RendererCache> highlightableRenderers;

	public int visibilityCheckFrame = -1;

	public bool visibilityChanged;

	public bool visible;

	public bool renderersDirty = true;

	public Color currentColor;

	public bool transitionActive;

	public float transitionValue;

	public float flashingFreq = 2f;

	public int _once;

	public Color onceColor = Color.red;

	public bool flashing;

	public Color flashingColorMin = new Color(0f, 1f, 1f, 0f);

	public Color flashingColorMax = new Color(0f, 1f, 1f, 1f);

	public bool constantly;

	public Color constantColor = Color.yellow;

	public bool occluder;

	public bool seeThrough = true;

	public int renderQueue = 1;

	public bool zTest = true;

	public bool stencilRef = true;

	public static Shader _opaqueShader;

	public static Shader _transparentShader;

	public static Shader _transparentBackgroundShader;

	public Material _opaqueMaterial;

	public static float HighlighterLimit = 1f;

	public static int dirtyFrame = -1;

	public static HashSet<Highlighter> highlighters = new HashSet<Highlighter>();

	public static HashSet<Highlighter>.Enumerator enumerator;

	public static bool managerInitialized = false;

	public bool highlighted { get; set; }

	public bool once
	{
		get
		{
			return _once == Time.frameCount;
		}
		set
		{
			_once = (value ? Time.frameCount : 0);
		}
	}

	public float zTestFloat
	{
		get
		{
			if (!zTest)
			{
				return 4f;
			}
			return 8f;
		}
	}

	public float stencilRefFloat
	{
		get
		{
			if (!stencilRef)
			{
				return 0f;
			}
			return 1f;
		}
	}

	public static Shader opaqueShader
	{
		get
		{
			if (_opaqueShader == null)
			{
				_opaqueShader = Shader.Find("Hidden/Highlighted/Opaque");
			}
			return _opaqueShader;
		}
	}

	public static Shader transparentShader
	{
		get
		{
			if (_transparentShader == null)
			{
				_transparentShader = Shader.Find("Hidden/Highlighted/Transparent");
			}
			return _transparentShader;
		}
	}

	public static Shader transparentBackgroundShader
	{
		get
		{
			if (_transparentBackgroundShader == null)
			{
				_transparentBackgroundShader = Shader.Find("Hidden/Highlighted/Transparent/Background");
			}
			return _transparentBackgroundShader;
		}
	}

	public Material opaqueMaterial
	{
		get
		{
			if (_opaqueMaterial == null)
			{
				_opaqueMaterial = new Material(opaqueShader);
				Initialize();
				_opaqueMaterial.SetFloat(_ZTest, zTestFloat);
				_opaqueMaterial.SetFloat(_StencilRef, stencilRefFloat);
			}
			return _opaqueMaterial;
		}
	}

	public static bool HighlightDirty
	{
		get
		{
			return dirtyFrame == Time.frameCount;
		}
		set
		{
			dirtyFrame = (value ? Time.frameCount : (-1));
		}
	}

	public static int _MainTex { get; set; }

	public static int _BackTex { get; set; }

	public static int _Outline { get; set; }

	public static int _Cutoff { get; set; }

	public static int _Intensity { get; set; }

	public static int _ZTest { get; set; }

	public static int _StencilRef { get; set; }

	public static int _Cull { get; set; }

	public static int _HighlightingBlur1 { get; set; }

	public static int _HighlightingBlur2 { get; set; }

	public static int _HighlightingBuffer { get; set; }

	public static int _HighlightingBlurred { get; set; }

	public static int _HighlightingBlurOffset { get; set; }

	public static int _HighlightingZWrite { get; set; }

	public static int _HighlightingOffsetFactor { get; set; }

	public static int _HighlightingOffsetUnits { get; set; }

	public static int _HighlightingBufferTexelSize { get; set; }

	[ContextMenu("Reinit Materials")]
	public void ReinitMaterials()
	{
		renderersDirty = true;
	}

	public void OnParams(Color color)
	{
		onceColor = color;
	}

	public void On()
	{
		once = true;
	}

	public void On(Color color)
	{
		onceColor = color;
		On();
	}

	public void FlashingParams(Color color1, Color color2, float freq)
	{
		flashingColorMin = color1;
		flashingColorMax = color2;
		flashingFreq = freq;
	}

	public void FlashingOn()
	{
		flashing = true;
	}

	public void FlashingOn(Color color1, Color color2)
	{
		flashingColorMin = color1;
		flashingColorMax = color2;
		FlashingOn();
	}

	public void FlashingOn(Color color1, Color color2, float freq)
	{
		flashingFreq = freq;
		FlashingOn(color1, color2);
	}

	public void FlashingOn(float freq)
	{
		flashingFreq = freq;
		FlashingOn();
	}

	public void FlashingOff()
	{
		flashing = false;
	}

	public void FlashingSwitch()
	{
		flashing = !flashing;
	}

	public void ConstantParams(Color color)
	{
		constantColor = color;
	}

	public void ConstantOn()
	{
		constantly = true;
		transitionActive = true;
	}

	public void ConstantOn(Color color)
	{
		constantColor = color;
		ConstantOn();
	}

	public void ConstantOff()
	{
		constantly = false;
		transitionActive = true;
	}

	public void ConstantSwitch()
	{
		constantly = !constantly;
		transitionActive = true;
	}

	public void ConstantOnImmediate()
	{
		constantly = true;
		transitionValue = 1f;
		transitionActive = false;
	}

	public void ConstantOnImmediate(Color color)
	{
		constantColor = color;
		ConstantOnImmediate();
	}

	public void ConstantOffImmediate()
	{
		constantly = false;
		transitionValue = 0f;
		transitionActive = false;
	}

	public void ConstantSwitchImmediate()
	{
		constantly = !constantly;
		transitionValue = (constantly ? 1f : 0f);
		transitionActive = false;
	}

	public void SeeThroughOn()
	{
		seeThrough = true;
	}

	public void SeeThroughOff()
	{
		seeThrough = false;
	}

	public void SeeThroughSwitch()
	{
		seeThrough = !seeThrough;
	}

	public void OccluderOn()
	{
		occluder = true;
	}

	public void OccluderOff()
	{
		occluder = false;
	}

	public void OccluderSwitch()
	{
		occluder = !occluder;
	}

	public void Off()
	{
		once = false;
		flashing = false;
		constantly = false;
		transitionValue = 0f;
		transitionActive = false;
	}

	public void Die()
	{
		UnityEngine.Object.Destroy(this);
	}

	public void Awake()
	{
		tr = GetComponent<Transform>();
		Initialize();
	}

	public void OnEnable()
	{
		if (CheckInstance())
		{
			AddHighlighter(this);
		}
	}

	public void OnDisable()
	{
		RemoveHighlighter(this);
		CleanUp();
		renderersDirty = true;
		highlighted = false;
		currentColor = Color.clear;
		transitionActive = false;
		transitionValue = 0f;
		once = false;
		flashing = false;
		constantly = false;
		occluder = false;
		seeThrough = false;
	}

	public void Update()
	{
		PerformTransition();
	}

	public void OnDestroy()
	{
		CleanUp();
		if (_opaqueMaterial != null)
		{
			UnityEngine.Object.Destroy(_opaqueMaterial);
		}
	}

	public void CleanUp()
	{
		if (highlightableRenderers != null)
		{
			for (int i = 0; i < highlightableRenderers.Count; i++)
			{
				highlightableRenderers[i].CleanUp();
			}
			highlightableRenderers.Clear();
		}
	}

	public bool UpdateHighlighting(bool isDepthAvailable)
	{
		bool flag = highlighted;
		bool flag2 = false;
		flag2 = false | UpdateRenderers();
		highlighted = once || flashing || constantly || transitionActive;
		int num = 0;
		if (highlighted)
		{
			UpdateShaderParams(seeThrough, sr: true);
			num = (seeThrough ? 2 : 0);
		}
		else if (occluder && (seeThrough || !isDepthAvailable))
		{
			UpdateShaderParams(zt: false, seeThrough);
			num = (seeThrough ? 1 : 0);
			highlighted = true;
		}
		if (renderQueue != num)
		{
			renderQueue = num;
			flag2 = true;
		}
		if (highlighted)
		{
			flag2 |= UpdateVisibility();
			if (visible)
			{
				UpdateColors();
			}
			else
			{
				highlighted = false;
			}
		}
		return flag2 |= flag != highlighted;
	}

	public void FillBuffer(CommandBuffer buffer, int renderQueue)
	{
		if (!highlighted || this.renderQueue != renderQueue)
		{
			return;
		}
		int count = highlightableRenderers.Count;
		while (count-- > 0)
		{
			if (!highlightableRenderers[count].FillBuffer(buffer))
			{
				highlightableRenderers[count].CleanUp();
				highlightableRenderers.RemoveAt(count);
			}
		}
	}

	public bool CheckInstance()
	{
		Highlighter[] components = GetComponents<Highlighter>();
		if (components.Length > 1 && components[0] != this)
		{
			base.enabled = false;
			Debug.LogWarning("HighlightingSystem : Multiple Highlighter components on a single GameObject is not allowed! Highlighter has been disabled on a GameObject with name '" + base.gameObject.name + "'.");
			return false;
		}
		return true;
	}

	public bool UpdateRenderers()
	{
		if (renderersDirty)
		{
			List<Renderer> renderers = new List<Renderer>();
			GrabRenderers(tr, ref renderers);
			highlightableRenderers = new List<RendererCache>();
			int count = renderers.Count;
			for (int i = 0; i < count; i++)
			{
				RendererCache item = new RendererCache(renderers[i], opaqueMaterial, zTestFloat, stencilRefFloat);
				highlightableRenderers.Add(item);
			}
			highlighted = false;
			renderersDirty = false;
			currentColor = Color.clear;
			return true;
		}
		bool result = false;
		int count2 = highlightableRenderers.Count;
		while (count2-- > 0)
		{
			if (highlightableRenderers[count2].IsDestroyed())
			{
				highlightableRenderers[count2].CleanUp();
				highlightableRenderers.RemoveAt(count2);
				renderersDirty = true;
				result = true;
			}
		}
		return result;
	}

	public bool UpdateVisibility()
	{
		if (visibilityCheckFrame == Time.frameCount)
		{
			return visibilityChanged;
		}
		visibilityCheckFrame = Time.frameCount;
		visible = false;
		visibilityChanged = false;
		int i = 0;
		for (int count = highlightableRenderers.Count; i < count; i++)
		{
			RendererCache rendererCache = highlightableRenderers[i];
			visibilityChanged |= rendererCache.UpdateVisibility();
			visible |= rendererCache.visible;
		}
		return visibilityChanged;
	}

	public void GrabRenderers(Transform t, ref List<Renderer> renderers)
	{
		GameObject gameObject = t.gameObject;
		int i = 0;
		IEnumerator enumerator;
		for (int count = types.Count; i < count; i++)
		{
			enumerator = gameObject.GetComponents(types[i]).GetEnumerator();
			while (enumerator.MoveNext())
			{
				Renderer renderer = enumerator.Current as Renderer;
				if (renderer.gameObject.layer != 1 && !renderer.material.name.Contains("KSP/Alpha/Translucent Additive"))
				{
					renderers.Add(renderer);
				}
			}
		}
		if (t.childCount == 0)
		{
			return;
		}
		enumerator = t.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Transform transform = enumerator.Current as Transform;
			if (!(transform.gameObject.GetComponent<Highlighter>() != null))
			{
				GrabRenderers(transform, ref renderers);
			}
		}
	}

	public void UpdateShaderParams(bool zt, bool sr)
	{
		if (zTest != zt)
		{
			zTest = zt;
			float num = zTestFloat;
			opaqueMaterial.SetFloat(_ZTest, num);
			for (int i = 0; i < highlightableRenderers.Count; i++)
			{
				highlightableRenderers[i].SetZTestForTransparent(num);
			}
		}
		if (stencilRef != sr)
		{
			stencilRef = sr;
			float num2 = stencilRefFloat;
			opaqueMaterial.SetFloat(_StencilRef, num2);
			for (int j = 0; j < highlightableRenderers.Count; j++)
			{
				highlightableRenderers[j].SetStencilRefForTransparent(num2);
			}
		}
	}

	public void UpdateColors()
	{
		if (once)
		{
			SetColor(onceColor);
		}
		else if (flashing)
		{
			Color color = Color.Lerp(flashingColorMin, flashingColorMax, 0.5f * Mathf.Sin(Time.realtimeSinceStartup * flashingFreq * ((float)Math.PI * 2f)) + 0.5f);
			SetColor(color);
		}
		else if (transitionActive)
		{
			Color color2 = new Color(constantColor.r, constantColor.g, constantColor.b, constantColor.a * transitionValue);
			SetColor(color2);
		}
		else if (constantly)
		{
			SetColor(constantColor);
		}
		else if (occluder)
		{
			SetColor(occluderColor);
		}
	}

	public void SetColor(Color value)
	{
		if (!(currentColor == value))
		{
			currentColor = value;
			currentColor.a *= HighlighterLimit;
			opaqueMaterial.SetColor(_Outline, currentColor);
			for (int i = 0; i < highlightableRenderers.Count; i++)
			{
				highlightableRenderers[i].SetColorForTransparent(currentColor);
			}
		}
	}

	public void PerformTransition()
	{
		if (transitionActive)
		{
			float num = (constantly ? 1f : 0f);
			if (transitionValue == num)
			{
				transitionActive = false;
			}
			else if (Time.timeScale != 0f)
			{
				float num2 = Time.deltaTime / Time.timeScale;
				transitionValue = Mathf.Clamp01(transitionValue + (constantly ? constantOnSpeed : (0f - constantOffSpeed)) * num2);
			}
		}
	}

	public static void SetZWrite(float value)
	{
		if (zWrite != value)
		{
			zWrite = value;
			Shader.SetGlobalFloat(_HighlightingZWrite, zWrite);
		}
	}

	public static void SetOffsetFactor(float value)
	{
		if (offsetFactor != value)
		{
			offsetFactor = value;
			Shader.SetGlobalFloat(_HighlightingOffsetFactor, offsetFactor);
		}
	}

	public static void SetOffsetUnits(float value)
	{
		if (offsetUnits != value)
		{
			offsetUnits = value;
			Shader.SetGlobalFloat(_HighlightingOffsetUnits, offsetUnits);
		}
	}

	public static void AddHighlighter(Highlighter highlighter)
	{
		highlighters.Add(highlighter);
	}

	public static void RemoveHighlighter(Highlighter instance)
	{
		if (highlighters.Remove(instance) && instance.highlighted)
		{
			HighlightDirty = true;
		}
	}

	public static HashSet<Highlighter>.Enumerator GetEnumerator()
	{
		enumerator = highlighters.GetEnumerator();
		return enumerator;
	}

	public static void Initialize()
	{
		if (!managerInitialized)
		{
			_MainTex = Shader.PropertyToID("_MainTex");
			_BackTex = Shader.PropertyToID("_BackTex");
			_Outline = Shader.PropertyToID("_Outline");
			_Cutoff = Shader.PropertyToID("_Cutoff");
			_Intensity = Shader.PropertyToID("_Intensity");
			_ZTest = Shader.PropertyToID("_ZTest");
			_StencilRef = Shader.PropertyToID("_StencilRef");
			_Cull = Shader.PropertyToID("_Cull");
			_HighlightingBlur1 = Shader.PropertyToID("_HighlightingBlur1");
			_HighlightingBlur2 = Shader.PropertyToID("_HighlightingBlur2");
			_HighlightingBuffer = Shader.PropertyToID("_HighlightingBuffer");
			_HighlightingBlurred = Shader.PropertyToID("_HighlightingBlurred");
			_HighlightingBlurOffset = Shader.PropertyToID("_HighlightingBlurOffset");
			_HighlightingZWrite = Shader.PropertyToID("_HighlightingZWrite");
			_HighlightingOffsetFactor = Shader.PropertyToID("_HighlightingOffsetFactor");
			_HighlightingOffsetUnits = Shader.PropertyToID("_HighlightingOffsetUnits");
			_HighlightingBufferTexelSize = Shader.PropertyToID("_HighlightingBufferTexelSize");
			managerInitialized = true;
		}
	}
}
