using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class PartCategorizerPopup : MonoBehaviour
{
	public delegate bool CriteriaAccept(string categoryName, out string reason);

	public delegate bool CriteriaDelete(out string reason);

	public TextMeshProUGUI title;

	public Button btnClose;

	public Button btnDelete;

	public Button btnAccept;

	public InputField inputField;

	public UIList scrollList;

	private Callback onAccept;

	private Callback onDelete;

	public CriteriaAccept onAcceptCriteria;

	public CriteriaDelete onDeleteCriteria;

	private bool _started;

	private PartCategorizer.PopupData currentPopupData;

	private List<PartCategorizerButton> buttonList;

	private Icon selectedIcon;

	public bool Showing
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
	public PartCategorizerPopup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(PartCategorizer.PopupData popupData, Callback onAccept, CriteriaAccept onAcceptCriteria, Callback onDelete, CriteriaDelete onDeleteCriteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputAccept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AcceptDelete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateIconList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateIconList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartCategorizerButton CreateIcon(Icon icon, Color color, Color colorIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnIconSelect(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
	{
		throw null;
	}
}
