using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace TMPro;

public class TMP_Text : MaskableGraphic
{
	protected enum TextInputSources
	{
		Text,
		SetText,
		SetCharArray,
		String
	}

	[SerializeField]
	protected string m_text;

	[SerializeField]
	protected bool m_isRightToLeft;

	[SerializeField]
	protected TMP_FontAsset m_fontAsset;

	protected TMP_FontAsset m_currentFontAsset;

	protected bool m_isSDFShader;

	[SerializeField]
	protected Material m_sharedMaterial;

	protected Material m_currentMaterial;

	protected MaterialReference[] m_materialReferences;

	protected Dictionary<int, int> m_materialReferenceIndexLookup;

	protected TMP_XmlTagStack<MaterialReference> m_materialReferenceStack;

	protected int m_currentMaterialIndex;

	[SerializeField]
	protected Material[] m_fontSharedMaterials;

	[SerializeField]
	protected Material m_fontMaterial;

	[SerializeField]
	protected Material[] m_fontMaterials;

	protected bool m_isMaterialDirty;

	[SerializeField]
	protected Color32 m_fontColor32;

	[SerializeField]
	protected Color m_fontColor;

	protected static Color32 s_colorWhite;

	protected Color32 m_underlineColor;

	protected Color32 m_strikethroughColor;

	protected Color32 m_highlightColor;

	[SerializeField]
	protected bool m_enableVertexGradient;

	[SerializeField]
	protected VertexGradient m_fontColorGradient;

	[SerializeField]
	protected TMP_ColorGradient m_fontColorGradientPreset;

	[SerializeField]
	protected TMP_SpriteAsset m_spriteAsset;

	[SerializeField]
	protected bool m_tintAllSprites;

	protected bool m_tintSprite;

	protected Color32 m_spriteColor;

	[SerializeField]
	protected bool m_overrideHtmlColors;

	[SerializeField]
	protected Color32 m_faceColor;

	[SerializeField]
	protected Color32 m_outlineColor;

	protected float m_outlineWidth;

	[SerializeField]
	protected float m_fontSize;

	protected float m_currentFontSize;

	[SerializeField]
	protected float m_fontSizeBase;

	protected TMP_XmlTagStack<float> m_sizeStack;

	[SerializeField]
	protected int m_fontWeight;

	protected int m_fontWeightInternal;

	protected TMP_XmlTagStack<int> m_fontWeightStack;

	[SerializeField]
	protected bool m_enableAutoSizing;

	protected float m_maxFontSize;

	protected float m_minFontSize;

	[SerializeField]
	protected float m_fontSizeMin;

	[SerializeField]
	protected float m_fontSizeMax;

	[SerializeField]
	protected FontStyles m_fontStyle;

	protected FontStyles m_style;

	protected TMP_BasicXmlTagStack m_fontStyleStack;

	protected bool m_isUsingBold;

	[SerializeField]
	[FormerlySerializedAs("m_lineJustification")]
	protected TextAlignmentOptions m_textAlignment;

	protected TextAlignmentOptions m_lineJustification;

	protected TMP_XmlTagStack<TextAlignmentOptions> m_lineJustificationStack;

	protected Vector3[] m_textContainerLocalCorners;

	[SerializeField]
	protected bool m_isAlignmentEnumConverted;

	[SerializeField]
	protected float m_characterSpacing;

	protected float m_cSpacing;

	protected float m_monoSpacing;

	[SerializeField]
	protected float m_wordSpacing;

	[SerializeField]
	protected float m_lineSpacing;

	protected float m_lineSpacingDelta;

	protected float m_lineHeight;

	[SerializeField]
	protected float m_lineSpacingMax;

	[SerializeField]
	protected float m_paragraphSpacing;

	[SerializeField]
	protected float m_charWidthMaxAdj;

	protected float m_charWidthAdjDelta;

	[SerializeField]
	protected bool m_enableWordWrapping;

	protected bool m_isCharacterWrappingEnabled;

	protected bool m_isNonBreakingSpace;

	protected bool m_isIgnoringAlignment;

	[SerializeField]
	protected float m_wordWrappingRatios;

	[SerializeField]
	protected TextOverflowModes m_overflowMode;

	[SerializeField]
	protected int m_firstOverflowCharacterIndex;

	[SerializeField]
	protected TMP_Text m_linkedTextComponent;

	[SerializeField]
	protected bool m_isLinkedTextComponent;

	[SerializeField]
	protected bool m_isTextTruncated;

	[SerializeField]
	protected bool m_enableKerning;

	[SerializeField]
	protected bool m_enableExtraPadding;

	[SerializeField]
	protected bool checkPaddingRequired;

	[SerializeField]
	protected bool m_isRichText;

	[SerializeField]
	protected bool m_parseCtrlCharacters;

