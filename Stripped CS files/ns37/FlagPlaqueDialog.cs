using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns37;

public class FlagPlaqueDialog : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI textHeader;

	[SerializeField]
	public TextMeshProUGUI textContent;

	[SerializeField]
	public Button buttonClose;

	public Callback onDismiss;

	public string plaqueText;

	public string siteName;

	public static FlagPlaqueDialog Spawn(string siteName, string plaqueText, Callback onDismiss)
	{
		FlagPlaqueDialog component = Object.Instantiate(AssetBase.GetPrefab("FlagPlaqueDialog")).GetComponent<FlagPlaqueDialog>();
		component.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		component.siteName = siteName;
		component.plaqueText = plaqueText;
		component.onDismiss = onDismiss;
		return component;
	}

	public void Terminate()
	{
		Object.Destroy(base.gameObject);
	}

	public void Start()
	{
		textHeader.text = Localizer.Format("#autoLOC_457848", siteName);
		textContent.text = plaqueText;
		buttonClose.onClick.AddListener(onBtnClose);
	}

	public void onBtnClose()
	{
		if (onDismiss != null)
		{
			onDismiss();
		}
		Terminate();
	}
}
