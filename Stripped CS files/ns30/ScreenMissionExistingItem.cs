using Expansions.Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns30;

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

	public Mission mission { get; set; }

	public void Start()
	{
		leftButton.onClick.AddListener(OnLeftButtonClicked);
		rightButton.onClick.AddListener(OnRightButtonClicked);
	}

	public void Setup(Mission mission)
	{
		if (!(mission == null))
		{
			this.mission = mission;
			titleText.text = mission.title;
		}
	}

	public void SetupError(string errorText)
	{
		mission = null;
		titleText.text = "<color=red>" + errorText + "</color>";
		titleText.enableWordWrapping = false;
		titleText.overflowMode = TextOverflowModes.Overflow;
		stateText.text = string.Empty;
		prestigeText.text = string.Empty;
		leftButton.gameObject.SetActive(value: false);
		rightButton.gameObject.SetActive(value: false);
	}

	public void OnLeftButtonClicked()
	{
		_ = mission == null;
	}

	public void OnRightButtonClicked()
	{
		_ = mission == null;
	}
}
