using ns2;
using ns9;
using TMPro;
using UnityEngine;

namespace ns11;

public class KbApp_PlanetInfo : KbApp
{
	public GameObject textPrefab;

	public override void ActivateApp(MapObject target)
	{
		appFrame.scrollList.Clear(destroyElements: true);
		GameObject gameObject = Object.Instantiate(textPrefab);
		TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
		CelestialBody celestialBody = target.celestialBody;
		if (OverlayGenerator.Instance.DisplayBody != celestialBody)
		{
			OverlayGenerator.Instance.ClearDisplay();
			OverlayGenerator.Instance.DisplayBody = celestialBody;
		}
		appFrame.appName.text = Localizer.Format("#autoLOC_7001301", celestialBody.displayName).ToUpper();
		component.text = celestialBody.bodyDescription;
		appFrame.scrollList.AddItem(gameObject.GetComponent<UIListItem>());
	}

	public override void DisplayApp()
	{
	}

	public override void HideApp()
	{
	}
}
