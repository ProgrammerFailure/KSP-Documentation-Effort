using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIConfirmDialog : MonoBehaviour
{
	[SerializeField]
	[Header("UI Components")]
	public TextMeshProUGUI textHeader;

	[SerializeField]
	public TextMeshProUGUI textDescription;

	[SerializeField]
	public TextMeshProUGUI textCancel;

	[SerializeField]
	public Button buttonCancel;

	[SerializeField]
	public TextMeshProUGUI textConfirmation;

	[SerializeField]
	public Button buttonConfirmation;

	[SerializeField]
	public TextMeshProUGUI textDontShowAgain;

	[SerializeField]
	public Toggle toggleDontShowAgain;

	public bool modal = true;

	public CanvasGroup canvasGroup;

	public Callback<bool> onOk;

	public Callback<bool> onCancel;

	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, Callback<bool> onCancel, bool showCancelBtn = true)
	{
		UIConfirmDialog component = Object.Instantiate(AssetBase.GetPrefab("UIConfirmDialog")).GetComponent<UIConfirmDialog>();
		component.transform.SetParent(PopupDialogController.PopupDialogCanvas.transform, worldPositionStays: true);
		component.transform.localScale = Vector3.one;
		component.transform.localPosition = Vector3.zero;
		component.textHeader.text = title;
		component.textDescription.text = message;
		component.onOk = onOk;
		component.onCancel = onCancel;
		if (!showCancelBtn)
		{
			component.buttonCancel.gameObject.SetActive(value: false);
		}
		return component;
	}

	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, bool showCancelBtn = true)
	{
		return Spawn(title, message, onOk, null, showCancelBtn);
	}

	public static UIConfirmDialog Spawn(string title, string message, Callback<bool> onOk, bool showDontShowAgain, string textDontShowAgain)
	{
		UIConfirmDialog uIConfirmDialog = Spawn(title, message, onOk, null, showCancelBtn: false);
		uIConfirmDialog.textDontShowAgain.gameObject.SetActive(showDontShowAgain);
		uIConfirmDialog.toggleDontShowAgain.gameObject.SetActive(showDontShowAgain);
		if (showDontShowAgain)
		{
			uIConfirmDialog.textDontShowAgain.text = textDontShowAgain;
		}
		return uIConfirmDialog;
	}

	public static UIConfirmDialog Spawn(string title, string message, string textCancel, string textOK, string textDontShowAgain, Callback<bool> onOk, Callback<bool> onCancel, bool showCancelBtn = true)
	{
		UIConfirmDialog uIConfirmDialog = Spawn(title, message, onOk, onCancel, showCancelBtn);
		uIConfirmDialog.textCancel.text = textCancel;
		uIConfirmDialog.textConfirmation.text = textOK;
		uIConfirmDialog.textDontShowAgain.text = textDontShowAgain;
		return uIConfirmDialog;
	}

	public void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			if (onCancel != null)
			{
				OnCancel();
			}
			else if (onOk != null)
			{
				OnConfirm();
			}
		}
	}

	public void Start()
	{
		buttonConfirmation.onClick.AddListener(delegate
		{
			OnConfirm();
		});
		buttonCancel.onClick.AddListener(delegate
		{
			OnCancel();
		});
		canvasGroup = GetComponent<CanvasGroup>();
		if (modal)
		{
			UIMasterController.Instance.RegisterModalDialog(canvasGroup);
		}
		else
		{
			UIMasterController.Instance.RegisterNonModalDialog(canvasGroup);
		}
		GameEvents.onSceneConfirmExit.Add(OnLeavingScene);
	}

	public void OnConfirm()
	{
		if (onOk != null)
		{
			bool arg = false;
			if (toggleDontShowAgain != null)
			{
				arg = toggleDontShowAgain.isOn;
			}
			onOk(arg);
		}
		CloseDialog();
	}

	public void OnCancel()
	{
		if (onCancel != null)
		{
			bool arg = false;
			if (toggleDontShowAgain != null)
			{
				arg = toggleDontShowAgain.isOn;
			}
			onCancel(arg);
		}
		CloseDialog();
	}

	public void CloseDialog()
	{
		Object.Destroy(base.gameObject);
	}

	public void OnLeavingScene(GameScenes scn)
	{
		CloseDialog();
	}

	public void OnDestroy()
	{
		if (modal)
		{
			UIMasterController.Instance.UnregisterModalDialog(canvasGroup);
		}
		else
		{
			UIMasterController.Instance.UnregisterNonModalDialog(canvasGroup);
		}
		buttonConfirmation.onClick.RemoveListener(delegate
		{
			OnConfirm();
		});
		buttonCancel.onClick.RemoveListener(delegate
		{
			OnCancel();
		});
		GameEvents.onSceneConfirmExit.Remove(OnLeavingScene);
	}
}
