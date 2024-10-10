using System;
using System.Collections.Generic;
using CommNet;
using ns12;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns37;

public class ExperimentsResultDialog : MonoBehaviour
{
	public static ExperimentsResultDialog Instance;

	[SerializeField]
	public TextMeshProUGUI textHeaderTitle;

	[SerializeField]
	public TextMeshProUGUI textResults;

	[SerializeField]
	public TextMeshProUGUI textDataField;

	[SerializeField]
	public TextMeshProUGUI textRecoveryField;

	[SerializeField]
	public TextMeshProUGUI textTransmitField;

	[SerializeField]
	public TextMeshProUGUI textTransmitBtnPercent;

	[SerializeField]
	public TextMeshProUGUI textTransmitBtnTotalPercent;

	[SerializeField]
	public TextMeshProUGUI textTransmitBtnSignalStrength;

	[SerializeField]
	public TooltipController_Text tooltipTransmitBtn;

	[SerializeField]
	public TooltipController_Text tooltipTransmitCommsBtn;

	[SerializeField]
	public TextMeshProUGUI textLabBtnPercent;

	[SerializeField]
	public TooltipController_Text tooltipLabBtn;

	[SerializeField]
	public TextMeshProUGUI textInopWarning;

	[SerializeField]
	public Slider sldDataPrimary;

	[SerializeField]
	public Slider sldDataSecondary;

	[SerializeField]
	public Slider sldXmitPrimary;

	[SerializeField]
	public Slider sldXmitSecondary;

	[SerializeField]
	public Button btnKeep;

	[SerializeField]
	public Button btnDiscard;

	[SerializeField]
	public Button btnReset;

	[SerializeField]
	public Button btnXmit;

	[SerializeField]
	public Button btnXmitComms;

	[SerializeField]
	public Button btnLab;

	[SerializeField]
	public Button btnPagePrev;

	[SerializeField]
	public Button btnPageNext;

	public List<ExperimentResultDialogPage> pages;

	public ExperimentResultDialogPage currentPage;

	public PopupDialog warningDialog;

