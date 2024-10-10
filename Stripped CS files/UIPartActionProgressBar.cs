using TMPro;
using UnityEngine.UI;

[UI_ProgressBar]
public class UIPartActionProgressBar : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldAmount;

	public Slider progBar;

	public float fieldValue;

	public UI_ProgressBar progBarControl => (UI_ProgressBar)control;

	public override void UpdateItem()
	{
		if (isModule)
		{
			fieldValue = field.GetValue<float>(partModule);
		}
		else
		{
			fieldValue = field.GetValue<float>(part);
		}
		fieldName.text = field.guiName;
		fieldAmount.text = KSPUtil.LocalizeNumber(fieldValue, field.guiFormat);
		progBar.value = (fieldValue - progBarControl.minValue) / (progBarControl.maxValue - progBarControl.minValue);
	}
}
