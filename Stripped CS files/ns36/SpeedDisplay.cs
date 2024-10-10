using System;
using System.Text;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class SpeedDisplay : MonoBehaviour
{
	public TextMeshProUGUI textTitle;

	public TextMeshProUGUI textSpeed;

	public Button button;

	public static double speedMultiplier = 1.0;

	public static string units = "m/s";

	public static string format = "0.0";

	public static SpeedDisplay Instance { get; set; }

	public void Awake()
	{
		Instance = this;
		units = Localizer.Format("#autoLOC_180095");
		button.onClick.AddListener(OnChangeMode);
		GameEvents.onSetSpeedMode.Add(OnSetSpeedMode);
	}

	public void Start()
	{
		OnSetSpeedMode(FlightGlobals.speedDisplayMode);
	}

	public void OnDestroy()
	{
		GameEvents.onSetSpeedMode.Remove(OnSetSpeedMode);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void LateUpdate()
	{
		if (FlightGlobals.ready)
		{
			StringBuilder stringBuilder = StringBuilderCache.Acquire();
			double value = Math.Round(FlightGlobals.GetDisplaySpeed() * speedMultiplier, 1);
			stringBuilder.Append(KSPUtil.LocalizeNumber(value, format) + units);
			textSpeed.text = stringBuilder.ToStringAndRelease();
		}
	}

	public void OnChangeMode()
	{
		FlightGlobals.CycleSpeedModes();
	}

	public void OnSetSpeedMode(FlightGlobals.SpeedDisplayModes mode)
	{
		textTitle.text = mode.displayDescription();
		switch (mode)
		{
		case FlightGlobals.SpeedDisplayModes.Orbit:
			textTitle.alignment = TextAlignmentOptions.MidlineRight;
			break;
		case FlightGlobals.SpeedDisplayModes.Surface:
			textTitle.alignment = TextAlignmentOptions.MidlineLeft;
			break;
		case FlightGlobals.SpeedDisplayModes.Target:
			textTitle.alignment = TextAlignmentOptions.Midline;
			break;
		}
	}
}
