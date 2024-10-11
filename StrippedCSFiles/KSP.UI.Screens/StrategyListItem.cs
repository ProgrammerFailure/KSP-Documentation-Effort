using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class StrategyListItem : MonoBehaviour
{
	public UIRadioButton toggleButton;

	public UIRadioButtonStateChanger toggleStateChanger;

	public RawImage icon;

	[SerializeField]
	private TextMeshProUGUI btnText;

	private string title;

	private string validColor;

	private string invalidColor;

	private bool _setup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StrategyListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(Texture texture, string title)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateTitle(string title, string colorHex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIRadioButton SetupButton(bool acceptable, Administration.StrategyWrapper wrapper, UnityAction<PointerEventData, UIRadioButton.CallType> onTrue, UnityAction<PointerEventData, UIRadioButton.CallType> onFalse)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateButton(bool acceptable)
	{
		throw null;
	}
}
