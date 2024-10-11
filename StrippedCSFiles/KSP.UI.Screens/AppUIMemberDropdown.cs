using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

[AppUI_Dropdown]
public class AppUIMemberDropdown : AppUIMember
{
	public class AppUIDropdownItemDictionary : DictionaryValueList<object, AppUIDropdownItem>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public AppUIDropdownItemDictionary()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int GetIndexOfKey(object key)
		{
			throw null;
		}
	}

	public class AppUIDropdownItem
	{
		public object key;

		public string text;

		public Sprite image;

		private TMP_Dropdown.OptionData tmp_OptionData;

		public TMP_Dropdown.OptionData TMP_OptionData
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AppUIDropdownItem()
		{
			throw null;
		}
	}

	public TMP_Dropdown dropdown;

	public string dropdownItemsFieldName;

	private List<AppUIDropdownItem> loadingList;

	public AppUIDropdownItemDictionary itemList;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIMemberDropdown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void BuildDropDownItemsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDropdownChanged(int value)
	{
		throw null;
	}
}
