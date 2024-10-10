using System.Collections.Generic;
using ns9;
using UnityEngine;

public class AudioFXSettings : DialogGUIVerticalLayout, ISettings
{
	public float masterVolume;

	public float shipVolume;

	public float ambienceVolume;

	public float musicVolume;

	public float voicesVolume;

	public float uiVolume;

	public bool normalizerEnabled;

	public float threshold;

	public float sharpness;

	public int skipsample;

	public void GetSettings()
	{
		masterVolume = GameSettings.MASTER_VOLUME;
		shipVolume = GameSettings.SHIP_VOLUME;
		ambienceVolume = GameSettings.AMBIENCE_VOLUME;
		musicVolume = GameSettings.MUSIC_VOLUME;
		uiVolume = GameSettings.UI_VOLUME;
		voicesVolume = GameSettings.VOICE_VOLUME;
		normalizerEnabled = GameSettings.SOUND_NORMALIZER_ENABLED;
		threshold = GameSettings.SOUND_NORMALIZER_THRESHOLD;
		sharpness = GameSettings.SOUND_NORMALIZER_RESPONSIVENESS;
	}

	public void DrawSettings()
	{
		padding = new RectOffset(8, 8, 8, 0);
		children.Add(new DialogGUIBox("VOLUME", -1f, 18f, null));
		children.Add(new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(), TextAnchor.UpperCenter));
		((DialogGUIVerticalLayout)children[children.Count - 1]).minHeight = -1f;
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Spacecraft: ", 150f), new DialogGUISlider(() => shipVolume, 0f, 1f, wholeNumbers: false, 200f, -1f, delegate(float f)
		{
			shipVolume = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => (shipVolume * 100f).ToString("0") + "%")));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Ambience: ", 150f), new DialogGUISlider(() => ambienceVolume, 0f, 1f, wholeNumbers: false, 200f, -1f, delegate(float f)
		{
			ambienceVolume = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => (ambienceVolume * 100f).ToString("0") + "%")));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("UI: ", 150f), new DialogGUISlider(() => uiVolume, 0f, 1f, wholeNumbers: false, 200f, -1f, delegate(float f)
		{
			uiVolume = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => (uiVolume * 100f).ToString("0") + "%")));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Music: ", 150f), new DialogGUISlider(() => musicVolume, 0f, 1f, wholeNumbers: false, 200f, -1f, delegate(float f)
		{
			musicVolume = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => (musicVolume * 100f).ToString("0") + "%")));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Voices: ", 150f), new DialogGUISlider(() => voicesVolume, 0f, 1f, wholeNumbers: false, 200f, -1f, delegate(float f)
		{
			voicesVolume = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => (voicesVolume * 100f).ToString("0") + "%")));
		children.Add(new DialogGUILayoutEnd());
		children.Add(new DialogGUIBox("SOUND NORMALIZER", -1f, 18f, null));
		children.Add(new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(), TextAnchor.UpperCenter));
		((DialogGUIVerticalLayout)children[children.Count - 1]).minHeight = -1f;
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Normalizer", 150f), new DialogGUIToggle(normalizerEnabled, () => (!normalizerEnabled) ? Localizer.Format("#autoLOC_6001071") : Localizer.Format("#autoLOC_6001072"), delegate(bool b)
		{
			normalizerEnabled = b;
		}, 200f)));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Threshold ", 150f), new DialogGUISlider(() => threshold, 0f, 5f, wholeNumbers: true, 200f, -1f, delegate(float f)
		{
			threshold = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => KSPUtil.LocalizeNumber(threshold, "0.0"))));
		children.Add(new DialogGUIHorizontalLayout(0f, 18f, 0f, new RectOffset(), TextAnchor.MiddleCenter, new DialogGUILabel("Responsiveness ", 150f), new DialogGUISlider(() => sharpness, 0f, 32f, wholeNumbers: true, 200f, -1f, delegate(float f)
		{
			sharpness = f;
		}), new DialogGUISpace(50f), new DialogGUILabel(() => sharpness.ToString("0"))));
		children.Add(new DialogGUILayoutEnd());
		children.Add(new DialogGUIFlexibleSpace());
	}

	public DialogGUIBase[] DrawMiniSettings()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		list.Add(new DialogGUIBox(Localizer.Format("#autoLOC_146152"), -1f, 18f, null));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146154"), 100f), new DialogGUISlider(() => masterVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			masterVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (masterVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146161"), 100f), new DialogGUISlider(() => shipVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			shipVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (shipVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146168"), 100f), new DialogGUISlider(() => ambienceVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			ambienceVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (ambienceVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146175"), 100f), new DialogGUISlider(() => uiVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			uiVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (uiVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146182"), 100f), new DialogGUISlider(() => musicVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			musicVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (musicVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		list.Add(new DialogGUIHorizontalLayout(TextAnchor.MiddleLeft, new DialogGUILabel(Localizer.Format("#autoLOC_146189"), 100f), new DialogGUISlider(() => voicesVolume, 0f, 1f, wholeNumbers: false, 150f, 18f, delegate(float f)
		{
			voicesVolume = f;
		}), new DialogGUISpace(30f), new DialogGUILabel(() => (voicesVolume * 100f).ToString("0") + "%"), new DialogGUIFlexibleSpace()));
		return list.ToArray();
	}

	public void ApplySettings()
	{
		GameSettings.MASTER_VOLUME = masterVolume;
		GameSettings.SHIP_VOLUME = shipVolume;
		GameSettings.AMBIENCE_VOLUME = ambienceVolume;
		GameSettings.MUSIC_VOLUME = musicVolume;
		GameSettings.UI_VOLUME = uiVolume;
		GameSettings.VOICE_VOLUME = voicesVolume;
		AudioListener.volume = GameSettings.MASTER_VOLUME;
		MusicLogic.SetVolume(musicVolume, musicVolume);
		GameSettings.SOUND_NORMALIZER_ENABLED = normalizerEnabled;
		GameSettings.SOUND_NORMALIZER_THRESHOLD = threshold;
		GameSettings.SOUND_NORMALIZER_RESPONSIVENESS = sharpness;
		GameSettings.SOUND_NORMALIZER_SKIPSAMPLES = skipsample;
	}

	public string GetName()
	{
		return Localizer.Format("#autoLOC_146223");
	}

	public new void OnUpdate()
	{
		Update();
	}
}
