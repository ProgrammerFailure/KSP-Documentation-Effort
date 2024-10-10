using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class UICollapsibleSection : MonoBehaviour
{
	public RectTransform layoutParent;

	public RectTransform containerChilden;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	public void Start()
	{
		buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
	}

	public void OnButtonCollapsePressed()
	{
		containerChilden.gameObject.SetActive(!containerChilden.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (containerChilden.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
		LayoutRebuilder.MarkLayoutForRebuild(layoutParent);
		LayoutRebuilder.MarkLayoutForRebuild(containerChilden);
	}
}
