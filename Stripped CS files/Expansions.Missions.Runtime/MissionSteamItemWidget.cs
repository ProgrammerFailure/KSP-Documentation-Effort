using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionSteamItemWidget : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI headertext;

	[SerializeField]
	public TextMeshProUGUI headerHilighttext;

	[SerializeField]
	public RawImage steamSubIcon;

	[SerializeField]
	public Texture2D steamSubscribedIcon;

	[SerializeField]
	public Texture2D steamUnSubscribedIcon;

	[SerializeField]
	public Toggle toggle;

	public RawImage missionIcon;

	public TextMeshProUGUI favoritetext;

	public TextMeshProUGUI thumbsUptext;

	public TextMeshProUGUI thumbsDowntext;

	public Callback<bool> onSelected;

	public static MissionSteamItemWidget Create(Callback<bool> selected)
	{
		MissionSteamItemWidget component = Object.Instantiate(MissionsUtils.MEPrefab("_UI5/Dialogs/MissionPlayDialog/prefabs/MissionSteamItemWidget.prefab")).GetComponent<MissionSteamItemWidget>();
		component.onSelected = selected;
		return component;
	}

	public void Start()
	{
		toggle.onValueChanged.AddListener(delegate(bool b)
		{
			onSelected(b);
		});
	}

	public void SetHeaderText(string text)
	{
		headertext.text = text;
		headerHilighttext.text = text;
	}

	public void SetSubIconState(bool subscribed)
	{
		if (subscribed)
		{
			steamSubIcon.texture = steamSubscribedIcon;
			steamSubIcon.color = XKCDColors.KSPBadassGreen;
		}
		else
		{
			steamSubIcon.texture = steamUnSubscribedIcon;
			steamSubIcon.color = XKCDColors.KSPUIGrey;
		}
	}
}