	protected bool m_isOverlay;

	[SerializeField]
	protected bool m_isOrthographic;

	[SerializeField]
	protected bool m_isCullingEnabled;

	[SerializeField]
	protected bool m_ignoreRectMaskCulling;

	[SerializeField]
	protected bool m_ignoreCulling;

	[SerializeField]
	protected TextureMappingOptions m_horizontalMapping;

	[SerializeField]
	protected TextureMappingOptions m_verticalMapping;

	[SerializeField]
	protected float m_uvLineOffset;

	protected TextRenderFlags m_renderMode;

	[SerializeField]
	protected VertexSortingOrder m_geometrySortingOrder;

	[SerializeField]
	protected int m_firstVisibleCharacter;

	protected int m_maxVisibleCharacters;

	protected int m_maxVisibleWords;

	protected int m_maxVisibleLines;

	[SerializeField]
	protected bool m_useMaxVisibleDescender;

	[SerializeField]
	protected int m_pageToDisplay;

	protected bool m_isNewPage;

	[SerializeField]
	protected Vector4 m_margin;

	protected float m_marginLeft;

	protected float m_marginRight;

	protected float m_marginWidth;

	protected float m_marginHeight;

	protected float m_width;

	[SerializeField]
	protected TMP_TextInfo m_textInfo;

	[SerializeField]
	protected bool m_havePropertiesChanged;

	[SerializeField]
	protected bool m_isUsingLegacyAnimationComponent;

	protected Transform m_transform;

	protected RectTransform m_rectTransform;

	protected bool m_autoSizeTextContainer;

	protected Mesh m_mesh;

	[SerializeField]
	protected bool m_isVolumetricText;

	[SerializeField]
	protected TMP_SpriteAnimator m_spriteAnimator;

	protected float m_flexibleHeight;

	protected float m_flexibleWidth;

	protected float m_minWidth;

	protected float m_minHeight;

	protected float m_maxWidth;

	protected float m_maxHeight;

	protected LayoutElement m_LayoutElement;

	protected float m_preferredWidth;

	protected float m_renderedWidth;

	protected bool m_isPreferredWidthDirty;

	protected float m_preferredHeight;

	protected float m_renderedHeight;

	protected bool m_isPreferredHeightDirty;

	protected bool m_isCalculatingPreferredValues;

	private int m_recursiveCount;

	protected int m_layoutPriority;

	protected bool m_isCalculateSizeRequired;

	protected bool m_isLayoutDirty;

	protected bool m_verticesAlreadyDirty;

	protected bool m_layoutAlreadyDirty;

	protected bool m_isAwake;

	[SerializeField]
	protected bool m_isInputParsingRequired;

	[SerializeField]
	protected TextInputSources m_inputSource;

	protected string old_text;

	protected float m_fontScale;

	protected float m_fontScaleMultiplier;

	protected char[] m_htmlTag;

	protected XML_TagAttribute[] m_xmlAttribute;

	protected float[] m_attributeParameterValues;

	protected float tag_LineIndent;

	protected float tag_Indent;

	protected TMP_XmlTagStack<float> m_indentStack;

	protected bool tag_NoParsing;

	protected bool m_isParsingText;

	protected Matrix4x4 m_FXMatrix;

	protected bool m_isFXMatrixSet;

	protected int[] m_char_buffer;

	private TMP_CharacterInfo[] m_internalCharacterInfo;

	protected char[] m_input_CharArray;

	private int m_charArray_Length;

	protected int m_totalCharacterCount;

	protected WordWrapState m_SavedWordWrapState;

	protected WordWrapState m_SavedLineState;

	protected int m_characterCount;

	protected int m_firstCharacterOfLine;

	protected int m_firstVisibleCharacterOfLine;

	protected int m_lastCharacterOfLine;

	protected int m_lastVisibleCharacterOfLine;

	protected int m_lineNumber;

	protected int m_lineVisibleCharacterCount;

	protected int m_pageNumber;

	protected float m_maxAscender;

	protected float m_maxCapHeight;

	protected float m_maxDescender;

	protected float m_maxLineAscender;

	protected float m_maxLineDescender;

	protected float m_startOfLineAscender;

	protected float m_lineOffset;

	protected Extents m_meshExtents;

	protected Color32 m_htmlColor;

	protected TMP_XmlTagStack<Color32> m_colorStack;

	protected TMP_XmlTagStack<Color32> m_underlineColorStack;

	protected TMP_XmlTagStack<Color32> m_strikethroughColorStack;

	protected TMP_XmlTagStack<Color32> m_highlightColorStack;

	protected TMP_ColorGradient m_colorGradientPreset;

	protected TMP_XmlTagStack<TMP_ColorGradient> m_colorGradientStack;

	protected float m_tabSpacing;

	protected float m_spacing;

