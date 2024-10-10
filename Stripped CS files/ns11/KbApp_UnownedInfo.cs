using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class KbApp_UnownedInfo : KbAppUnowned
{
	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public KbItem_unownedInfo unownedInfoPrefab;

	public GameObject textPrefab;

	public CometVessel cometVessel;

	public UIStateRawImage update_objectTypeIcon;

	public TextMeshProUGUI update_trackingStatus;

	public TextMeshProUGUI update_lastSeen;

	public TextMeshProUGUI update_lastSeenValue;

	public TextMeshProUGUI update_objSize;

	public Slider update_signalStrength;

	public TextMeshProUGUI update_objectSituation;

	public TextMeshProUGUI update_objectType;

	public TextMeshProUGUI update_obText;

	public float lastUpdate;

	public static string cacheAutoLOC_463116;

	public static string cacheAutoLOC_5050038;

	public override void ActivateApp(MapObject target)
	{
		currentUnowned = target.Discoverable;
		Debug.Log(target.vessel);
		CreateUnownedTrackingInfoList(currentUnowned);
	}

	public void CreateUnownedTrackingInfoList(IDiscoverable o)
	{
		appFrame.appName.text = RUIutils.CutString(currentUnowned.DiscoveryInfo.displayName.Value, 20, "..");
		appFrame.scrollList.Clear(destroyElements: true);
		if (cascadingList != null)
		{
			cascadingList.gameObject.DestroyGameObject();
		}
		cascadingList = Object.Instantiate(cascadingListPrefab);
		cascadingList.Setup(appFrame.scrollList);
		cascadingList.transform.SetParent(base.transform, worldPositionStays: false);
		UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463080"), out var button, scaleBg: true);
		List<UIListItem> list = new List<UIListItem>();
		KbItem_unownedInfo kbItem_unownedInfo = Object.Instantiate(unownedInfoPrefab);
		update_objectTypeIcon = kbItem_unownedInfo.image;
		update_trackingStatus = kbItem_unownedInfo.txtTrackingStatus;
		update_lastSeenValue = kbItem_unownedInfo.txtLastSeenValue;
		update_lastSeen = kbItem_unownedInfo.txtLastSeen;
		update_objSize = kbItem_unownedInfo.txtSizeClassValue;
		update_signalStrength = kbItem_unownedInfo.slider;
		list.Add(kbItem_unownedInfo.uiListItem);
		UIListItem uIListItem = cascadingList.CreateBody("<color=#c8d782>" + o.DiscoveryInfo.situation.OneLiner + "</color>", "");
		update_objectSituation = uIListItem.GetTextElement("keyRich");
		list.Add(uIListItem);
		cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list, button);
		header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463099"), out button, scaleBg: true);
		list.Clear();
		GameObject gameObject = Object.Instantiate(textPrefab);
		update_obText = gameObject.GetComponent<TextMeshProUGUI>();
		update_obText.text = DiscoveryInfo.GetSizeClassDescription(o.DiscoveryInfo.objectSize);
		Vessel vessel = o as Vessel;
		if (vessel != null)
		{
			cometVessel = vessel.Comet;
		}
		list.Add(gameObject.GetComponent<UIListItem>());
		cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list, button);
	}

	public void UpdateTrackingInfoList(IDiscoverable o)
	{
		update_objectTypeIcon.SetState(o.DiscoveryInfo.type.Value.Replace(" ", "").ToLower());
		update_trackingStatus.text = "<color=#c8d782>" + o.DiscoveryInfo.trackingStatus.Value + "</color>";
		update_lastSeen.text = (o.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors) ? cacheAutoLOC_463116 : cacheAutoLOC_5050038);
		update_lastSeenValue.text = Localizer.Format("#autoLOC_463117", KSPUtil.PrintTime(Planetarium.GetUniversalTime() - o.DiscoveryInfo.lastObservedTime, 2, explicitPositive: false));
		update_objSize.text = "<color=#c8d782>" + o.DiscoveryInfo.size.Value + "</color>";
		update_signalStrength.value = (float)o.DiscoveryInfo.GetSignalStrength(Planetarium.GetUniversalTime());
		update_objectSituation.text = "<color=#c8d782>" + o.DiscoveryInfo.situation.Value + "</color>";
		if (o.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.StateVectors) && cometVessel != null)
		{
			TextMeshProUGUI componentInChildren = appFrame.scrollList.GetUilistItemAt(4).GetComponentInChildren<TextMeshProUGUI>();
			if (componentInChildren != null)
			{
				componentInChildren.text = Localizer.Format("#autoLOC_6011138");
			}
			update_obText.text = "<color=#c8d782>" + cometVessel.CometType.displayName + "</color>\n" + cometVessel.CometType.description;
		}
		else
		{
			update_obText.text = DiscoveryInfo.GetSizeClassDescription(o.DiscoveryInfo.objectSize);
		}
	}

	public void FixedUpdate()
	{
		if (appFrame.gameObject.activeSelf && currentUnowned != null && MapView.MapIsEnabled && UpdateNowQuestionmark(0.1f, ref lastUpdate))
		{
			UpdateTrackingInfoList(currentUnowned);
		}
	}

	public bool UpdateNowQuestionmark(float updateInterval, ref float lastUpdate)
	{
		if (Time.time - updateInterval > lastUpdate)
		{
			lastUpdate = Time.time;
			return true;
		}
		return false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_463116 = Localizer.Format("#autoLOC_463116");
		cacheAutoLOC_5050038 = Localizer.Format("#autoLOC_5050038");
	}
}
