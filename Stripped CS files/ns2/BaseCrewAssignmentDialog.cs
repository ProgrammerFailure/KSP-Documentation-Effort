using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class BaseCrewAssignmentDialog : MonoBehaviour
{
	public UIList scrollListCrew;

	public UIList scrollListAvail;

	[SerializeField]
	public CrewListItem widgetCrew;

	[SerializeField]
	public CrewListItem widgetCrewEmpty;

	[SerializeField]
	public UIListItem widgetBorder;

	[SerializeField]
	public Sprite disabledCrewListSprite;

	public VesselCrewManifest defaultManifest;

	public VesselCrewManifest listManifest;

	public static EventData<VesselCrewManifest> onCrewDialogChange = new EventData<VesselCrewManifest>("onCrewDialogChange");

	public bool listIsValid;

	public static Color disabledColor = new Color(1f, 1f, 1f, 0.5f);

	public VesselCrewManifest CurrentManifestUnsafe => listManifest;

	public KerbalRoster CurrentCrewRoster
	{
		get
		{
			return GetCurrentCrewRoster();
		}
		set
		{
			SetCurrentCrewRoster(value);
		}
	}

	public virtual void Awake()
	{
		scrollListCrew.onDrop.AddListener(DropOnCrewList);
		scrollListAvail.onDrop.AddListener(DropOnAvailList);
	}

	public virtual void SetCurrentCrewRoster(KerbalRoster newRoster)
	{
	}

	public virtual KerbalRoster GetCurrentCrewRoster()
	{
		return null;
	}

	public virtual void DropOnCrewList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		CrewListItem component = insertItem.GetComponent<CrewListItem>();
		UIListItem uilistItemAt = scrollListCrew.GetUilistItemAt(insertIndex);
		CrewListItem crewListItem = null;
		if (uilistItemAt != null)
		{
			crewListItem = uilistItemAt.GetComponent<CrewListItem>();
		}
		if (fromList == scrollListAvail)
		{
			int index = scrollListAvail.GetIndex(insertItem);
			scrollListAvail.RemoveItem(insertItem);
			component.SetButton(CrewListItem.ButtonTypes.const_0);
			component.pUid = crewListItem.pUid;
			if (crewListItem.isEmpty)
			{
				scrollListCrew.RemoveItem(uilistItemAt, deleteItem: true);
				scrollListCrew.InsertItem(insertItem, insertIndex, forceZ: true, worldPositionStays: true);
				return;
			}
			scrollListCrew.RemoveItem(uilistItemAt);
			scrollListCrew.InsertItem(insertItem, insertIndex, forceZ: true, worldPositionStays: true);
			crewListItem.pUid = 0u;
			crewListItem.SetButton(CrewListItem.ButtonTypes.const_1);
			scrollListAvail.InsertItem(uilistItemAt, index, forceZ: true, worldPositionStays: true);
		}
		else if (crewListItem != null && fromList == scrollListCrew)
		{
			uint pUid = component.pUid;
			uint pUid2 = crewListItem.pUid;
			component.pUid = pUid2;
			crewListItem.pUid = pUid;
			scrollListCrew.SwapItems(uilistItemAt, insertItem, forzeZ: true, worldPositionStays: true);
		}
	}

	public virtual void DropOnAvailList(UIList fromList, UIListItem insertItem, int insertIndex)
	{
		if (!(fromList == scrollListAvail))
		{
			CrewListItem component = insertItem.GetComponent<CrewListItem>();
			int index = scrollListCrew.GetIndex(insertItem);
			uint pUid = component.pUid;
			scrollListCrew.RemoveItem(insertItem);
			UIListItem uIListItem = UnityEngine.Object.Instantiate(scrollListCrew.placeholder);
			if (index != -1)
			{
				scrollListCrew.InsertItem(uIListItem, index);
			}
			uIListItem.GetComponent<CrewListItem>().pUid = pUid;
			component.SetButton(CrewListItem.ButtonTypes.const_1);
			component.pUid = 0u;
			scrollListAvail.InsertItem(insertItem, insertIndex, forceZ: true, worldPositionStays: true);
		}
	}

	public virtual void OnDestroy()
	{
	}

	public void RefreshCrewLists(VesselCrewManifest crewManifest, bool setAsDefault, bool updateUI, Func<PartCrewManifest, bool> displayFilter = null)
	{
		if (defaultManifest == null || setAsDefault)
		{
			defaultManifest = crewManifest;
		}
		listManifest = crewManifest;
		if (updateUI)
		{
			CreateCrewList(crewManifest, displayFilter);
			CreateAvailList(crewManifest);
			listIsValid = true;
		}
		else
		{
			listIsValid = false;
		}
		onCrewDialogChange.Fire(crewManifest);
	}

	public virtual void Refresh()
	{
		RefreshCrewLists(GetManifest(), setAsDefault: false, updateUI: true);
	}

	public void SetDefaultManifest(VesselCrewManifest manifest)
	{
		defaultManifest = manifest;
	}

	public void ClearLists()
	{
		scrollListCrew.Clear(destroyElements: true);
		scrollListAvail.Clear(destroyElements: true);
	}

	public void CreateCrewList(VesselCrewManifest manifest, Func<PartCrewManifest, bool> displayFilter = null)
	{
		if (manifest == null)
		{
			return;
		}
		scrollListCrew.Clear(destroyElements: true);
		int i = 0;
		for (int count = manifest.PartManifests.Count; i < count; i++)
		{
			PartCrewManifest partCrewManifest = manifest.PartManifests[i];
			if (partCrewManifest.partCrew.Length == 0)
			{
				continue;
			}
			if (displayFilter != null && !displayFilter(partCrewManifest))
			{
				AddCrewListBorder("[DETACHED]" + partCrewManifest.PartInfo.title, XKCDColors.KSPNotSoGoodOrange);
			}
			else
			{
				AddCrewListBorder(partCrewManifest.PartInfo.title, XKCDColors.KSPBadassGreen);
			}
			int num = partCrewManifest.partCrew.Length;
			for (int j = 0; j < num; j++)
			{
				ProtoCrewMember protoCrewMember = CurrentCrewRoster[partCrewManifest.partCrew[j]];
				if (protoCrewMember == null)
				{
					AddCrewItemEmpty(partCrewManifest.PartID);
				}
				else
				{
					AddCrewItem(protoCrewMember, partCrewManifest.PartID);
				}
			}
		}
	}

	public void CreateAvailList(VesselCrewManifest manifest)
	{
		if (manifest == null)
		{
			return;
		}
		scrollListAvail.Clear(destroyElements: true);
		IEnumerator<ProtoCrewMember> enumerator = CurrentCrewRoster.Kerbals(ProtoCrewMember.KerbalType.Crew, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!manifest.Contains(enumerator.Current))
			{
				AddAvailItem(enumerator.Current);
			}
		}
		enumerator = CurrentCrewRoster.Kerbals(ProtoCrewMember.KerbalType.Tourist, default(ProtoCrewMember.RosterStatus)).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!manifest.Contains(enumerator.Current))
			{
				AddAvailItem(enumerator.Current);
			}
		}
	}

	public void AddCrewListBorder(string text, Color textColor, bool expandHeight = false)
	{
		UIListItem uIListItem = UnityEngine.Object.Instantiate(widgetBorder);
		uIListItem.GetComponent<UIStatePanel>().SetText("partname", text, textColor);
		uIListItem.GetComponent<UIStatePanel>().SetTextOverflowMode("partname", TextOverflowModes.Ellipsis);
		if (expandHeight)
		{
			uIListItem.GetComponent<LayoutElement>().minHeight = 50f;
		}
		scrollListCrew.AddItem(uIListItem);
	}

	public void AddCrewItemEmpty(uint pUid, int index = -1)
	{
		CrewListItem crewListItem = UnityEngine.Object.Instantiate(widgetCrewEmpty);
		crewListItem.pUid = pUid;
		UIListItem component = crewListItem.GetComponent<UIListItem>();
		if (index == -1)
		{
			scrollListCrew.AddItem(component);
		}
		else
		{
			scrollListCrew.InsertItem(component, index);
		}
	}

	public virtual CrewListItem AddItem(uint pUid, UIList list, ProtoCrewMember crew)
	{
		CrewListItem crewListItem = UnityEngine.Object.Instantiate(widgetCrew);
		crewListItem.pUid = pUid;
		UIListItem component = crewListItem.GetComponent<UIListItem>();
		crewListItem.AddButtonInputDelegate(ListItemButtonClick);
		list.AddItem(component);
		return crewListItem;
	}

	public virtual void AddAvailItem(ProtoCrewMember crew, UIList list = null, CrewListItem.ButtonTypes type = CrewListItem.ButtonTypes.const_1)
	{
		AddAvailItem(crew, out var _, list, type);
	}

	public virtual void AddAvailItem(ProtoCrewMember crew, out CrewListItem item, UIList list = null, CrewListItem.ButtonTypes type = CrewListItem.ButtonTypes.const_1)
	{
		CrewListItem crewListItem = AddItem(0u, list ?? scrollListAvail, crew);
		crewListItem.SetName(crew.name);
		crewListItem.SetButton(type);
		crewListItem.SetStats(crew);
		crewListItem.SetXP(crew);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetKerbalAsApplicableType(crew);
		crewListItem.SetTooltip(crew);
		SuitCombos component = GameDatabase.Instance.GetComponent<SuitCombos>();
		if (crew.name == component.helmetSuitPickerWindow.crew.name)
		{
			component.helmetSuitPickerWindow.SetupSuitTypeButtons(crewListItem, crew, component.helmetSuitPickerWindow.kerbalType);
		}
		if (crew.inactive)
		{
			crewListItem.GetComponent<UIDragPanel>().dragEnabled = false;
			UIHoverPanel component2 = crewListItem.GetComponent<UIHoverPanel>();
			component2.backgroundImage.sprite = (component2.backgroundHover = (component2.backgroundNormal = disabledCrewListSprite));
			crewListItem.kerbalName.color = Color.grey;
			crewListItem.xp_trait.color = Color.grey;
			crewListItem.kerbalSprite.color = disabledColor;
			crewListItem.MouseoverEnabled = false;
		}
		item = crewListItem;
	}

	public void AddAvailItem(string name, string trait, float xp, int level)
	{
		CrewListItem crewListItem = AddItem(0u, scrollListAvail, null);
		crewListItem.SetName(name);
		crewListItem.SetButton(CrewListItem.ButtonTypes.const_1);
		crewListItem.SetXP(trait, xp, level);
	}

	public void AddCrewItem(ProtoCrewMember crew, uint pUid)
	{
		CrewListItem crewListItem = AddItem(pUid, scrollListCrew, crew);
		crewListItem.SetName(crew.name);
		crewListItem.SetButton(CrewListItem.ButtonTypes.const_0);
		crewListItem.SetStats(crew);
		crewListItem.SetXP(crew);
		crewListItem.SetCrewRef(crew);
		crewListItem.SetKerbalAsApplicableType(crew);
		crewListItem.SetTooltip(crew);
		SuitCombos component = GameDatabase.Instance.GetComponent<SuitCombos>();
		if (crew.name == component.helmetSuitPickerWindow.crew.name)
		{
			component.helmetSuitPickerWindow.SetupSuitTypeButtons(crewListItem, crew, component.helmetSuitPickerWindow.kerbalType);
		}
	}

	public virtual void ListItemButtonClick(CrewListItem.ButtonTypes type, CrewListItem clickItem)
	{
		if (type == CrewListItem.ButtonTypes.const_1)
		{
			int num = 0;
			while (true)
			{
				UIListItem uilistItemAt = scrollListCrew.GetUilistItemAt(++num);
				if (uilistItemAt != null)
				{
					CrewListItem component = uilistItemAt.GetComponent<CrewListItem>();
					if (component != null && component.isEmpty)
					{
						MoveCrewToEmptySeat(scrollListAvail, scrollListCrew, clickItem.GetComponent<UIListItem>(), num);
						break;
					}
					continue;
				}
				break;
			}
		}
		else
		{
			MoveCrewToAvail(scrollListCrew, scrollListAvail, clickItem.GetComponent<UIListItem>());
			ExecuteEvents.GetEventHandler<IPointerClickHandler>(base.gameObject);
		}
	}

	public virtual void MoveCrewToEmptySeat(UIList fromlist, UIList tolist, UIListItem itemToMove, int index)
	{
		CrewListItem component = itemToMove.GetComponent<CrewListItem>();
		if (component == null)
		{
			Debug.LogError("[CrewAssignmentDialog] attempting to move a non CrewListItem.");
		}
		if (tolist.GetUilistItemAt(index) == null)
		{
			Debug.LogError("[CrewAssignmentDialog] attempting to move to a invalid seat.");
		}
		component.pUid = tolist.GetUilistItemAt(index).GetComponent<CrewListItem>().pUid;
		fromlist.RemoveItem(itemToMove);
		component.SetButton(CrewListItem.ButtonTypes.const_0);
		tolist.RemoveItem(index, deleteItem: true);
		tolist.InsertItem(itemToMove, index, forceZ: true, worldPositionStays: true);
	}

	public virtual void MoveCrewToAvail(UIList fromlist, UIList tolist, UIListItem itemToMove)
	{
		if (fromlist == scrollListCrew)
		{
			CrewListItem component = itemToMove.GetComponent<CrewListItem>();
			int index = scrollListCrew.GetIndex(itemToMove);
			fromlist.RemoveItem(itemToMove);
			AddCrewItemEmpty(component.pUid, index);
			component.SetButton(CrewListItem.ButtonTypes.const_1);
			component.pUid = 0u;
			tolist.InsertItem(itemToMove, 0, forceZ: true, worldPositionStays: true);
		}
	}

	public void ButtonFill()
	{
		int num = 0;
		while (true)
		{
			UIListItem uilistItemAt = scrollListAvail.GetUilistItemAt(0);
			if (uilistItemAt != null)
			{
				UIListItem uilistItemAt2 = scrollListCrew.GetUilistItemAt(++num);
				if (uilistItemAt2 != null)
				{
					CrewListItem component = uilistItemAt2.GetComponent<CrewListItem>();
					if (component != null && component.isEmpty)
					{
						MoveCrewToEmptySeat(scrollListAvail, scrollListCrew, uilistItemAt, num);
					}
					continue;
				}
				break;
			}
			break;
		}
	}

	public void ButtonClear()
	{
		int num = 0;
		while (true)
		{
			UIListItem uilistItemAt = scrollListCrew.GetUilistItemAt(++num);
			if (uilistItemAt != null)
			{
				CrewListItem component = uilistItemAt.GetComponent<CrewListItem>();
				if (component != null && !component.isEmpty)
				{
					MoveCrewToAvail(scrollListCrew, scrollListAvail, uilistItemAt);
				}
				continue;
			}
			break;
		}
	}

	public void ButtonReset()
	{
		Refresh();
	}

	public VesselCrewManifest GetManifest(bool createClone = true)
	{
		if (defaultManifest == null)
		{
			return null;
		}
		if (!listIsValid)
		{
			return listManifest;
		}
		VesselCrewManifest vesselCrewManifest = ((!createClone) ? listManifest : VesselCrewManifest.CloneOf(listManifest, blank: true));
		int num = 0;
		int i = 0;
		for (int count = scrollListCrew.Count; i < count; i++)
		{
			CrewListItem component = scrollListCrew.GetUilistItemAt(i).GetComponent<CrewListItem>();
			if (component == null)
			{
				num = 0;
				continue;
			}
			if (component != null && component.GetCrewRef() != null)
			{
				vesselCrewManifest.GetPartCrewManifest(component.pUid).AddCrewToSeat(component.GetCrewRef(), num);
			}
			num++;
		}
		return vesselCrewManifest;
	}
}
