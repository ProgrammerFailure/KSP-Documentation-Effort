using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class KbApp_VesselCrew : KbAppVessel
{
	public delegate bool boolDelegate_KbApp_VesselCrew(KbApp_VesselCrew kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public KbItem_kerbalInfo kerbalInfoPrefab;

	public List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>> pcmByPartName = new List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>>();

	public static boolDelegate_KbApp_VesselCrew CallbackActivate;

	public static boolDelegate_KbApp_VesselCrew CallbackAfterActivate;

	public override void ActivateApp(MapObject target)
	{
		if (CallbackActivate == null || CallbackActivate(this, target))
		{
			vessel = target.vessel;
			CreateVesselCrewList(vessel);
		}
		if (CallbackAfterActivate != null)
		{
			CallbackAfterActivate(this, target);
		}
	}

	public static int CompareKVP(KeyValuePair<AvailablePart, List<ProtoCrewMember>> a, KeyValuePair<AvailablePart, List<ProtoCrewMember>> b)
	{
		return a.Key.title.CompareTo(b.Key.title);
	}

	public static int CompareSeatIdx(ProtoCrewMember r1, ProtoCrewMember r2)
	{
		return r1.seatIdx.CompareTo(r2.seatIdx);
	}

	public void CreateVesselCrewList(Vessel v)
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
		if (v.state == Vessel.State.DEAD)
		{
			Debug.Log("[KnowledgeBase.KbApp_VesselCrew]: " + v.GetDisplayName() + " is dead, can't display crew info for it", v.gameObject);
			return;
		}
		Button button;
		if (v.isEVA)
		{
			ProtoCrewMember pcm = v.GetVesselCrew()[0];
			UIListItem header = cascadingList.CreateHeader(Localizer.Format("#autoLOC_6001968"), out button, scaleBg: true);
			List<UIListItem> list = new List<UIListItem>();
			KbItem_kerbalInfo kerbalInfo = Object.Instantiate(kerbalInfoPrefab);
			SetupKerbalItem(ref kerbalInfo, pcm, "#autoLOC_6001969");
			list.Add(kerbalInfo.uiListItem);
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list, button);
			return;
		}
		pcmByPartName = KnowledgeBase.GetVesselCrewByAvailablePart(v);
		pcmByPartName.Sort(CompareKVP);
		int count = pcmByPartName.Count;
		for (int i = 0; i < count; i++)
		{
			KeyValuePair<AvailablePart, List<ProtoCrewMember>> keyValuePair = pcmByPartName[i];
			keyValuePair.Value.Sort(CompareSeatIdx);
			UIListItem header = cascadingList.CreateHeader("<color=#e6752a>" + RUIutils.CutString(keyValuePair.Key.title, 28, "..") + "</color>", out button, scaleBg: true);
			List<UIListItem> list2 = new List<UIListItem>();
			int count2 = keyValuePair.Value.Count;
			for (int j = 0; j < count2; j++)
			{
				if (keyValuePair.Value[j] == null)
				{
					continue;
				}
				ProtoCrewMember protoCrewMember = keyValuePair.Value[j];
				KbItem_kerbalInfo kerbalInfo2 = Object.Instantiate(kerbalInfoPrefab);
				string seatName = "Passenger seat";
				if (v.loaded)
				{
					if (protoCrewMember.seat != null)
					{
						seatName = protoCrewMember.seat.displayseatName;
					}
				}
				else
				{
					InternalModel internalPart = PartLoader.GetInternalPart(keyValuePair.Key.internalConfig.GetValue("name"));
					if (internalPart != null && internalPart.seats != null && internalPart.seats.Count > protoCrewMember.seatIdx && protoCrewMember.seatIdx >= 0)
					{
						seatName = internalPart.seats[protoCrewMember.seatIdx].displayseatName;
					}
				}
				SetupKerbalItem(ref kerbalInfo2, protoCrewMember, seatName);
				list2.Add(kerbalInfo2.uiListItem);
			}
			cascadingList.ruiList.AddCascadingItem(header, cascadingList.CreateFooter(), list2, button);
		}
	}

	public void SetupKerbalItem(ref KbItem_kerbalInfo kerbalInfo, ProtoCrewMember pcm, string seatName)
	{
		kerbalInfo.txtName.text = pcm.name;
		kerbalInfo.txtSeat.text = seatName;
		ProtoCrewMember.Gender gender = pcm.gender;
		if (gender != 0 && gender == ProtoCrewMember.Gender.Female)
		{
			if (pcm.veteran)
			{
				kerbalInfo.kerbalIcon.texture = AssetBase.GetTexture("kerbalicon_suit_orange_female");
			}
			else
			{
				kerbalInfo.kerbalIcon.texture = AssetBase.GetTexture("kerbalicon_suit_female");
			}
		}
		else if (pcm.veteran)
		{
			kerbalInfo.kerbalIcon.texture = AssetBase.GetTexture("kerbalicon_suit_orange");
		}
		else
		{
			kerbalInfo.kerbalIcon.texture = AssetBase.GetTexture("kerbalicon_suit");
		}
		kerbalInfo.txtXp.text = pcm.experienceTrait.Title;
		kerbalInfo.xpStars.SetState(pcm.experienceLevel);
	}
}
