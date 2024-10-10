using ns12;
using ns9;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

public class UITimeWarpController : MonoBehaviour
{
	public UIStateImage stateImageHigh;

	public string[] stateTextHigh = new string[8];

	public UIStateImage stateImageLow;

	public string[] stateTextLow = new string[8];

	public Button[] buttons = new Button[8];

	public TooltipController_Text[] tooltips;

	public bool setInstant;

	public void Start()
	{
		tooltips = new TooltipController_Text[8];
		for (int i = 0; i < 8; i++)
		{
			tooltips[i] = buttons[i].GetComponent<TooltipController_Text>();
			tooltips[i].textString = Localizer.Format(stateTextHigh[i]);
		}
		buttons[0].onClick.AddListener(ClickTimewarp0);
		buttons[1].onClick.AddListener(ClickTimewarp1);
		buttons[2].onClick.AddListener(ClickTimewarp2);
		buttons[3].onClick.AddListener(ClickTimewarp3);
		buttons[4].onClick.AddListener(ClickTimewarp4);
		buttons[5].onClick.AddListener(ClickTimewarp5);
		buttons[6].onClick.AddListener(ClickTimewarp6);
		buttons[7].onClick.AddListener(ClickTimewarp7);
		GameEvents.onTimeWarpRateChanged.Add(OnTimeWarpRateChanged);
		OnTimeWarpRateChanged();
	}

	public void OnDestroy()
	{
		GameEvents.onTimeWarpRateChanged.Remove(OnTimeWarpRateChanged);
	}

	public void ClickTimewarp0()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(0, setInstant);
		}
	}

	public void ClickTimewarp1()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(1, setInstant);
		}
	}

	public void ClickTimewarp2()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(2, setInstant);
		}
	}

	public void ClickTimewarp3()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(3, setInstant);
		}
	}

	public void ClickTimewarp4()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(4, setInstant);
		}
	}

	public void ClickTimewarp5()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(5, setInstant);
		}
	}

	public void ClickTimewarp6()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(6, setInstant);
		}
	}

	public void ClickTimewarp7()
	{
		if (CanSetHighRate())
		{
			TimeWarp.fetch.CancelAutoWarp();
			TimeWarp.SetRate(7, setInstant);
		}
	}

	public bool CanSetHighRate()
	{
		if (InputLockManager.IsUnlocked(ControlTypes.TIMEWARP))
		{
			if (!HighLogic.CurrentGame.Parameters.Flight.CanTimeWarpHigh)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_481056"));
				return false;
			}
			return true;
		}
		return false;
	}

	public void OnTimeWarpRateChanged()
	{
		if (TimeWarp.WarpMode == TimeWarp.Modes.HIGH)
		{
			stateImageHigh.gameObject.SetActive(value: true);
			if (stateImageLow != null)
			{
				stateImageLow.gameObject.SetActive(value: false);
			}
			int i = 0;
			for (int num = stateTextHigh.Length; i < num; i++)
			{
				stateImageHigh.SetState(Mathf.Clamp(TimeWarp.CurrentRateIndex, 0, stateImageHigh.states.Length - 1));
			}
			for (int j = 0; j < 8; j++)
			{
				tooltips[j].textString = Localizer.Format(stateTextHigh[j]);
			}
		}
		else
		{
			stateImageHigh.gameObject.SetActive(value: false);
			if (stateImageLow != null)
			{
				stateImageLow.gameObject.SetActive(value: true);
				stateImageLow.SetState(Mathf.Clamp(TimeWarp.CurrentRateIndex, 0, stateImageLow.states.Length - 1));
			}
			for (int k = 0; k < 8; k++)
			{
				tooltips[k].textString = Localizer.Format(stateTextLow[k]);
			}
		}
	}
}
