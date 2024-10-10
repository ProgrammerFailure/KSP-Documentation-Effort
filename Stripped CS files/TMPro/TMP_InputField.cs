using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro;

[AddComponentMenu("UI/TextMeshPro - Input Field", 11)]
public class TMP_InputField : Selectable, IUpdateSelectedHandler, IEventSystemHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, ISubmitHandler, ICanvasElement, IScrollHandler
{
	public enum ContentType
	{
		Standard,
		Autocorrected,
		IntegerNumber,
		DecimalNumber,
		Alphanumeric,
		Name,
		EmailAddress,
		Password,
		Pin,
		Custom
	}

	public enum InputType
	{
		Standard,
		AutoCorrect,
		Password
	}

	public enum CharacterValidation
	{
		None,
		Digit,
		Integer,
		Decimal,
		Alphanumeric,
		Name,
		Regex,
		EmailAddress,
		CustomValidator
	}

	public enum LineType
	{
		SingleLine,
		MultiLineSubmit,
		MultiLineNewline
	}

	public delegate char OnValidateInput(string text, int charIndex, char addedChar);

	[Serializable]
	public class SubmitEvent : UnityEvent<string>
	{
	}

	[Serializable]
	public class OnChangeEvent : UnityEvent<string>
	{
	}

	[Serializable]
	public class SelectionEvent : UnityEvent<string>
	{
	}

	[Serializable]
	public class TextSelectionEvent : UnityEvent<string, int, int>
	{
	}

	public enum EditState
	{
		Continue,
		Finish
	}

	public TouchScreenKeyboard m_Keyboard;

	public static readonly char[] kSeparators = new char[6] { ' ', '.', ',', '\t', '\r', '\n' };

	[SerializeField]
	public RectTransform m_TextViewport;

	[SerializeField]
	public TMP_Text m_TextComponent;

	public RectTransform m_TextComponentRectTransform;

	[SerializeField]
	public Graphic m_Placeholder;

	[SerializeField]
	public Scrollbar m_VerticalScrollbar;

	[SerializeField]
	public TMP_ScrollbarEventHandler m_VerticalScrollbarEventHandler;

	public float m_ScrollPosition;

	[SerializeField]
	public float m_ScrollSensitivity = 1f;

	[SerializeField]
	public ContentType m_ContentType;

	[SerializeField]
	public InputType m_InputType;

	[SerializeField]
	public char m_AsteriskChar = '*';

	[SerializeField]
	public TouchScreenKeyboardType m_KeyboardType;

	[SerializeField]
	public LineType m_LineType;

	[SerializeField]
	public bool m_HideMobileInput;

	[SerializeField]
	public CharacterValidation m_CharacterValidation;

	[SerializeField]
	public string m_RegexValue = string.Empty;

	[SerializeField]
	public float m_GlobalPointSize = 14f;

	[SerializeField]
	public int m_CharacterLimit;

	[SerializeField]
	public SubmitEvent m_OnEndEdit = new SubmitEvent();

	[SerializeField]
	public SubmitEvent m_OnSubmit = new SubmitEvent();

	[SerializeField]
	public SelectionEvent m_OnSelect = new SelectionEvent();

	[SerializeField]
	public SelectionEvent m_OnDeselect = new SelectionEvent();

	[SerializeField]
	public TextSelectionEvent m_OnTextSelection = new TextSelectionEvent();

	[SerializeField]
	public TextSelectionEvent m_OnEndTextSelection = new TextSelectionEvent();

	[SerializeField]
	public OnChangeEvent m_OnValueChanged = new OnChangeEvent();

	[SerializeField]
	public OnValidateInput m_OnValidateInput;

	[SerializeField]
	public Color m_CaretColor = new Color(10f / 51f, 10f / 51f, 10f / 51f, 1f);

	[SerializeField]
	public bool m_CustomCaretColor;

	[SerializeField]
	public Color m_SelectionColor = new Color(56f / 85f, 0.80784315f, 1f, 64f / 85f);

	[SerializeField]
	public string m_Text = string.Empty;

	[Range(0f, 4f)]
	[SerializeField]
	public float m_CaretBlinkRate = 0.85f;

	[SerializeField]
	[Range(1f, 5f)]
	public int m_CaretWidth = 1;

	[SerializeField]
	public bool m_ReadOnly;

	[SerializeField]
	public bool m_RichText = true;

	public int m_StringPosition;

	public int m_StringSelectPosition;

	public int m_CaretPosition;

	public int m_CaretSelectPosition;

	public RectTransform caretRectTrans;

	public UIVertex[] m_CursorVerts;

	public CanvasRenderer m_CachedInputRenderer;

	public Vector2 m_DefaultTransformPosition;

	public Vector2 m_LastPosition;

	[NonSerialized]
	public Mesh m_Mesh;

	public bool m_AllowInput;

	public bool m_ShouldActivateNextUpdate;

	public bool m_UpdateDrag;

	public bool m_DragPositionOutOfBounds;

	public const float kHScrollSpeed = 0.05f;

	public const float kVScrollSpeed = 0.1f;

	public bool m_CaretVisible;

	public Coroutine m_BlinkCoroutine;

	public float m_BlinkStartTime;

	public Coroutine m_DragCoroutine;

	public string m_OriginalText = "";

	public bool m_WasCanceled;

	public bool m_HasDoneFocusTransition;

	public bool m_IsScrollbarUpdateRequired;

	public bool m_IsUpdatingScrollbarValues;

	public bool m_isLastKeyBackspace;

	public float m_ClickStartTime;

	public float m_DoubleClickDelay = 0.5f;

	public const string kEmailSpecialCharacters = "!#$%&'*+-/=?^_`{|}~";

	[SerializeField]
	public TMP_FontAsset m_GlobalFontAsset;

	[SerializeField]
	public bool m_OnFocusSelectAll = true;

	public bool m_isSelectAll;

	[SerializeField]
	public bool m_ResetOnDeActivation = true;

	[SerializeField]
	public bool m_RestoreOriginalTextOnEscape = true;

	[SerializeField]
	public bool m_isRichTextEditingAllowed = true;

	[SerializeField]
	public TMP_InputValidator m_InputValidator;

	public bool m_isSelected;

	public bool isStringPositionDirty;

	public bool m_forceRectTransformAdjustment;

	public Event m_ProcessingEvent = new Event();

	public Mesh mesh
	{
		get
		{
			if (m_Mesh == null)
			{
				m_Mesh = new Mesh();
			}
			return m_Mesh;
		}
	}

	public bool shouldHideMobileInput
	{
		get
		{
			switch (Application.platform)
			{
			default:
				return true;
			case RuntimePlatform.IPhonePlayer:
			case RuntimePlatform.Android:
			case RuntimePlatform.TizenPlayer:
			case RuntimePlatform.tvOS:
				return m_HideMobileInput;
			}
		}
		set
		{
			SetPropertyUtility.SetStruct(ref m_HideMobileInput, value);
		}
	}

	public string text
	{
		get
		{
			return m_Text;
		}
		set
		{
			if (!(text == value))
			{
				if (value == null)
				{
					value = string.Empty;
				}
				m_Text = value;
				if (m_Keyboard != null)
				{
					m_Keyboard.text = m_Text;
				}
				if (m_StringPosition > m_Text.Length)
				{
					m_StringPosition = (m_StringSelectPosition = m_Text.Length);
				}
				AdjustTextPositionRelativeToViewport(0f);
				m_forceRectTransformAdjustment = true;
				SendOnValueChangedAndUpdateLabel();
			}
		}
	}

	public bool isFocused => m_AllowInput;

