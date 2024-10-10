using ns11;
using UnityEngine;

namespace ns36;

public class StagingLED : MonoBehaviour
{
	public GClass9 stagingLed;

	public void LateUpdate()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		if (InputLockManager.IsLocked(ControlTypes.STAGING))
		{
			if (stagingLed.currentColor != GClass9.colorIndices.purple)
			{
				stagingLed.SetColor(GClass9.colorIndices.purple);
				stagingLed.SetOn();
			}
			return;
		}
		stagingLed.SetColor(StageManager.CanSeparate ? GClass9.colorIndices.green : GClass9.colorIndices.yellow);
		if ((stagingLed.currentColor != GClass9.colorIndices.green || stagingLed.currentColor != GClass9.colorIndices.yellow) && !stagingLed.IsBlinking)
		{
			stagingLed.SetOn();
		}
		if (StageManager.CurrentStage == StageManager.StageCount && !stagingLed.IsBlinking)
		{
			stagingLed.Blink(0.3f);
		}
		if (StageManager.CurrentStage == 0)
		{
			stagingLed.setOff();
		}
		else if (StageManager.CanSeparate && StageManager.CurrentStage != StageManager.StageCount)
		{
			if (!stagingLed.IsOn)
			{
				stagingLed.SetOn();
			}
		}
		else if (!stagingLed.IsBlinking)
		{
			stagingLed.Blink(0.2f);
		}
	}
}
