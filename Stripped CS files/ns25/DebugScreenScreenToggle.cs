using UnityEngine;
using UnityEngine.UI;

namespace ns25;

public class DebugScreenScreenToggle : MonoBehaviour
{
	public GameObject shownObject;

	public GameObject hiddenObject;

	public Button toggleButton;

	public bool isShown = true;

	public void Awake()
	{
		toggleButton.onClick.AddListener(OnToggleClick);
		SetShown();
	}

	public void OnToggleClick()
	{
		isShown = !isShown;
		SetShown();
	}

	public void SetShown()
	{
		if (isShown)
		{
			if (shownObject != null && !shownObject.activeSelf)
			{
				shownObject.SetActive(value: true);
			}
			if (hiddenObject != null && hiddenObject.activeSelf)
			{
				hiddenObject.SetActive(value: false);
			}
		}
		else
		{
			if (shownObject != null && shownObject.activeSelf)
			{
				shownObject.SetActive(value: false);
			}
			if (hiddenObject != null && !hiddenObject.activeSelf)
			{
				hiddenObject.SetActive(value: true);
			}
		}
	}
}