	public float caretBlinkRate
	{
		get
		{
			return m_CaretBlinkRate;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_CaretBlinkRate, value) && m_AllowInput)
			{
				SetCaretActive();
			}
		}
	}

	public int caretWidth
	{
		get
		{
			return m_CaretWidth;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_CaretWidth, value))
			{
				MarkGeometryAsDirty();
			}
		}
	}

	public RectTransform textViewport
	{
		get
		{
			return m_TextViewport;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_TextViewport, value);
		}
	}

	public TMP_Text textComponent
	{
		get
		{
			return m_TextComponent;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_TextComponent, value);
		}
	}

	public Graphic placeholder
	{
		get
		{
			return m_Placeholder;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_Placeholder, value);
		}
	}

	public Scrollbar verticalScrollbar
	{
		get
		{
			return m_VerticalScrollbar;
		}
		set
		{
			if (m_VerticalScrollbar != null)
			{
				m_VerticalScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChange);
			}
			SetPropertyUtility.SetClass(ref m_VerticalScrollbar, value);
			if ((bool)m_VerticalScrollbar)
			{
				m_VerticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
			}
		}
	}

	public float scrollSensitivity
	{
		get
		{
			return m_ScrollSensitivity;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_ScrollSensitivity, value))
			{
				MarkGeometryAsDirty();
			}
		}
	}

	public Color caretColor
	{
		get
		{
			if (!customCaretColor)
			{
				return textComponent.color;
			}
			return m_CaretColor;
		}
		set
		{
			if (SetPropertyUtility.SetColor(ref m_CaretColor, value))
			{
				MarkGeometryAsDirty();
			}
		}
	}

	public bool customCaretColor
	{
		get
		{
			return m_CustomCaretColor;
		}
		set
		{
			if (m_CustomCaretColor != value)
			{
				m_CustomCaretColor = value;
				MarkGeometryAsDirty();
			}
		}
	}

	public Color selectionColor
	{
		get
		{
			return m_SelectionColor;
		}
		set
		{
			if (SetPropertyUtility.SetColor(ref m_SelectionColor, value))
			{
				MarkGeometryAsDirty();
			}
		}
	}

	public SubmitEvent onEndEdit
	{
		get
		{
			return m_OnEndEdit;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnEndEdit, value);
		}
	}

	public SubmitEvent onSubmit
	{
		get
		{
			return m_OnSubmit;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnSubmit, value);
		}
	}

	public SelectionEvent onSelect
	{
		get
		{
			return m_OnSelect;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnSelect, value);
		}
	}

	public SelectionEvent onDeselect
	{
		get
		{
			return m_OnDeselect;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnDeselect, value);
		}
	}

	public TextSelectionEvent onTextSelection
	{
		get
		{
			return m_OnTextSelection;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnTextSelection, value);
		}
	}

	public TextSelectionEvent onEndTextSelection
	{
		get
		{
			return m_OnEndTextSelection;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnEndTextSelection, value);
		}
	}

	public OnChangeEvent onValueChanged
	{
		get
		{
			return m_OnValueChanged;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnValueChanged, value);
		}
	}

	public OnValidateInput onValidateInput
	{
		get
		{
			return m_OnValidateInput;
		}
		set
		{
			SetPropertyUtility.SetClass(ref m_OnValidateInput, value);
		}
	}

	public int characterLimit
	{
		get
		{
			return m_CharacterLimit;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_CharacterLimit, Math.Max(0, value)))
			{
				UpdateLabel();
			}
		}
	}

	public float pointSize
	{
		get
		{
			return m_GlobalPointSize;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_GlobalPointSize, Math.Max(0f, value)))
			{
				SetGlobalPointSize(m_GlobalPointSize);
				UpdateLabel();
			}
		}
	}

	public TMP_FontAsset fontAsset
	{
		get
		{
			return m_GlobalFontAsset;
		}
		set
		{
			if (SetPropertyUtility.SetClass(ref m_GlobalFontAsset, value))
			{
				SetGlobalFontAsset(m_GlobalFontAsset);
				UpdateLabel();
			}
		}
	}

	public bool onFocusSelectAll
	{
		get
		{
			return m_OnFocusSelectAll;
		}
		set
		{
			m_OnFocusSelectAll = value;
		}
	}

	public bool resetOnDeActivation
	{
		get
		{
			return m_ResetOnDeActivation;
		}
		set
		{
			m_ResetOnDeActivation = value;
		}
	}

	public bool restoreOriginalTextOnEscape
	{
		get
		{
			return m_RestoreOriginalTextOnEscape;
		}
		set
		{
			m_RestoreOriginalTextOnEscape = value;
		}
	}

	public bool isRichTextEditingAllowed
	{
		get
		{
			return m_isRichTextEditingAllowed;
		}
		set
		{
			m_isRichTextEditingAllowed = value;
		}
	}

	public ContentType contentType
	{
		get
		{
			return m_ContentType;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_ContentType, value))
			{
				EnforceContentType();
			}
		}
	}

	public LineType lineType
	{
		get
		{
			return m_LineType;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_LineType, value))
			{
				SetTextComponentWrapMode();
			}
			SetToCustomIfContentTypeIsNot(ContentType.Standard, ContentType.Autocorrected);
		}
	}

	public InputType inputType
	{
		get
		{
			return m_InputType;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_InputType, value))
			{
				SetToCustom();
			}
		}
	}

	public TouchScreenKeyboardType keyboardType
	{
		get
		{
			return m_KeyboardType;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_KeyboardType, value))
			{
				SetToCustom();
			}
		}
	}

	public CharacterValidation characterValidation
	{
		get
		{
			return m_CharacterValidation;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_CharacterValidation, value))
			{
				SetToCustom();
			}
		}
	}

	public TMP_InputValidator inputValidator
	{
		get
		{
			return m_InputValidator;
		}
		set
		{
			if (SetPropertyUtility.SetClass(ref m_InputValidator, value))
			{
				SetToCustom(CharacterValidation.CustomValidator);
			}
		}
	}

	public bool readOnly
	{
		get
		{
			return m_ReadOnly;
		}
		set
		{
			m_ReadOnly = value;
		}
	}

	public bool richText
	{
		get
		{
			return m_RichText;
		}
		set
		{
			m_RichText = value;
			SetTextComponentRichTextMode();
		}
	}

	public bool multiLine
	{
		get
		{
			if (m_LineType != LineType.MultiLineNewline)
			{
				return lineType == LineType.MultiLineSubmit;
			}
			return true;
		}
	}

	public char asteriskChar
	{
		get
		{
			return m_AsteriskChar;
		}
		set
		{
			if (SetPropertyUtility.SetStruct(ref m_AsteriskChar, value))
			{
				UpdateLabel();
			}
		}
	}

	public bool wasCanceled => m_WasCanceled;

	public int caretPositionInternal
	{
		get
		{
			return m_CaretPosition + Input.compositionString.Length;
		}
		set
		{
			m_CaretPosition = value;
			ClampCaretPos(ref m_CaretPosition);
		}
	}

	public int stringPositionInternal
	{
		get
		{
			return m_StringPosition + Input.compositionString.Length;
		}
		set
		{
			m_StringPosition = value;
			ClampStringPos(ref m_StringPosition);
		}
	}

	public int caretSelectPositionInternal
	{
		get
		{
			return m_CaretSelectPosition + Input.compositionString.Length;
		}
		set
		{
			m_CaretSelectPosition = value;
			ClampCaretPos(ref m_CaretSelectPosition);
		}
	}

	public int stringSelectPositionInternal
	{
		get
		{
			return m_StringSelectPosition + Input.compositionString.Length;
		}
		set
		{
			m_StringSelectPosition = value;
			ClampStringPos(ref m_StringSelectPosition);
		}
	}

	public bool hasSelection => stringPositionInternal != stringSelectPositionInternal;

	public int caretPosition
	{
		get
		{
			return caretSelectPositionInternal;
		}
		set
		{
			selectionAnchorPosition = value;
			selectionFocusPosition = value;
			isStringPositionDirty = true;
		}
	}

	public int selectionAnchorPosition
	{
		get
		{
			return caretPositionInternal;
		}
		set
		{
			if (Input.compositionString.Length == 0)
			{
				caretPositionInternal = value;
				isStringPositionDirty = true;
			}
		}
	}

	public int selectionFocusPosition
	{
		get
		{
			return caretSelectPositionInternal;
		}
		set
		{
			if (Input.compositionString.Length == 0)
			{
				caretSelectPositionInternal = value;
				isStringPositionDirty = true;
			}
		}
	}

	public int stringPosition
	{
		get
		{
			return stringSelectPositionInternal;
		}
		set
		{
			selectionStringAnchorPosition = value;
			selectionStringFocusPosition = value;
		}
	}

	public int selectionStringAnchorPosition
	{
		get
		{
			return stringPositionInternal;
		}
		set
		{
			if (Input.compositionString.Length == 0)
			{
				stringPositionInternal = value;
			}
		}
	}

	public int selectionStringFocusPosition
	{
		get
		{
			return stringSelectPositionInternal;
		}
		set
		{
			if (Input.compositionString.Length == 0)
			{
				stringSelectPositionInternal = value;
			}
		}
	}

	public static string clipboard
	{
		get
		{
			return GUIUtility.systemCopyBuffer;
		}
		set
		{
			GUIUtility.systemCopyBuffer = value;
		}
	}

	Transform ICanvasElement.transform => base.transform;

	public void ClampStringPos(ref int pos)
	{
		if (pos < 0)
		{
			pos = 0;
		}
		else if (pos > text.Length)
		{
			pos = text.Length;
		}
	}

	public void ClampCaretPos(ref int pos)
	{
		if (pos < 0)
		{
			pos = 0;
		}
		else if (pos > m_TextComponent.textInfo.characterCount - 1)
		{
			pos = m_TextComponent.textInfo.characterCount - 1;
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		if (m_Text == null)
		{
			m_Text = string.Empty;
		}
		if (Application.isPlaying && m_CachedInputRenderer == null && m_TextComponent != null)
		{
			GameObject gameObject = new GameObject(base.transform.name + " Input Caret", typeof(RectTransform));
			TMP_SelectionCaret tMP_SelectionCaret = gameObject.AddComponent<TMP_SelectionCaret>();
			tMP_SelectionCaret.raycastTarget = false;
			tMP_SelectionCaret.color = Color.clear;
			gameObject.hideFlags = HideFlags.DontSave;
			gameObject.transform.SetParent(m_TextComponent.transform.parent);
			gameObject.transform.SetAsFirstSibling();
			gameObject.layer = base.gameObject.layer;
			caretRectTrans = gameObject.GetComponent<RectTransform>();
			m_CachedInputRenderer = gameObject.GetComponent<CanvasRenderer>();
			m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
			gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
			AssignPositioningIfNeeded();
		}
		if (m_CachedInputRenderer != null)
		{
			m_CachedInputRenderer.SetMaterial(Graphic.defaultGraphicMaterial, Texture2D.whiteTexture);
		}
		if (m_TextComponent != null)
		{
			m_TextComponent.RegisterDirtyVerticesCallback(MarkGeometryAsDirty);
			m_TextComponent.RegisterDirtyVerticesCallback(UpdateLabel);
			m_TextComponent.ignoreRectMaskCulling = true;
			m_DefaultTransformPosition = m_TextComponent.rectTransform.localPosition;
			if (m_VerticalScrollbar != null)
			{
				m_VerticalScrollbar.onValueChanged.AddListener(OnScrollbarValueChange);
			}
			UpdateLabel();
		}
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
	}

	public override void OnDisable()
	{
		m_BlinkCoroutine = null;
		DeactivateInputField();
		if (m_TextComponent != null)
		{
			m_TextComponent.UnregisterDirtyVerticesCallback(MarkGeometryAsDirty);
			m_TextComponent.UnregisterDirtyVerticesCallback(UpdateLabel);
			if (m_VerticalScrollbar != null)
			{
				m_VerticalScrollbar.onValueChanged.RemoveListener(OnScrollbarValueChange);
			}
		}
		CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(this);
		if (m_CachedInputRenderer != null)
		{
			m_CachedInputRenderer.Clear();
		}
		if (m_Mesh != null)
		{
			UnityEngine.Object.DestroyImmediate(m_Mesh);
		}
		m_Mesh = null;
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
		base.OnDisable();
	}

	public void ON_TEXT_CHANGED(UnityEngine.Object obj)
	{
		if (obj == m_TextComponent && Application.isPlaying)
		{
			caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
		}
	}

	public IEnumerator CaretBlink()
	{
		m_CaretVisible = true;
		yield return null;
		while (m_CaretBlinkRate > 0f)
		{
			float num = 1f / m_CaretBlinkRate;
			bool flag = (Time.unscaledTime - m_BlinkStartTime) % num < num / 2f;
			if (m_CaretVisible != flag)
			{
				m_CaretVisible = flag;
				if (!hasSelection)
				{
					MarkGeometryAsDirty();
				}
			}
			yield return null;
		}
		m_BlinkCoroutine = null;
	}

	public void SetCaretVisible()
	{
		if (m_AllowInput)
		{
			m_CaretVisible = true;
			m_BlinkStartTime = Time.unscaledTime;
			SetCaretActive();
		}
	}

	public void SetCaretActive()
	{
		if (!m_AllowInput)
		{
			return;
		}
		if (m_CaretBlinkRate > 0f)
		{
			if (m_BlinkCoroutine == null)
			{
				m_BlinkCoroutine = StartCoroutine(CaretBlink());
			}
		}
		else
		{
			m_CaretVisible = true;
		}
	}

	public void OnFocus()
	{
		if (m_OnFocusSelectAll)
		{
			SelectAll();
		}
	}

	public void SelectAll()
	{
		m_isSelectAll = true;
		stringPositionInternal = text.Length;
		stringSelectPositionInternal = 0;
	}

	public void MoveTextEnd(bool shift)
	{
		if (m_isRichTextEditingAllowed)
		{
			int length = text.Length;
			if (shift)
			{
				stringSelectPositionInternal = length;
			}
			else
			{
				stringPositionInternal = length;
				stringSelectPositionInternal = stringPositionInternal;
			}
		}
		else
		{
			int num = m_TextComponent.textInfo.characterCount - 1;
			if (shift)
			{
				caretSelectPositionInternal = num;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(num);
			}
			else
			{
				int num3 = (caretSelectPositionInternal = num);
				caretPositionInternal = num3;
				num3 = (stringPositionInternal = GetStringIndexFromCaretPosition(num));
				stringSelectPositionInternal = num3;
			}
		}
		UpdateLabel();
	}

	public void MoveTextStart(bool shift)
	{
		if (m_isRichTextEditingAllowed)
		{
			int num = 0;
			if (shift)
			{
				stringSelectPositionInternal = num;
			}
			else
			{
				stringPositionInternal = num;
				stringSelectPositionInternal = stringPositionInternal;
			}
		}
		else
		{
			int num2 = 0;
			if (shift)
			{
				caretSelectPositionInternal = num2;
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(num2);
			}
			else
			{
				int num4 = (caretSelectPositionInternal = num2);
				caretPositionInternal = num4;
				num4 = (stringPositionInternal = GetStringIndexFromCaretPosition(num2));
				stringSelectPositionInternal = num4;
			}
		}
		UpdateLabel();
	}

	public void MoveToEndOfLine(bool shift, bool ctrl)
	{
		int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
		int num = (ctrl ? (m_TextComponent.textInfo.characterCount - 1) : m_TextComponent.textInfo.lineInfo[lineNumber].lastCharacterIndex);
		num = GetStringIndexFromCaretPosition(num);
		if (shift)
		{
			stringSelectPositionInternal = num;
		}
		else
		{
			stringPositionInternal = num;
			stringSelectPositionInternal = stringPositionInternal;
		}
		UpdateLabel();
	}

	public void MoveToStartOfLine(bool shift, bool ctrl)
	{
		int lineNumber = m_TextComponent.textInfo.characterInfo[caretPositionInternal].lineNumber;
		int num = ((!ctrl) ? m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex : 0);
		num = GetStringIndexFromCaretPosition(num);
		if (shift)
		{
			stringSelectPositionInternal = num;
		}
		else
		{
			stringPositionInternal = num;
			stringSelectPositionInternal = stringPositionInternal;
		}
		UpdateLabel();
	}

	public bool InPlaceEditing()
	{
		return !TouchScreenKeyboard.isSupported;
	}

	public virtual void LateUpdate()
	{
		if (m_ShouldActivateNextUpdate)
		{
			if (!isFocused)
			{
				ActivateInputFieldInternal();
				m_ShouldActivateNextUpdate = false;
				return;
			}
			m_ShouldActivateNextUpdate = false;
		}
		if (m_IsScrollbarUpdateRequired)
		{
			UpdateScrollbar();
			m_IsScrollbarUpdateRequired = false;
		}
		if (InPlaceEditing() || !isFocused)
		{
			return;
		}
		AssignPositioningIfNeeded();
		if (m_Keyboard != null && m_Keyboard.active)
		{
			string text = m_Keyboard.text;
			if (m_Text != text)
			{
				if (m_ReadOnly)
				{
					m_Keyboard.text = m_Text;
				}
				else
				{
					m_Text = "";
					for (int i = 0; i < text.Length; i++)
					{
						char c = text[i];
						if (c == '\r' || c == '\u0003')
						{
							c = '\n';
						}
						if (onValidateInput != null)
						{
							c = onValidateInput(m_Text, m_Text.Length, c);
						}
						else if (characterValidation != 0)
						{
							c = Validate(m_Text, m_Text.Length, c);
						}
						if (lineType != LineType.MultiLineSubmit || c != '\n')
						{
							if (c != 0)
							{
								m_Text += c;
							}
							continue;
						}
						m_Keyboard.text = m_Text;
						OnSubmit(null);
						OnDeselect(null);
						return;
					}
					if (characterLimit > 0 && m_Text.Length > characterLimit)
					{
						m_Text = m_Text.Substring(0, characterLimit);
					}
					int num = (stringSelectPositionInternal = m_Text.Length);
					stringPositionInternal = num;
					if (m_Text != text)
					{
						m_Keyboard.text = m_Text;
					}
					SendOnValueChangedAndUpdateLabel();
				}
			}
			if (m_Keyboard.done)
			{
				if (m_Keyboard.wasCanceled)
				{
					m_WasCanceled = true;
				}
				OnDeselect(null);
			}
			return;
		}
		if (m_Keyboard != null)
		{
			if (!m_ReadOnly)
			{
				this.text = m_Keyboard.text;
			}
			if (m_Keyboard.wasCanceled)
			{
				m_WasCanceled = true;
			}
			if (m_Keyboard.done)
			{
				OnSubmit(null);
			}
		}
		OnDeselect(null);
	}

	public bool MayDrag(PointerEventData eventData)
	{
		if (IsActive() && IsInteractable() && eventData.button == PointerEventData.InputButton.Left && m_TextComponent != null)
		{
			return m_Keyboard == null;
		}
		return false;
	}

	public virtual void OnBeginDrag(PointerEventData eventData)
	{
		if (MayDrag(eventData))
		{
			m_UpdateDrag = true;
		}
	}

	public virtual void OnDrag(PointerEventData eventData)
	{
		if (MayDrag(eventData))
		{
			CaretPosition cursor;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
			switch (cursor)
			{
			case CaretPosition.Left:
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
				break;
			case CaretPosition.Right:
				stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
				break;
			}
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			MarkGeometryAsDirty();
			m_DragPositionOutOfBounds = !RectTransformUtility.RectangleContainsScreenPoint(textViewport, eventData.position, eventData.pressEventCamera);
			if (m_DragPositionOutOfBounds && m_DragCoroutine == null)
			{
				m_DragCoroutine = StartCoroutine(MouseDragOutsideRect(eventData));
			}
			eventData.Use();
		}
	}

	public IEnumerator MouseDragOutsideRect(PointerEventData eventData)
	{
		while (m_UpdateDrag && m_DragPositionOutOfBounds)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(textViewport, eventData.position, eventData.pressEventCamera, out var localPoint);
			Rect rect = textViewport.rect;
			if (multiLine)
			{
				if (localPoint.y > rect.yMax)
				{
					MoveUp(shift: true, goToFirstChar: true);
				}
				else if (localPoint.y < rect.yMin)
				{
					MoveDown(shift: true, goToLastChar: true);
				}
			}
			else if (localPoint.x < rect.xMin)
			{
				MoveLeft(shift: true, ctrl: false);
			}
			else if (localPoint.x > rect.xMax)
			{
				MoveRight(shift: true, ctrl: false);
			}
			UpdateLabel();
			float seconds = (multiLine ? 0.1f : 0.05f);
			yield return new WaitForSeconds(seconds);
		}
		m_DragCoroutine = null;
	}

	public virtual void OnEndDrag(PointerEventData eventData)
	{
		if (MayDrag(eventData))
		{
			m_UpdateDrag = false;
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (!MayDrag(eventData))
		{
			return;
		}
		EventSystem.current.SetSelectedGameObject(base.gameObject, eventData);
		bool allowInput = m_AllowInput;
		base.OnPointerDown(eventData);
		if (!InPlaceEditing() && (m_Keyboard == null || !m_Keyboard.active))
		{
			OnSelect(eventData);
			return;
		}
		bool flag = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
		bool flag2 = false;
		float unscaledTime = Time.unscaledTime;
		if (m_ClickStartTime + m_DoubleClickDelay > unscaledTime)
		{
			flag2 = true;
		}
		m_ClickStartTime = unscaledTime;
		if (allowInput || !m_OnFocusSelectAll)
		{
			CaretPosition cursor;
			int cursorIndexFromPosition = TMP_TextUtilities.GetCursorIndexFromPosition(m_TextComponent, eventData.position, eventData.pressEventCamera, out cursor);
			if (flag)
			{
				switch (cursor)
				{
				case CaretPosition.Left:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition);
					break;
				case CaretPosition.Right:
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1;
					break;
				}
			}
			else
			{
				switch (cursor)
				{
				case CaretPosition.Left:
				{
					int num2 = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition));
					stringPositionInternal = num2;
					break;
				}
				case CaretPosition.Right:
				{
					int num2 = (stringSelectPositionInternal = GetStringIndexFromCaretPosition(cursorIndexFromPosition) + 1);
					stringPositionInternal = num2;
					break;
				}
				}
			}
			if (flag2)
			{
				int num3 = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, eventData.position, eventData.pressEventCamera);
				if (num3 != -1)
				{
					caretPositionInternal = m_TextComponent.textInfo.wordInfo[num3].firstCharacterIndex;
					caretSelectPositionInternal = m_TextComponent.textInfo.wordInfo[num3].lastCharacterIndex + 1;
					stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
					stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
				}
				else
				{
					caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
					stringSelectPositionInternal++;
					caretSelectPositionInternal = caretPositionInternal + 1;
					caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
				}
			}
			else
			{
				int num2 = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal));
				caretPositionInternal = num2;
			}
		}
		UpdateLabel();
		eventData.Use();
	}

	public EditState KeyPressed(Event evt)
	{
		EventModifiers modifiers = evt.modifiers;
		RuntimePlatform platform = Application.platform;
		bool flag = ((platform == RuntimePlatform.OSXEditor || platform == RuntimePlatform.OSXPlayer) ? ((modifiers & EventModifiers.Command) != 0) : ((modifiers & EventModifiers.Control) != 0));
		bool flag2 = (modifiers & EventModifiers.Shift) != 0;
		bool flag3 = (modifiers & EventModifiers.Alt) != 0;
		bool flag4 = flag && !flag3 && !flag2;
		switch (evt.keyCode)
		{
		case KeyCode.Backspace:
			Backspace();
			return EditState.Continue;
		case KeyCode.A:
			if (flag4)
			{
				SelectAll();
				return EditState.Continue;
			}
			break;
		case KeyCode.Escape:
			m_WasCanceled = true;
			return EditState.Finish;
		case KeyCode.V:
			if (flag4)
			{
				Append(clipboard);
				return EditState.Continue;
			}
			break;
		case KeyCode.C:
			if (flag4)
			{
				if (inputType != InputType.Password)
				{
					clipboard = GetSelectedString();
				}
				else
				{
					clipboard = "";
				}
				return EditState.Continue;
			}
			break;
		case KeyCode.Return:
		case KeyCode.KeypadEnter:
			if (lineType != LineType.MultiLineNewline)
			{
				return EditState.Finish;
			}
			break;
		case KeyCode.UpArrow:
			MoveUp(flag2);
			return EditState.Continue;
		case KeyCode.DownArrow:
			MoveDown(flag2);
			return EditState.Continue;
		case KeyCode.RightArrow:
			MoveRight(flag2, flag);
			return EditState.Continue;
		case KeyCode.LeftArrow:
			MoveLeft(flag2, flag);
			return EditState.Continue;
		case KeyCode.Home:
			MoveToStartOfLine(flag2, flag);
			return EditState.Continue;
		case KeyCode.End:
			MoveToEndOfLine(flag2, flag);
			return EditState.Continue;
		case KeyCode.PageUp:
			MovePageUp(flag2);
			return EditState.Continue;
		case KeyCode.PageDown:
			MovePageDown(flag2);
			return EditState.Continue;
		case KeyCode.Delete:
			ForwardSpace();
			return EditState.Continue;
		case KeyCode.X:
			if (flag4)
			{
				if (inputType != InputType.Password)
				{
					clipboard = GetSelectedString();
				}
				else
				{
					clipboard = "";
				}
				Delete();
				SendOnValueChangedAndUpdateLabel();
				return EditState.Continue;
			}
			break;
		}
		char c = evt.character;
		if (!multiLine && (c == '\t' || c == '\r' || c == '\n'))
		{
			return EditState.Continue;
		}
		if (c == '\r' || c == '\u0003')
		{
			c = '\n';
		}
		if (IsValidChar(c))
		{
			Append(c);
		}
		if (c == '\0' && Input.compositionString.Length > 0)
		{
			UpdateLabel();
		}
		return EditState.Continue;
	}

	public bool IsValidChar(char c)
	{
		switch (c)
		{
		case '\u007f':
			return false;
		default:
			return m_TextComponent.font.HasCharacter(c, searchFallbacks: true);
		case '\t':
		case '\n':
			return true;
		}
	}

	public void ProcessEvent(Event e)
	{
		KeyPressed(e);
	}

	public virtual void OnUpdateSelected(BaseEventData eventData)
	{
		if (!isFocused)
		{
			return;
		}
		bool flag = false;
		while (Event.PopEvent(m_ProcessingEvent))
		{
			if (m_ProcessingEvent.rawType == EventType.KeyDown)
			{
				flag = true;
				if (KeyPressed(m_ProcessingEvent) == EditState.Finish)
				{
					SendOnSubmit();
					DeactivateInputField();
					break;
				}
			}
			EventType type = m_ProcessingEvent.type;
			if ((uint)(type - 13) <= 1u)
			{
				string commandName = m_ProcessingEvent.commandName;
				if (commandName == "SelectAll")
				{
					SelectAll();
					flag = true;
				}
			}
		}
		if (flag)
		{
			UpdateLabel();
		}
		eventData.Use();
	}

	public virtual void OnScroll(PointerEventData eventData)
	{
		if (!(m_TextComponent.preferredHeight < m_TextViewport.rect.height))
		{
			float num = 0f - eventData.scrollDelta.y;
			m_ScrollPosition += 1f / (float)m_TextComponent.textInfo.lineCount * num * m_ScrollSensitivity;
			m_ScrollPosition = Mathf.Clamp01(m_ScrollPosition);
			AdjustTextPositionRelativeToViewport(m_ScrollPosition);
			m_AllowInput = false;
			if ((bool)m_VerticalScrollbar)
			{
				m_IsUpdatingScrollbarValues = true;
				m_VerticalScrollbar.value = m_ScrollPosition;
			}
		}
	}

	public string GetSelectedString()
	{
		if (!hasSelection)
		{
			return "";
		}
		int num = stringPositionInternal;
		int num2 = stringSelectPositionInternal;
		if (num > num2)
		{
			int num3 = num;
			num = num2;
			num2 = num3;
		}
		return text.Substring(num, num2 - num);
	}

	public int FindtNextWordBegin()
	{
		if (stringSelectPositionInternal + 1 >= text.Length)
		{
			return text.Length;
		}
		int num = text.IndexOfAny(kSeparators, stringSelectPositionInternal + 1);
		if (num == -1)
		{
			return text.Length;
		}
		return num + 1;
	}

	public void MoveRight(bool shift, bool ctrl)
	{
		int num2;
		if (hasSelection && !shift)
		{
			num2 = (stringSelectPositionInternal = Mathf.Max(stringPositionInternal, stringSelectPositionInternal));
			stringPositionInternal = num2;
			num2 = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
			caretPositionInternal = num2;
			return;
		}
		int num3 = (ctrl ? FindtNextWordBegin() : ((!m_isRichTextEditingAllowed) ? GetStringIndexFromCaretPosition(caretSelectPositionInternal + 1) : (stringSelectPositionInternal + 1)));
		if (shift)
		{
			stringSelectPositionInternal = num3;
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			return;
		}
		num2 = (stringPositionInternal = num3);
		stringSelectPositionInternal = num2;
		num2 = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
		caretSelectPositionInternal = num2;
	}

	public int FindtPrevWordBegin()
	{
		if (stringSelectPositionInternal - 2 < 0)
		{
			return 0;
		}
		int num = text.LastIndexOfAny(kSeparators, stringSelectPositionInternal - 2);
		if (num == -1)
		{
			return 0;
		}
		return num + 1;
	}

	public void MoveLeft(bool shift, bool ctrl)
	{
		int num2;
		if (hasSelection && !shift)
		{
			num2 = (stringSelectPositionInternal = Mathf.Min(stringPositionInternal, stringSelectPositionInternal));
			stringPositionInternal = num2;
			num2 = (caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
			caretPositionInternal = num2;
			return;
		}
		int num3 = (ctrl ? FindtPrevWordBegin() : ((!m_isRichTextEditingAllowed) ? GetStringIndexFromCaretPosition(caretSelectPositionInternal - 1) : (stringSelectPositionInternal - 1)));
		if (shift)
		{
			stringSelectPositionInternal = num3;
			caretSelectPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal);
			return;
		}
		num2 = (stringPositionInternal = num3);
		stringSelectPositionInternal = num2;
		num2 = (caretPositionInternal = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
		caretSelectPositionInternal = num2;
	}

	public int LineUpCharacterPosition(int originalPos, bool goToFirstChar)
	{
		if (originalPos >= m_TextComponent.textInfo.characterCount)
		{
			originalPos--;
		}
		TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
		int lineNumber = tMP_CharacterInfo.lineNumber;
		if (lineNumber - 1 < 0)
		{
			if (!goToFirstChar)
			{
				return originalPos;
			}
			return 0;
		}
		int num = m_TextComponent.textInfo.lineInfo[lineNumber].firstCharacterIndex - 1;
		int num2 = -1;
		float num3 = 32767f;
		float num4 = 0f;
		int num5 = m_TextComponent.textInfo.lineInfo[lineNumber - 1].firstCharacterIndex;
		float num7;
		while (true)
		{
			if (num5 < num)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[num5];
				float num6 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				num7 = num6 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num7 >= 0f && !(num7 > 1f))
				{
					break;
				}
				num6 = Mathf.Abs(num6);
				if (num6 < num3)
				{
					num2 = num5;
					num3 = num6;
					num4 = num7;
				}
				num5++;
				continue;
			}
			if (num2 == -1)
			{
				return num;
			}
			if (num4 < 0.5f)
			{
				return num2;
			}
			return num2 + 1;
		}
		if (num7 < 0.5f)
		{
			return num5;
		}
		return num5 + 1;
	}

	public int LineDownCharacterPosition(int originalPos, bool goToLastChar)
	{
		if (originalPos >= m_TextComponent.textInfo.characterCount)
		{
			return m_TextComponent.textInfo.characterCount - 1;
		}
		TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
		int lineNumber = tMP_CharacterInfo.lineNumber;
		if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
		{
			if (!goToLastChar)
			{
				return originalPos;
			}
			return m_TextComponent.textInfo.characterCount - 1;
		}
		int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[lineNumber + 1].lastCharacterIndex;
		int num = -1;
		float num2 = 32767f;
		float num3 = 0f;
		int num4 = m_TextComponent.textInfo.lineInfo[lineNumber + 1].firstCharacterIndex;
		float num6;
		while (true)
		{
			if (num4 < lastCharacterIndex)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[num4];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f && !(num6 > 1f))
				{
					break;
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num2)
				{
					num = num4;
					num2 = num5;
					num3 = num6;
				}
				num4++;
				continue;
			}
			if (num == -1)
			{
				return lastCharacterIndex;
			}
			if (num3 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}
		if (num6 < 0.5f)
		{
			return num4;
		}
		return num4 + 1;
	}

	public int PageUpCharacterPosition(int originalPos, bool goToFirstChar)
	{
		if (originalPos >= m_TextComponent.textInfo.characterCount)
		{
			originalPos--;
		}
		TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
		int lineNumber = tMP_CharacterInfo.lineNumber;
		if (lineNumber - 1 < 0)
		{
			if (!goToFirstChar)
			{
				return originalPos;
			}
			return 0;
		}
		float height = m_TextViewport.rect.height;
		int num = lineNumber - 1;
		while (num > 0 && !(m_TextComponent.textInfo.lineInfo[num].baseline > m_TextComponent.textInfo.lineInfo[lineNumber].baseline + height))
		{
			num--;
		}
		int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[num].lastCharacterIndex;
		int num2 = -1;
		float num3 = 32767f;
		float num4 = 0f;
		int num5 = m_TextComponent.textInfo.lineInfo[num].firstCharacterIndex;
		float num7;
		while (true)
		{
			if (num5 < lastCharacterIndex)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[num5];
				float num6 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				num7 = num6 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num7 >= 0f && !(num7 > 1f))
				{
					break;
				}
				num6 = Mathf.Abs(num6);
				if (num6 < num3)
				{
					num2 = num5;
					num3 = num6;
					num4 = num7;
				}
				num5++;
				continue;
			}
			if (num2 == -1)
			{
				return lastCharacterIndex;
			}
			if (num4 < 0.5f)
			{
				return num2;
			}
			return num2 + 1;
		}
		if (num7 < 0.5f)
		{
			return num5;
		}
		return num5 + 1;
	}

	public int PageDownCharacterPosition(int originalPos, bool goToLastChar)
	{
		if (originalPos >= m_TextComponent.textInfo.characterCount)
		{
			return m_TextComponent.textInfo.characterCount - 1;
		}
		TMP_CharacterInfo tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[originalPos];
		int lineNumber = tMP_CharacterInfo.lineNumber;
		if (lineNumber + 1 >= m_TextComponent.textInfo.lineCount)
		{
			if (!goToLastChar)
			{
				return originalPos;
			}
			return m_TextComponent.textInfo.characterCount - 1;
		}
		float height = m_TextViewport.rect.height;
		int i;
		for (i = lineNumber + 1; i < m_TextComponent.textInfo.lineCount - 1 && !(m_TextComponent.textInfo.lineInfo[i].baseline < m_TextComponent.textInfo.lineInfo[lineNumber].baseline - height); i++)
		{
		}
		int lastCharacterIndex = m_TextComponent.textInfo.lineInfo[i].lastCharacterIndex;
		int num = -1;
		float num2 = 32767f;
		float num3 = 0f;
		int num4 = m_TextComponent.textInfo.lineInfo[i].firstCharacterIndex;
		float num6;
		while (true)
		{
			if (num4 < lastCharacterIndex)
			{
				TMP_CharacterInfo tMP_CharacterInfo2 = m_TextComponent.textInfo.characterInfo[num4];
				float num5 = tMP_CharacterInfo.origin - tMP_CharacterInfo2.origin;
				num6 = num5 / (tMP_CharacterInfo2.xAdvance - tMP_CharacterInfo2.origin);
				if (num6 >= 0f && !(num6 > 1f))
				{
					break;
				}
				num5 = Mathf.Abs(num5);
				if (num5 < num2)
				{
					num = num4;
					num2 = num5;
					num3 = num6;
				}
				num4++;
				continue;
			}
			if (num == -1)
			{
				return lastCharacterIndex;
			}
			if (num3 < 0.5f)
			{
				return num;
			}
			return num + 1;
		}
		if (num6 < 0.5f)
		{
			return num4;
		}
		return num4 + 1;
	}

	public void MoveDown(bool shift)
	{
		MoveDown(shift, goToLastChar: true);
	}

	public void MoveDown(bool shift, bool goToLastChar)
	{
		int num2;
		if (hasSelection && !shift)
		{
			num2 = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
			caretPositionInternal = num2;
		}
		int num3 = (multiLine ? LineDownCharacterPosition(caretSelectPositionInternal, goToLastChar) : (m_TextComponent.textInfo.characterCount - 1));
		if (shift)
		{
			caretSelectPositionInternal = num3;
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			return;
		}
		num2 = (caretPositionInternal = num3);
		caretSelectPositionInternal = num2;
		num2 = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal));
		stringSelectPositionInternal = num2;
	}

	public void MoveUp(bool shift)
	{
		MoveUp(shift, goToFirstChar: true);
	}

	public void MoveUp(bool shift, bool goToFirstChar)
	{
		int num2;
		if (hasSelection && !shift)
		{
			num2 = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
			caretPositionInternal = num2;
		}
		int num3 = (multiLine ? LineUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0);
		if (shift)
		{
			caretSelectPositionInternal = num3;
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			return;
		}
		num2 = (caretPositionInternal = num3);
		caretSelectPositionInternal = num2;
		num2 = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal));
		stringSelectPositionInternal = num2;
	}

	public void MovePageUp(bool shift)
	{
		MovePageUp(shift, goToFirstChar: true);
	}

	public void MovePageUp(bool shift, bool goToFirstChar)
	{
		if (hasSelection && !shift)
		{
			int num2 = (caretSelectPositionInternal = Mathf.Min(caretPositionInternal, caretSelectPositionInternal));
			caretPositionInternal = num2;
		}
		int num3 = (multiLine ? PageUpCharacterPosition(caretSelectPositionInternal, goToFirstChar) : 0);
		if (shift)
		{
			caretSelectPositionInternal = num3;
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
		}
		else
		{
			int num2 = (caretPositionInternal = num3);
			caretSelectPositionInternal = num2;
			num2 = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal));
			stringSelectPositionInternal = num2;
		}
		if (m_LineType != 0)
		{
			float height = m_TextViewport.rect.height;
			float num5 = m_TextComponent.rectTransform.position.y + m_TextComponent.textBounds.max.y;
			float num6 = m_TextViewport.position.y + m_TextViewport.rect.yMax;
			height = ((num6 > num5 + height) ? height : (num6 - num5));
			m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, height);
			AssignPositioningIfNeeded();
			m_IsScrollbarUpdateRequired = true;
		}
	}

	public void MovePageDown(bool shift)
	{
		MovePageDown(shift, goToLastChar: true);
	}

	public void MovePageDown(bool shift, bool goToLastChar)
	{
		if (hasSelection && !shift)
		{
			int num2 = (caretSelectPositionInternal = Mathf.Max(caretPositionInternal, caretSelectPositionInternal));
			caretPositionInternal = num2;
		}
		int num3 = (multiLine ? PageDownCharacterPosition(caretSelectPositionInternal, goToLastChar) : (m_TextComponent.textInfo.characterCount - 1));
		if (shift)
		{
			caretSelectPositionInternal = num3;
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
		}
		else
		{
			int num2 = (caretPositionInternal = num3);
			caretSelectPositionInternal = num2;
			num2 = (stringPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal));
			stringSelectPositionInternal = num2;
		}
		if (m_LineType != 0)
		{
			float height = m_TextViewport.rect.height;
			float num5 = m_TextComponent.rectTransform.position.y + m_TextComponent.textBounds.min.y;
			float num6 = m_TextViewport.position.y + m_TextViewport.rect.yMin;
			height = ((num6 > num5 + height) ? height : (num6 - num5));
			m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, height);
			AssignPositioningIfNeeded();
			m_IsScrollbarUpdateRequired = true;
		}
	}

	public void Delete()
	{
		if (m_ReadOnly || stringPositionInternal == stringSelectPositionInternal)
		{
			return;
		}
		if (!m_isRichTextEditingAllowed && !m_isSelectAll)
		{
			stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal);
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(caretSelectPositionInternal);
			if (caretPositionInternal < caretSelectPositionInternal)
			{
				m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
				stringSelectPositionInternal = stringPositionInternal;
				caretSelectPositionInternal = caretPositionInternal;
			}
			else
			{
				m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
				stringPositionInternal = stringSelectPositionInternal;
				stringPositionInternal = stringSelectPositionInternal;
				caretPositionInternal = caretSelectPositionInternal;
			}
		}
		else
		{
			if (stringPositionInternal < stringSelectPositionInternal)
			{
				m_Text = text.Substring(0, stringPositionInternal) + text.Substring(stringSelectPositionInternal, text.Length - stringSelectPositionInternal);
				stringSelectPositionInternal = stringPositionInternal;
			}
			else
			{
				m_Text = text.Substring(0, stringSelectPositionInternal) + text.Substring(stringPositionInternal, text.Length - stringPositionInternal);
				stringPositionInternal = stringSelectPositionInternal;
			}
			m_isSelectAll = false;
		}
	}

	public void ForwardSpace()
	{
		if (m_ReadOnly)
		{
			return;
		}
		if (hasSelection)
		{
			Delete();
			SendOnValueChangedAndUpdateLabel();
		}
		else if (m_isRichTextEditingAllowed)
		{
			if (stringPositionInternal < text.Length)
			{
				m_Text = text.Remove(stringPositionInternal, 1);
				SendOnValueChangedAndUpdateLabel();
			}
		}
		else if (caretPositionInternal < m_TextComponent.textInfo.characterCount - 1)
		{
			int num = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal));
			stringSelectPositionInternal = num;
			m_Text = text.Remove(stringPositionInternal, 1);
			SendOnValueChangedAndUpdateLabel();
		}
	}

	public void Backspace()
	{
		if (m_ReadOnly)
		{
			return;
		}
		if (hasSelection)
		{
			Delete();
			SendOnValueChangedAndUpdateLabel();
			return;
		}
		if (m_isRichTextEditingAllowed)
		{
			if (stringPositionInternal > 0)
			{
				m_Text = text.Remove(stringPositionInternal - 1, 1);
				stringSelectPositionInternal = --stringPositionInternal;
				m_isLastKeyBackspace = true;
				SendOnValueChangedAndUpdateLabel();
			}
			return;
		}
		if (caretPositionInternal > 0)
		{
			m_Text = text.Remove(GetStringIndexFromCaretPosition(caretPositionInternal - 1), 1);
			caretSelectPositionInternal = --caretPositionInternal;
			int num = (stringPositionInternal = GetStringIndexFromCaretPosition(caretPositionInternal));
			stringSelectPositionInternal = num;
		}
		m_isLastKeyBackspace = true;
		SendOnValueChangedAndUpdateLabel();
	}

	public virtual void Append(string input)
	{
		if (m_ReadOnly || !InPlaceEditing())
		{
			return;
		}
		int i = 0;
		for (int length = input.Length; i < length; i++)
		{
			char c = input[i];
			if (c >= ' ' || c == '\t' || c == '\r' || c == '\n' || c == '\n')
			{
				Append(c);
			}
		}
	}

	public virtual void Append(char input)
	{
		if (m_ReadOnly || !InPlaceEditing())
		{
			return;
		}
		if (onValidateInput != null)
		{
			input = onValidateInput(text, stringPositionInternal, input);
		}
		else
		{
			if (characterValidation == CharacterValidation.CustomValidator)
			{
				input = Validate(text, stringPositionInternal, input);
				if (input != 0)
				{
					SendOnValueChanged();
					UpdateLabel();
				}
				return;
			}
			if (characterValidation != 0)
			{
				input = Validate(text, stringPositionInternal, input);
			}
		}
		if (input != 0)
		{
			Insert(input);
		}
	}

	public void Insert(char c)
	{
		if (!m_ReadOnly)
		{
			string text = c.ToString();
			Delete();
			if (characterLimit <= 0 || this.text.Length < characterLimit)
			{
				m_Text = this.text.Insert(m_StringPosition, text);
				stringSelectPositionInternal = (stringPositionInternal += text.Length);
				SendOnValueChanged();
			}
		}
	}

	public void SendOnValueChangedAndUpdateLabel()
	{
		SendOnValueChanged();
		UpdateLabel();
	}

	public void SendOnValueChanged()
	{
		if (onValueChanged != null)
		{
			onValueChanged.Invoke(text);
		}
	}

	public void SendOnEndEdit()
	{
		if (onEndEdit != null)
		{
			onEndEdit.Invoke(m_Text);
		}
	}

	public void SendOnSubmit()
	{
		if (onSubmit != null)
		{
			onSubmit.Invoke(m_Text);
		}
	}

	public void SendOnFocus()
	{
		if (onSelect != null)
		{
			onSelect.Invoke(m_Text);
		}
	}

	public void SendOnFocusLost()
	{
		if (onDeselect != null)
		{
			onDeselect.Invoke(m_Text);
		}
	}

	public void SendOnTextSelection()
	{
		m_isSelected = true;
		if (onTextSelection != null)
		{
			onTextSelection.Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
		}
	}

	public void SendOnEndTextSelection()
	{
		if (m_isSelected)
		{
			if (onEndTextSelection != null)
			{
				onEndTextSelection.Invoke(m_Text, stringPositionInternal, stringSelectPositionInternal);
			}
			m_isSelected = false;
		}
	}

	public void UpdateLabel()
	{
		if (m_TextComponent != null && m_TextComponent.font != null)
		{
			string text = ((Input.compositionString.Length <= 0) ? this.text : (this.text.Substring(0, m_StringPosition) + Input.compositionString + this.text.Substring(m_StringPosition)));
			string text2 = ((inputType != InputType.Password) ? text : new string(asteriskChar, text.Length));
			bool flag = string.IsNullOrEmpty(text);
			if (m_Placeholder != null)
			{
				m_Placeholder.enabled = flag;
			}
			if (!flag)
			{
				SetCaretVisible();
			}
			m_TextComponent.text = text2 + "\u200b";
			MarkGeometryAsDirty();
			m_IsScrollbarUpdateRequired = true;
		}
	}

	public void UpdateScrollbar()
	{
		if ((bool)m_VerticalScrollbar)
		{
			float size = m_TextViewport.rect.height / m_TextComponent.preferredHeight;
			m_IsUpdatingScrollbarValues = true;
			m_VerticalScrollbar.size = size;
			float scrollPosition = (m_VerticalScrollbar.value = m_TextComponent.rectTransform.anchoredPosition.y / (m_TextComponent.preferredHeight - m_TextViewport.rect.height));
			m_ScrollPosition = scrollPosition;
		}
	}

	public void OnScrollbarValueChange(float value)
	{
		if (m_IsUpdatingScrollbarValues)
		{
			m_IsUpdatingScrollbarValues = false;
		}
		else if (!(value < 0f) && value <= 1f)
		{
			AdjustTextPositionRelativeToViewport(value);
			m_ScrollPosition = value;
		}
	}

	public void AdjustTextPositionRelativeToViewport(float relativePosition)
	{
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		if (textInfo != null && textInfo.lineInfo != null && textInfo.lineCount != 0 && textInfo.lineCount <= textInfo.lineInfo.Length)
		{
			m_TextComponent.rectTransform.anchoredPosition = new Vector2(m_TextComponent.rectTransform.anchoredPosition.x, (m_TextComponent.preferredHeight - m_TextViewport.rect.height) * relativePosition);
			AssignPositioningIfNeeded();
		}
	}

	public int GetCaretPositionFromStringIndex(int stringIndex)
	{
		int characterCount = m_TextComponent.textInfo.characterCount;
		int num = 0;
		while (true)
		{
			if (num < characterCount)
			{
				if (m_TextComponent.textInfo.characterInfo[num].index >= stringIndex)
				{
					break;
				}
				num++;
				continue;
			}
			return characterCount;
		}
		return num;
	}

	public int GetStringIndexFromCaretPosition(int caretPosition)
	{
		ClampCaretPos(ref caretPosition);
		return m_TextComponent.textInfo.characterInfo[caretPosition].index;
	}

	public void ForceLabelUpdate()
	{
		UpdateLabel();
	}

	public void MarkGeometryAsDirty()
	{
		CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
	}

	public virtual void Rebuild(CanvasUpdate update)
	{
		if (update == CanvasUpdate.LatePreRender)
		{
			UpdateGeometry();
		}
	}

	public virtual void LayoutComplete()
	{
	}

	public virtual void GraphicUpdateComplete()
	{
	}

	public void UpdateGeometry()
	{
		if (shouldHideMobileInput && !(m_CachedInputRenderer == null))
		{
			OnFillVBO(mesh);
			m_CachedInputRenderer.SetMesh(mesh);
		}
	}

	public void AssignPositioningIfNeeded()
	{
		if (m_TextComponent != null && caretRectTrans != null && (caretRectTrans.localPosition != m_TextComponent.rectTransform.localPosition || caretRectTrans.localRotation != m_TextComponent.rectTransform.localRotation || caretRectTrans.localScale != m_TextComponent.rectTransform.localScale || caretRectTrans.anchorMin != m_TextComponent.rectTransform.anchorMin || caretRectTrans.anchorMax != m_TextComponent.rectTransform.anchorMax || caretRectTrans.anchoredPosition != m_TextComponent.rectTransform.anchoredPosition || caretRectTrans.sizeDelta != m_TextComponent.rectTransform.sizeDelta || caretRectTrans.pivot != m_TextComponent.rectTransform.pivot))
		{
			caretRectTrans.localPosition = m_TextComponent.rectTransform.localPosition;
			caretRectTrans.localRotation = m_TextComponent.rectTransform.localRotation;
			caretRectTrans.localScale = m_TextComponent.rectTransform.localScale;
			caretRectTrans.anchorMin = m_TextComponent.rectTransform.anchorMin;
			caretRectTrans.anchorMax = m_TextComponent.rectTransform.anchorMax;
			caretRectTrans.anchoredPosition = m_TextComponent.rectTransform.anchoredPosition;
			caretRectTrans.sizeDelta = m_TextComponent.rectTransform.sizeDelta;
			caretRectTrans.pivot = m_TextComponent.rectTransform.pivot;
		}
	}

	public void OnFillVBO(Mesh vbo)
	{
		using VertexHelper vertexHelper = new VertexHelper();
		if (!isFocused && m_ResetOnDeActivation)
		{
			vertexHelper.FillMesh(vbo);
			return;
		}
		if (isStringPositionDirty)
		{
			stringPositionInternal = GetStringIndexFromCaretPosition(m_CaretPosition);
			stringSelectPositionInternal = GetStringIndexFromCaretPosition(m_CaretSelectPosition);
			isStringPositionDirty = false;
		}
		if (!hasSelection)
		{
			GenerateCaret(vertexHelper, Vector2.zero);
			SendOnEndTextSelection();
		}
		else
		{
			GenerateHightlight(vertexHelper, Vector2.zero);
			SendOnTextSelection();
		}
		vertexHelper.FillMesh(vbo);
	}

	public void GenerateCaret(VertexHelper vbo, Vector2 roundingOffset)
	{
		if (m_CaretVisible)
		{
			if (m_CursorVerts == null)
			{
				CreateCursorVerts();
			}
			float num = m_CaretWidth;
			int characterCount = m_TextComponent.textInfo.characterCount;
			Vector2 zero = Vector2.zero;
			float num2 = 0f;
			caretPositionInternal = GetCaretPositionFromStringIndex(stringPositionInternal);
			TMP_CharacterInfo tMP_CharacterInfo;
			if (caretPositionInternal == 0)
			{
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[0];
				zero = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			else if (caretPositionInternal < characterCount)
			{
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[caretPositionInternal];
				zero = new Vector2(tMP_CharacterInfo.origin, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			else
			{
				tMP_CharacterInfo = m_TextComponent.textInfo.characterInfo[characterCount - 1];
				zero = new Vector2(tMP_CharacterInfo.xAdvance, tMP_CharacterInfo.descender);
				num2 = tMP_CharacterInfo.ascender - tMP_CharacterInfo.descender;
			}
			if ((isFocused && zero != m_LastPosition) || m_forceRectTransformAdjustment)
			{
				AdjustRectTransformRelativeToViewport(zero, num2, tMP_CharacterInfo.isVisible);
			}
			m_LastPosition = zero;
			float num3 = zero.y + num2;
			float y = num3 - num2;
			m_CursorVerts[0].position = new Vector3(zero.x, y, 0f);
			m_CursorVerts[1].position = new Vector3(zero.x, num3, 0f);
			m_CursorVerts[2].position = new Vector3(zero.x + num, num3, 0f);
			m_CursorVerts[3].position = new Vector3(zero.x + num, y, 0f);
			m_CursorVerts[0].color = caretColor;
			m_CursorVerts[1].color = caretColor;
			m_CursorVerts[2].color = caretColor;
			m_CursorVerts[3].color = caretColor;
			vbo.AddUIVertexQuad(m_CursorVerts);
			int height = Screen.height;
			zero.y = (float)height - zero.y;
			Input.compositionCursorPos = zero;
		}
	}

	public void CreateCursorVerts()
	{
		m_CursorVerts = new UIVertex[4];
		for (int i = 0; i < m_CursorVerts.Length; i++)
		{
			m_CursorVerts[i] = UIVertex.simpleVert;
			m_CursorVerts[i].uv0 = Vector2.zero;
		}
	}

	public void GenerateHightlight(VertexHelper vbo, Vector2 roundingOffset)
	{
		TMP_TextInfo textInfo = m_TextComponent.textInfo;
		caretPositionInternal = (m_CaretPosition = GetCaretPositionFromStringIndex(stringPositionInternal));
		caretSelectPositionInternal = (m_CaretSelectPosition = GetCaretPositionFromStringIndex(stringSelectPositionInternal));
		float num = 0f;
		Vector2 startPosition;
		if (caretSelectPositionInternal < textInfo.characterCount)
		{
			startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal].origin, textInfo.characterInfo[caretSelectPositionInternal].descender);
			num = textInfo.characterInfo[caretSelectPositionInternal].ascender - textInfo.characterInfo[caretSelectPositionInternal].descender;
		}
		else
		{
			startPosition = new Vector2(textInfo.characterInfo[caretSelectPositionInternal - 1].xAdvance, textInfo.characterInfo[caretSelectPositionInternal - 1].descender);
			num = textInfo.characterInfo[caretSelectPositionInternal - 1].ascender - textInfo.characterInfo[caretSelectPositionInternal - 1].descender;
		}
		AdjustRectTransformRelativeToViewport(startPosition, num, isCharVisible: true);
		int num2 = Mathf.Max(0, caretPositionInternal);
		int num3 = Mathf.Max(0, caretSelectPositionInternal);
		if (num2 > num3)
		{
			int num4 = num2;
			num2 = num3;
			num3 = num4;
		}
		num3--;
		int num5 = textInfo.characterInfo[num2].lineNumber;
		int lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.uv0 = Vector2.zero;
		simpleVert.color = selectionColor;
		for (int i = num2; i <= num3 && i < textInfo.characterCount; i++)
		{
			if (i == lastCharacterIndex || i == num3)
			{
				TMP_CharacterInfo tMP_CharacterInfo = textInfo.characterInfo[num2];
				TMP_CharacterInfo tMP_CharacterInfo2 = textInfo.characterInfo[i];
				if (i > 0 && tMP_CharacterInfo2.character == '\n' && textInfo.characterInfo[i - 1].character == '\r')
				{
					tMP_CharacterInfo2 = textInfo.characterInfo[i - 1];
				}
				Vector2 vector = new Vector2(tMP_CharacterInfo.origin, textInfo.lineInfo[num5].ascender);
				Vector2 vector2 = new Vector2(tMP_CharacterInfo2.xAdvance, textInfo.lineInfo[num5].descender);
				int currentVertCount = vbo.currentVertCount;
				simpleVert.position = new Vector3(vector.x, vector2.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector2.x, vector2.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector2.x, vector.y, 0f);
				vbo.AddVert(simpleVert);
				simpleVert.position = new Vector3(vector.x, vector.y, 0f);
				vbo.AddVert(simpleVert);
				vbo.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
				vbo.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
				num2 = i + 1;
				num5++;
				if (num5 < textInfo.lineCount)
				{
					lastCharacterIndex = textInfo.lineInfo[num5].lastCharacterIndex;
				}
			}
		}
		m_IsScrollbarUpdateRequired = true;
	}

	public void AdjustRectTransformRelativeToViewport(Vector2 startPosition, float height, bool isCharVisible)
	{
		float xMin = m_TextViewport.rect.xMin;
		float xMax = m_TextViewport.rect.xMax;
		float num = xMax - (m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x + m_TextComponent.margin.z + (float)m_CaretWidth);
		if (num < 0f && (!multiLine || (multiLine && isCharVisible)))
		{
			m_TextComponent.rectTransform.anchoredPosition += new Vector2(num, 0f);
			AssignPositioningIfNeeded();
		}
		float num2 = m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x - m_TextComponent.margin.x - xMin;
		if (num2 < 0f)
		{
			m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f - num2, 0f);
			AssignPositioningIfNeeded();
		}
		if (m_LineType != 0)
		{
			float num3 = m_TextViewport.rect.yMax - (m_TextComponent.rectTransform.anchoredPosition.y + startPosition.y + height);
			if (num3 < -0.0001f)
			{
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(0f, num3);
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
			}
			float num4 = m_TextComponent.rectTransform.anchoredPosition.y + startPosition.y - m_TextViewport.rect.yMin;
			if (num4 < 0f)
			{
				m_TextComponent.rectTransform.anchoredPosition -= new Vector2(0f, num4);
				AssignPositioningIfNeeded();
				m_IsScrollbarUpdateRequired = true;
			}
		}
		if (m_isLastKeyBackspace)
		{
			float num5 = m_TextComponent.rectTransform.anchoredPosition.x + m_TextComponent.textInfo.characterInfo[0].origin - m_TextComponent.margin.x;
			float num6 = m_TextComponent.rectTransform.anchoredPosition.x + m_TextComponent.textInfo.characterInfo[m_TextComponent.textInfo.characterCount - 1].origin + m_TextComponent.margin.z;
			if (m_TextComponent.rectTransform.anchoredPosition.x + startPosition.x <= xMin + 0.0001f)
			{
				if (num5 < xMin)
				{
					float x = Mathf.Min((xMax - xMin) / 2f, xMin - num5);
					m_TextComponent.rectTransform.anchoredPosition += new Vector2(x, 0f);
					AssignPositioningIfNeeded();
				}
			}
			else if (num6 < xMax && num5 < xMin)
			{
				float x2 = Mathf.Min(xMax - num6, xMin - num5);
				m_TextComponent.rectTransform.anchoredPosition += new Vector2(x2, 0f);
				AssignPositioningIfNeeded();
			}
			m_isLastKeyBackspace = false;
		}
		m_forceRectTransformAdjustment = false;
	}

	public char Validate(string text, int pos, char ch)
	{
		if (characterValidation != 0 && base.enabled)
		{
			if (characterValidation != CharacterValidation.Integer && characterValidation != CharacterValidation.Decimal)
			{
				if (characterValidation == CharacterValidation.Digit)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
				}
				else if (characterValidation == CharacterValidation.Alphanumeric)
				{
					if (ch >= 'A' && ch <= 'Z')
					{
						return ch;
					}
					if (ch >= 'a' && ch <= 'z')
					{
						return ch;
					}
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
				}
				else if (characterValidation == CharacterValidation.Name)
				{
					char c = ((text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ');
					char c2 = ((text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n');
					if (char.IsLetter(ch))
					{
						if (char.IsLower(ch) && c == ' ')
						{
							return char.ToUpper(ch);
						}
						if (char.IsUpper(ch) && c != ' ' && c != '\'')
						{
							return char.ToLower(ch);
						}
						return ch;
					}
					switch (ch)
					{
					case '\'':
						if (c != ' ' && c != '\'' && c2 != '\'' && !text.Contains("'"))
						{
							return ch;
						}
						break;
					case ' ':
						if (c != ' ' && c != '\'' && c2 != ' ' && c2 != '\'')
						{
							return ch;
						}
						break;
					}
				}
				else if (characterValidation == CharacterValidation.EmailAddress)
				{
					if (ch >= 'A' && ch <= 'Z')
					{
						return ch;
					}
					if (ch >= 'a' && ch <= 'z')
					{
						return ch;
					}
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '@' && text.IndexOf('@') == -1)
					{
						return ch;
					}
					if ("!#$%&'*+-/=?^_`{|}~".IndexOf(ch) != -1)
					{
						return ch;
					}
					if (ch == '.')
					{
						char num = ((text.Length > 0) ? text[Mathf.Clamp(pos, 0, text.Length - 1)] : ' ');
						char c3 = ((text.Length > 0) ? text[Mathf.Clamp(pos + 1, 0, text.Length - 1)] : '\n');
						if (num != '.' && c3 != '.')
						{
							return ch;
						}
					}
				}
				else if (characterValidation == CharacterValidation.Regex)
				{
					if (Regex.IsMatch(ch.ToString(), m_RegexValue))
					{
						return ch;
					}
				}
				else if (characterValidation == CharacterValidation.CustomValidator && m_InputValidator != null)
				{
					char result = m_InputValidator.Validate(ref text, ref pos, ch);
					m_Text = text;
					int num3 = (stringPositionInternal = pos);
					stringSelectPositionInternal = num3;
					return result;
				}
			}
			else
			{
				bool num4 = pos == 0 && text.Length > 0 && text[0] == '-';
				bool flag = stringPositionInternal == 0 || stringSelectPositionInternal == 0;
				if (!num4)
				{
					if (ch >= '0' && ch <= '9')
					{
						return ch;
					}
					if (ch == '-' && (pos == 0 || flag))
					{
						return ch;
					}
					if (ch == '.' && characterValidation == CharacterValidation.Decimal && !text.Contains("."))
					{
						return ch;
					}
				}
			}
			return '\0';
		}
		return ch;
	}

	public void ActivateInputField()
	{
		if (!(m_TextComponent == null) && !(m_TextComponent.font == null) && IsActive() && IsInteractable())
		{
			if (isFocused && m_Keyboard != null && !m_Keyboard.active)
			{
				m_Keyboard.active = true;
				m_Keyboard.text = m_Text;
			}
			m_ShouldActivateNextUpdate = true;
		}
	}

	public void ActivateInputFieldInternal()
	{
		if (EventSystem.current == null)
		{
			return;
		}
		if (EventSystem.current.currentSelectedGameObject != base.gameObject)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
		}
		if (TouchScreenKeyboard.isSupported)
		{
			if (Input.touchSupported)
			{
				TouchScreenKeyboard.hideInput = shouldHideMobileInput;
			}
			m_Keyboard = ((inputType == InputType.Password) ? TouchScreenKeyboard.Open(m_Text, keyboardType, autocorrection: false, multiLine, secure: true) : TouchScreenKeyboard.Open(m_Text, keyboardType, inputType == InputType.AutoCorrect, multiLine));
			MoveTextEnd(shift: false);
		}
		else
		{
			Input.imeCompositionMode = IMECompositionMode.On;
			OnFocus();
		}
		m_AllowInput = true;
		m_OriginalText = text;
		m_WasCanceled = false;
		SetCaretVisible();
		UpdateLabel();
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		SendOnFocus();
		ActivateInputField();
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ActivateInputField();
		}
	}

	public void OnControlClick()
	{
	}

	public void DeactivateInputField()
	{
		if (!m_AllowInput)
		{
			return;
		}
		m_HasDoneFocusTransition = false;
		m_AllowInput = false;
		if (m_Placeholder != null)
		{
			m_Placeholder.enabled = string.IsNullOrEmpty(m_Text);
		}
		if (m_TextComponent != null && IsInteractable())
		{
			if (m_WasCanceled && m_RestoreOriginalTextOnEscape)
			{
				text = m_OriginalText;
			}
			if (m_Keyboard != null)
			{
				m_Keyboard.active = false;
				m_Keyboard = null;
			}
			if (m_ResetOnDeActivation)
			{
				m_StringSelectPosition = 0;
				m_StringPosition = 0;
				m_CaretSelectPosition = 0;
				m_CaretPosition = 0;
				m_TextComponent.rectTransform.localPosition = m_DefaultTransformPosition;
				if (caretRectTrans != null)
				{
					caretRectTrans.localPosition = Vector3.zero;
				}
			}
			SendOnEndEdit();
			SendOnEndTextSelection();
			Input.imeCompositionMode = IMECompositionMode.Auto;
		}
		MarkGeometryAsDirty();
		m_IsScrollbarUpdateRequired = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		DeactivateInputField();
		base.OnDeselect(eventData);
		SendOnFocusLost();
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		if (IsActive() && IsInteractable())
		{
			if (!isFocused)
			{
				m_ShouldActivateNextUpdate = true;
			}
			SendOnSubmit();
		}
	}

	public void EnforceContentType()
	{
		switch (contentType)
		{
		case ContentType.Standard:
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.Default;
			m_CharacterValidation = CharacterValidation.None;
			break;
		case ContentType.Autocorrected:
			m_InputType = InputType.AutoCorrect;
			m_KeyboardType = TouchScreenKeyboardType.Default;
			m_CharacterValidation = CharacterValidation.None;
			break;
		case ContentType.IntegerNumber:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.NumberPad;
			m_CharacterValidation = CharacterValidation.Integer;
			break;
		case ContentType.DecimalNumber:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
			m_CharacterValidation = CharacterValidation.Decimal;
			break;
		case ContentType.Alphanumeric:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.ASCIICapable;
			m_CharacterValidation = CharacterValidation.Alphanumeric;
			break;
		case ContentType.Name:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.Default;
			m_CharacterValidation = CharacterValidation.Name;
			break;
		case ContentType.EmailAddress:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Standard;
			m_KeyboardType = TouchScreenKeyboardType.EmailAddress;
			m_CharacterValidation = CharacterValidation.EmailAddress;
			break;
		case ContentType.Password:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Password;
			m_KeyboardType = TouchScreenKeyboardType.Default;
			m_CharacterValidation = CharacterValidation.None;
			break;
		case ContentType.Pin:
			m_LineType = LineType.SingleLine;
			m_TextComponent.enableWordWrapping = false;
			m_InputType = InputType.Password;
			m_KeyboardType = TouchScreenKeyboardType.NumberPad;
			m_CharacterValidation = CharacterValidation.Digit;
			break;
		}
	}

	public void SetTextComponentWrapMode()
	{
		if (!(m_TextComponent == null))
		{
			if (m_LineType == LineType.SingleLine)
			{
				m_TextComponent.enableWordWrapping = false;
			}
			else
			{
				m_TextComponent.enableWordWrapping = true;
			}
		}
	}

	public void SetTextComponentRichTextMode()
	{
		if (!(m_TextComponent == null))
		{
			m_TextComponent.richText = m_RichText;
		}
	}

	public void SetToCustomIfContentTypeIsNot(params ContentType[] allowedContentTypes)
	{
		if (contentType == ContentType.Custom)
		{
			return;
		}
		for (int i = 0; i < allowedContentTypes.Length; i++)
		{
			if (contentType == allowedContentTypes[i])
			{
				return;
			}
		}
		contentType = ContentType.Custom;
	}

	public void SetToCustom()
	{
		if (contentType != ContentType.Custom)
		{
			contentType = ContentType.Custom;
		}
	}

	public void SetToCustom(CharacterValidation characterValidation)
	{
		if (contentType == ContentType.Custom)
		{
			characterValidation = CharacterValidation.CustomValidator;
			return;
		}
		contentType = ContentType.Custom;
		characterValidation = CharacterValidation.CustomValidator;
	}

	public override void DoStateTransition(SelectionState state, bool instant)
	{
		if (m_HasDoneFocusTransition)
		{
			state = SelectionState.Highlighted;
		}
		else if (state == SelectionState.Pressed)
		{
			m_HasDoneFocusTransition = true;
		}
		base.DoStateTransition(state, instant);
	}

	public void SetGlobalPointSize(float pointSize)
	{
		TMP_Text tMP_Text = m_Placeholder as TMP_Text;
		if (tMP_Text != null)
		{
			tMP_Text.fontSize = pointSize;
		}
		textComponent.fontSize = pointSize;
	}

	public void SetGlobalFontAsset(TMP_FontAsset fontAsset)
	{
		TMP_Text tMP_Text = m_Placeholder as TMP_Text;
		if (tMP_Text != null)
		{
			tMP_Text.font = fontAsset;
		}
		textComponent.font = fontAsset;
	}
}
