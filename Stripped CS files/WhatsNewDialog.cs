using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Expansions;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhatsNewDialog : MonoBehaviour
{
	[SerializeField]
	public RawImage bannerIcon;

	[SerializeField]
	public Button moreInfoButton;

	[SerializeField]
	public Button closeButton;

	[SerializeField]
	public Toggle dontShowAgainToggle;

	[SerializeField]
	public TMP_Text textBody;

	[SerializeField]
	public ScrollRect bodyScroll;

	[SerializeField]
	public float scrollStep;

	[SerializeField]
	public Button merchButton;

	public WhatsNewModes currentMode;

	public string whatsNewText = "";

	public bool dontShowAgain = true;

	public TMP_Text moreInfoText;

	[SerializeField]
	public string merchandiseURL;

	public void Start()
	{
		moreInfoText = moreInfoButton.GetComponentInChildren<TMP_Text>();
		moreInfoButton.onClick.AddListener(OnMoreInfoButton);
		closeButton.onClick.AddListener(OnCloseButton);
		dontShowAgainToggle.onValueChanged.AddListener(OnDontShowAgainToggle);
		currentMode = WhatsNewModes.WhatsNew;
		dontShowAgainToggle.isOn = true;
		merchButton.onClick.AddListener(OnMerchButton);
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic, hasText: true, limitCheck: true);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Dismiss();
		}
		if (Input.GetKey(KeyCode.PageDown))
		{
			bodyScroll.verticalNormalizedPosition -= scrollStep;
		}
		else if (Input.GetKey(KeyCode.PageUp))
		{
			bodyScroll.verticalNormalizedPosition += scrollStep;
		}
	}

	public void OnDestroy()
	{
		moreInfoButton.onClick.RemoveListener(OnMoreInfoButton);
		closeButton.onClick.RemoveListener(OnCloseButton);
		dontShowAgainToggle.onValueChanged.RemoveListener(OnDontShowAgainToggle);
		merchButton.onClick.RemoveListener(OnMerchButton);
	}

	public WhatsNewDialog Create()
	{
		WhatsNewDialog whatsNewDialog = UnityEngine.Object.Instantiate(this);
		whatsNewDialog.gameObject.SetActive(value: true);
		whatsNewDialog.transform.position = Vector3.zero;
		whatsNewDialog.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "whatsNewDialog");
		return whatsNewDialog;
	}

	public void Dismiss()
	{
		InputLockManager.RemoveControlLock("whatsNewDialog");
		if (dontShowAgain)
		{
			GameSettings.SHOW_WHATSNEW_DIALOG = !dontShowAgain;
			GameSettings.SaveSettings();
		}
		if (dontShowAgain && GameSettings.SHOW_WHATSNEW_DIALOG_VersionsShown.Split(',').IndexOf(VersioningBase.GetVersionString()) < 0)
		{
			GameSettings.SHOW_WHATSNEW_DIALOG_VersionsShown = GameSettings.SHOW_WHATSNEW_DIALOG_VersionsShown + ((GameSettings.SHOW_WHATSNEW_DIALOG_VersionsShown != "") ? "," : "") + VersioningBase.GetVersionString();
			GameSettings.SaveGameSettingsOnly();
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void OnDontShowAgainToggle(bool value)
	{
		dontShowAgain = value;
	}

	public void OnCloseButton()
	{
		Dismiss();
	}

	public void OnMoreInfoButton()
	{
		if (currentMode == WhatsNewModes.WhatsNew)
		{
			currentMode = WhatsNewModes.ChangeLog;
			bannerIcon.gameObject.SetActive(value: false);
			textBody.text = "<b><color=#ffbc00ff><size=18>" + Localizer.Format("#autoLOC_6005010") + ":</size></color></b>\n" + GetChangeLogText();
			moreInfoText.text = "#autoLOC_900211";
		}
		else
		{
			currentMode = WhatsNewModes.WhatsNew;
			bannerIcon.gameObject.SetActive(value: true);
			textBody.text = whatsNewText;
			moreInfoText.text = "#autoLOC_6005010";
		}
		bodyScroll.verticalNormalizedPosition = 1f;
	}

	public void OnMerchButton()
	{
		Process.Start(merchandiseURL);
	}

	public string GetChangeLogText()
	{
		string text = "";
		string readmePath = Path.Combine(KSPUtil.ApplicationRootPath, "readme.txt");
		text = ParseReadMeFile(readmePath, VersioningBase.GetVersionString());
		List<ExpansionsLoader.ExpansionInfo> installedExpansions = ExpansionsLoader.GetInstalledExpansions();
		for (int i = 0; i < installedExpansions.Count; i++)
		{
			string readmePath2 = Path.Combine(KSPUtil.ApplicationRootPath, "GameData/SquadExpansion/" + installedExpansions[i].FolderName + "/readme.txt");
			text = text + "<b><color=#ffbc00ff><size=16>" + installedExpansions[i].DisplayName + " " + Localizer.Format("#autoLOC_6005012") + ":</size></color></b>\n";
			text += ParseReadMeFile(readmePath2, VersioningBase.GetVersionString(), installedExpansions[i].Version);
		}
		return text;
	}

	public string ParseReadMeFile(string readmePath, string relevantVersion)
	{
		return ParseReadMeFile(readmePath, relevantVersion, "-1");
	}

	public string ParseReadMeFile(string readmePath, string relevantVersion, string relevantDLCVersion)
	{
		bool flag = relevantDLCVersion != "-1";
		int num = 0;
		string text = "";
		Version versionFromString = Versioning.GetVersionFromString(relevantVersion);
		Version version = versionFromString;
		if (flag)
		{
			version = Versioning.GetVersionFromString(relevantDLCVersion);
		}
		int num2 = versionFromString.Build;
		string text2 = (flag ? (version.Major + "." + version.Minor) : (versionFromString.Major + "." + versionFromString.Minor));
		if (File.Exists(readmePath))
		{
			StreamReader streamReader = File.OpenText(readmePath);
			int num3 = 0;
			string text3;
			while ((text3 = streamReader.ReadLine()) != null)
			{
				if (text3.StartsWith("===="))
				{
					int num4 = text3.IndexOf("= v" + text2);
					if (num4 <= 0)
					{
						if (num2 < 0 || text3.IndexOf("= v") > 0)
						{
							break;
						}
						continue;
					}
					int num5 = num4 + text3.Substring(num4, 12).LastIndexOf('.');
					string text4 = text3.Substring(num5 + 1, 1);
					if (flag)
					{
						int num6 = text3.IndexOf("Requires KSP v");
						if (num6 > 0)
						{
							int num7 = text3.LastIndexOf('.');
							text3.Substring(num6 + 1).IndexOf('.');
							string text5 = text3.Substring(num6 + 14, num7 - num6 - 14);
							int num8 = num4 + text3.Substring(num4, 12).IndexOf('.');
							int num9 = num4 + text3.Substring(num4, 12).LastIndexOf('.');
							string text6 = text3.Substring(num4 + 3, num9 - num8 + 1);
							int num10 = text3.LastIndexOf('.');
							text4 = text3.Substring(num10 + 1, 1);
							if (text6 == text2 && text5 == versionFromString.Major + "." + versionFromString.Minor)
							{
								text = text + "<color=#ff9600ff>" + Localizer.Format("#autoLOC_6005015", version.Major.ToString(), version.Minor.ToString(), text4.ToString()) + "</color>";
								text += ((text4 != "0") ? Localizer.Format("#autoLOC_6005013", version.Major.ToString(), version.Minor.ToString()) : "");
								num = num3;
								num2--;
							}
							else
							{
								string s = text5.Substring(text5.IndexOf('.') + 1);
								int result = 0;
								if (int.TryParse(s, out result) && result < versionFromString.Minor)
								{
									break;
								}
							}
						}
					}
					else if (text4 == num2.ToString())
					{
						text = text + "<color=#ff9600ff>" + Localizer.Format("#autoLOC_6005015", versionFromString.Major.ToString(), versionFromString.Minor.ToString(), num2.ToString()) + "</color>";
						text += ((text4 != "0") ? Localizer.Format("#autoLOC_6005013", versionFromString.Major.ToString(), versionFromString.Minor.ToString()) : "");
						num = num3;
						num2--;
					}
				}
				if (num > 0 && num3 > num)
				{
					if (text3.Contains("+++"))
					{
						text3 = text3.Replace("+++", "<b>");
						text3 += "</b>";
					}
					text = text + text3 + "\n";
				}
				num3++;
			}
		}
		else
		{
			text = Localizer.Format("#autoLOC_6005009") + "\n\n";
		}
		return text;
	}
}
