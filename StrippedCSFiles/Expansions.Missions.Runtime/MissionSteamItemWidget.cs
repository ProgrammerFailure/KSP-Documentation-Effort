using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Runtime;

public class MissionSteamItemWidget : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI headertext;

	[SerializeField]
	private TextMeshProUGUI headerHilighttext;

	[SerializeField]
	private RawImage steamSubIcon;

	[SerializeField]
	private Texture2D steamSubscribedIcon;

	[SerializeField]
	private Texture2D steamUnSubscribedIcon;

	[SerializeField]
	private Toggle toggle;

	public RawImage missionIcon;

	public TextMeshProUGUI favoritetext;

	public TextMeshProUGUI thumbsUptext;

	public TextMeshProUGUI thumbsDowntext;

	private Callback<bool> onSelected;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MissionSteamItemWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MissionSteamItemWidget Create(Callback<bool> selected)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHeaderText(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSubIconState(bool subscribed)
	{
		throw null;
	}
}
