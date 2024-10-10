using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions;

public class MissionListGroup : MonoBehaviour
{
	public TextMeshProUGUI title;

	public RectTransform containerChildren;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	[SerializeField]
	public Image headerImage;

	public MissionPack pack;

	public List<MissionFileInfo> missions;

	public static readonly string defaultPackName = "___None___";

	public static readonly string defaultPackDisplayName = "#autoLOC_8005048";

	public MissionPack Pack => pack;

	public List<MissionFileInfo> Missions => missions;

	public void Awake()
	{
		title.text = base.name;
		missions = new List<MissionFileInfo>();
		buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
	}

	public void OnDestroy()
	{
		buttonCollapse.onClick.RemoveListener(OnButtonCollapsePressed);
	}

	public static MissionListGroup Create(MissionPack pack)
	{
		try
		{
			MissionListGroup component = UnityEngine.Object.Instantiate(MissionsUtils.MEPrefab("Prefabs/MissionListGroup.prefab")).GetComponent<MissionListGroup>();
			component.pack = pack;
			component.name = "MissionPack_" + pack.name;
			component.title.text = pack.displayName;
			if (!string.IsNullOrEmpty(pack.color))
			{
				component.SetHeaderColor(pack.color);
			}
			return component;
		}
		catch (Exception ex)
		{
			Debug.LogError("Could not create missions pack, reason: " + ex.Message);
		}
		return null;
	}

	public static MissionListGroup CreateDefault()
	{
		return Create(new MissionPack
		{
			name = defaultPackName,
			displayName = defaultPackDisplayName
		});
	}

	public void AddMission(MissionFileInfo newMission)
	{
		missions.Add(newMission);
	}

	public bool ContainsMission(MissionFileInfo mission)
	{
		return missions.Contains(mission);
	}

	public void CollapseGroup()
	{
		containerChildren.gameObject.SetActive(value: false);
		((Image)buttonCollapse.targetGraphic).sprite = spriteCollapseOn;
	}

	public void OnButtonCollapsePressed()
	{
		containerChildren.gameObject.SetActive(!containerChildren.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (containerChildren.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
	}

	public void SetHeaderColor(Color newColor)
	{
		if (headerImage != null)
		{
			headerImage.color = newColor;
		}
	}

	public void SetHeaderColor(string htmlColorCode)
	{
		try
		{
			ColorUtility.TryParseHtmlString(pack.color, out var color);
			SetHeaderColor(color);
		}
		catch (Exception)
		{
			Debug.LogErrorFormat("Unable to parse pack color for {0}", pack.name);
		}
	}
}
