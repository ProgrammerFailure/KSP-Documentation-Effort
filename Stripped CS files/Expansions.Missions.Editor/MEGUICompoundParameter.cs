using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUICompoundParameter : MEGUIParameter
{
	public bool subParametersSelectable;

	public bool subParametersPinneable = true;

	public Dictionary<string, MEGUIParameter> subParameters;

	public List<MEGUIParameterGroup> supParametersGroups;

	public List<MEGUIParameter> parametersToSort;

	public bool isSubParameterMouseOver
	{
		get
		{
			Dictionary<string, MEGUIParameter>.KeyCollection.Enumerator enumerator = subParameters.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MEGUIParameter mEGUIParameter = subParameters[enumerator.Current];
					if (mEGUIParameter != null && mEGUIParameter.isMouseOver)
					{
						return true;
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
			return false;
		}
	}

	public override void Update()
	{
		if (isSelectable && base.isMouseOver && Input.GetMouseButtonDown(0) && (!subParametersSelectable || (subParametersSelectable && !isSubParameterMouseOver)))
		{
			MissionEditorLogic.Instance.actionPane.OnParameterClick(this);
		}
	}

	public override void Setup(string name)
	{
		if (field != null)
		{
			Setup(name, field.GetValue());
		}
	}

	public virtual void Setup(string name, object value)
	{
		Setup(name, value, base.transform);
	}

	public virtual void Setup(string name, object value, Transform transform)
	{
		subParameters = new Dictionary<string, MEGUIParameter>();
		parametersToSort = new List<MEGUIParameter>();
		supParametersGroups = new List<MEGUIParameterGroup>();
		BaseAPFieldList baseAPFieldList = new BaseAPFieldList(value);
		Dictionary<string, BaseAPFieldList> dictionary = new Dictionary<string, BaseAPFieldList>();
		for (int i = 0; i < baseAPFieldList.Count; i++)
		{
			if (baseAPFieldList[i].HideWhenSiblingsExist && baseAPFieldList.Count > 1)
			{
				continue;
			}
			if (baseAPFieldList[i].Group == string.Empty)
			{
				MEGUIParameter mEGUIParameter = AddSubParameter(baseAPFieldList[i], transform);
				mEGUIParameter.parentGroup = parentGroup;
				if (mEGUIParameter.Order != -1)
				{
					parametersToSort.Add(mEGUIParameter);
				}
			}
			else if (dictionary.ContainsKey(baseAPFieldList[i].Group))
			{
				dictionary[baseAPFieldList[i].Group].Add(baseAPFieldList[i]);
			}
			else
			{
				BaseAPFieldList baseAPFieldList2 = new BaseAPFieldList();
				baseAPFieldList2.Add(baseAPFieldList[i]);
				dictionary.Add(baseAPFieldList[i].Group, baseAPFieldList2);
			}
		}
		foreach (KeyValuePair<string, BaseAPFieldList> item in dictionary)
		{
			string displayName = item.Key;
			for (int j = 0; j < item.Value.Count; j++)
			{
				if (!string.IsNullOrEmpty(item.Value[j].GroupDisplayName))
				{
					displayName = item.Value[j].GroupDisplayName;
					break;
				}
			}
			MEGUIParameterGroup mEGUIParameterGroup = MEActionPane.fetch.DisplayModuleHeader(item.Key, displayName, item.Value, transform);
			for (int k = 0; k < mEGUIParameterGroup.GroupFields.Count; k++)
			{
				AddSubParameter(mEGUIParameterGroup.GroupFields[k], mEGUIParameterGroup.containerChilden).parentGroup = mEGUIParameterGroup;
			}
			supParametersGroups.Add(mEGUIParameterGroup);
			parametersToSort.Add(mEGUIParameterGroup);
			mEGUIParameterGroup.Display();
			mEGUIParameterGroup.Depth++;
		}
		SortParameters();
	}

	public MEGUIParameter AddSubParameter(BaseAPField field, Transform parent)
	{
		MEGUIParameter control = MEGUIParametersController.Instance.GetControl(field.Attribute.GetType());
		MEGUIParameter mEGUIParameter = null;
		if (control != null)
		{
			mEGUIParameter = control.Create(field, parent);
			if (!subParametersPinneable && mEGUIParameter.pinToggle != null)
			{
				mEGUIParameter.pinToggle.gameObject.SetActive(value: false);
			}
			mEGUIParameter.isSelectable = mEGUIParameter.isSelectable && subParametersSelectable;
			if (!subParametersSelectable && mEGUIParameter.selectedIndicator != null)
			{
				mEGUIParameter.selectedIndicator.gameObject.SetActive(value: false);
			}
			subParameters.Add(field.name, mEGUIParameter);
		}
		return mEGUIParameter;
	}

	public override void Display()
	{
		base.Display();
		if (supParametersGroups != null)
		{
			for (int i = 0; i < supParametersGroups.Count; i++)
			{
				supParametersGroups[i].transform.SetParent(base.transform);
				bool flag = false;
				for (int j = 0; j < supParametersGroups[i].containerChilden.childCount; j++)
				{
					MEGUIParameter component = supParametersGroups[i].containerChilden.GetChild(j).GetComponent<MEGUIParameter>();
					flag = flag || component.gameObject.activeSelf;
				}
				supParametersGroups[i].gameObject.SetActive(flag);
			}
		}
		SortParameters();
	}

	public void SortParameters()
	{
		if (parametersToSort != null)
		{
			parametersToSort.Sort(SortParametersByOrder);
			int num = base.transform.childCount - parametersToSort.Count;
			for (int i = 0; i < parametersToSort.Count; i++)
			{
				parametersToSort[i].transform.SetSiblingIndex(i + num);
			}
		}
	}

	public MEGUIParameter GetSubParameter(string fieldId)
	{
		if (subParameters.ContainsKey(fieldId))
		{
			return subParameters[fieldId];
		}
		return null;
	}

	public static int SortParametersByOrder(MEGUIParameter p1, MEGUIParameter p2)
	{
		return p1.Order.CompareTo(p2.Order);
	}

	public void OnDestroy()
	{
		if (subParameters == null)
		{
			return;
		}
		Dictionary<string, MEGUIParameter>.KeyCollection.Enumerator enumerator = subParameters.Keys.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				MEGUIParameter mEGUIParameter = subParameters[enumerator.Current];
				if (mEGUIParameter != null && mEGUIParameter.gameObject != null)
				{
					Object.Destroy(mEGUIParameter.gameObject);
				}
			}
		}
		finally
		{
			enumerator.Dispose();
		}
	}
}
