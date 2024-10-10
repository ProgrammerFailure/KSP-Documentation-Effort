using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_DynamicModule]
public class MEGUIParameterDynamicModule : MEGUICompoundParameter
{
	public Button buttonExpand;

	public GameObject arrowExpand;

	public Button removeButton;

	public GameObject container;

	public Transform childRoot;

	public MEGUIParameterDynamicModuleList parentParameter;

	public DynamicModule dynamicModule { get; set; }

	public MEGUIParameterDynamicModule Create(DynamicModule dynamicModule, MEGUIParameterDynamicModuleList parent)
	{
		MEGUIParameterDynamicModule mEGUIParameterDynamicModule = Create(null, parent.container, dynamicModule.GetDisplayName()) as MEGUIParameterDynamicModule;
		mEGUIParameterDynamicModule.dynamicModule = dynamicModule;
		mEGUIParameterDynamicModule.parentParameter = parent;
		mEGUIParameterDynamicModule.subParametersPinneable = parent.FieldValue.node != null;
		if (mEGUIParameterDynamicModule.tooltipComponent != null && !string.IsNullOrEmpty(dynamicModule.GetDisplayToolTip()))
		{
			mEGUIParameterDynamicModule.tooltipComponent.enabled = true;
			mEGUIParameterDynamicModule.tooltipComponent.SetText(dynamicModule.GetDisplayToolTip());
		}
		mEGUIParameterDynamicModule.Setup(dynamicModule.GetDisplayName(), dynamicModule, mEGUIParameterDynamicModule.childRoot);
		return mEGUIParameterDynamicModule;
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		title.text = name;
		buttonExpand.onClick.AddListener(ToggleExpand);
		arrowExpand.transform.eulerAngles = new Vector3(0f, 0f, 180f);
		removeButton.onClick.AddListener(OnRemoveButton);
		container.SetActive(value: true);
	}

	public override void Setup(string name, object value, Transform transform)
	{
		List<string> defaultPinnedParameters = (value as DynamicModule).GetDefaultPinnedParameters();
		int i = 0;
		for (int count = defaultPinnedParameters.Count; i < count; i++)
		{
			if (!(value as DynamicModule).HasNodeBodyParameter(defaultPinnedParameters[i]))
			{
				(value as DynamicModule).AddParameterToNodeBodyAndUpdateUI(defaultPinnedParameters[i]);
			}
		}
		base.Setup(name, value, transform);
	}

	public void ToggleExpand()
	{
		bool flag = !container.activeSelf;
		container.SetActive(flag);
		arrowExpand.transform.eulerAngles = (flag ? new Vector3(0f, 0f, 180f) : Vector3.zero);
	}

	public void OnRemoveButton()
	{
		if (parentParameter != null)
		{
			parentParameter.RemoveModule(dynamicModule);
		}
	}
}
