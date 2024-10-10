using System.Collections.Generic;
using CommNet;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class KbApp_VesselInfo : KbAppVessel
{
	public delegate bool boolDelegate_KbApp_VesselInfo(KbApp_VesselInfo kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public KbItem_vesselInfo vesselInfoPrefab;

	public KbItem_flagImage flagImagePrefab;

	public TextMeshProUGUI update_type;

	public TextMeshProUGUI update_partCount;

	public TextMeshProUGUI update_mass;

	public UIStateRawImage update_typeIcon;

	public TextMeshProUGUI update_soi;

	public TextMeshProUGUI update_situation;

	public TextMeshProUGUI update_missiontime;

	public TextMeshProUGUI update_latitude;

	public TextMeshProUGUI update_longitude;

	public TextMeshProUGUI update_velocity;

	public TextMeshProUGUI update_altitude;

	public double vesselBrakeTime;

	public TextMeshProUGUI update_maxAccel;

	public TextMeshProUGUI update_burnTimeToZero;

	public static boolDelegate_KbApp_VesselInfo CallbackActivate;

	public static boolDelegate_KbApp_VesselInfo CallbackAfterActivate;

	public TextMeshProUGUI update_commNet_FirstHopDestination;

	public TextMeshProUGUI update_commNet_FirstHopStrength;

	public TextMeshProUGUI update_commNet_FirstHopDistance;

	public TextMeshProUGUI update_commNet_SignalStrength;

	public Button button_commNet_Mode;

	public float lastUpdate;

	public static string cacheAutoLOC_7003285;

	public static string cacheAutoLOC_7001411;

	public override void ActivateApp(MapObject target)
	{
		if (CallbackActivate == null || CallbackActivate(this, target))
		{
			vessel = target.vessel;
			CreateVesselInfoList(vessel);
		}
		if (CallbackAfterActivate != null)
		{
			CallbackAfterActivate(this, target);
		}
	}

	public void CreateVesselInfoList(Vessel v)
	{
		appFrame.appName.text = RUIutils.CutString(vessel.GetDisplayName(), 20, "..");
		appFrame.scrollList.Clear(destroyElements: true);
		if (cascadingList != null)
		{
			cascadingList.gameObject.DestroyGameObject();
		}
		cascadingList = Object.Instantiate(cascadingListPrefab);
		cascadingList.Setup(appFrame.scrollList);
		cascadingList.transform.SetParent(base.transform, worldPositionStays: false);
		Button button;
		if (vessel.vesselType == VesselType.Flag)
		{
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463378"), out button, scaleBg: true);
			List<UIListItem> list = new List<UIListItem>();
			string empty = string.Empty;
			string text = string.Empty;
			if (vessel.loaded)
			{
				empty = vessel.rootPart.flagURL;
				text = vessel.rootPart.Modules.GetModule<FlagSite>().placedBy;
			}
			else
			{
				ProtoPartSnapshot protoPartSnapshot = vessel.protoVessel.protoPartSnapshots[0];
				empty = protoPartSnapshot.flagURL;
				int count = protoPartSnapshot.modules.Count;
				while (count-- > 0)
				{
					if (protoPartSnapshot.modules[count].moduleName == "FlagSite")
					{
						text = protoPartSnapshot.modules[count].moduleValues.GetValue("placedBy");
						break;
					}
				}
			}
			if (empty == string.Empty)
			{
				KbItem_vesselInfo kbItem_vesselInfo = CreateVesselInfoBox();
				kbItem_vesselInfo.image.SetState("flag");
				kbItem_vesselInfo.txtType.text = "<color=#e6752a>Flag:</color>";
				kbItem_vesselInfo.txtMass.text = string.Empty;
				kbItem_vesselInfo.txtMassValue.text = string.Empty;
				kbItem_vesselInfo.txtPartcount.text = string.Empty;
				kbItem_vesselInfo.txtPartcountValue.text = string.Empty;
				list.Add(kbItem_vesselInfo.uiListItem);
			}
			else
			{
				KbItem_flagImage kbItem_flagImage = Object.Instantiate(flagImagePrefab);
				kbItem_flagImage.flag.texture = GameDatabase.Instance.GetTexture(empty, asNormalMap: false);
				list.Add(kbItem_flagImage.uiListItem);
			}
			UIListItem item = cascadingList.CreateBody(Localizer.Format("#autoLOC_463422"), "<color=#c8d782>" + text + "</color>");
			list.Add(item);
			item = cascadingList.CreateBody(Localizer.Format("#autoLOC_7003415"), "<color=#c8d782>" + KSPUtil.PrintDate(vessel.launchTime, includeTime: false) + "</color>");
			list.Add(item);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list, button);
		}
		else
		{
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463431"), out button, scaleBg: true);
			List<UIListItem> list2 = new List<UIListItem>();
			KbItem_vesselInfo kbItem_vesselInfo2 = CreateVesselInfoBox();
			list2.Add(kbItem_vesselInfo2.uiListItem);
			if (!vessel.loaded)
			{
				kbItem_vesselInfo2.txtPartcountValue.text = "<color=#c8d782>" + vessel.protoVessel.protoPartSnapshots.Count + "</color>";
				kbItem_vesselInfo2.txtMassValue.text = "<color=#c8d782>" + KSPUtil.LocalizeNumber(KnowledgeBase.GetUnloadedVesselMass(vessel), "0.##") + Localizer.Format("#autoLOC_7001407") + "</color>";
			}
			UIListItem uIListItem = cascadingList.CreateBody(Localizer.Format("#autoLOC_463444"), string.Empty);
			update_soi = uIListItem.GetTextElement("valueRich");
			list2.Add(uIListItem);
			uIListItem = cascadingList.CreateBody(Localizer.Format("#autoLOC_463448"), string.Empty);
			update_situation = uIListItem.GetTextElement("valueRich");
			list2.Add(uIListItem);
			uIListItem = cascadingList.CreateBody(Localizer.Format("#autoLOC_463452"), string.Empty);
			update_missiontime = uIListItem.GetTextElement("valueRich");
			list2.Add(uIListItem);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list2, button);
		}
		if (vessel.situation != Vessel.Situations.LANDED && vessel.situation != Vessel.Situations.PRELAUNCH && vessel.situation != Vessel.Situations.SPLASHED && vessel.vesselType != VesselType.Flag)
		{
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463486"), out button, scaleBg: true);
			List<UIListItem> list3 = new List<UIListItem>();
			UIListItem uIListItem2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463489"), string.Empty);
			update_velocity = uIListItem2.GetTextElement("valueRich");
			list3.Add(uIListItem2);
			uIListItem2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463493"), string.Empty);
			update_altitude = uIListItem2.GetTextElement("valueRich");
			list3.Add(uIListItem2);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list3, button);
			if (HighLogic.LoadedSceneIsFlight)
			{
				header = cascadingList.CreateHeader("<color=#e6752a>" + Localizer.Format("#autoLOC_443343") + "</color>", out button, scaleBg: true);
				list3 = new List<UIListItem>();
				uIListItem2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_7003413"), string.Empty);
				update_maxAccel = uIListItem2.GetTextElement("valueRich");
				list3.Add(uIListItem2);
				uIListItem2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_7003414"), string.Empty);
				update_burnTimeToZero = uIListItem2.GetTextElement("valueRich");
				list3.Add(uIListItem2);
				cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list3, button);
			}
		}
		else
		{
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463464"), out button, scaleBg: true);
			List<UIListItem> list4 = new List<UIListItem>();
			UIListItem item2;
			if (vessel.vesselType == VesselType.Flag)
			{
				item2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463470"), "<color=#c8d782>" + vessel.mainBody.displayName.LocalizeRemoveGender() + "</color>");
				list4.Add(item2);
			}
			item2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463474"), string.Empty);
			update_latitude = item2.GetTextElement("valueRich");
			list4.Add(item2);
			item2 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463478"), string.Empty);
			update_longitude = item2.GetTextElement("valueRich");
			list4.Add(item2);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list4, button);
		}
		if (CommNetScenario.Instance != null)
		{
			List<UIListItem> list5 = new List<UIListItem>();
			UIListItem uIListItem3 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463525"), string.Empty);
			update_commNet_FirstHopDestination = uIListItem3.GetTextElement("valueRich");
			list5.Add(uIListItem3);
			uIListItem3 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463529"), string.Empty);
			update_commNet_FirstHopDistance = uIListItem3.GetTextElement("valueRich");
			list5.Add(uIListItem3);
			uIListItem3 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463533"), string.Empty);
			update_commNet_FirstHopStrength = uIListItem3.GetTextElement("valueRich");
			list5.Add(uIListItem3);
			uIListItem3 = cascadingList.CreateBody(Localizer.Format("#autoLOC_463537"), string.Empty);
			update_commNet_SignalStrength = uIListItem3.GetTextElement("valueRich");
			list5.Add(uIListItem3);
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_463541"), out button, scaleBg: true);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list5, button);
		}
	}

	public KbItem_vesselInfo CreateVesselInfoBox()
	{
		KbItem_vesselInfo kbItem_vesselInfo = Object.Instantiate(vesselInfoPrefab);
		update_type = kbItem_vesselInfo.txtType;
		update_partCount = kbItem_vesselInfo.txtPartcountValue;
		update_mass = kbItem_vesselInfo.txtMassValue;
		update_typeIcon = kbItem_vesselInfo.image;
		return kbItem_vesselInfo;
	}

	public void FixedUpdate()
	{
		if (appFrame.gameObject.activeSelf && vessel != null && MapView.MapIsEnabled && UpdateNowQuestionmark(0.1f, ref lastUpdate))
		{
			UpdateVesselInfoList(vessel);
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

	public void UpdateVesselInfoList(Vessel v)
	{
		if (v.vesselType != VesselType.Flag)
		{
			update_type.text = "<color=#e6752a>" + v.vesselType.displayDescription() + ":</color>";
			update_typeIcon.SetState(v.vesselType.ToString().ToLower());
			if (v.loaded)
			{
				update_partCount.text = "<color=#c8d782>" + v.Parts.Count + "</color>";
				update_mass.text = Localizer.Format("#autoLOC_463597", v.GetTotalMass().ToString("0.##"));
			}
			update_soi.text = "<color=#c8d782>" + Localizer.Format("#autoLOC_7001301", v.mainBody.displayName) + "</color>";
			update_situation.text = "<color=#c8d782>" + v.SituationString + "</color>";
			update_missiontime.text = "<color=#c8d782>" + KSPUtil.PrintTimeCompact(v.missionTime, explicitPositive: true) + "</color>";
		}
		if (v.situation != Vessel.Situations.LANDED && v.situation != Vessel.Situations.PRELAUNCH && v.situation != Vessel.Situations.SPLASHED && v.vesselType != VesselType.Flag)
		{
			update_velocity.text = Localizer.Format("#autoLOC_463614", v.orbit.GetRelativeVel().magnitude.ToString("0.0"));
			update_altitude.text = Localizer.Format("#autoLOC_463615", v.orbit.altitude.ToString("N0"));
			if (HighLogic.LoadedSceneIsFlight)
			{
				vesselBrakeTime = FlightGlobals.GetDisplaySpeed() / v.specificAcceleration;
				if (!double.IsInfinity(vesselBrakeTime))
				{
					update_maxAccel.text = Localizer.Format("#autoLOC_463622", v.specificAcceleration.ToString("0.0"));
					update_burnTimeToZero.text = "<color=#c8d782>" + KSPUtil.PrintTime(vesselBrakeTime, 2, explicitPositive: false) + "</color>";
				}
				else
				{
					update_maxAccel.text = cacheAutoLOC_7003285;
					update_burnTimeToZero.text = cacheAutoLOC_7003285;
				}
			}
		}
		else
		{
			update_latitude.text = "<color=#c8d782>" + KSPUtil.PrintLatitude(v.latitude) + "</color>";
			update_longitude.text = "<color=#c8d782>" + KSPUtil.PrintLongitude(ResourceUtilities.clampLon(v.longitude)) + "</color>";
		}
		if (CommNetScenario.Instance != null)
		{
			UpdateCommNet(v);
		}
	}

	public void UpdateCommNet(Vessel v)
	{
		CommNetVessel connection = v.Connection;
		if (!(connection == null))
		{
			if (connection != null && connection.IsConnected)
			{
				_ = connection.Comm;
				CommPath controlPath = connection.ControlPath;
				CommLink first = controlPath.First;
				update_commNet_FirstHopDestination.text = "<color=#c8d782>" + Localizer.Format(first.end.displayName) + "</color>";
				update_commNet_FirstHopDistance.text = "<color=#c8d782>" + KSPUtil.PrintSI(first.cost, cacheAutoLOC_7001411) + "</color>";
				update_commNet_FirstHopStrength.text = string.Concat("<color=", (SignalStrengthColor)first.signal, ">", KSPUtil.LocalizeNumber(first.signalStrength, "F2"), "</color>");
				update_commNet_SignalStrength.text = string.Concat("<color=", (SignalStrengthColor)controlPath.signal, ">", KSPUtil.LocalizeNumber(controlPath.signalStrength, "F2"), "</color>");
			}
			else
			{
				update_commNet_FirstHopDestination.text = "<color=#c8d782>-</color>";
				update_commNet_FirstHopDistance.text = "<color=#c8d782>-</color>";
				update_commNet_FirstHopStrength.text = "<color=#c8d782>-</color>";
				update_commNet_SignalStrength.text = "<color=#c8d782>-</color>";
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003285 = "<color=#c8d782>" + Localizer.Format("#autoLOC_7003285") + "</color>";
		cacheAutoLOC_7001411 = Localizer.Format("#autoLOC_7001411");
	}
}
