using System.Runtime.CompilerServices;
using Expansions.Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Mission;

public class ScreenMissionExistingItem : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI stateText;

	public TextMeshProUGUI prestigeText;

	public Button leftButton;

	public TextMeshProUGUI leftButtonText;

	public Button rightButton;

	public TextMeshProUGUI rightButtonText;

	public TextMeshProUGUI errorText;

	public Expansions.Missions.Mission mission
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenMissionExistingItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Expansions.Missions.Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupError(string errorText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeftButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRightButtonClicked()
	{
		throw null;
	}
}
