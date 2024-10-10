using UnityEngine;

namespace Expansions.Missions.Editor;

public class ActionPaneDisplay_BiSelector : ActionPaneDisplay
{
	public GAPUtil_BiSelector selector;

	public override void Awake()
	{
		base.Awake();
		GameObject original = (GameObject)BundleLoader.LoadAsset<GAPUtil_BiSelector>("makinghistory_assets", "Assets/Expansions/Missions/Prefabs/GAPUtil_BiSelector.prefab");
		selector = Object.Instantiate(original).GetComponent<GAPUtil_BiSelector>();
		selector.transform.SetParent(base.transform, worldPositionStays: false);
		selector.transform.localScale = Vector3.one;
		RectTransform component = selector.GetComponent<RectTransform>();
		component.localPosition = Vector3.zero;
		component.offsetMin = new Vector2(0f, 0f);
		component.offsetMax = new Vector2(0f, 0f);
	}
}
