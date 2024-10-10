using System;
using System.Collections.Generic;
using System.Linq;
using SoftMasking.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Sprites;
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

	public struct SourceParameters
	{
		public Image image;

		public Sprite sprite;

		public BorderMode spriteBorderMode;

		public Texture2D texture;

		public Rect textureUVRect;
	}

	public class MaterialReplacerImpl : IMaterialReplacer
	{
		public readonly SoftMask _owner;

		public int order => 0;

		public MaterialReplacerImpl(SoftMask owner)
		{
			_owner = owner;
		}

		public Material Replace(Material original)
		{
			if (!(original == null) && !original.HasDefaultUIShader())
			{
				if (original.HasDefaultETC1UIShader())
				{
					return Replace(original, _owner._defaultETC1Shader);
				}
				if (original.SupportsSoftMask())
				{
					return new Material(original);
				}
				return null;
			}
			return Replace(original, _owner._defaultShader);
		}

		public static Material Replace(Material original, Shader defaultReplacementShader)
		{
			Material material = (defaultReplacementShader ? new Material(defaultReplacementShader) : null);
			if ((bool)material && (bool)original)
			{
				material.CopyPropertiesFromMaterial(original);
			}
			return material;
		}
	}

	public static class Mathr
	{
		public static Vector4 ToVector(Rect r)
		{
			return new Vector4(r.xMin, r.yMin, r.xMax, r.yMax);
		}

		public static Vector4 Div(Vector4 v, Vector2 s)
		{
			return new Vector4(v.x / s.x, v.y / s.y, v.z / s.x, v.w / s.y);
		}

		public static Vector2 Div(Vector2 v, Vector2 s)
		{
			return new Vector2(v.x / s.x, v.y / s.y);
		}

		public static Vector4 Mul(Vector4 v, Vector2 s)
		{
			return new Vector4(v.x * s.x, v.y * s.y, v.z * s.x, v.w * s.y);
		}

		public static Vector2 Size(Vector4 r)
		{
			return new Vector2(r.z - r.x, r.w - r.y);
		}

		public static Vector4 Move(Vector4 v, Vector2 o)
		{
			return new Vector4(v.x + o.x, v.y + o.y, v.z + o.x, v.w + o.y);
		}

		public static Vector4 BorderOf(Vector4 outer, Vector4 inner)
		{
			return new Vector4(inner.x - outer.x, inner.y - outer.y, outer.z - inner.z, outer.w - inner.w);
		}

		public static Vector4 ApplyBorder(Vector4 v, Vector4 b)
		{
			return new Vector4(v.x + b.x, v.y + b.y, v.z - b.z, v.w - b.w);
		}

		public static Vector2 Min(Vector4 r)
		{
			return new Vector2(r.x, r.y);
		}

		public static Vector2 Max(Vector4 r)
		{
			return new Vector2(r.z, r.w);
		}

		public static Vector2 Remap(Vector2 c, Vector4 from, Vector4 to)
		{
			Vector2 s = Max(from) - Min(from);
			Vector2 b = Max(to) - Min(to);
			return Vector2.Scale(Div(c - Min(from), s), b) + Min(to);
		}

		public static bool Inside(Vector2 v, Vector4 r)
		{
			if (v.x >= r.x && v.y >= r.y && v.x <= r.z)
			{
				return v.y <= r.w;
			}
			return false;
		}
	}

	public struct MaterialParameters
	{
		public static class Ids
		{
			public static readonly int SoftMask = Shader.PropertyToID("_SoftMask");

			public static readonly int SoftMask_Rect = Shader.PropertyToID("_SoftMask_Rect");

			public static readonly int SoftMask_UVRect = Shader.PropertyToID("_SoftMask_UVRect");

			public static readonly int SoftMask_ChannelWeights = Shader.PropertyToID("_SoftMask_ChannelWeights");

			public static readonly int SoftMask_WorldToMask = Shader.PropertyToID("_SoftMask_WorldToMask");

			public static readonly int SoftMask_BorderRect = Shader.PropertyToID("_SoftMask_BorderRect");

			public static readonly int SoftMask_UVBorderRect = Shader.PropertyToID("_SoftMask_UVBorderRect");

			public static readonly int SoftMask_TileRepeat = Shader.PropertyToID("_SoftMask_TileRepeat");

			public static readonly int SoftMask_InvertMask = Shader.PropertyToID("_SoftMask_InvertMask");

			public static readonly int SoftMask_InvertOutsides = Shader.PropertyToID("_SoftMask_InvertOutsides");
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
			get
			{
				if (!texture)
				{
					return Texture2D.whiteTexture;
				}
				return texture;
			}
		}

		public bool SampleMask(Vector2 localPos, out float mask)
		{
			Vector2 vector = XY2UV(localPos);
			try
			{
				mask = MaskValue(texture.GetPixelBilinear(vector.x, vector.y));
				return true;
			}
			catch (UnityException)
			{
				mask = 0f;
				return false;
			}
		}

		public void Apply(Material mat)
		{
			mat.SetTexture(Ids.SoftMask, activeTexture);
			mat.SetVector(Ids.SoftMask_Rect, maskRect);
			mat.SetVector(Ids.SoftMask_UVRect, maskRectUV);
			mat.SetColor(Ids.SoftMask_ChannelWeights, maskChannelWeights);
			mat.SetMatrix(Ids.SoftMask_WorldToMask, worldToMask);
			mat.SetFloat(Ids.SoftMask_InvertMask, invertMask ? 1 : 0);
			mat.SetFloat(Ids.SoftMask_InvertOutsides, invertOutsides ? 1 : 0);
			mat.EnableKeyword("SOFTMASK_SIMPLE", borderMode == BorderMode.Simple);
			mat.EnableKeyword("SOFTMASK_SLICED", borderMode == BorderMode.Sliced);
			mat.EnableKeyword("SOFTMASK_TILED", borderMode == BorderMode.Tiled);
			if (borderMode != 0)
			{
				mat.SetVector(Ids.SoftMask_BorderRect, maskBorder);
				mat.SetVector(Ids.SoftMask_UVBorderRect, maskBorderUV);
				if (borderMode == BorderMode.Tiled)
				{
					mat.SetVector(Ids.SoftMask_TileRepeat, tileRepeat);
				}
			}
		}

		public Vector2 XY2UV(Vector2 localPos)
		{
			switch (borderMode)
			{
			default:
				Debug.LogError("Unknown BorderMode");
				return MapSimple(localPos);
			case BorderMode.Simple:
				return MapSimple(localPos);
			case BorderMode.Sliced:
				return MapBorder(localPos, repeat: false);
			case BorderMode.Tiled:
				return MapBorder(localPos, repeat: true);
			}
		}

		public Vector2 MapSimple(Vector2 localPos)
		{
			return Mathr.Remap(localPos, maskRect, maskRectUV);
		}

		public Vector2 MapBorder(Vector2 localPos, bool repeat)
		{
			return new Vector2(Inset(localPos.x, maskRect.x, maskBorder.x, maskBorder.z, maskRect.z, maskRectUV.x, maskBorderUV.x, maskBorderUV.z, maskRectUV.z, repeat ? tileRepeat.x : 1f), Inset(localPos.y, maskRect.y, maskBorder.y, maskBorder.w, maskRect.w, maskRectUV.y, maskBorderUV.y, maskBorderUV.w, maskRectUV.w, repeat ? tileRepeat.y : 1f));
		}

		public float Inset(float v, float x1, float x2, float u1, float u2, float repeat = 1f)
		{
			float num = x2 - x1;
			return Mathf.Lerp(u1, u2, (num != 0f) ? Frac((v - x1) / num * repeat) : 0f);
		}

		public float Inset(float v, float x1, float x2, float x3, float x4, float u1, float u2, float u3, float u4, float repeat = 1f)
		{
			if (v < x2)
			{
				return Inset(v, x1, x2, u1, u2);
			}
			if (v < x3)
			{
				return Inset(v, x2, x3, u2, u3, repeat);
			}
			return Inset(v, x3, x4, u3, u4);
		}

		public float Frac(float v)
		{
			return v - Mathf.Floor(v);
		}

		public float MaskValue(Color mask)
		{
			Color color = mask * maskChannelWeights;
			return color.a + color.r + color.g + color.b;
		}
	}

	public struct Diagnostics
	{
		public SoftMask _softMask;

		public Image image => _softMask.DeduceSourceParameters().image;

		public Sprite sprite => _softMask.DeduceSourceParameters().sprite;

		public Texture2D texture => _softMask.DeduceSourceParameters().texture;

		public Diagnostics(SoftMask softMask)
		{
			_softMask = softMask;
		}

		public Errors PollErrors()
		{
			SoftMask softMask = _softMask;
			Errors errors = Errors.NoError;
			softMask.GetComponentsInChildren(s_maskables);
			using (new ClearListAtExit<SoftMaskable>(s_maskables))
			{
				if (s_maskables.Any((SoftMaskable m) => m.mask == softMask && m.shaderIsNotSupported))
				{
					errors |= Errors.UnsupportedShaders;
				}
			}
			if (ThereAreNestedMasks())
			{
				errors |= Errors.NestedMasks;
			}
			errors |= CheckSprite(sprite);
			errors |= CheckImage();
			return errors | CheckTexture();
		}

		public static Errors CheckSprite(Sprite sprite)
		{
			Errors errors = Errors.NoError;
			if (!sprite)
			{
				return errors;
			}
			if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
			{
				errors |= Errors.TightPackedSprite;
			}
			if ((bool)sprite.associatedAlphaSplitTexture)
			{
				errors |= Errors.AlphaSplitSprite;
			}
			return errors;
		}

		public bool ThereAreNestedMasks()
		{
			SoftMask softMask = _softMask;
			bool flag = false;
			using (new ClearListAtExit<SoftMask>(s_masks))
			{
				softMask.GetComponentsInParent(includeInactive: false, s_masks);
				flag |= s_masks.Any((SoftMask x) => AreCompeting(softMask, x));
				softMask.GetComponentsInChildren(includeInactive: false, s_masks);
				return flag | s_masks.Any((SoftMask x) => AreCompeting(softMask, x));
			}
		}

		public Errors CheckImage()
		{
			Errors errors = Errors.NoError;
			if (!_softMask.isBasedOnGraphic)
			{
				return errors;
			}
			if ((bool)image && !IsSupportedImageType(image.type))
			{
				errors |= Errors.UnsupportedImageType;
			}
			return errors;
		}

		public Errors CheckTexture()
		{
			Errors errors = Errors.NoError;
			if (_softMask.isUsingRaycastFiltering && (bool)texture && !IsReadable(texture))
			{
				errors |= Errors.UnreadableTexture;
			}
			return errors;
		}

		public static bool AreCompeting(SoftMask softMask, SoftMask other)
		{
			if (softMask.isMaskingEnabled && softMask != other && other.isMaskingEnabled && softMask.canvas.rootCanvas == other.canvas.rootCanvas)
			{
				return !SelectChild(softMask, other).canvas.overrideSorting;
			}
			return false;
		}

		public static T SelectChild<T>(T first, T second) where T : Component
		{
			if (!first.transform.IsChildOf(second.transform))
			{
				return second;
			}
			return first;
		}

		public static bool IsReadable(Texture2D texture)
		{
			try
			{
				texture.GetPixel(0, 0);
				return true;
			}
			catch (UnityException)
			{
				return false;
			}
		}

		public static bool IsSupportedImageType(Image.Type type)
		{
			if (type != 0 && type != Image.Type.Sliced)
			{
				return type == Image.Type.Tiled;
			}
			return true;
		}
	}

	[SerializeField]
	public Shader _defaultShader;

	[SerializeField]
	public Shader _defaultETC1Shader;

	[SerializeField]
	public MaskSource _source;

	[SerializeField]
	public RectTransform _separateMask;

	[SerializeField]
	public Sprite _sprite;

	[SerializeField]
	public BorderMode _spriteBorderMode;

	[SerializeField]
	public Texture2D _texture;

	[SerializeField]
	public Rect _textureUVRect = DefaultUVRect;

	[SerializeField]
	public Color _channelWeights = MaskChannel.alpha;

	[SerializeField]
	public float _raycastThreshold;

	[SerializeField]
	public bool _invertMask;

	[SerializeField]
	public bool _invertOutsides;

	public MaterialReplacements _materials;

	public MaterialParameters _parameters;

	public Sprite _lastUsedSprite;

	public Rect _lastMaskRect;

	public bool _maskingWasEnabled;

	public bool _destroyed;

	public bool _dirty;

	public RectTransform _maskTransform;

	public Graphic _graphic;

	public Canvas _canvas;

	public static readonly Rect DefaultUVRect = new Rect(0f, 0f, 1f, 1f);

	public static readonly List<SoftMask> s_masks = new List<SoftMask>();

	public static readonly List<SoftMaskable> s_maskables = new List<SoftMaskable>();

	public Shader defaultShader
	{
		get
		{
			return _defaultShader;
		}
		set
		{
			SetShader(ref _defaultShader, value);
		}
	}

	public Shader defaultETC1Shader
	{
		get
		{
			return _defaultETC1Shader;
		}
		set
		{
			SetShader(ref _defaultETC1Shader, value, warnIfNotSet: false);
		}
	}

	public MaskSource source
	{
		get
		{
			return _source;
		}
		set
		{
			if (_source != value)
			{
				Set(ref _source, value);
			}
		}
	}

	public RectTransform separateMask
	{
		get
		{
			return _separateMask;
		}
		set
		{
			if (_separateMask != value)
			{
				Set(ref _separateMask, value);
				_graphic = null;
				_maskTransform = null;
			}
		}
	}

	public Sprite sprite
	{
		get
		{
			return _sprite;
		}
		set
		{
			if (_sprite != value)
			{
				Set(ref _sprite, value);
			}
		}
	}

	public BorderMode spriteBorderMode
	{
		get
		{
			return _spriteBorderMode;
		}
		set
		{
			if (_spriteBorderMode != value)
			{
				Set(ref _spriteBorderMode, value);
			}
		}
	}

	public Texture2D texture
	{
		get
		{
			return _texture;
		}
		set
		{
			if (_texture != value)
			{
				Set(ref _texture, value);
			}
		}
	}

	public Rect textureUVRect
	{
		get
		{
			return _textureUVRect;
		}
		set
		{
			if (_textureUVRect != value)
			{
				Set(ref _textureUVRect, value);
			}
		}
	}

	public Color channelWeights
	{
		get
		{
			return _channelWeights;
		}
		set
		{
			if (_channelWeights != value)
			{
				Set(ref _channelWeights, value);
			}
		}
	}

	public float raycastThreshold
	{
		get
		{
			return _raycastThreshold;
		}
		set
		{
			_raycastThreshold = value;
		}
	}

	public bool invertMask
	{
		get
		{
			return _invertMask;
		}
		set
		{
			if (_invertMask != value)
			{
				Set(ref _invertMask, value);
			}
		}
	}

	public bool invertOutsides
	{
		get
		{
			return _invertOutsides;
		}
		set
		{
			if (_invertOutsides != value)
			{
				Set(ref _invertOutsides, value);
			}
		}
	}

	public bool isUsingRaycastFiltering => _raycastThreshold > 0f;

	public bool isMaskingEnabled
	{
		get
		{
			if (base.isActiveAndEnabled)
			{
				return canvas;
			}
			return false;
		}
	}

	public RectTransform maskTransform
	{
		get
		{
			if (!_maskTransform)
			{
				return _maskTransform = (_separateMask ? _separateMask : GetComponent<RectTransform>());
			}
			return _maskTransform;
		}
	}

	public Canvas canvas
	{
		get
		{
			if (!_canvas)
			{
				return _canvas = NearestEnabledCanvas();
			}
			return _canvas;
		}
	}

	public bool isBasedOnGraphic => _source == MaskSource.Graphic;

	bool ISoftMask.isAlive
	{
		get
		{
			if ((bool)this)
			{
				return !_destroyed;
			}
			return false;
		}
	}

	public SoftMask()
	{
		MaterialReplacerChain replacer = new MaterialReplacerChain(MaterialReplacer.globalReplacers, new MaterialReplacerImpl(this));
		_materials = new MaterialReplacements(replacer, delegate(Material m)
		{
			_parameters.Apply(m);
		});
	}

	public Errors PollErrors()
	{
		return new Diagnostics(this).PollErrors();
	}

	public bool IsRaycastLocationValid(Vector2 sp, Camera cam)
	{
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(maskTransform, sp, cam, out var localPoint))
		{
			return false;
		}
		if (!Mathr.Inside(localPoint, LocalMaskRect(Vector4.zero)))
		{
			return _invertOutsides;
		}
		if (!_parameters.texture)
		{
			return true;
		}
		if (!isUsingRaycastFiltering)
		{
			return true;
		}
		if (!_parameters.SampleMask(localPoint, out var mask))
		{
			Debug.LogErrorFormat(this, "Raycast Threshold greater than 0 can't be used on Soft Mask with texture '{0}' because it's not readable. You can make the texture readable in the Texture Import Settings.", _parameters.activeTexture.name);
			return true;
		}
		if (_invertMask)
		{
			mask = 1f - mask;
		}
		return mask >= _raycastThreshold;
	}

	public override void Start()
	{
		base.Start();
		WarnIfDefaultShaderIsNotSet();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		SubscribeOnWillRenderCanvases();
		SpawnMaskablesInChildren(base.transform);
		FindGraphic();
		if (isMaskingEnabled)
		{
			UpdateMaskParameters();
		}
		NotifyChildrenThatMaskMightChanged();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		UnsubscribeFromWillRenderCanvases();
		if ((bool)_graphic)
		{
			_graphic.UnregisterDirtyVerticesCallback(OnGraphicDirty);
			_graphic.UnregisterDirtyMaterialCallback(OnGraphicDirty);
			_graphic = null;
		}
		NotifyChildrenThatMaskMightChanged();
		DestroyMaterials();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		_destroyed = true;
		NotifyChildrenThatMaskMightChanged();
	}

	public virtual void LateUpdate()
	{
		bool flag;
		if (flag = isMaskingEnabled)
		{
			if (_maskingWasEnabled != flag)
			{
				SpawnMaskablesInChildren(base.transform);
			}
			Graphic graphic = _graphic;
			FindGraphic();
			if (_lastMaskRect != maskTransform.rect || (object)_graphic != graphic)
			{
				_dirty = true;
			}
		}
		_maskingWasEnabled = flag;
	}

	public override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		_dirty = true;
	}

	public override void OnDidApplyAnimationProperties()
	{
		base.OnDidApplyAnimationProperties();
		_dirty = true;
	}

	public override void OnTransformParentChanged()
	{
		base.OnTransformParentChanged();
		_canvas = null;
		_dirty = true;
	}

	public override void OnCanvasHierarchyChanged()
	{
		base.OnCanvasHierarchyChanged();
		_canvas = null;
		_dirty = true;
		NotifyChildrenThatMaskMightChanged();
	}

	public void OnTransformChildrenChanged()
	{
		SpawnMaskablesInChildren(base.transform);
	}

	public void SubscribeOnWillRenderCanvases()
	{
		Touch(CanvasUpdateRegistry.instance);
		Canvas.willRenderCanvases += OnWillRenderCanvases;
	}

	public void UnsubscribeFromWillRenderCanvases()
	{
		Canvas.willRenderCanvases -= OnWillRenderCanvases;
	}

	public void OnWillRenderCanvases()
	{
		if (isMaskingEnabled)
		{
			UpdateMaskParameters();
		}
	}

	public static T Touch<T>(T obj)
	{
		return obj;
	}

	Material ISoftMask.GetReplacement(Material original)
	{
		return _materials.Get(original);
	}

	void ISoftMask.ReleaseReplacement(Material replacement)
	{
		_materials.Release(replacement);
	}

	void ISoftMask.UpdateTransformChildren(Transform transform)
	{
		SpawnMaskablesInChildren(transform);
	}

	public void OnGraphicDirty()
	{
		if (isBasedOnGraphic)
		{
			_dirty = true;
		}
	}

	public void FindGraphic()
	{
		if (!_graphic && isBasedOnGraphic)
		{
			_graphic = maskTransform.GetComponent<Graphic>();
			if ((bool)_graphic)
			{
				_graphic.RegisterDirtyVerticesCallback(OnGraphicDirty);
				_graphic.RegisterDirtyMaterialCallback(OnGraphicDirty);
			}
		}
	}

	public Canvas NearestEnabledCanvas()
	{
		Canvas[] componentsInParent = GetComponentsInParent<Canvas>(includeInactive: false);
		int num = 0;
		while (true)
		{
			if (num < componentsInParent.Length)
			{
				if (componentsInParent[num].isActiveAndEnabled)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return componentsInParent[num];
	}

	public void UpdateMaskParameters()
	{
		if (_dirty || maskTransform.hasChanged)
		{
			CalculateMaskParameters();
			maskTransform.hasChanged = false;
			_lastMaskRect = maskTransform.rect;
			_dirty = false;
		}
		_materials.ApplyAll();
	}

	public void SpawnMaskablesInChildren(Transform root)
	{
		using (new ClearListAtExit<SoftMaskable>(s_maskables))
		{
			for (int i = 0; i < root.childCount; i++)
			{
				Transform child = root.GetChild(i);
				child.GetComponents(s_maskables);
				if (s_maskables.Count == 0)
				{
					child.gameObject.AddComponent<SoftMaskable>();
				}
			}
		}
	}

	public void InvalidateChildren()
	{
		ForEachChildMaskable(delegate(SoftMaskable x)
		{
			x.Invalidate();
		});
	}

	public void NotifyChildrenThatMaskMightChanged()
	{
		ForEachChildMaskable(delegate(SoftMaskable x)
		{
			x.MaskMightChanged();
		});
	}

	public void ForEachChildMaskable(Action<SoftMaskable> f)
	{
		base.transform.GetComponentsInChildren(s_maskables);
		using (new ClearListAtExit<SoftMaskable>(s_maskables))
		{
			for (int i = 0; i < s_maskables.Count; i++)
			{
				SoftMaskable softMaskable = s_maskables[i];
				if ((bool)softMaskable && softMaskable.gameObject != base.gameObject)
				{
					f(softMaskable);
				}
			}
		}
	}

	public void DestroyMaterials()
	{
		_materials.DestroyAllAndClear();
	}

	public SourceParameters DeduceSourceParameters()
	{
		SourceParameters result = default(SourceParameters);
		switch (_source)
		{
		default:
			Debug.LogErrorFormat(this, "Unknown MaskSource: {0}", _source);
			break;
		case MaskSource.Graphic:
			if (_graphic is Image)
			{
				result.image = (Image)_graphic;
				result.sprite = result.image.sprite;
				result.spriteBorderMode = ToBorderMode(result.image.type);
				result.texture = (result.sprite ? result.sprite.texture : null);
			}
			else if (_graphic is RawImage)
			{
				RawImage rawImage = (RawImage)_graphic;
				result.texture = rawImage.texture as Texture2D;
				result.textureUVRect = rawImage.uvRect;
			}
			break;
		case MaskSource.Sprite:
			result.sprite = _sprite;
			result.spriteBorderMode = _spriteBorderMode;
			result.texture = (result.sprite ? result.sprite.texture : null);
			break;
		case MaskSource.Texture:
			result.texture = _texture;
			result.textureUVRect = _textureUVRect;
			break;
		}
		return result;
	}

	public BorderMode ToBorderMode(Image.Type imageType)
	{
		switch (imageType)
		{
		default:
			Debug.LogErrorFormat(this, "SoftMask doesn't support image type {0}. Image type Simple will be used.", imageType);
			return BorderMode.Simple;
		case Image.Type.Simple:
			return BorderMode.Simple;
		case Image.Type.Sliced:
			return BorderMode.Sliced;
		case Image.Type.Tiled:
			return BorderMode.Tiled;
		}
	}

	public void CalculateMaskParameters()
	{
		SourceParameters sourceParameters = DeduceSourceParameters();
		if ((bool)sourceParameters.sprite)
		{
			CalculateSpriteBased(sourceParameters.sprite, sourceParameters.spriteBorderMode);
		}
		else if ((bool)sourceParameters.texture)
		{
			CalculateTextureBased(sourceParameters.texture, sourceParameters.textureUVRect);
		}
		else
		{
			CalculateSolidFill();
		}
	}

	public void CalculateSpriteBased(Sprite sprite, BorderMode borderMode)
	{
		Sprite lastUsedSprite = _lastUsedSprite;
		_lastUsedSprite = sprite;
		Errors errors = Diagnostics.CheckSprite(sprite);
		if (errors != 0)
		{
			if (lastUsedSprite != sprite)
			{
				WarnSpriteErrors(errors);
			}
			CalculateSolidFill();
			return;
		}
		if (!sprite)
		{
			CalculateSolidFill();
			return;
		}
		FillCommonParameters();
		Vector4 innerUV = DataUtility.GetInnerUV(sprite);
		Vector4 outerUV = DataUtility.GetOuterUV(sprite);
		Vector4 padding = DataUtility.GetPadding(sprite);
		Vector4 vector = LocalMaskRect(Vector4.zero);
		_parameters.maskRectUV = outerUV;
		if (borderMode == BorderMode.Simple)
		{
			Vector4 v = Mathr.Div(padding, sprite.rect.size);
			_parameters.maskRect = Mathr.ApplyBorder(vector, Mathr.Mul(v, Mathr.Size(vector)));
		}
		else
		{
			_parameters.maskRect = Mathr.ApplyBorder(vector, padding * SpriteToCanvasScale(sprite));
			Vector4 border = AdjustBorders(sprite.border * SpriteToCanvasScale(sprite), vector);
			_parameters.maskBorder = LocalMaskRect(border);
			_parameters.maskBorderUV = innerUV;
		}
		_parameters.texture = sprite.texture;
		_parameters.borderMode = borderMode;
		if (borderMode == BorderMode.Tiled)
		{
			_parameters.tileRepeat = MaskRepeat(sprite, _parameters.maskBorder);
		}
	}

	public static Vector4 AdjustBorders(Vector4 border, Vector4 rect)
	{
		Vector2 vector = Mathr.Size(rect);
		for (int i = 0; i <= 1; i++)
		{
			float num = border[i] + border[i + 2];
			if (vector[i] < num && num != 0f)
			{
				float num2 = vector[i] / num;
				border[i] *= num2;
				border[i + 2] *= num2;
			}
		}
		return border;
	}

	public void CalculateTextureBased(Texture2D texture, Rect uvRect)
	{
		FillCommonParameters();
		_parameters.maskRect = LocalMaskRect(Vector4.zero);
		_parameters.maskRectUV = Mathr.ToVector(uvRect);
		_parameters.texture = texture;
		_parameters.borderMode = BorderMode.Simple;
	}

	public void CalculateSolidFill()
	{
		CalculateTextureBased(null, DefaultUVRect);
	}

	public void FillCommonParameters()
	{
		_parameters.worldToMask = WorldToMask();
		_parameters.maskChannelWeights = _channelWeights;
		_parameters.invertMask = _invertMask;
		_parameters.invertOutsides = _invertOutsides;
	}

	public float SpriteToCanvasScale(Sprite sprite)
	{
		float num = (canvas ? canvas.referencePixelsPerUnit : 100f);
		float num2 = (sprite ? sprite.pixelsPerUnit : 100f);
		return num / num2;
	}

	public Matrix4x4 WorldToMask()
	{
		return maskTransform.worldToLocalMatrix * canvas.rootCanvas.transform.localToWorldMatrix;
	}

	public Vector4 LocalMaskRect(Vector4 border)
	{
		return Mathr.ApplyBorder(Mathr.ToVector(maskTransform.rect), border);
	}

	public Vector2 MaskRepeat(Sprite sprite, Vector4 centralPart)
	{
		Vector4 r = Mathr.ApplyBorder(Mathr.ToVector(sprite.rect), sprite.border);
		return Mathr.Div(Mathr.Size(centralPart) * SpriteToCanvasScale(sprite), Mathr.Size(r));
	}

	public void WarnIfDefaultShaderIsNotSet()
	{
		if (!_defaultShader)
		{
			Debug.LogWarning("SoftMask may not work because its defaultShader is not set", this);
		}
	}

	public void WarnSpriteErrors(Errors errors)
	{
		if ((errors & Errors.TightPackedSprite) != 0)
		{
			Debug.LogError("SoftMask doesn't support tight packed sprites", this);
		}
		if ((errors & Errors.AlphaSplitSprite) != 0)
		{
			Debug.LogError("SoftMask doesn't support sprites with an alpha split texture", this);
		}
	}

	public void Set<T>(ref T field, T value)
	{
		field = value;
		_dirty = true;
	}

	public void SetShader(ref Shader field, Shader value, bool warnIfNotSet = true)
	{
		if (field != value)
		{
			field = value;
			if (warnIfNotSet)
			{
				WarnIfDefaultShaderIsNotSet();
			}
			DestroyMaterials();
			InvalidateChildren();
		}
	}
}
