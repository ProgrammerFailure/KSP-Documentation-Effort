using UnityEngine;
using UnityEngine.UI;

namespace ns28;

public class ScreenRobotics : MonoBehaviour
{
	public Toggle dataActionMenus;

	public void Start()
	{
		dataActionMenus.isOn = PhysicsGlobals.RoboticJointDataDisplay;
		AddListeners();
	}

	public void AddListeners()
	{
		dataActionMenus.onValueChanged.AddListener(OnDataActionMenusToggle);
	}

	public void OnDataActionMenusToggle(bool on)
	{
		PhysicsGlobals.RoboticJointDataDisplay = on;
	}
}
