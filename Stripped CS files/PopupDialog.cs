using System.Collections.Generic;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupDialog : MonoBehaviour
{
	public RectTransform rTrf;

	public MultiOptionDialog dialogToDisplay;

	public string defaultSkinName = "MainMenuSkin";

	public GameObject popupWindow;

	public DragPanel dragPanel;

	public Callback OnDismiss;

	public bool hover;

	public static List<PopupDialog> instantiatedPopUps = new List<PopupDialog>();

	public bool modal = true;

	public List<GameObject> childItems = new List<GameObject>();

	public Vector2 anchoredPosition = Vector2.zero;

	public Vector2 prevSize = Vector2.zero;

	public CanvasGroup canvasGroup;

	public UnityEvent onDestroy = new UnityEvent();

	public bool Hover => hover;

	public RectTransform RTrf => rTrf;

	public bool IsDraggable => dragPanel.enabled;

	public static void ClearPopUps()
	{
		foreach (PopupDialog instantiatedPopUp in instantiatedPopUps)
		{
			if (instantiatedPopUp != null)
			{
				Object.Destroy(instantiatedPopUp.gameObject);
			}
		}
		instantiatedPopUps.Clear();
	}

	public static void DismissPopup(string name)
	{
		GameObject gameObject = GameObject.Find(name + " dialog handler");
		if (gameObject != null)
		{
			gameObject.GetComponent<PopupDialog>().Dismiss();
		}
	}

	public static PopupDialog SpawnPopupDialog(Vector2 anchorMin, Vector2 anchorMax, string dialogName, string title, string message, string buttonMessage, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		return SpawnPopupDialog(anchorMin, anchorMax, new MultiOptionDialog(dialogName, message, title, skin, new DialogGUIButton(buttonMessage, delegate
		{
		}, dismissOnSelect: true)), persistAcrossScenes, skin, isModal, titleExtra);
	}

	public static PopupDialog SpawnPopupDialog(Vector2 anchorMin, Vector2 anchorMax, MultiOptionDialog dialog, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		PopupDialog popupDialog = SpawnPopupDialog(dialog, persistAcrossScenes, skin, isModal, titleExtra);
		popupDialog.GetComponent<RectTransform>().anchorMin = anchorMin;
		popupDialog.GetComponent<RectTransform>().anchorMax = anchorMax;
		popupDialog.transform.localScale = Vector3.one;
		popupDialog.transform.localPosition = Vector3.zero;
		popupDialog.modal = isModal;
		if (!persistAcrossScenes)
		{
			instantiatedPopUps.Add(popupDialog);
		}
		return popupDialog;
	}

	public static PopupDialog SpawnPopupDialog(MultiOptionDialog dialog, bool persistAcrossScenes, UISkinDef skin, bool isModal = true, string titleExtra = "")
	{
		GameObject gameObject = GameObject.Find(dialog.name + " dialog handler" + titleExtra);
		if (gameObject != null)
		{
			PopupDialog component = gameObject.GetComponent<PopupDialog>();
			if (component != null)
			{
				component.Dismiss();
			}
			else
			{
				Object.Destroy(gameObject);
			}
		}
		PopupDialog popupDialog = Object.Instantiate(PopupDialogController.PopupDialogBase);
		popupDialog.transform.SetParent(PopupDialogController.PopupDialogCanvas.transform, worldPositionStays: false);
		popupDialog.transform.localScale = Vector3.one;
		popupDialog.transform.localPosition = Vector3.zero;
		popupDialog.anchoredPosition = dialog.dialogRect.position;
		popupDialog.modal = isModal;
		UIMasterController.ClampToScreen((RectTransform)popupDialog.transform, Vector2.one * 30f);
		popupDialog.dragPanel = popupDialog.GetComponent<DragPanel>();
		popupDialog.rTrf = popupDialog.GetComponent<RectTransform>();
		popupDialog.SetPopupData(dialog, (skin == null) ? HighLogic.UISkin : skin);
		popupDialog.gameObject.name = dialog.name + " dialog handler" + titleExtra;
		popupDialog.dialogToDisplay = dialog;
		popupDialog.dialogToDisplay.id = popupDialog.GetInstanceID();
		if (persistAcrossScenes)
		{
			Object.DontDestroyOnLoad(popupDialog.gameObject);
		}
		else
		{
			instantiatedPopUps.Add(popupDialog);
		}
		popupDialog.gameObject.SetActive(value: true);
		popupDialog.dialogToDisplay.Update();
		popupDialog.dialogToDisplay.Resize();
		popupDialog.transform.localScale = Vector3.one;
		popupDialog.transform.localPosition = Vector3.zero;
		return popupDialog;
	}

	public void SetPopupData(MultiOptionDialog dialog, UISkinDef skin)
	{
		Stack<Transform> layouts = new Stack<Transform>();
		layouts.Push(popupWindow.transform);
		popupWindow.SetActive(value: true);
		popupWindow.GetComponent<Image>().sprite = skin.window.normal.background;
		DialogGUIBase.SetUpTextObject(popupWindow.GetChild("Title").GetComponent<TextMeshProUGUI>(), dialog.title, skin.window, skin);
		if (!string.IsNullOrEmpty(dialog.message))
		{
			GameObject gameObject = Object.Instantiate(UISkinManager.GetPrefab("UITextPrefab"));
			gameObject.SetActive(value: true);
			gameObject.transform.SetParent(layouts.Peek(), worldPositionStays: false);
			gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
			DialogGUIBase.SetUpTextObject(gameObject.GetComponent<TextMeshProUGUI>(), dialog.message, skin.label, skin);
			childItems.Add(gameObject);
		}
		dialog.Create(ref layouts, skin);
		popupWindow.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dialog.dialogRect.width);
		popupWindow.AddComponent<LayoutElement>().minHeight = dialog.dialogRect.height;
	}

	public static bool CheckForOpenDialogs()
	{
		bool result = false;
		for (int i = 0; i < instantiatedPopUps.Count; i++)
		{
			if ((bool)instantiatedPopUps[i] && instantiatedPopUps[i].gameObject.activeInHierarchy)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	public void OnRectTransformDimensionsChange()
	{
		RectTransform component = popupWindow.GetComponent<RectTransform>();
		component.pivot = component.anchorMin;
		if (GetComponentInParent<CanvasScaler>() != null && component.sizeDelta != prevSize)
		{
			prevSize = component.sizeDelta;
			Vector2 vector = new Vector2(anchoredPosition.x * (float)Screen.width, anchoredPosition.y * (float)Screen.height) - new Vector2((float)Screen.width * component.pivot.x, (float)Screen.height * component.pivot.y);
			vector -= vector * (GetComponentInParent<CanvasScaler>().scaleFactor - 1f);
			component.anchoredPosition = vector;
		}
		if (dialogToDisplay != null)
		{
			dialogToDisplay.Resize();
		}
	}

	public void Dismiss()
	{
		Dismiss(KeepMouseState: false);
	}

	public void Dismiss(bool KeepMouseState)
	{
		if (!KeepMouseState)
		{
			Mouse.Left.ClearMouseState();
		}
		if (base.gameObject != null)
		{
			base.gameObject.SetActive(value: false);
		}
		if (instantiatedPopUps.Contains(this))
		{
			instantiatedPopUps.Remove(this);
		}
		Object.Destroy(base.gameObject);
	}

	public void SetDraggable(bool draggable)
	{
		dragPanel.enabled = draggable;
	}

	public bool Contains(Vector3 point)
	{
		return rTrf.Contains(point);
	}

	public void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (modal)
		{
			UIMasterController.Instance.RegisterModalDialog(canvasGroup);
		}
		else
		{
			UIMasterController.Instance.RegisterNonModalDialog(canvasGroup);
		}
	}

	public void OnDestroy()
	{
		if (UIMasterController.Instance != null)
		{
			if (modal)
			{
				UIMasterController.Instance.UnregisterModalDialog(canvasGroup);
			}
			else
			{
				UIMasterController.Instance.UnregisterNonModalDialog(canvasGroup);
			}
		}
		onDestroy.Invoke();
	}

	public void Update()
	{
		if (dialogToDisplay != null)
		{
			dialogToDisplay.Update();
			hover = Contains(Mouse.screenPos);
		}
		if (Input.GetKeyUp(KeyCode.Escape) && !UIMasterController.Instance.CameraMode)
		{
			if (OnDismiss != null)
			{
				OnDismiss();
			}
			Dismiss();
		}
	}

	public void FixedUpdate()
	{
		if (dialogToDisplay != null)
		{
			dialogToDisplay.FixedUpdate();
		}
	}

	public void LateUpdate()
	{
		if (dialogToDisplay != null)
		{
			dialogToDisplay.LateUpdate();
		}
	}

	public void OnRenderObject()
	{
		if (dialogToDisplay != null)
		{
			dialogToDisplay.OnRenderObject();
		}
	}
}