	protected TMP_XmlTagStack<int> m_styleStack;

	protected TMP_XmlTagStack<int> m_actionStack;

	protected float m_padding;

	protected float m_baselineOffset;

	protected TMP_XmlTagStack<float> m_baselineOffsetStack;

	protected float m_xAdvance;

	protected TMP_TextElementType m_textElementType;

	protected TMP_TextElement m_cached_TextElement;

	protected TMP_Glyph m_cached_Underline_GlyphInfo;

	protected TMP_Glyph m_cached_Ellipsis_GlyphInfo;

	protected TMP_SpriteAsset m_defaultSpriteAsset;

	protected TMP_SpriteAsset m_currentSpriteAsset;

	protected int m_spriteCount;

	protected int m_spriteIndex;

	protected int m_spriteAnimationID;

	protected bool m_ignoreActiveState;

	private readonly float[] k_Power;

	protected static Vector2 k_LargePositiveVector2;

	protected static Vector2 k_LargeNegativeVector2;

	protected static float k_LargePositiveFloat;

	protected static float k_LargeNegativeFloat;

	protected static int k_LargePositiveInt;

	protected static int k_LargeNegativeInt;

	public string text
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

	public bool isRightToLeftText
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

	public TMP_FontAsset font
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

	public virtual Material fontSharedMaterial
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

	public virtual Material[] fontSharedMaterials
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

	public Material fontMaterial
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

	public virtual Material[] fontMaterials
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

	public override Color color
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

	public float alpha
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

	public bool enableVertexGradient
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

	public VertexGradient colorGradient
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

	public TMP_ColorGradient colorGradientPreset
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

	public TMP_SpriteAsset spriteAsset
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

	public bool tintAllSprites
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

	public bool overrideColorTags
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

	public Color32 faceColor
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

	public Color32 outlineColor
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

	public float outlineWidth
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

	public float fontSize
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

	public float fontScale
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int fontWeight
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

	public float pixelsPerUnit
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool enableAutoSizing
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

	public float fontSizeMin
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

	public float fontSizeMax
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

	public FontStyles fontStyle
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

	public bool isUsingBold
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public TextAlignmentOptions alignment
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

	public float characterSpacing
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

	public float wordSpacing
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

	public float lineSpacing
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

	public float lineSpacingAdjustment
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

	public float paragraphSpacing
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

	public float characterWidthAdjustment
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

	public bool enableWordWrapping
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

	public float wordWrappingRatios
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

	public TextOverflowModes overflowMode
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

	public bool isTextOverflowing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int firstOverflowCharacterIndex
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public TMP_Text linkedTextComponent
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

	public bool isLinkedTextComponent
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

	public bool isTextTruncated
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool enableKerning
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

	public bool extraPadding
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

	public bool richText
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

	public bool parseCtrlCharacters
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

	public bool isOverlay
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

	public bool isOrthographic
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

	public bool enableCulling
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

	public bool ignoreRectMaskCulling
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

	public bool ignoreVisibility
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

	public TextureMappingOptions horizontalMapping
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

	public TextureMappingOptions verticalMapping
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

	public float mappingUvLineOffset
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

	public TextRenderFlags renderMode
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

	public VertexSortingOrder geometrySortingOrder
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

	public int firstVisibleCharacter
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

	public int maxVisibleCharacters
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

	public int maxVisibleWords
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

	public int maxVisibleLines
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

	public bool useMaxVisibleDescender
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

	public int pageToDisplay
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

	public virtual Vector4 margin
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

	public TMP_TextInfo textInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool havePropertiesChanged
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

	public bool isUsingLegacyAnimationComponent
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

	public new Transform transform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public new RectTransform rectTransform
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual bool autoSizeTextContainer
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	public virtual Mesh mesh
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isVolumetricText
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

