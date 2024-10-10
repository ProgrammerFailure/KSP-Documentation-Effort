using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmClockMessageDialog : MonoBehaviour
{
	[Header("UI Components")]
	[SerializeField]
	public TextMeshProUGUI textHeader;

	[SerializeField]
	public TextMeshProUGUI textDate;

	[SerializeField]
	public TextMeshProUGUI textDescription;

	[SerializeField]
	public TextMeshProUGUI textWarpState;

	[SerializeField]
	public Button deleteOnClose;

	[SerializeField]
	public Button jumpButton;

	[SerializeField]
	public Button closeButton;

	public AlarmTypeBase alarm;

	public Callback onClose;

	public bool isClosing;

	public static AlarmClockMessageDialog Spawn(AlarmTypeBase alarm, Callback onClose = null)
	{
		AlarmClockMessageDialog component = Object.Instantiate(AlarmClockScenario.MessagePrefab.gameObject).GetComponent<AlarmClockMessageDialog>();
		component.alarm = alarm;
		component.transform.SetParent(PopupDialogController.PopupDialogCanvas.transform, worldPositionStays: true);
		component.transform.localScale = Vector3.one;
		component.transform.localPosition = Vector3.zero;
		component.textHeader.text = alarm.title;
		component.textDate.text = KSPUtil.PrintDate(alarm.ut, includeTime: true, includeSeconds: true);
		component.textWarpState.gameObject.SetActive(alarm.actions.warp != AlarmActions.WarpEnum.DoNothing);
		if (alarm.PauseGame)
		{
			component.textWarpState.text = Localizer.Format("#autoLOC_8003533");
		}
		else if (alarm.HaltWarp)
		{
			component.textWarpState.text = Localizer.Format("#autoLOC_8003534");
		}
		component.textDescription.text = alarm.description;
		component.closeButton.gameObject.SetActive(!alarm.actions.deleteWhenDone);
		component.jumpButton.gameObject.SetActive(alarm.vesselId != 0 && FlightGlobals.PersistentVesselIds.Contains(alarm.vesselId) && (FlightGlobals.ActiveVessel == null || FlightGlobals.ActiveVessel.persistentId != alarm.vesselId));
		component.onClose = onClose;
		return component;
	}

	public void Awake()
	{
	}

	public void Start()
	{
		closeButton.onClick.AddListener(OnClose);
		deleteOnClose.onClick.AddListener(OnDeleteOnClose);
		jumpButton.onClick.AddListener(OnJump);
		GameEvents.onAlarmRemoved.Add(OnAlarmRemoved);
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			OnClose();
		}
	}

	public void OnDestroy()
	{
		GameEvents.onAlarmRemoved.Remove(OnAlarmRemoved);
		closeButton.onClick.RemoveListener(OnClose);
		deleteOnClose.onClick.RemoveListener(OnDeleteOnClose);
		jumpButton.onClick.RemoveListener(OnJump);
	}

	public void OnDeleteOnClose()
	{
		alarm.actions.deleteWhenDone = true;
		OnClose();
	}

	public void OnJump()
	{
		if (alarm.Vessel == null)
		{
			Debug.LogError("[AlarmClockMessageDialog]: Vessel reference is null, Unable to switch");
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			FlightGlobals.SetActiveVessel(alarm.Vessel);
			return;
		}
		GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.OVERWRITE, GameScenes.FLIGHT);
		FlightDriver.StartAndFocusVessel("persistent", FlightGlobals.Vessels.IndexOf(alarm.Vessel));
	}

	public void OnClose()
	{
		if (onClose != null)
		{
			isClosing = true;
			onClose();
		}
		CloseDialog();
	}

	public void CloseDialog()
	{
		if (base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void OnAlarmRemoved(uint alarmID)
	{
		if (!isClosing && alarm.Id == alarmID)
		{
			OnClose();
		}
	}
}
