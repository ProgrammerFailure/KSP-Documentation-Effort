using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ns2;
using TMPro;
using UnityEngine;

namespace ns11;

[AppUI_Dropdown]
public class AppUIMemberDropdown : AppUIMember
{
	public class AppUIDropdownItemDictionary : DictionaryValueList<object, AppUIDropdownItem>
	{
		public int GetIndexOfKey(object key)
		{
			int num = 0;
			while (true)
			{
				if (num < base.Count)
				{
					if (base.ValuesList[num].key.Equals(key))
					{
						break;
					}
					num++;
					continue;
				}
				return -1;
			}
			return num;
		}
	}

	public class AppUIDropdownItem
	{
		public object key;

		public string text;

		public Sprite image;

		public TMP_Dropdown.OptionData tmp_OptionData;

		public TMP_Dropdown.OptionData TMP_OptionData
		{
			get
			{
				tmp_OptionData.text = text;
				tmp_OptionData.image = image;
				return tmp_OptionData;
			}
		}

		public AppUIDropdownItem()
		{
			tmp_OptionData = new TMP_Dropdown.OptionData();
		}
	}

	public TMP_Dropdown dropdown;

	public string dropdownItemsFieldName;

	public List<AppUIDropdownItem> loadingList;

	public AppUIDropdownItemDictionary itemList;

	public override void OnStart()
	{
		dropdown.onValueChanged.AddListener(OnDropdownChanged);
	}

	public void OnDestroy()
	{
		dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
	}

	public override void OnInitialized()
	{
		if (_attribs is AppUI_Dropdown appUI_Dropdown)
		{
			dropdownItemsFieldName = (string.IsNullOrEmpty(appUI_Dropdown.dropdownItemsFieldName) ? "" : appUI_Dropdown.dropdownItemsFieldName);
		}
	}

	public virtual void BuildDropDownItemsList()
	{
		itemList = new AppUIDropdownItemDictionary();
		if (isEnum && string.IsNullOrWhiteSpace(dropdownItemsFieldName))
		{
			IEnumerator enumerator = Enum.GetValues(valueType).GetEnumerator();
			while (enumerator.MoveNext())
			{
				Enum @enum = (Enum)enumerator.Current;
				itemList.Add(@enum, new AppUIDropdownItem
				{
					key = @enum,
					text = @enum.displayDescription()
				});
			}
		}
		else if (!string.IsNullOrWhiteSpace(dropdownItemsFieldName))
		{
			FieldInfo field = _host.GetType().GetField(dropdownItemsFieldName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				Debug.LogError("[AppUIMemberDropdown]: Unable to get items list from field: " + dropdownItemsFieldName + ". List will be empty");
				return;
			}
			loadingList = (List<AppUIDropdownItem>)field.GetValue(_host);
			if (loadingList == null)
			{
				Debug.LogError("[AppUIMemberDropdown]: Unable to get items list from instance field: " + dropdownItemsFieldName + ". List will be empty");
				return;
			}
			for (int i = 0; i < loadingList.Count; i++)
			{
				itemList.Add(loadingList[i].key, new AppUIDropdownItem
				{
					key = loadingList[i].key,
					text = loadingList[i].text,
					image = loadingList[i].image
				});
			}
		}
		else
		{
			Debug.LogError("[AppUIMemberDropdown]: " + base.Name + " is not an enum and no List<AppUIDropdownItem> provided in dropdownItemsFieldName attribute. List will be empty");
		}
	}

	public override void OnRefreshUI()
	{
		BuildDropDownItemsList();
		dropdown.options.Clear();
		for (int i = 0; i < itemList.Count; i++)
		{
			dropdown.options.Add(itemList.ValuesList[i].TMP_OptionData);
		}
		if (itemList.GetIndexOfKey(GetValue()) > -1)
		{
			dropdown.value = itemList.GetIndexOfKey(GetValue());
		}
		else
		{
			dropdown.value = 0;
		}
		dropdown.RefreshShownValue();
	}

	public void OnDropdownChanged(int value)
	{
		if (value < itemList.Count)
		{
			SetValue(itemList.KeyAt(value));
		}
	}
}
