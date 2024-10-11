using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KSP.UI.Screens;

public class RDDropDownList : MonoBehaviour
{
	public RDDropDownList listPrefab;

	public RDDropDownListItem listItemPrefab;

	public int headerHeight;

	public UIList scrollList;

	public TextMeshProUGUI nameField;

	[NonSerialized]
	public new string name;

	[NonSerialized]
	public bool open;

	[NonSerialized]
	public string sortingBy;

	[NonSerialized]
	public UIListItem selectedItem;

	private OnOpen callbackOpenClose;

	private OnSelectItem callbackSelectItem;

	private bool over;

	private int radioGroup;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDDropDownList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Close(bool force = false, bool callback = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CloseInternal(bool force = false, bool callback = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Open(bool force = false, bool callback = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenInternal(bool force = false, bool callback = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStandardToggleButtonInputDelegate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDDropDownList CreateInstanceFromPrefab(string name, string displayName, OnOpen listCallback, OnSelectItem itemCallback, int radioGroup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearList(bool removeFiltering = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem AddItem(string id, string header, string description)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateSelectedItem(string description, bool append = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateItem(int index, string description)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSelectedItem(string id, string header, string description)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Func<RDArchivesController.ReportData, bool> GetFilter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Func<RDArchivesController.ReportData, bool> GetBiomeFilter()
	{
		throw null;
	}
}
