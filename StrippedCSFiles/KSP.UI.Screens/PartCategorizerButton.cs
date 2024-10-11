using System;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using RUI.Icons.Selectable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class PartCategorizerButton : MonoBehaviour
{
	public UIListItem container;

	public UIRadioButton btnToggleGeneric;

	public Button btnGeneric;

	public PointerEnterExitHandler hoverHandler;

	public Image divider;

	public Image dividerBottom;

	public Image dividerOverlay;

	public Image dividerSpaceUnder;

	public RawImage iconSpriteToggle;

	public RawImage iconSprite;

	public TooltipController_Text tooltipController;

	[NonSerialized]
	public string categoryName;

	[NonSerialized]
	public string categorydisplayName;

	[NonSerialized]
	public Icon icon;

	public Callback OnBtnTap;

	private bool lastBtn;

	private bool isHovering;

	private Color colorWhite;

	private Color colorBlack;

	public string displayCategoryName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public UIRadioButton activeButton
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

	public Color color
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

	public Color iconColor
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

	public bool IsToggleBtn
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

	public bool LastBtn
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
	public PartCategorizerButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitializeToggleBtn(string categoryName, string categorydisplayName, Icon icon, Color color, Color iconColor, bool last = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InitializeBtn(string categoryName, string categorydisplayName, Icon icon, Color color, Color iconColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(Icon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateIconState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceAspect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetRadioGroup(int group)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableDividers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDividerOverlay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDividerSpaceUnder()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_click()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_pointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetIcon(bool selected)
	{
		throw null;
	}
}
