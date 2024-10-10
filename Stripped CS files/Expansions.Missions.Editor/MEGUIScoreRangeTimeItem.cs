namespace Expansions.Missions.Editor;

public class MEGUIScoreRangeTimeItem : MEGUIScoreRangeItem
{
	public MEGUITimeControl minRangeTimeControl;

	public MEGUITimeControl maxRangeTimeControl;

	public override void Setup(ScoreRange scoreRange)
	{
		base.scoreRange = scoreRange;
		minRangeTimeControl.time = scoreRange.minRange;
		maxRangeTimeControl.time = scoreRange.maxRange;
		scoreInput.text = scoreRange.score.ToString();
		scoreInput.onValueChanged.AddListener(base.OnInputValueChange);
		scoreInput.onEndEdit.AddListener(base.OnScoreInputEndEdit);
		minRangeTimeControl.onValueChange.AddListener(OnMinRangeTimeValueChange);
		maxRangeTimeControl.onValueChange.AddListener(OnMaxRangeTimeValueChange);
		removeButton.onClick.AddListener(base.OnRemoveButton);
	}

	public void OnMinRangeTimeValueChange(double value)
	{
		scoreRange.minRange = value;
		onUpdateValue();
	}

	public void OnMaxRangeTimeValueChange(double value)
	{
		scoreRange.maxRange = value;
		onUpdateValue();
	}
}
