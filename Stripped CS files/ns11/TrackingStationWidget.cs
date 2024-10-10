using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class TrackingStationWidget : MonoBehaviour
{
	public Callback<Vessel> onClickEventHandler;

	public TextMeshProUGUI textName;

	public TextMeshProUGUI textStatus;

	public TextMeshProUGUI textInfo;

	public Toggle toggle;

	public VesselIconSprite iconSprite;

	public bool selected;

	public bool hover;

	public const float maneuverFlashSpeed = 2f;

	public bool maneuverChecked;

	public bool maneuverExists;

	public double maneuverTime;

	public static string cacheAutoLOC_482129;

	public Vessel vessel { get; set; }

	public void Awake()
	{
		toggle.onValueChanged.AddListener(onToggle);
		EventTrigger eventTrigger = GetComponent<EventTrigger>();
		if (eventTrigger == null)
		{
			eventTrigger = base.gameObject.AddComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
		}
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerEnter;
		entry.callback.AddListener(OnMouseEnter);
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.PointerExit;
		entry2.callback.AddListener(OnMouseExit);
		eventTrigger.triggers.Add(entry);
		eventTrigger.triggers.Add(entry2);
	}

	public void SetVessel(Vessel v, Callback<Vessel> onClickEventHandler, ToggleGroup toggleGroup)
	{
		toggle.group = toggleGroup;
		toggleGroup.RegisterToggle(toggle);
		vessel = v;
		this.onClickEventHandler = onClickEventHandler;
		Update();
	}

	public void onToggle(bool st)
	{
		if (st && !selected && vessel != null)
		{
			if (hover)
			{
				onClickEventHandler(vessel);
			}
			selected = true;
		}
		else if (st && selected && vessel != null && Mouse.Left.GetDoubleClick(isDelegate: true))
		{
			onClickEventHandler(vessel);
		}
		if (!st && selected)
		{
			selected = false;
		}
	}

	public void OnMouseEnter(BaseEventData data)
	{
		hover = true;
	}

	public void OnMouseExit(BaseEventData data)
	{
		hover = false;
	}

	public void Update()
	{
		if (vessel == null)
		{
			return;
		}
		string value = vessel.DiscoveryInfo.displayName.Value;
		string text = "";
		string text2 = "";
		if (vessel.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors))
		{
			if (vessel.DiscoveryInfo.Level == DiscoveryLevels.Owned)
			{
				if (vessel.vesselType == VesselType.Flag)
				{
					text = Localizer.Format("#autoLOC_200035", vessel.mainBody.displayName);
					text2 = Localizer.Format("#autoLOC_196884", KSPUtil.PrintDate(vessel.launchTime, includeTime: false));
				}
				else if (vessel.vesselType == VesselType.SpaceObject)
				{
					text = Vessel.GetSituationString(vessel);
					text2 = Localizer.Format("#autoLOC_6001919", KSPUtil.PrintDate(vessel.launchTime, includeTime: false));
				}
				else
				{
					text = Vessel.GetSituationString(vessel);
					text2 = ((vessel.situation != Vessel.Situations.PRELAUNCH || string.IsNullOrEmpty(vessel.landedAt) || !(vessel.displaylandedAt != "Untagged")) ? GetMissionInfo(vessel) : Localizer.Format(vessel.displaylandedAt));
				}
			}
			else
			{
				text = Vessel.GetSituationString(vessel);
				text2 = Localizer.Format("#autoLOC_6001919", KSPUtil.PrintDate(vessel.launchTime, includeTime: false));
			}
		}
		else
		{
			text = Localizer.Format("#autoLOC_121438", (vessel.DiscoveryInfo.GetSignalStrength(Planetarium.GetUniversalTime()) * 100.0).ToString("0") + "%");
			text2 = Localizer.Format("#autoLOC_6001920", KSPUtil.PrintDate(vessel.DiscoveryInfo.lastObservedTime, includeTime: false));
		}
		if (textName.text != value)
		{
			textName.text = value;
		}
		if (textStatus.text != text)
		{
			textStatus.text = text;
		}
		if (textInfo.text != text2)
		{
			textInfo.text = text2;
		}
		if (iconSprite.vesselType != vessel.vesselType)
		{
			iconSprite.SetType(vessel.vesselType);
		}
	}

	public void Terminate()
	{
		if (toggle.group != null)
		{
			toggle.group.UnregisterToggle(toggle);
		}
		Object.Destroy(base.gameObject);
	}

	public string GetMissionInfo(Vessel v)
	{
		CheckManeuverNode(v);
		if (maneuverExists)
		{
			if (Mathf.RoundToInt(Mathf.PingPong(Time.time / 2f, 1f) / 1f) != 0)
			{
				return GetManeuverString(v);
			}
			return GetMETString(v);
		}
		return GetMETString(v);
	}

	public string GetMETString(Vessel v)
	{
		return cacheAutoLOC_482129 + Vessel.GetMETString(v);
	}

	public string GetManeuverString(Vessel v)
	{
		string text = "T " + KSPUtil.PrintTime(Planetarium.GetUniversalTime() - maneuverTime, 3, explicitPositive: true);
		return "<color=" + XKCDColors.HexFormat.KSPNotSoGoodOrange + ">" + Localizer.Format("#autoLOC_6001921", text) + "</color>";
	}

	public void CheckManeuverNode(Vessel v)
	{
		if (!maneuverChecked)
		{
			maneuverTime = Vessel.GetNextManeuverTime(v, out maneuverExists);
			maneuverChecked = true;
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_482129 = Localizer.Format("#autoLOC_482129");
	}
}
