using ns9;
using TMPro;
using UnityEngine;

namespace ns36;

public class METDisplay : MonoBehaviour
{
	public TextMeshProUGUI text;

	public TextMeshProUGUI buttonText;

	public bool displayUT;

	public bool gamePaused;

	public static string cacheMETDatePortion = "";

	public string currentMETDatePortion = "";

	public static string cacheAutoLOC_7003242;

	public void Reset()
	{
		text = GetComponent<TextMeshProUGUI>();
	}

	public void Start()
	{
		cacheMETDatePortion = null;
		GameEvents.onGamePause.Add(onGamePause);
		GameEvents.onGameUnpause.Add(onGameUnPause);
	}

	public void OnDestroy()
	{
		GameEvents.onGamePause.Remove(onGamePause);
		GameEvents.onGameUnpause.Remove(onGameUnPause);
	}

	public void LateUpdate()
	{
		if (!FlightGlobals.ready || (object)text == null)
		{
			return;
		}
		if (displayUT)
		{
			text.text = KSPUtil.PrintDateCompact(Planetarium.GetUniversalTime(), includeTime: true, includeSeconds: true);
		}
		else
		{
			currentMETDatePortion = KSPUtil.PrintTimeStampCompact(FlightLogger.met, days: true, years: true);
			if (currentMETDatePortion != cacheMETDatePortion)
			{
				text.text = cacheAutoLOC_7003242 + " " + currentMETDatePortion;
				cacheMETDatePortion = currentMETDatePortion;
			}
		}
		if (!gamePaused)
		{
			text.color = ((Time.deltaTime < Time.maximumDeltaTime * 1f) ? Color.green : ((Time.deltaTime < Time.maximumDeltaTime * 1.3f) ? Color.yellow : Color.red));
		}
	}

	public void ToggleTimeMode()
	{
		displayUT = !displayUT;
		cacheMETDatePortion = null;
		if ((object)buttonText != null)
		{
			buttonText.text = (displayUT ? "#autoLOC_6001498" : "#autoLOC_900615");
		}
	}

	public void SetTimeMode(bool displayUT)
	{
		this.displayUT = displayUT;
		cacheMETDatePortion = null;
		if ((object)buttonText != null)
		{
			buttonText.text = (displayUT ? "#autoLOC_6001498" : "#autoLOC_900615");
		}
	}

	public void onGamePause()
	{
		text.color = new Color(1f, 1f, 1f, 0.4375f);
		gamePaused = true;
	}

	public void onGameUnPause()
	{
		text.color = Color.green;
		gamePaused = false;
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_7003242 = Localizer.Format("#autoLOC_7003242");
	}
}
