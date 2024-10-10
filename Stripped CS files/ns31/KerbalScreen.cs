using System.Collections.Generic;
using UnityEngine;

namespace ns31;

public class KerbalScreen : MonoBehaviour
{
	public KerbalScreenItem itemPrefab;

	public RectTransform listParent;

	public bool rosterDirty;

	public List<KerbalScreenItem> items = new List<KerbalScreenItem>();

	public void Awake()
	{
	}

	public void Start()
	{
		GameEvents.onGameStateCreated.Add(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Add(OnGameStateLoaded);
		GameEvents.onKerbalAdded.Add(OnKerbalAdded);
		GameEvents.onKerbalRemoved.Add(OnKerbalRemoved);
		GameEvents.onKerbalStatusChange.Add(OnKerbalStatusChange);
		UpdateRoster();
	}

	public void OnDestroy()
	{
		GameEvents.onGameStateCreated.Remove(OnGameStateCreated);
		GameEvents.onGameStatePostLoad.Remove(OnGameStateLoaded);
		GameEvents.onKerbalAdded.Remove(OnKerbalAdded);
		GameEvents.onKerbalRemoved.Remove(OnKerbalRemoved);
		GameEvents.onKerbalStatusChange.Remove(OnKerbalStatusChange);
	}

	public void Update()
	{
		if (rosterDirty)
		{
			UpdateRoster();
		}
	}

	public void OnGameStateCreated(Game game)
	{
		rosterDirty = true;
	}

	public void OnGameStateLoaded(ConfigNode node)
	{
		rosterDirty = true;
	}

	public void OnKerbalAdded(ProtoCrewMember pcm)
	{
		AddKerbal(pcm);
	}

	public void OnKerbalRemoved(ProtoCrewMember pcm)
	{
		RemoveKerbal(pcm);
	}

	public void OnKerbalStatusChange(ProtoCrewMember pcm, ProtoCrewMember.RosterStatus rsOld, ProtoCrewMember.RosterStatus rsNew)
	{
		UpdateKerbal(pcm);
	}

	public void UpdateRoster()
	{
		if (!base.gameObject.activeSelf)
		{
			rosterDirty = true;
			return;
		}
		rosterDirty = false;
		ClearList();
		if (HighLogic.CurrentGame != null)
		{
			int i = 0;
			for (int count = HighLogic.CurrentGame.CrewRoster.Count; i < count; i++)
			{
				CreateItem(HighLogic.CurrentGame.CrewRoster[i]);
			}
		}
	}

	public void ClearList()
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			Object.Destroy(items[i].gameObject);
		}
		items.Clear();
	}

	public void CreateItem(ProtoCrewMember pcm)
	{
		KerbalScreenItem kerbalScreenItem = Object.Instantiate(itemPrefab);
		kerbalScreenItem.transform.SetParent(listParent, worldPositionStays: false);
		kerbalScreenItem.Setup(pcm);
		items.Add(kerbalScreenItem);
	}

	public void AddKerbal(ProtoCrewMember pcm)
	{
		if (!base.gameObject.activeSelf)
		{
			rosterDirty = true;
		}
		else
		{
			if (rosterDirty)
			{
				return;
			}
			int num = 0;
			int count = items.Count;
			KerbalScreenItem kerbalScreenItem;
			while (true)
			{
				if (num < count)
				{
					kerbalScreenItem = items[num];
					if (kerbalScreenItem.name == pcm.name)
					{
						break;
					}
					num++;
					continue;
				}
				CreateItem(pcm);
				return;
			}
			kerbalScreenItem.Setup(pcm);
		}
	}

	public void RemoveKerbal(ProtoCrewMember pcm)
	{
		if (!base.gameObject.activeSelf)
		{
			rosterDirty = true;
		}
		else
		{
			if (rosterDirty)
			{
				return;
			}
			int num = 0;
			int count = items.Count;
			KerbalScreenItem kerbalScreenItem;
			while (true)
			{
				if (num < count)
				{
					kerbalScreenItem = items[num];
					if (kerbalScreenItem.name == pcm.name)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			items.RemoveAt(num);
			Object.Destroy(kerbalScreenItem);
		}
	}

	public void UpdateKerbal(ProtoCrewMember pcm)
	{
		if (!base.gameObject.activeSelf)
		{
			rosterDirty = true;
		}
		else
		{
			if (rosterDirty)
			{
				return;
			}
			int num = 0;
			int count = items.Count;
			KerbalScreenItem kerbalScreenItem;
			while (true)
			{
				if (num < count)
				{
					kerbalScreenItem = items[num];
					if (kerbalScreenItem.name == pcm.name)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			kerbalScreenItem.Setup(HighLogic.CurrentGame.CrewRoster[num]);
		}
	}
}
