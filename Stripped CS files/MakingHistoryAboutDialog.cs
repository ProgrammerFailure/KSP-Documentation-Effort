using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MakingHistoryAboutDialog : MonoBehaviour
{
	[SerializeField]
	public RawImage bannerIcon;

	[SerializeField]
	public Button moreInfoButton;

	[SerializeField]
	public Button closeButton;

	[SerializeField]
	public Toggle dontShowAgainToggle;

	public Texture[] banners;

	public bool dontShowAgain;

	public string makingHistoryExpansionURL;

	public void Start()
	{
		Random.InitState((int)KSPUtil.SystemDateTime.DateTimeNow().Ticks);
		int num = Random.Range(0, banners.Length);
		bannerIcon.texture = banners[num];
		moreInfoButton.onClick.AddListener(OnMoreInfoButton);
		closeButton.onClick.AddListener(OnCloseButton);
		dontShowAgainToggle.onValueChanged.AddListener(OnDontShowAgainToggle);
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic);
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Dismiss();
		}
	}

	public MakingHistoryAboutDialog Create(string expansionURL)
	{
		MakingHistoryAboutDialog makingHistoryAboutDialog = Object.Instantiate(this);
		makingHistoryAboutDialog.gameObject.SetActive(value: true);
		makingHistoryAboutDialog.transform.position = Vector3.zero;
		makingHistoryAboutDialog.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		makingHistoryAboutDialog.makingHistoryExpansionURL = expansionURL;
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "makingHistoryAboutDialog");
		return makingHistoryAboutDialog;
	}

	public void Dismiss()
	{
		InputLockManager.RemoveControlLock("makingHistoryAboutDialog");
		if (dontShowAgain)
		{
			GameSettings.MISSION_SHOW_EXPANSION_INFO = !dontShowAgain;
			GameSettings.SaveSettings();
		}
		Object.Destroy(base.gameObject);
	}

	public void OnDontShowAgainToggle(bool value)
	{
		dontShowAgain = value;
	}

	public void OnCloseButton()
	{
		Dismiss();
	}

	public void OnMoreInfoButton()
	{
		Process.Start(makingHistoryExpansionURL);
	}
}
