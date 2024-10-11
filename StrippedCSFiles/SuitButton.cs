using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SuitButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum EVAHelmetNeckRingState
	{
		HelmetNeckRingOn,
		OnlyNeckRingOn,
		HelmetNeckRingOff
	}

	public int buttonIndex;

	[SerializeField]
	private Button button;

	[SerializeField]
	private Button suitLightPickerButton;

	[SerializeField]
	private SuitLightColorPicker suitLightColorPicker;

	public RawImage thumbImg;

	[SerializeField]
	private Image thumbMask;

	private KerbalPreview kerbalPreview;

	private ProtoCrewMember.KerbalSuit kerbalSuit;

	[SerializeField]
	private PreviewPanel previewPanel;

	[SerializeField]
	private Image previewPanelImg;

	[SerializeField]
	private Button helmetNeckRingButton;

	[SerializeField]
	private Sprite[] helmetNeckRingState;

	public ComboSelector comboSelector;

	[SerializeField]
	private TextMeshProUGUI comboName;

	private int helmetStates;

	public EVAHelmetNeckRingState evaHelmetNeckRingState;

	public int helmetNeckRingIndex;

	private float previewRotationSpeed;

	private float previewRotation;

	private Quaternion startRot;

	public SuitLightColorPicker SuitLightColorPicker
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public KerbalPreview KerbalPreview
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

	public ProtoCrewMember.KerbalSuit KerbalSuit
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

	public Button HelmetNeckRingButton
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Sprite[] HelmetNeckRingState
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public TextMeshProUGUI ComboName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int HelmetStates
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SuitButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RotatePreview()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenSuitLightPicker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHelmetNeckringSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GenerateButtonName(string kerbalName, string suitType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}
}
