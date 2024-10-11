using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SoftMasking;

[AddComponentMenu("UI/Soft Mask", 14)]
[ExecuteInEditMode]
[HelpURL("https://docs.google.com/document/d/1Y5YEfiNkGr5RUYZRnsK24r9qptwUftjms7hAk75LUD0")]
[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class SoftMask : UIBehaviour, ISoftMask, ICanvasRaycastFilter
{
	[Serializable]
	public enum MaskSource
	{
		Graphic,
		Sprite,
		Texture
	}

	[Serializable]
	public enum BorderMode
	{
		Simple,
		Sliced,
		Tiled
	}

	[Serializable]
	[Flags]
	public enum Errors
	{
		NoError = 0,
		UnsupportedShaders = 1,
		NestedMasks = 2,
		TightPackedSprite = 4,
		AlphaSplitSprite = 8,
		UnsupportedImageType = 0x10,
		UnreadableTexture = 0x20
	}

	private struct SourceParameters
	{
		public Image image;

		public Sprite sprite;

		public BorderMode spriteBorderMode;

		public Texture2D texture;

		public Rect textureUVRect;
	}

	private class MaterialReplacerImpl : IMaterialReplacer
	{
		private readonly SoftMask _owner;

		public int order
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MaterialReplacerImpl(SoftMask owner)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Material Replace(Material original)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Material Replace(Material original, Shader defaultReplacementShader)
		{
			throw null;
		}
	}

	private static class Mathr
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 ToVector(Rect r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 Div(Vector4 v, Vector2 s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector2 Div(Vector2 v, Vector2 s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 Mul(Vector4 v, Vector2 s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector2 Size(Vector4 r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 Move(Vector4 v, Vector2 o)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 BorderOf(Vector4 outer, Vector4 inner)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector4 ApplyBorder(Vector4 v, Vector4 b)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector2 Min(Vector4 r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector2 Max(Vector4 r)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Vector2 Remap(Vector2 c, Vector4 from, Vector4 to)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static bool Inside(Vector2 v, Vector4 r)
		{
			throw null;
		}
	}

	private struct MaterialParameters
	{
		private static class Ids
		{
			public static readonly int SoftMask;

			public static readonly int SoftMask_Rect;

			public static readonly int SoftMask_UVRect;

			public static readonly int SoftMask_ChannelWeights;

			public static readonly int SoftMask_WorldToMask;

			public static readonly int SoftMask_BorderRect;

			public static readonly int SoftMask_UVBorderRect;

			public static readonly int SoftMask_TileRepeat;

			public static readonly int SoftMask_InvertMask;

			public static readonly int SoftMask_InvertOutsides;

			[MethodImpl(MethodImplOptions.NoInlining)]
			static Ids()
			{
				throw null;
			}
		}

		public Vector4 maskRect;

		public Vector4 maskBorder;

		public Vector4 maskRectUV;

		public Vector4 maskBorderUV;

		public Vector2 tileRepeat;

		public Color maskChannelWeights;

		public Matrix4x4 worldToMask;

		public Texture2D texture;

		public BorderMode borderMode;

		public bool invertMask;

		public bool invertOutsides;

		public Texture2D activeTexture
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool SampleMask(Vector2 localPos, out float mask)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Apply(Material mat)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector2 XY2UV(Vector2 localPos)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector2 MapSimple(Vector2 localPos)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector2 MapBorder(Vector2 localPos, bool repeat)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float Inset(float v, float x1, float x2, float u1, float u2, float repeat = 1f)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float Inset(float v, float x1, float x2, float x3, float x4, float u1, float u2, float u3, float u4, float repeat = 1f)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float Frac(float v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float MaskValue(Color mask)
		{
			throw null;
		}
	}

	private struct Diagnostics
	{
		private SoftMask _softMask;

		private Image image
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private Sprite sprite
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		private Texture2D texture
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Diagnostics(SoftMask softMask)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Errors PollErrors()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Errors CheckSprite(Sprite sprite)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool ThereAreNestedMasks()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Errors CheckImage()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Errors CheckTexture()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool AreCompeting(SoftMask softMask, SoftMask other)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static T SelectChild<T>(T first, T second) where T : Component
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool IsReadable(Texture2D texture)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static bool IsSupportedImageType(Image.Type type)
		{
			throw null;
		}
	}

	[SerializeField]
	private Shader _defaultShader;

	[SerializeField]
	private Shader _defaultETC1Shader;

	[SerializeField]
	private MaskSource _source;

	[SerializeField]
	private RectTransform _separateMask;

	[SerializeField]
	private Sprite _sprite;

	[SerializeField]
	private BorderMode _spriteBorderMode;

	[SerializeField]
	private Texture2D _texture;

	[SerializeField]
	private Rect _textureUVRect;

	[SerializeField]
	private Color _channelWeights;

	[SerializeField]
	private float _raycastThreshold;

	[SerializeField]
	private bool _invertMask;

	[SerializeField]
	private bool _invertOutsides;

	private MaterialReplacements _materials;

	private MaterialParameters _parameters;

	private Sprite _lastUsedSprite;

	private Rect _lastMaskRect;

	private bool _maskingWasEnabled;

	private bool _destroyed;

	private bool _dirty;

	private RectTransform _maskTransform;

	private Graphic _graphic;

	private Canvas _canvas;

	private static readonly Rect DefaultUVRect;

	private static readonly List<SoftMask> s_masks;

	private static readonly List<SoftMaskable> s_maskables;

	public Shader defaultShader
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

	public Shader defaultETC1Shader
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

	public MaskSource source
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

	public RectTransform separateMask
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

	public Sprite sprite
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

	public BorderMode spriteBorderMode
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

	public Texture2D texture
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

	public Rect textureUVRect
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

	public Color channelWeights
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

	public float raycastThreshold
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

	public bool invertMask
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

	public bool invertOutsides
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

	public bool isUsingRaycastFiltering
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isMaskingEnabled
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private RectTransform maskTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private Canvas canvas
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	private bool isBasedOnGraphic
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	bool ISoftMask.isAlive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SoftMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SoftMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Errors PollErrors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsRaycastLocationValid(Vector2 sp, Camera cam)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRectTransformDimensionsChange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnDidApplyAnimationProperties()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTransformParentChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnCanvasHierarchyChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTransformChildrenChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SubscribeOnWillRenderCanvases()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnsubscribeFromWillRenderCanvases()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnWillRenderCanvases()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static T Touch<T>(T obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	Material ISoftMask.GetReplacement(Material original)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void ISoftMask.ReleaseReplacement(Material replacement)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void ISoftMask.UpdateTransformChildren(Transform transform)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGraphicDirty()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindGraphic()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Canvas NearestEnabledCanvas()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateMaskParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SpawnMaskablesInChildren(Transform root)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InvalidateChildren()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotifyChildrenThatMaskMightChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ForEachChildMaskable(Action<SoftMaskable> f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private SourceParameters DeduceSourceParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BorderMode ToBorderMode(Image.Type imageType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateMaskParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSpriteBased(Sprite sprite, BorderMode borderMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4 AdjustBorders(Vector4 border, Vector4 rect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateTextureBased(Texture2D texture, Rect uvRect)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateSolidFill()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FillCommonParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SpriteToCanvasScale(Sprite sprite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Matrix4x4 WorldToMask()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector4 LocalMaskRect(Vector4 border)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector2 MaskRepeat(Sprite sprite, Vector4 centralPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WarnIfDefaultShaderIsNotSet()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void WarnSpriteErrors(Errors errors)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Set<T>(ref T field, T value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetShader(ref Shader field, Shader value, bool warnIfNotSet = true)
	{
		throw null;
	}
}
