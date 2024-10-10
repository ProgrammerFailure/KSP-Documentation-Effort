using ns9;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ns27;

public class MaxProgression : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (!(ProgressTracking.Instance == null))
		{
			PointerEventData.InputButton button = eventData.button;
			if (button == PointerEventData.InputButton.Right)
			{
				ProgressTracking.Instance.CheatEarlyProgression();
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001914", FlightGlobals.GetHomeBodyDisplayName()), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			else
			{
				ProgressTracking.Instance.CheatProgression();
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6001915"), 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
	}
}
