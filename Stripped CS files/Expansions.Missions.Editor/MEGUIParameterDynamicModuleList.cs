using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_DynamicModuleList]
public sealed class MEGUIParameterDynamicModuleList : MEGUICompoundParameter
{
	public Transform container;

	[SerializeField]
	public Transform collapseContainer;

	public TMP_Dropdown dropdownList;

	public MEGUIParameterDynamicModule parameterDynamicModule;

	public TextMeshProUGUI addButtonText;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	public List<Type> availableDynamicModules;

	public DynamicModuleList FieldValue
	{
		get
		{
			return (DynamicModuleList)field.GetValue();
		}
		set
		{
			field.SetValue(value);
		}
	}

	public bool allowMultipleModuleInstance => ((MEGUI_DynamicModuleList)field.Attribute).allowMultipleModuleInstances;

	public override void OnEnable()
	{
		base.OnEnable();
		if (field != null && subParameters != null)
		{
			RefreshUI();
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		if (buttonCollapse != null)
		{
			buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
		}
		RefreshUI();
		int i = 0;
		for (int count = FieldValue.activeModules.Count; i < count; i++)
		{
			if (FieldValue.activeModules[i].canBeDisplayedInEditor)
			{
				if (parameterDynamicModule == null)
				{
					parameterDynamicModule = MEGUIParametersController.Instance.GetControl(typeof(MEGUI_DynamicModule)) as MEGUIParameterDynamicModule;
				}
				if (parameterDynamicModule != null)
				{
					subParameters.Add(FieldValue.activeModules[i].id, parameterDynamicModule.Create(FieldValue.activeModules[i], this));
				}
			}
		}
	}

	public void CollapseGroup()
	{
		collapseContainer.gameObject.SetActive(value: false);
		((Image)buttonCollapse.targetGraphic).sprite = spriteCollapseOn;
	}

	public void OnButtonCollapsePressed()
	{
		collapseContainer.gameObject.SetActive(!collapseContainer.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (collapseContainer.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
	}

	public override void RefreshUI()
	{
		dropdownList.onValueChanged.RemoveListener(OnAddValueChange);
		List<string> list = new List<string>();
		availableDynamicModules = new List<Type>();
		List<Type> supportedTypes = FieldValue.GetSupportedTypes();
		int i = 0;
		for (int count = supportedTypes.Count; i < count; i++)
		{
			if (supportedTypes[i].IsSubclassOf(typeof(DynamicModule)) && !availableDynamicModules.Contains(supportedTypes[i]) && !FieldValueContainsModule(supportedTypes[i]))
			{
				availableDynamicModules.Add(supportedTypes[i]);
				DynamicModule dynamicModule = Activator.CreateInstance(supportedTypes[i], new object[1] { FieldValue.node }) as DynamicModule;
				if (dynamicModule.canBeDisplayedInEditor)
				{
					list.Add(dynamicModule.GetDisplayName());
				}
			}
		}
		if (list.Count == 0 && ((MEGUI_DynamicModuleList)field.Attribute).displayMessageWhenEmpty)
		{
			list.Add(((MEGUI_DynamicModuleList)field.Attribute).displayEmptyMessage);
		}
		list.Add("");
		dropdownList.ClearOptions();
		dropdownList.AddOptions(list);
		dropdownList.value = list.Count - 1;
		dropdownList.options.RemoveAt(list.Count - 1);
		if (!MissionEditorHistory.Instance.ActionInProgress)
		{
			int count2 = FieldValue.activeModules.Count;
			while (count2-- > 0)
			{
				if (!supportedTypes.Contains(FieldValue.activeModules[count2].GetType()))
				{
					MissionEditorHistory.PushUndoAction(this, OnHistoryRemoveModule);
					if (subParameters.ContainsKey(FieldValue.activeModules[count2].id))
					{
						subParameters[FieldValue.activeModules[count2].id].gameObject.SetActive(value: false);
					}
					FieldValue.activeModules.RemoveAt(count2);
				}
			}
		}
		UpdateNodeBodyUI();
		dropdownList.onValueChanged.AddListener(OnAddValueChange);
	}

	public void RemoveModule(DynamicModule module)
	{
		if (subParameters.ContainsKey(module.id))
		{
			MissionEditorHistory.PushUndoAction(this, OnHistoryRemoveModule);
			FieldValue.activeModules.Remove(module);
			subParameters[module.id].gameObject.SetActive(value: false);
			if (field.OnValueChange != null)
			{
				field.OnValueChange.Invoke(field.host, new object[1] { FieldValue });
			}
		}
		UpdateNodeBodyUI();
		RefreshUI();
		addButtonText.gameObject.transform.parent.parent.gameObject.SetActive(value: true);
	}

	public void AddModule(DynamicModule dynamicModule)
	{
		if (parameterDynamicModule == null)
		{
			parameterDynamicModule = MEGUIParametersController.Instance.GetControl(typeof(MEGUI_DynamicModule)) as MEGUIParameterDynamicModule;
		}
		if (parameterDynamicModule != null && (!FieldValue.activeModules.Contains(dynamicModule) || allowMultipleModuleInstance))
		{
			MEGUICompoundParameter mEGUICompoundParameter = parameterDynamicModule.Create(dynamicModule, this);
			MissionEditorHistory.PushUndoAction(this, OnHistoryAddModule, true, mEGUICompoundParameter);
			FieldValue.activeModules.Add(dynamicModule);
			subParameters.Add(dynamicModule.id, mEGUICompoundParameter);
			if (field.OnValueChange != null)
			{
				field.OnValueChange.Invoke(field.host, new object[1] { FieldValue });
			}
		}
	}

	public bool FieldValueContainsModule(Type module)
	{
		if (!allowMultipleModuleInstance)
		{
			int count = FieldValue.activeModules.Count;
			while (count-- > 0)
			{
				if (FieldValue.activeModules[count].GetType() == module)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void OnAddValueChange(int value)
	{
		if (value > -1 && value < availableDynamicModules.Count)
		{
			DynamicModule dynamicModule = Activator.CreateInstance(availableDynamicModules[value], new object[1] { FieldValue.node }) as DynamicModule;
			AddModule(dynamicModule);
			UpdateNodeBodyUI();
			RefreshUI();
		}
		if (availableDynamicModules.Count <= 0)
		{
			StartCoroutine(WaitForDropdownList());
		}
	}

	public IEnumerator WaitForDropdownList()
	{
		yield return new WaitForSeconds(0.18f);
		addButtonText.gameObject.transform.parent.parent.gameObject.SetActive(value: false);
	}

	public void AddModules(ConfigNode data)
	{
		if (!data.HasNode("MODULES"))
		{
			return;
		}
		ConfigNode[] nodes = data.GetNode("MODULES").GetNodes("MODULE");
		int num = 0;
		string value;
		while (true)
		{
			if (num >= nodes.Length)
			{
				return;
			}
			ConfigNode obj = nodes[num];
			bool flag = false;
			value = obj.GetValue("id");
			int count = FieldValue.activeModules.Count;
			while (count-- > 0)
			{
				if (FieldValue.activeModules[count].id == value)
				{
					flag = true;
					break;
				}
			}
			if (!flag && subParameters.ContainsKey(value))
			{
				break;
			}
			num++;
		}
		MEGUIParameterDynamicModule mEGUIParameterDynamicModule = subParameters[value] as MEGUIParameterDynamicModule;
		mEGUIParameterDynamicModule.gameObject.SetActive(value: true);
		FieldValue.activeModules.Add(mEGUIParameterDynamicModule.dynamicModule);
		RefreshUI();
	}

	public void RemoveModules(ConfigNode data)
	{
		if (!data.HasNode("MODULES"))
		{
			return;
		}
		ConfigNode[] nodes = data.GetNode("MODULES").GetNodes("MODULE");
		int count = FieldValue.activeModules.Count;
		while (count-- > 0)
		{
			bool flag = false;
			ConfigNode[] array = nodes;
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i].GetValue("id");
				if (FieldValue.activeModules[count].id == value)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				RemoveModule(FieldValue.activeModules[count]);
			}
		}
	}

	public void OnHistoryRemoveModule(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Redo)
		{
			RemoveModules(data);
		}
		else
		{
			AddModules(data);
		}
	}

	public void OnHistoryAddModule(ConfigNode data, HistoryType type)
	{
		if (type == HistoryType.Undo)
		{
			RemoveModules(data);
		}
		else
		{
			AddModules(data);
		}
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("pinned", isPinned);
		FieldValue.Save(configNode);
		return configNode;
	}
}
