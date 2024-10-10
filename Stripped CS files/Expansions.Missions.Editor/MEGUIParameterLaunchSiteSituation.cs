using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_LaunchSiteSituation]
public class MEGUIParameterLaunchSiteSituation : MEGUICompoundParameter
{
	public RectTransform containerHeader;

	public Image imageHeader;

	public Image imageBackGround;

	public TMP_InputField inputLaunchSiteName;

	public Button buttonExpand;

	public string guiSkinName = "MainMenuSkin";

	[SerializeField]
	public GameObject CollapsibleGroup;

	[SerializeField]
	public GameObject CollapsibleButtonIcon;

	public MEGUIParameterCelestialBody_VesselGroundLocation launchSiteGroundLocation;

	public LaunchSiteSituation launchSiteSituation;

	public MEGUIParameterDropdownList editorFacility;

	public LaunchSiteSituation FieldValue
	{
		get
		{
			return launchSiteSituation;
		}
		set
		{
			launchSiteSituation = value;
			if (field != null)
			{
				field.SetValue(value);
			}
		}
	}

	public override void Display()
	{
		base.Display();
		launchSiteGroundLocation.Display();
	}

	public override void Setup(string name, object value)
	{
		base.Setup(name, value);
		launchSiteSituation = value as LaunchSiteSituation;
		editorFacility = subParameters["facility"] as MEGUIParameterDropdownList;
		if (editorFacility != null && CollapsibleGroup != null)
		{
			editorFacility.transform.SetParent(CollapsibleGroup.transform);
			editorFacility.GetComponent<RectTransform>().SetAsFirstSibling();
		}
		launchSiteGroundLocation = subParameters["launchSiteGroundLocation"] as MEGUIParameterCelestialBody_VesselGroundLocation;
		if (launchSiteGroundLocation != null)
		{
			launchSiteGroundLocation.gameObject.SetActive(value: true);
			launchSiteGroundLocation.Display();
			if (CollapsibleGroup != null)
			{
				launchSiteGroundLocation.transform.SetParent(CollapsibleGroup.transform);
				launchSiteGroundLocation.GetComponent<RectTransform>().SetAsFirstSibling();
			}
		}
		inputLaunchSiteName.text = launchSiteSituation.launchSiteName;
		buttonExpand.onClick.AddListener(toggleExpand);
		inputLaunchSiteName.onEndEdit.AddListener(InputTextAction_SetLaunchSiteName);
	}

	public void InputTextAction_SetLaunchSiteName(string newTitle)
	{
		MissionEditorHistory.PushUndoAction(this, onHistorysetLaunchSiteName);
		if (FieldValue.launchSiteName != newTitle)
		{
			FieldValue.RemoveLaunchSite();
		}
		FieldValue.launchSiteName = newTitle;
		StartCoroutine(FieldValue.AddLaunchSite(createObject: false));
		UpdateNodeBodyUI();
	}

	public void toggleExpand()
	{
		bool flag = !CollapsibleGroup.activeSelf;
		CollapsibleGroup.SetActive(flag);
		CollapsibleButtonIcon.transform.eulerAngles = (flag ? new Vector3(0f, 0f, 180f) : Vector3.zero);
	}

	public override ConfigNode GetState()
	{
		ConfigNode configNode = new ConfigNode();
		configNode.AddValue("launchSiteName", FieldValue.launchSiteName);
		return configNode;
	}

	public void onHistorysetLaunchSiteName(ConfigNode data, HistoryType type)
	{
		data.TryGetValue("launchSiteName", ref FieldValue.launchSiteName);
		inputLaunchSiteName.text = FieldValue.launchSiteName;
	}
}
