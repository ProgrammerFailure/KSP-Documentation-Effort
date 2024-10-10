using TMPro;
using UnityEngine;

namespace ns2;

public class UIPlanetariumDateTime : MonoBehaviour
{
	public TextMeshProUGUI textDate;

	public bool showTime = true;

	public bool showSeconds;

	public void Start()
	{
		GameEvents.onGamePause.Add(onGamePause);
		GameEvents.onGameUnpause.Add(onGameUnPause);
		if (FlightDriver.Pause)
		{
			onGamePause();
		}
		SetTime();
	}

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(onGamePause);
		GameEvents.onGameUnpause.Remove(onGameUnPause);
	}

	public void Update()
	{
		SetTime();
	}

	public void SetTime()
	{
		if (!(Planetarium.fetch == null))
		{
			textDate.text = KSPUtil.PrintDate(Planetarium.GetUniversalTime(), showTime, showSeconds);
		}
	}

	public void onGamePause()
	{
		textDate.color = new Color(1f, 1f, 1f, 0.4375f);
	}

	public void onGameUnPause()
	{
		textDate.color = Color.green;
	}
}
