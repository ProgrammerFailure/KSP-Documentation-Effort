using System;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class KbAppFrame : MonoBehaviour
{
	public TextMeshProUGUI appName;

	public TextMeshProUGUI appTitle;

	public TextMeshProUGUI label;

	public Image header;

	[NonSerialized]
	public Button headerBtn;

	[NonSerialized]
	public PointerClickHandler headerClickHandler;

	public UIList scrollList;

	public RectTransform anchorTop;

	public RectTransform anchorBottom;

	public RectTransform anchorPieChart;

	public RectTransform anchorPieChartLeft;

	public ApplicationLauncherButton appLauncherButton;

	public RectTransform rectTransform;

	public void Awake()
	{
		rectTransform = base.transform as RectTransform;
		headerBtn = header.GetComponent<Button>();
		headerClickHandler = header.GetComponent<PointerClickHandler>();
	}

	public void OnDestroy()
	{
		if ((bool)ApplicationLauncher.Instance)
		{
			ApplicationLauncher.Instance.RemoveOnRepositionCallback(Reposition);
		}
	}

	public void Setup(ApplicationLauncherButton appLauncherButton, string appName, string appTitle)
	{
		this.appLauncherButton = appLauncherButton;
		this.appName.text = appName;
		this.appTitle.text = appTitle;
		label.text = Localizer.Format(label.text);
		Reposition();
		base.gameObject.SetActive(value: false);
	}

	public void Reposition()
	{
		base.transform.position = Vector3.zero;
		base.transform.SetParent(ApplicationLauncher.Instance.appSpace, worldPositionStays: false);
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			base.transform.position = appLauncherButton.GetAnchorUL();
		}
		else
		{
			base.transform.position = new Vector3(appLauncherButton.GetAnchorUR().x, appLauncherButton.GetAnchorUR().y + rectTransform.sizeDelta.y * GameSettings.UI_SCALE * GameSettings.UI_SCALE_APPS, appLauncherButton.GetAnchorUR().z);
		}
	}
}
