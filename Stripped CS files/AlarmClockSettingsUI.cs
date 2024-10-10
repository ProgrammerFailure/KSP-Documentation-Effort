using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;

public class AlarmClockSettingsUI : DialogGUIVerticalLayout, ISettings
{
	public double defaultRawTime = 300.0;

	public double defaultManeuverMargin = 60.0;

	public string soundName = "";

	public int soundRepeats = 3;

	public List<string> soundNames;

	public int selectedSound;

	public string defaultSoundName = "Classic";

	public void GetSettings()
	{
		if (!(AlarmClockScenario.Instance == null))
		{
			defaultRawTime = AlarmClockScenario.Instance.settings.defaultRawTime;
			defaultManeuverMargin = AlarmClockScenario.Instance.settings.defaultMapNodeMargin;
			soundName = AlarmClockScenario.Instance.settings.soundName;
			soundRepeats = AlarmClockScenario.Instance.settings.soundRepeats;
			soundNames = AlarmClockScenario.AudioController.GetStockAlarmSounds();
			selectedSound = soundNames.IndexOf(soundName);
			if (selectedSound < 0)
			{
				selectedSound = soundNames.IndexOf(defaultSoundName);
			}
			if (selectedSound > -1)
			{
				soundName = soundNames[selectedSound];
			}
		}
	}

	public DialogGUIBase[] DrawMiniSettings()
	{
		TextAnchor achr = TextAnchor.MiddleLeft;
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		if (AlarmClockScenario.Instance == null)
		{
			return list.ToArray();
		}
		list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_8003578"), -1f, 18f, null));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003579"), 150f), new DialogGUISlider(() => (float)defaultRawTime, 30f, 600f, wholeNumbers: true, 120f, 20f, delegate(float f)
		{
			defaultRawTime = f;
		}), new DialogGUISpace(10f), new DialogGUILabel(() => AlarmClockUIFrame.PrintTimeStampCompact(defaultRawTime, days: false, years: false, timeAsDateTime: true), 80f)));
		list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003580"), 150f), new DialogGUISlider(() => (float)defaultManeuverMargin, 30f, 180f, wholeNumbers: true, 120f, 20f, delegate(float f)
		{
			defaultManeuverMargin = f;
		}), new DialogGUISpace(10f), new DialogGUILabel(() => AlarmClockUIFrame.PrintTimeStampCompact(defaultManeuverMargin, days: false, years: false, timeAsDateTime: true), 80f)));
		if (soundNames != null && soundNames.Count > 0)
		{
			if (soundNames.Count > 1)
			{
				list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003581"), 150f), new DialogGUISlider(() => selectedSound, 0f, soundNames.Count - 1, wholeNumbers: true, 120f, 20f, delegate(float f)
				{
					selectedSound = (int)f;
					soundName = soundNames[selectedSound];
				})));
				list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUISpace(160f), new DialogGUILabel(() => soundName, 280f)));
			}
			else
			{
				list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003581"), 150f), new DialogGUILabel(() => soundName, 280f)));
			}
			list.Add(new DialogGUIHorizontalLayout(achr, new DialogGUILabel(() => Localizer.Format("#autoLOC_8003582"), 150f), new DialogGUISlider(() => soundRepeats, 1f, 10f, wholeNumbers: true, 120f, 20f, delegate(float f)
			{
				soundRepeats = (int)f;
			}), new DialogGUISpace(10f), new DialogGUILabel(() => soundRepeats.ToString(), 80f)));
		}
		return list.ToArray();
	}

	public void ApplySettings()
	{
		if (!(AlarmClockScenario.Instance == null))
		{
			AlarmClockScenario.Instance.settings.defaultRawTime = defaultRawTime;
			AlarmClockScenario.Instance.settings.defaultMapNodeMargin = defaultManeuverMargin;
			AlarmClockScenario.Instance.settings.soundName = soundName;
			AlarmClockScenario.Instance.settings.soundRepeats = soundRepeats;
		}
	}

	public string GetName()
	{
		return "Alarm Clock";
	}

	public new void OnUpdate()
	{
		Update();
	}

	public void DrawSettings()
	{
	}
}
