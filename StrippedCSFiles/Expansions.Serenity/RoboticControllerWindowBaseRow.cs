using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Serenity;

public abstract class RoboticControllerWindowBaseRow : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public enum rowTypes
	{
		Action,
		Axis,
		None
	}

	private struct HighlightStateStore
	{
		public bool state;

		public Color color;
	}

	[SerializeField]
	protected LayoutElement layout;

	public TextMeshProUGUI titleTextPart;

	public Button editNickButton;

	protected UIHoverText editNickTextHover;

	public TMP_InputField partNickNameInput;

	public TextMeshProUGUI titleTextField;

	[SerializeField]
	protected Button removeRowButton;

	protected UIHoverText removeRowTextHover;

	[SerializeField]
	protected RectTransform headerTransform;

	[SerializeField]
	protected float heightCollapsed;

	[SerializeField]
	protected float heightExpanded;

	protected bool expanded;

	protected PointerClickHandler headerClickHandler;

	[SerializeField]
	protected List<GameObject> hideWhenCollapsed;

	protected ControlledBase controlledItem;

	protected ControlledAxis controlledAxis;

	protected ControlledAction controlledAction;

	internal int rowIndex;

	private bool isMouseOver;

	private HighlightStateStore[] lastHighlightState;

	private int lastHighlightPartCount;

	public bool Expanded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public ModuleRoboticController Controller
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	public RoboticControllerWindow Window
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	internal ControlledBase ControlledItem
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public rowTypes rowType
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	internal uint PartPersistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal uint PartModulePersistentId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal string RowName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAxis
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsAction
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected RoboticControllerWindowBaseRow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AssignBaseReferenceVars(RoboticControllerWindow window, ModuleRoboticController controller, ControlledBase controlledItem)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	protected abstract void OnRowStart();

	protected abstract void OnRowDestroy();

	protected abstract void OnRowExpanded();

	protected abstract void OnRowCollapsed();

	protected abstract void UpdateUILayout(bool recreateLine = false);

	public abstract void InsertPoint(float timeValue);

	public abstract void SelectAllPoints();

	public abstract void SelectPointAtTime(float timeValue);

	protected abstract void OnPointSelectionChanged(CurvePanel panel, List<CurvePanelPoint> points);

	protected abstract void OnPointDragging(List<CurvePanelPoint> points);

	public abstract void ReverseCurve();

	internal abstract void ReloadCurve();

	internal abstract void RedrawCurve();

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
	private void EditButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputFieldDone(string newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClickRow(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRemoveRowClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleExpansion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void HighlightParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetPartHighlight(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UnHighlightParts()
	{
		throw null;
	}
}
