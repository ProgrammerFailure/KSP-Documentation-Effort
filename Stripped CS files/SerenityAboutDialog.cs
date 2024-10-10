using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SerenityAboutDialog : MonoBehaviour
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

	public string serenityExpansionURL;

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

	public SerenityAboutDialog Create(string expansionURL)
	{
		SerenityAboutDialog serenityAboutDialog = Object.Instantiate(this);
		serenityAboutDialog.gameObject.SetActive(value: true);
		serenityAboutDialog.transform.position = Vector3.zero;
		serenityAboutDialog.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		serenityAboutDialog.serenityExpansionURL = expansionURL;
		InputLockManager.SetControlLock(ControlTypes.MAIN_MENU, "serenityAboutDialog");
		return serenityAboutDialog;
	}

	public void Dismiss()
	{
		InputLockManager.RemoveControlLock("serenityAboutDialog");
		if (dontShowAgain)
		{
			GameSettings.SERENITY_SHOW_EXPANSION_INFO = !dontShowAgain;
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
		Process.Start(serenityExpansionURL);
	}
}
