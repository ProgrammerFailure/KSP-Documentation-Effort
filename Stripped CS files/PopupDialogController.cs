using UnityEngine;

public class PopupDialogController : MonoBehaviour
{
	public PopupDialog popupDialogBase;

	public Transform popupDialogCanvas;

	public static PopupDialogController Instance { get; set; }

	public static PopupDialog PopupDialogBase => Instance.popupDialogBase;

	public static Transform PopupDialogCanvas => Instance.popupDialogCanvas;

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
