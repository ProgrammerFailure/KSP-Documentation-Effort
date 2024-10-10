using Expansions.Serenity;
using ns2;
using UnityEngine.UI;

namespace ns11;

public class EditorActionControllerOpenButton : UISelectableGridLayoutGroupItem
{
	public Button openButton;

	public ModuleRoboticController controller;

	public void Setup(ModuleRoboticController controller)
	{
		this.controller = controller;
		openButton.onClick.AddListener(OpenButtonClicked);
	}

	public void OpenButtonClicked()
	{
		if (RoboticControllerManager.Instance != null)
		{
			RoboticControllerWindow.Spawn(controller);
		}
	}
}
