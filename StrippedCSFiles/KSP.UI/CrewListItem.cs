using System;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KSP.UI;

public class CrewListItem : MonoBehaviour
{
	[Serializable]
	public class ClickEvent<ButtonTypes, CrewListItem> : UnityEvent<ButtonTypes, CrewListItem>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public ClickEvent()
		{
			throw null;
		}
	}

	public enum ButtonTypes
	{
		X,
		V,
		X2
	}

	public enum KerbalTypes
	{
		AVAILABLE,
		BADASS,
		EVA,
		RECRUIT,
		TOURIST
	}

	private bool mouseoverEnabled;

	public UIStateButton button;

	[SerializeField]
	private Button coatHangerButton;

	[SerializeField]
	private Button kerbalIconButton;

	public TextMeshProUGUI kerbalName;

	[SerializeField]
	public RawImage kerbalSprite;

	public TextMeshProUGUI xp_trait;

	[SerializeField]
	private Slider xp_slider;

	[SerializeField]
	private UIStateImage xp_levels;

	[SerializeField]
	private Slider slider_courage;

	[SerializeField]
	private Slider slider_stupidity;

	[SerializeField]
	private TextMeshProUGUI label;

	[SerializeField]
	private TooltipController_CrewAC tooltipController;

	[SerializeField]
	private UIHoverPanel hoverPanel;

	private ProtoCrewMember crew;

	private bool setup;

	private bool over;

	public bool isEmpty;

	public uint pUid;

	private KerbalTypes lastType;

	public ClickEvent<ButtonTypes, CrewListItem> onClick;

	private SuitCombos suitCombos;

	public bool MouseoverEnabled
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenHelmetSuitPickerWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTooltip(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLabel(string label)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetXP(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetXP(string trait, float xp, int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetButton(ButtonTypes type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetButtonEnabled(bool state, string disabledReasonTitle = "", string disabledReasonCaption = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetKerbal(ProtoCrewMember crew, KerbalTypes type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetKerbalAsApplicableType(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddButtonInputDelegate(UnityAction<ButtonTypes, CrewListItem> del)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStats(ProtoCrewMember pcm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStats(float courage, float stupidity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCourage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetStupidity()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCrewRef(ProtoCrewMember crewRef)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupSuitButtons(ProtoCrewMember crew)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoCrewMember GetCrewRef()
	{
		throw null;
	}
}
