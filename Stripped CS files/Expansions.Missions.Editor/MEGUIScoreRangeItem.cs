using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIScoreRangeItem : MonoBehaviour
{
	public TMP_InputField minRangeInput;

	public TMP_InputField maxRangeInput;

	public TMP_InputField scoreInput;

	public Button removeButton;

	public ScoreRange scoreRange;

	public bool isDirty;

	public MEGUI_ScoreRangeList attrib;

	public Action<MEGUIScoreRangeItem, ScoreRange> OnDeleteItem;

	public Action onUpdateValue;

	public MEGUIScoreRangeItem Create(ScoreRange scoreRange, MEGUI_ScoreRangeList attrib, Transform parent, Action<MEGUIScoreRangeItem, ScoreRange> onDeleteItem, Action onUpdateValue)
	{
		MEGUIScoreRangeItem mEGUIScoreRangeItem = UnityEngine.Object.Instantiate(this, parent);
		mEGUIScoreRangeItem.transform.localPosition = Vector3.zero;
		mEGUIScoreRangeItem.attrib = attrib;
		mEGUIScoreRangeItem.OnDeleteItem = onDeleteItem;
		mEGUIScoreRangeItem.onUpdateValue = onUpdateValue;
		mEGUIScoreRangeItem.Setup(scoreRange);
		return mEGUIScoreRangeItem;
	}

	public virtual void Setup(ScoreRange scoreRange)
	{
		this.scoreRange = scoreRange;
		minRangeInput.text = scoreRange.minRange.ToString();
		maxRangeInput.text = scoreRange.maxRange.ToString();
		scoreInput.text = scoreRange.score.ToString();
		if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.DecimalNumber)
		{
			minRangeInput.contentType = TMP_InputField.ContentType.DecimalNumber;
			maxRangeInput.contentType = TMP_InputField.ContentType.DecimalNumber;
		}
		else if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.IntegerNumber)
		{
			minRangeInput.contentType = TMP_InputField.ContentType.IntegerNumber;
			maxRangeInput.contentType = TMP_InputField.ContentType.IntegerNumber;
		}
		else if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.Percentage)
		{
			minRangeInput.contentType = TMP_InputField.ContentType.Standard;
			maxRangeInput.contentType = TMP_InputField.ContentType.Standard;
			TMP_InputField tMP_InputField = minRangeInput;
			tMP_InputField.onValidateInput = (TMP_InputField.OnValidateInput)Delegate.Combine(tMP_InputField.onValidateInput, new TMP_InputField.OnValidateInput(OnValidatePercentageInput));
			TMP_InputField tMP_InputField2 = maxRangeInput;
			tMP_InputField2.onValidateInput = (TMP_InputField.OnValidateInput)Delegate.Combine(tMP_InputField2.onValidateInput, new TMP_InputField.OnValidateInput(OnValidatePercentageInput));
			minRangeInput.onSelect.AddListener(OnSelectMinRangeInput);
			maxRangeInput.onSelect.AddListener(OnSelectMaxRangeInput);
			maxRangeInput.text += "%";
			minRangeInput.text += "%";
		}
		minRangeInput.onValueChanged.AddListener(OnInputValueChange);
		maxRangeInput.onValueChanged.AddListener(OnInputValueChange);
		scoreInput.onValueChanged.AddListener(OnInputValueChange);
		minRangeInput.onEndEdit.AddListener(OnMinRangeInputEndEdit);
		maxRangeInput.onEndEdit.AddListener(OnMaxRangeInputEndEdit);
		scoreInput.onEndEdit.AddListener(OnScoreInputEndEdit);
		removeButton.onClick.AddListener(OnRemoveButton);
	}

	public void Terminate()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnInputValueChange(string value)
	{
		isDirty = true;
	}

	public void OnScoreInputEndEdit(string value)
	{
		if (isDirty)
		{
			scoreRange.score = float.Parse(value);
		}
		onUpdateValue();
		isDirty = false;
	}

	public void OnMaxRangeInputEndEdit(string value)
	{
		if (isDirty)
		{
			scoreRange.maxRange = float.Parse(value);
			if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.Percentage)
			{
				maxRangeInput.text += "%";
			}
			onUpdateValue();
		}
		isDirty = false;
	}

	public void OnMinRangeInputEndEdit(string value)
	{
		if (isDirty)
		{
			scoreRange.minRange = float.Parse(value);
			if (attrib.ContentType == MEGUI_ScoreRangeList.RangeContentType.Percentage)
			{
				minRangeInput.text += "%";
			}
			onUpdateValue();
		}
		isDirty = false;
	}

	public char OnValidatePercentageInput(string text, int charIndex, char addedChar)
	{
		if (!char.IsNumber(addedChar) && (addedChar != '.' || text.IndexOf('.') != -1))
		{
			return '\0';
		}
		return addedChar;
	}

	public void OnSelectMinRangeInput(string value)
	{
		minRangeInput.text = minRangeInput.text.Replace("%", "");
	}

	public void OnSelectMaxRangeInput(string value)
	{
		maxRangeInput.text = maxRangeInput.text.Replace("%", "");
	}

	public void OnRemoveButton()
	{
		OnDeleteItem(this, scoreRange);
	}
}
