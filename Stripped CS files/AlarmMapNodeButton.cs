using ns11;
using UnityEngine;
using UnityEngine.UI;

public class AlarmMapNodeButton : MonoBehaviour
{
	public MapObject mapObject;

	[SerializeField]
	public Button addAlarmButton;

	public bool hover;

	public bool Hover => hover;

	public void OnPointerEnter()
	{
		mapObject.uiNode.addAlarmHover = true;
		hover = true;
	}

	public void OnPointerExit()
	{
		mapObject.uiNode.addAlarmHover = false;
		mapObject.uiNode.OnAddAlarmPointerExit();
		hover = false;
	}

	public void Awake()
	{
		addAlarmButton.onClick.AddListener(OnAddAlarm);
	}

	public void OnDestroy()
	{
		addAlarmButton.onClick.RemoveListener(OnAddAlarm);
	}

	public void OnAddAlarm()
	{
		if (!AlarmClockScenario.MapNodeDefined(mapObject.type))
		{
			return;
		}
		AlarmTypeBase alarmTypeBase = AlarmClockScenario.CreateAlarmByMapNodeType(mapObject.type);
		if (alarmTypeBase != null)
		{
			if (mapObject.vesselRef != null)
			{
				alarmTypeBase.vesselId = mapObject.vesselRef.persistentId;
			}
			if (alarmTypeBase.InitializeFromMapObject(mapObject))
			{
				alarmTypeBase.OnScenarioUpdate();
				if (AlarmClockScenario.AddAlarm(alarmTypeBase))
				{
					AlarmClockApp.EditAlarm(alarmTypeBase);
				}
			}
		}
		OnPointerExit();
	}
}