	public Bounds bounds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Bounds textBounds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected TMP_SpriteAnimator spriteAnimator
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float flexibleHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float flexibleWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float minWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float minHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float maxWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float maxHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	protected LayoutElement layoutElement
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual float preferredWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual float preferredHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual float renderedWidth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public virtual float renderedHeight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int layoutPriority
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_Text()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static TMP_Text()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void LoadFontAsset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetSharedMaterial(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Material GetMaterial(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetFontBaseMaterial(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Material[] GetSharedMaterials()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetSharedMaterials(Material[] materials)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Material[] GetMaterials(Material[] mats)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Material CreateMaterialInstance(Material source)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetVertexColorGradient(TMP_ColorGradient gradient)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetTextSortingOrder(VertexSortingOrder order)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetTextSortingOrder(int[] order)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetFaceColor(Color32 color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetOutlineColor(Color32 color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetOutlineThickness(float thickness)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetShaderDepth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetCulling()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetPaddingForMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual float GetPaddingForMaterial(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector3[] GetTextContainerLocalCorners()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ForceMeshUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ForceMeshUpdate(bool ignoreActiveState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetTextInternal(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateGeometry(Mesh mesh, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateVertexData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetVertices(Vector3[] vertices)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateMeshPadding()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void CrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ParseInputText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text, bool syncTextInputBox)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text, float arg0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text, float arg0, float arg1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(string text, float arg0, float arg1, float arg2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetText(StringBuilder text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCharArray(char[] sourceText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCharArray(char[] sourceText, int start, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCharArray(int[] sourceText, int start, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetTextArrayToCharArray(char[] sourceText, ref int[] charBuffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void StringToCharArray(string sourceText, ref int[] charBuffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void StringBuilderToIntArray(StringBuilder sourceText, ref int[] charBuffer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceOpeningStyleTag(ref string sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceOpeningStyleTag(ref int[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceOpeningStyleTag(ref char[] sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceOpeningStyleTag(ref StringBuilder sourceText, int srcIndex, out int srcOffset, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceClosingStyleTag(ref string sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceClosingStyleTag(ref int[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceClosingStyleTag(ref char[] sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ReplaceClosingStyleTag(ref StringBuilder sourceText, int srcIndex, ref int[] charBuffer, ref int writeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTagName(ref string text, string tag, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTagName(ref char[] text, string tag, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTagName(ref int[] text, string tag, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsTagName(ref StringBuilder text, string tag, int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetTagHashCode(ref string text, int index, out int closeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetTagHashCode(ref char[] text, int index, out int closeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetTagHashCode(ref int[] text, int index, out int closeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetTagHashCode(ref StringBuilder text, int index, out int closeIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ResizeInternalArray<T>(ref T[] array)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddFloatToCharArray(float number, ref int index, int precision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddIntToCharArray(int number, ref int index, int precision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual int SetArraySizes(int[] chars)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void GenerateTextMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetPreferredValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetPreferredValues(float width, float height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetPreferredValues(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetPreferredValues(string text, float width, float height)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetPreferredWidth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetPreferredWidth(Vector2 margin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetPreferredHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetPreferredHeight(Vector2 margin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetRenderedValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Vector2 GetRenderedValues(bool onlyVisibleCharacters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetRenderedWidth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetRenderedWidth(bool onlyVisibleCharacters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetRenderedHeight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetRenderedHeight(bool onlyVisibleCharacters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Vector2 CalculatePreferredValues(float defaultFontSize, Vector2 marginSize, bool ignoreTextAutoSizing)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual Bounds GetCompoundBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Bounds GetTextBounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Bounds GetTextBounds(bool onlyVisibleCharacters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void AdjustLineOffset(int startIndex, int endIndex, float offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ResizeLineExtents(int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual TMP_TextInfo GetTextInfo(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ComputeMarginSize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SaveWordWrappingState(ref WordWrapState state, int index, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int RestoreWordWrappingState(ref WordWrapState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SaveGlyphVertexInfo(float padding, float style_padding, Color32 vertexColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SaveSpriteVertexInfo(Color32 vertexColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FillCharacterVertexBuffers(int i, int index_X4)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FillCharacterVertexBuffers(int i, int index_X4, bool isVolumetric)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void FillSpriteVertexBuffers(int i, int index_X4)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DrawUnderlineMesh(Vector3 start, Vector3 end, ref int index, float startScale, float endScale, float maxScale, float sdfScale, Color32 underlineColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void DrawTextHighlight(Vector3 start, Vector3 end, ref int index, Color32 highlightColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void LoadDefaultSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void GetSpecialCharacters(TMP_FontAsset fontAsset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ReplaceTagWithCharacter(int[] chars, int insertionIndex, int tagLength, char c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected TMP_FontAsset GetFontAssetForWeight(int fontWeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetActiveSubMeshes(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ClearSubMeshObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ClearMesh()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void ClearMesh(bool uploadGeometry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual string GetParsedText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector2 PackUV(float x, float y, float scale)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float PackUV(float x, float y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int HexToInt(char hex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int GetUTF16(string text, int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int GetUTF16(StringBuilder text, int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int GetUTF32(string text, int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected int GetUTF32(StringBuilder text, int i)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Color32 HexCharsToColor(char[] hexChars, int tagCount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Color32 HexCharsToColor(char[] hexChars, int startIndex, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private int GetAttributeParameters(char[] chars, int startIndex, int length, ref float[] parameters)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float ConvertToFloat(char[] chars, int startIndex, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float ConvertToFloat(char[] chars, int startIndex, int length, out int lastIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected bool ValidateHtmlTag(int[] chars, int startIndex, out int endIndex)
	{
		throw null;
	}
}