	public static ExperimentsResultDialog DisplayResult(ExperimentResultDialogPage resultPage)
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("ScienceResultsDialog")).GetComponent<ExperimentsResultDialog>();
			Instance.name = "Experiments Result Dialog Handler";
			RectTransform obj = Instance.transform as RectTransform;
			obj.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
			obj.transform.localPosition = Vector3.zero;
		}
		int num = -1;
		int count = Instance.pages.Count;
		while (count-- > 0)
		{
			if (Instance.pages[count].pageData == resultPage.pageData)
			{
				num = count;
				break;
			}
		}
		if (num == -1)
		{
			Instance.addPage(resultPage);
		}
		else
		{
			Instance.currentPage = Instance.pages[num];
		}
		return Instance;
	}

	public void Awake()
	{
		pages = new List<ExperimentResultDialogPage>();
		btnDiscard.onClick.AddListener(onBtnDiscard);
		btnReset.onClick.AddListener(onBtnReset);
		btnKeep.onClick.AddListener(onBtnKeep);
		btnXmitComms.gameObject.SetActive(CommNetScenario.CommNetEnabled);
		btnXmit.gameObject.SetActive(!CommNetScenario.CommNetEnabled);
		btnXmitComms.onClick.AddListener(onBtnTransmit);
		btnXmit.onClick.AddListener(onBtnTransmit);
		btnLab.onClick.AddListener(onBtnLab);
		btnPageNext.onClick.AddListener(onBtnPageNext);
		btnPagePrev.onClick.AddListener(onBtnPagePrev);
		GameEvents.onVesselChange.Add(onVesselFocusChange);
		GameEvents.onPartDie.Add(onPartDie);
		GameEvents.onVesselWasModified.Add(onVesselModified);
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneLoadRequested);
		GameEvents.onGamePause.Add(OnPause);
		GameEvents.onGameUnpause.Add(OnUnpause);
	}

	public void Update()
	{
		if (!Input.GetKeyUp(KeyCode.Return) && !Input.GetKeyUp(KeyCode.KeypadEnter))
		{
			if (Input.GetKeyUp(KeyCode.Tab))
			{
				NextPage();
			}
		}
		else
		{
			Dismiss();
		}
	}

	public void OnDestroy()
	{
		while (pages.Count > 0)
		{
			ExperimentResultDialogPage experimentResultDialogPage = pages[0];
			experimentResultDialogPage.OnDiscardData = (Callback<ScienceData>)Delegate.Remove(experimentResultDialogPage.OnDiscardData, new Callback<ScienceData>(dismissCurrentPage));
			ExperimentResultDialogPage experimentResultDialogPage2 = pages[0];
			experimentResultDialogPage2.OnKeepData = (Callback<ScienceData>)Delegate.Remove(experimentResultDialogPage2.OnKeepData, new Callback<ScienceData>(dismissCurrentPage));
			ExperimentResultDialogPage experimentResultDialogPage3 = pages[0];
			experimentResultDialogPage3.OnTransmitData = (Callback<ScienceData>)Delegate.Remove(experimentResultDialogPage3.OnTransmitData, new Callback<ScienceData>(dismissCurrentPage));
			pages[0].OnKeepData(pages[0].pageData);
			pages.RemoveAt(0);
		}
		GameEvents.onVesselChange.Remove(onVesselFocusChange);
		GameEvents.onPartDie.Remove(onPartDie);
		GameEvents.onVesselWasModified.Remove(onVesselModified);
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneLoadRequested);
		GameEvents.onGamePause.Remove(OnPause);
		GameEvents.onGameUnpause.Remove(OnUnpause);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnGameSceneLoadRequested(GameScenes scene)
	{
		Dismiss();
	}

	public void OnPause()
	{
		if (warningDialog != null)
		{
			warningDialog.gameObject.SetActive(value: false);
		}
	}

	public void OnUnpause()
	{
		if (warningDialog != null)
		{
			warningDialog.gameObject.SetActive(value: true);
		}
	}

	public void Dismiss()
	{
		Mouse.Left.ClearMouseState();
		UnityEngine.Object.Destroy(base.gameObject);
		if (warningDialog != null)
		{
			warningDialog.Dismiss();
		}
	}

	public void onPartDie(Part p)
	{
		if (p == currentPage.host)
		{
			Dismiss();
		}
	}

	public void onVesselFocusChange(Vessel v)
	{
		Dismiss();
	}

	public void onVesselModified(Vessel v)
	{
		if (v == FlightGlobals.ActiveVessel)
		{
			Dismiss();
		}
	}

	public void onBtnDiscard()
	{
		currentPage.OnDiscardData(currentPage.pageData);
	}

	public void onBtnReset()
	{
		GameEvents.OnROCExperimentReset.Fire(currentPage.pageData);
		currentPage.OnDiscardData(currentPage.pageData);
	}

	public void onBtnKeep()
	{
		GameEvents.OnExperimentStored.Fire(currentPage.pageData);
		GameEvents.OnROCExperimentStored.Fire(currentPage.pageData);
		currentPage.OnKeepData(currentPage.pageData);
	}

	public void onBtnTransmit()
	{
		if (currentPage.showTransmitWarning)
		{
			if (warningDialog == null)
			{
				warningDialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("ExperimentTrasmitWarning", Localizer.Format("#autoLOC_6001489", currentPage.transmitWarningMessage), Localizer.Format("#autoLOC_6001490", currentPage.host.partInfo.title), HighLogic.UISkin, new DialogGUIButton<ScienceData>("#autoLOC_6001430", currentPage.OnTransmitData, currentPage.pageData), new DialogGUIButton("#autoLOC_190768", delegate
				{
				})), persistAcrossScenes: false, UISkinManager.GetSkin("MainMenuSkin"));
			}
		}
		else
		{
			currentPage.OnTransmitData(currentPage.pageData);
		}
	}

	public void onBtnLab()
	{
		currentPage.OnSendToLab(currentPage.pageData);
	}

	public void onBtnPageNext()
	{
		NextPage();
	}

	public void onBtnPagePrev()
	{
		PrevPage();
	}

	public void OnPagesModified()
	{
		btnPagePrev.gameObject.SetActive(pages.Count > 1);
		btnPageNext.gameObject.SetActive(pages.Count > 1);
	}

	public void SetPage(ExperimentResultDialogPage page)
	{
		textHeaderTitle.text = "[" + (pages.IndexOf(page) + 1) + "/" + pages.Count + "] " + page.title;
		textResults.text = page.resultText;
		textDataField.text = Localizer.Format("#autoLOC_419739", page.dataSize.ToString("0"));
		textInopWarning.gameObject.SetActive(page.showTransmitWarning);
		if (!string.IsNullOrEmpty(page.transmitWarningMessage))
		{
			textInopWarning.text = Localizer.Format("#autoLOC_419744");
		}
		else
		{
			textInopWarning.text = Localizer.Format("#autoLOC_6001491");
		}
		textRecoveryField.text = Localizer.Format("#autoLOC_7003000", page.scienceValue.ToString("0.0"));
		if (page.refValue == 0f)
		{
			sldDataSecondary.normalizedValue = 0f;
			sldDataPrimary.normalizedValue = 0f;
		}
		else if (page.scienceValueRatio < 1f)
		{
			sldDataPrimary.normalizedValue = page.scienceValue / page.refValue;
			sldDataSecondary.normalizedValue = page.remainingScience / page.refValue;
		}
		else
		{
			sldDataSecondary.normalizedValue = page.scienceValue / page.refValue;
			sldDataPrimary.normalizedValue = page.scienceValue / page.refValue - page.valueAfterRecovery / page.refValue;
		}
		textTransmitField.text = Localizer.Format("#autoLOC_7003001", (page.baseTransmitValue * page.TransmitBonus).ToString("0.0"));
		if (page.refValue == 0f)
		{
			sldXmitSecondary.normalizedValue = 0f;
			sldXmitPrimary.normalizedValue = 0f;
		}
		else if (page.scienceValueRatio < 1f)
		{
			sldXmitPrimary.normalizedValue = page.baseTransmitValue * page.TransmitBonus / page.refValue;
			sldXmitSecondary.normalizedValue = page.remainingScience * page.TransmitBonus / page.refValue;
		}
		else
		{
			sldXmitSecondary.normalizedValue = page.baseTransmitValue * page.TransmitBonus / page.refValue;
			sldXmitPrimary.normalizedValue = page.baseTransmitValue * page.TransmitBonus / page.refValue - page.valueAfterTransmit / page.refValue;
		}
		btnReset.gameObject.SetActive(page.showReset);
		btnDiscard.gameObject.SetActive(!page.showReset);
		if (page.refValue == 0f)
		{
			textTransmitBtnPercent.text = "0%";
		}
		else
		{
			textTransmitBtnTotalPercent.text = (page.baseTransmitValue * page.TransmitBonus / page.refValue * 100f).ToString("0") + "%";
		}
		textTransmitBtnSignalStrength.text = "+" + (page.CommBonus * 100f).ToString("0") + "%";
		tooltipTransmitCommsBtn.SetText(Localizer.Format("#autoLOC_419765", textTransmitBtnTotalPercent.text));
		if (page.refValue == 0f)
		{
			textTransmitBtnPercent.text = "0%";
		}
		else
		{
			textTransmitBtnPercent.text = (page.baseTransmitValue * page.TransmitBonus / page.refValue * 100f).ToString("0") + "%";
		}
		tooltipTransmitBtn.SetText(Localizer.Format("#autoLOC_419765", textTransmitBtnPercent.text));
		if (page.labSearch.HasAnyLabs)
		{
			page.UpdatePageLabValue();
		}
		btnLab.gameObject.SetActive(page.labSearch.HasAnyLabs);
		if (page.labSearch.HasAnyLabs)
		{
			btnLab.interactable = page.labSearch.NextLabForDataFound;
			if (page.labSearch.NextLabForDataFound)
			{
				textLabBtnPercent.text = "+" + page.labSearch.ScienceExpectation;
				tooltipLabBtn.SetText(Localizer.Format("#autoLOC_6001492", Convert.ToInt32(page.labSearch.HasMultipleLabs), page.labSearch.DataExpectationSummary));
			}
			else
			{
				textLabBtnPercent.text = "---";
				tooltipLabBtn.SetText(page.labSearch.ErrorString);
			}
		}
		currentPage = page;
	}

	public void addPage(ExperimentResultDialogPage page)
	{
		page.OnDiscardData = (Callback<ScienceData>)Delegate.Combine(page.OnDiscardData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnKeepData = (Callback<ScienceData>)Delegate.Combine(page.OnKeepData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnTransmitData = (Callback<ScienceData>)Delegate.Combine(page.OnTransmitData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnSendToLab = (Callback<ScienceData>)Delegate.Combine(page.OnSendToLab, new Callback<ScienceData>(dismissCurrentPage));
		pages.Add(page);
		SetPage(page);
		OnPagesModified();
	}

	public void dismissCurrentPage(ScienceData pageData)
	{
		removePage(currentPage);
	}

	public void removePage(ExperimentResultDialogPage page)
	{
		page.OnDiscardData = (Callback<ScienceData>)Delegate.Remove(page.OnDiscardData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnKeepData = (Callback<ScienceData>)Delegate.Remove(page.OnKeepData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnTransmitData = (Callback<ScienceData>)Delegate.Remove(page.OnTransmitData, new Callback<ScienceData>(dismissCurrentPage));
		page.OnSendToLab = (Callback<ScienceData>)Delegate.Remove(page.OnSendToLab, new Callback<ScienceData>(dismissCurrentPage));
		pages.Remove(page);
		if (pages.Count > 0)
		{
			NextPage();
			OnPagesModified();
		}
		else
		{
			Dismiss();
		}
	}

	public void NextPage()
	{
		SetPage(pages[UtilMath.WrapAround(pages.IndexOf(currentPage) + 1, 0, pages.Count)]);
	}

	public void PrevPage()
	{
		SetPage(pages[UtilMath.WrapAround(pages.IndexOf(currentPage) - 1, 0, pages.Count)]);
	}
}
