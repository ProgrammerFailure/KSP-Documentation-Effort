using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace Highlighting;

public class Highlighter : MonoBehaviour
{
	[Serializable]
	private class RendererCache
	{
		private struct Data
		{
			public Material material;

			public int submeshIndex;

			public bool transparent;
		}

		private static readonly string sRenderType;

		private static readonly string sOpaque;

		private static readonly string sTransparent;

		private static readonly string sTransparentCutout;

		private static readonly string sMainTex;

		private static readonly string sBackTex;

		private const int opaquePassID = 0;

		private const int transparentPassID = 1;

		private GameObject go;

		private Renderer renderer;

		private List<Data> data;

		public bool visible
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
		public RendererCache(Renderer r, Material sharedOpaqueMaterial, float zTest, float stencilRef)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static RendererCache()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool UpdateVisibility()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool FillBuffer(CommandBuffer buffer)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetColorForTransparent(Color clr)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetZTestForTransparent(float zTest)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetStencilRefForTransparent(float stencilRef)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool IsVisible()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool IsDestroyed()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CleanUp()
		{
			throw null;
		}
	}

	private static float constantOnSpeed;

	private static float constantOffSpeed;

	private static float transparentCutoff;

	public const int highlightingLayer = 7;

	public static readonly List<Type> types;

	private const float doublePI = (float)Math.PI * 2f;

	private readonly Color occluderColor;

	private const float zTestLessEqual = 4f;

	private const float zTestAlways = 8f;

	private const float cullOff = 0f;

	private static float zWrite;

	private static float offsetFactor;

	private static float offsetUnits;

	public static Color colorPartHighlightDefault;

	public static Color colorPartEditorAttached;

	public static Color colorPartEditorDetached;

	public static Color colorPartEditorActionSelected;

	public static Color colorPartEditorActionHighlight;

	public static Color colorPartEditorAxisSelected;

	public static Color colorPartEditorAxisHighlight;

	public static Color colorPartRootToolHighlight;

	public static Color colorPartRootToolHighlightEdge;

	public static Color colorPartRootToolHover;

	public static Color colorPartRootToolHoverEdge;

	public static Color colorPartEngineerAppHighlight;

	public static Color colorPartTransferSourceHighlight;

	public static Color colorPartTransferSourceHover;

	public static Color colorPartTransferDestHighlight;

	public static Color colorPartTransferDestHover;

	public static Color colorPartInventoryContainer;

	public static Color colorPartInventoryUnavailableSpace;

	public static Color colorPartConstructionValid;

	public static Color colorConstructionPartDropAsNewVessel;

	private Transform tr;

	private List<RendererCache> highlightableRenderers;

	private int visibilityCheckFrame;

	private bool visibilityChanged;

	private bool visible;

	private bool renderersDirty;

	private Color currentColor;

	private bool transitionActive;

	private float transitionValue;

	private float flashingFreq;

	private int _once;

	private Color onceColor;

	private bool flashing;

	private Color flashingColorMin;

	private Color flashingColorMax;

	private bool constantly;

	private Color constantColor;

	private bool occluder;

	private bool seeThrough;

	private int renderQueue;

	private bool zTest;

	private bool stencilRef;

	private static Shader _opaqueShader;

	private static Shader _transparentShader;

	private static Shader _transparentBackgroundShader;

	private Material _opaqueMaterial;

	public static float HighlighterLimit;

	private static int dirtyFrame;

	private static HashSet<Highlighter> highlighters;

	private static HashSet<Highlighter>.Enumerator enumerator;

	private static bool managerInitialized;

	public bool highlighted
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

	private bool once
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

	private float zTestFloat
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private float stencilRefFloat
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Shader opaqueShader
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Shader transparentShader
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static Shader transparentBackgroundShader
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Material opaqueMaterial
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public static bool HighlightDirty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		private set
		{
			throw null;
		}
	}

	public static int _MainTex
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

	public static int _BackTex
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

	public static int _Outline
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

	public static int _Cutoff
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

	public static int _Intensity
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

	public static int _ZTest
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

	public static int _StencilRef
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

	public static int _Cull
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

	public static int _HighlightingBlur1
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

	public static int _HighlightingBlur2
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

	public static int _HighlightingBuffer
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

	public static int _HighlightingBlurred
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

	public static int _HighlightingBlurOffset
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

	public static int _HighlightingZWrite
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

	public static int _HighlightingOffsetFactor
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

	public static int _HighlightingOffsetUnits
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

	public static int _HighlightingBufferTexelSize
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
	public Highlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Highlighter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reinit Materials")]
	public void ReinitMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnParams(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void On()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void On(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingParams(Color color1, Color color2, float freq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingOn(Color color1, Color color2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingOn(Color color1, Color color2, float freq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingOn(float freq)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlashingSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantParams(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOn(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOnImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOnImmediate(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantOffImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ConstantSwitchImmediate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SeeThroughOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SeeThroughOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SeeThroughSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OccluderOn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OccluderOff()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OccluderSwitch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Off()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Die()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CleanUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UpdateHighlighting(bool isDepthAvailable)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FillBuffer(CommandBuffer buffer, int renderQueue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckInstance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateRenderers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool UpdateVisibility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GrabRenderers(Transform t, ref List<Renderer> renderers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateShaderParams(bool zt, bool sr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetColor(Color value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PerformTransition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetZWrite(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetOffsetFactor(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetOffsetUnits(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddHighlighter(Highlighter highlighter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void RemoveHighlighter(Highlighter instance)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static HashSet<Highlighter>.Enumerator GetEnumerator()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void Initialize()
	{
		throw null;
	}
}
