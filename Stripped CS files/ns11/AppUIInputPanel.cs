using System.Collections.Generic;
using System.Reflection;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class AppUIInputPanel : MonoBehaviour
{
	public Transform rowParent;

	public AppUI_Data data;

	public TextMeshProUGUI textTargetForHover;

	public ScrollRect scrollTargetForMemberEvents;

	public bool setupComplete;

	public List<AppUIMember> rows;

	public bool SetupComplete => setupComplete;

	public void Setup(AppUI_Data data, Callback onDataChanged)
	{
		this.data = data;
		data.uiPanel = this;
		DestroyChildren();
		rows = new List<AppUIMember>();
		MemberInfo[] members = this.data.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		foreach (MemberInfo memberInfo in members)
		{
			AppUI_Control customAttribute = memberInfo.GetCustomAttribute<AppUI_Control>();
			if (customAttribute != null && AppUIMaster.TryGetAppUIControlPrefab(customAttribute, out var prefab))
			{
				AppUIMember component = Object.Instantiate(prefab).GetComponent<AppUIMember>();
				component.Setup(data, memberInfo, customAttribute, this);
				component.SetHoverTextTarget(textTargetForHover);
				rows.Add(component);
			}
		}
		rows.Sort(AppUIMember.SortByOrder);
		for (int j = 0; j < rows.Count; j++)
		{
			rows[j].transform.SetParent(rowParent);
			rows[j].transform.localScale = Vector3.one;
			rows[j].transform.localPosition = Vector3.zero;
		}
		data.SetupDataChangeCallback(onDataChanged);
		setupComplete = true;
	}

	public void ReleaseData()
	{
		data = null;
		DestroyChildren();
		setupComplete = false;
	}

	public void SetErrorState(bool state)
	{
		if (rows == null)
		{
			return;
		}
		for (int i = 0; i < rows.Count; i++)
		{
			if (rows[i].hideOnError)
			{
				rows[i].gameObject.SetActive(!state);
			}
		}
	}

	public void DestroyChildren()
	{
		int childCount = rowParent.transform.childCount;
		while (childCount-- > 0)
		{
			Object.Destroy(rowParent.transform.GetChild(childCount).gameObject);
		}
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (data != null)
		{
			data.UIInputPanelUpdate();
		}
	}

	[ContextMenu("Refresh Members")]
	public void RefreshUI()
	{
		for (int i = 0; i < rows.Count; i++)
		{
			rows[i].RefreshUI();
		}
	}

	public AppUIMember GetControl(string fieldName)
	{
		if (rows != null)
		{
			for (int i = 0; i < rows.Count; i++)
			{
				if (rows[i].Name == fieldName)
				{
					return rows[i];
				}
			}
		}
		return null;
	}

	public bool AnyTextFieldHasFocus()
	{
		int num = 0;
		while (true)
		{
			if (num < rows.Count)
			{
				if (rows[num].AnyTextFieldHasFocus())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
