using System.Diagnostics;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PrivacyDialog : MonoBehaviour
{
	[SerializeField]
	public Button closeButton;

	[SerializeField]
	public Button UnityData;

	public void Start()
	{
		closeButton.onClick.AddListener(OnCloseButton);
		UnityData.onClick.AddListener(OnUnityData);
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Dismiss();
		}
	}

	public PrivacyDialog Create()
	{
		PrivacyDialog privacyDialog = Object.Instantiate(this);
		privacyDialog.gameObject.SetActive(value: true);
		privacyDialog.transform.position = Vector3.zero;
		privacyDialog.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "privacyDialog");
		return privacyDialog;
	}

	public void Dismiss()
	{
		InputLockManager.RemoveControlLock("privacyDialog");
		Object.Destroy(base.gameObject);
	}

	public void OnCloseButton()
	{
		Dismiss();
	}

	public void OnUnityData()
	{
		DataPrivacy.FetchPrivacyUrl(OnPrivacyURLReceived, OnPrivacyURLFailure);
	}

	public void OnPrivacyURLFailure(string reason)
	{
		UnityEngine.Debug.LogWarningFormat("Failed to get data privacy page URL: {0}", reason);
	}

	public void OnPrivacyURLReceived(string url)
	{
		Process.Start(url);
	}
}
