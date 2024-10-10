using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageGroupInfoItem : MonoBehaviour
{
	public HorizontalLayoutGroup hLayout;

	public TextMeshProUGUI infoName;

	public TextMeshProUGUI infoValue;

	public LayoutElement infoNameLayout;

	public LayoutElement infoValueLayout;

	public bool updateFunctionIsNull;

	public Func<DeltaVStageInfo, DeltaVSituationOptions, string> updateFunction;

	public string valueSuffix = "";

	public bool OnUpdateInEditor;

	public Func<DeltaVStageInfo, DeltaVSituationOptions, string> UpdateFunction
	{
		get
		{
			return updateFunction;
		}
		set
		{
			updateFunction = value;
			updateFunctionIsNull = value == null;
		}
	}

	public void Awake()
	{
	}

	public void Setup(DeltaVAppValues.InfoLine info, float panelWidth)
	{
		infoName.text = info.displayStageName;
		UpdateFunction = info.infoValue;
		valueSuffix = info.valueSuffix;
		infoValueLayout = infoValue.GetComponent<LayoutElement>();
		infoNameLayout = infoName.GetComponent<LayoutElement>();
		if (hLayout != null && infoNameLayout != null && infoValueLayout != null)
		{
			float num = panelWidth - (float)hLayout.padding.left - (float)hLayout.padding.right;
			infoNameLayout.preferredWidth = num * GameSettings.STAGE_GROUP_INFO_NAME_PERCENTAGE;
			infoValueLayout.preferredWidth = num * (1f - GameSettings.STAGE_GROUP_INFO_NAME_PERCENTAGE);
		}
	}

	public virtual void UpdateValue(DeltaVStageInfo stage, DeltaVSituationOptions situation)
	{
		if (!updateFunctionIsNull)
		{
			infoValue.text = updateFunction(stage, situation) + valueSuffix;
		}
	}

	public virtual void OnUpdate(DeltaVStageInfo stage, DeltaVSituationOptions situation)
	{
		if (OnUpdateInEditor || !HighLogic.LoadedSceneIsEditor)
		{
			UpdateValue(stage, situation);
		}
	}
}
