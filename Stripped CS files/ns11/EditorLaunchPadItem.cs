using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class EditorLaunchPadItem : MonoBehaviour
{
	public Toggle toggleSetDefault;

	public string siteName;

	public TextMeshProUGUI textName;

	public Button buttonLaunch;

	public void Awake()
	{
		buttonLaunch.onClick.AddListener(MouseInput_Launch);
		toggleSetDefault.onValueChanged.AddListener(MouseInput_SelectDefault);
	}

	public void OnDestroy()
	{
		buttonLaunch.onClick.RemoveAllListeners();
		toggleSetDefault.onValueChanged.RemoveAllListeners();
	}

	public void Create(UILaunchsiteController controller, string siteName, string textName)
	{
		this.siteName = siteName;
		this.textName.text = textName;
	}

	public void MouseInput_Launch()
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			EditorLogic.fetch.launchVessel(siteName);
		}
		else if (HighLogic.LoadedScene == GameScenes.SPACECENTER && VesselSpawnDialog.Instance != null)
		{
			VesselSpawnDialog.Instance.siteName = siteName;
			VesselSpawnDialog.Instance.ButtonLaunch();
		}
	}

	public void MouseInput_SelectDefault(bool value)
	{
		if (!value)
		{
			return;
		}
		if (EditorDriver.setLaunchSite(siteName))
		{
			if (HighLogic.LoadedScene == GameScenes.SPACECENTER && VesselSpawnDialog.Instance != null)
			{
				EditorDriver.saveselectedLaunchSite();
				VesselSpawnDialog.Instance.siteName = EditorDriver.SelectedLaunchSiteName;
			}
		}
		else
		{
			ScreenMessages.PostScreenMessage("Failed to Set Launchsite to " + textName, 5f);
		}
	}
}
