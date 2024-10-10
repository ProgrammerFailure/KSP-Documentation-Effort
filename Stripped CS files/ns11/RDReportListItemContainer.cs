using System;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class RDReportListItemContainer : MonoBehaviour
{
	public UIStateButton button_report;

	public TextMeshProUGUI label_description;

	public TextMeshProUGUI label_science;

	public TextMeshProUGUI label_value;

	public Slider slider_value;

	public TextMeshProUGUI label_data;

	public GUISkin popupSkin;

	[NonSerialized]
	public string id;

	[NonSerialized]
	public string description;

	[NonSerialized]
	public float science;

	[NonSerialized]
	public float data;

	[NonSerialized]
	public float value;

	public PopupDialog scienceReport;

	public void Start()
	{
		button_report.onClick.AddListener(OnButtonInput);
	}

	public void OnDestroy()
	{
		if (scienceReport != null)
		{
			scienceReport.Dismiss();
		}
		button_report.onClick.RemoveListener(OnButtonInput);
	}

	public void OnButtonInput()
	{
		string text = "";
		text = RDEnvironmentAdapter.GetResults(id);
		text = label_description.text + "\n\n" + text;
		scienceReport = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ScienceReport", text, Localizer.Format("#autoLOC_471396"), UISkinManager.GetSkin("MainMenuSkin"), new DialogGUIButton(Localizer.Format("#autoLOC_471397"), ClosePopup)), persistAcrossScenes: false, UISkinManager.GetSkin("MainMenuSkin"));
		scienceReport.OnDismiss = ClosePopup;
	}

	public void ClosePopup()
	{
	}

	public void SetDescriptionLabel(string text)
	{
		label_description.text = text;
	}

	public void SetScienceLabel(float value)
	{
		label_science.text = Localizer.Format("#autoLOC_471414", value.ToString("0.00"));
	}

	public void SetDataLabel(float value)
	{
		label_data.text = Localizer.Format("#autoLOC_471419", value.ToString("0.00"));
	}

	public void SetValueSlider(float value)
	{
		slider_value.value = value;
	}

	public void PushDataToGUI()
	{
		SetDescriptionLabel(description);
		SetScienceLabel(science);
		SetDataLabel(data);
		SetValueSlider(value);
	}
}
