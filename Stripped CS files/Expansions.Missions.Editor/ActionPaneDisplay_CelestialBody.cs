using UnityEngine;

namespace Expansions.Missions.Editor;

public class ActionPaneDisplay_CelestialBody : ActionPaneDisplay
{
	public GAPUtil_CelestialBody selector;

	public GAPUtil_CelestialBody Selector => selector;

	public override void Awake()
	{
		base.Awake();
		GameObject original = MissionsUtils.MEPrefab("Prefabs/GAPUtil_CelestialBody.prefab");
		selector = Object.Instantiate(original).GetComponent<GAPUtil_CelestialBody>();
		selector.transform.SetParent(base.transform);
		selector.transform.localScale = Vector3.one;
		RectTransform component = selector.GetComponent<RectTransform>();
		component.localPosition = Vector3.zero;
		component.offsetMin = new Vector2(0f, 0f);
		component.offsetMax = new Vector2(0f, 0f);
	}
}
