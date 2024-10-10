using ns11;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ns2;

public class SimpleAppFrame : MonoBehaviour
{
	public PointerEnterExitHandler hoverController;

	public ApplicationLauncherButton appLauncherButton;

	public RectTransform rectTransform;

	public void Awake()
	{
		rectTransform = base.transform as RectTransform;
	}

	public void Setup(ApplicationLauncherButton appLauncherButton, string appName)
	{
		this.appLauncherButton = appLauncherButton;
		ApplicationLauncher.Instance.AddOnRepositionCallback(Reposition);
		Reposition();
	}

	public void AddGlobalInputDelegate(UnityAction<PointerEventData> pointerEnter, UnityAction<PointerEventData> pointerExit)
	{
		hoverController.onPointerEnter.AddListener(pointerEnter);
		hoverController.onPointerExit.AddListener(pointerExit);
	}

	public void Reposition()
	{
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			base.gameObject.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
			rectTransform.anchoredPosition = appLauncherButton.GetAnchorTopRight();
		}
		else
		{
			base.gameObject.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
		}
	}

	public void OnDestroy()
	{
		if ((bool)ApplicationLauncher.Instance)
		{
			ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
		}
	}
}
