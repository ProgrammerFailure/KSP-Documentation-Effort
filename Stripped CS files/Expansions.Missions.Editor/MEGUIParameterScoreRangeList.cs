using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ScoreRangeList]
public class MEGUIParameterScoreRangeList : MEGUIParameter
{
	public MEGUIScoreRangeItem scoreRangeItemPrefab;

	public MEGUIScoreRangeTimeItem scoreRangeTimeItemPrefab;

	public Button addButton;

	public TextMeshProUGUI minTitleLabel;

	public TextMeshProUGUI maxTitleLabel;

	public TextMeshProUGUI valueTitleLabel;

	public List<MEGUIScoreRangeItem> scoreRangeList;

	public MEGUI_ScoreRangeList attrib;

	public List<ScoreRange> FieldValue
	{
		get
		{
			return field.GetValue() as List<ScoreRange>;
		}
		set
		{
			field.SetValue(value);
		}
	}

	public override void Setup(string name)
	{
		base.Setup(name);
		attrib = field.Attribute as MEGUI_ScoreRangeList;
		minTitleLabel.text = attrib.minLabel;
		maxTitleLabel.text = attrib.maxLabel;
		valueTitleLabel.text = attrib.valueLabel;
		scoreRangeList = new List<MEGUIScoreRangeItem>();
		int i = 0;
		for (int count = FieldValue.Count; i < count; i++)
		{
			if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.Time)
			{
				scoreRangeList.Add(scoreRangeTimeItemPrefab.Create(FieldValue[i], attrib, base.transform, OnRemoveScoreRange, base.UpdateNodeBodyUI));
			}
			else
			{
				scoreRangeList.Add(scoreRangeItemPrefab.Create(FieldValue[i], attrib, base.transform, OnRemoveScoreRange, base.UpdateNodeBodyUI));
			}
		}
		addButton.onClick.AddListener(OnAddButton);
	}

	public void AddScoreRange()
	{
		ScoreRange scoreRange = new ScoreRange();
		FieldValue.Add(scoreRange);
		if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.Time)
		{
			scoreRangeList.Add(scoreRangeTimeItemPrefab.Create(scoreRange, attrib, base.transform, OnRemoveScoreRange, base.UpdateNodeBodyUI));
		}
		else
		{
			scoreRangeList.Add(scoreRangeItemPrefab.Create(scoreRange, attrib, base.transform, OnRemoveScoreRange, base.UpdateNodeBodyUI));
		}
		UpdateNodeBodyUI();
	}

	public void OnAddButton()
	{
		AddScoreRange();
	}

	public void OnRemoveScoreRange(MEGUIScoreRangeItem scoreRangeItem, ScoreRange scoreRange)
	{
		FieldValue.Remove(scoreRange);
		scoreRangeList.Remove(scoreRangeItem);
		scoreRangeItem.Terminate();
		UpdateNodeBodyUI();
	}
}
