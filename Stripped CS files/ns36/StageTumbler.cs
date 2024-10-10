using ns11;
using UnityEngine;

namespace ns36;

public class StageTumbler : MonoBehaviour
{
	public ns11.Tumbler tumbler;

	public void Awake()
	{
		SetTumblerPosition();
	}

	public void SetTumblerPosition()
	{
		base.transform.localPosition = new Vector3(base.transform.localPosition.x, base.transform.localPosition.y, -1500f + 1222f * GameSettings.UI_SCALE - 320f * GameSettings.UI_SCALE * GameSettings.UI_SCALE);
	}

	public void Reset()
	{
		tumbler = GetComponent<ns11.Tumbler>();
	}

	public void LateUpdate()
	{
		if (FlightGlobals.ready && tumbler != null)
		{
			tumbler.SetValue(Mathf.Clamp(StageManager.CurrentStage, 0, StageManager.LastStage));
		}
	}
}
