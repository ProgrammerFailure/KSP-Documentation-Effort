using UnityEngine;
using UnityEngine.UI;

public class UIConstructionPanelPart : MonoBehaviour
{
	public RectTransform containerChilden;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	public void Start()
	{
		buttonCollapse.onClick.AddListener(OnButtonCollapsePressed);
	}

	public void OnDestroy()
	{
		buttonCollapse.onClick.RemoveListener(OnButtonCollapsePressed);
	}

	public void Update()
	{
	}

	public void OnButtonCollapsePressed()
	{
		containerChilden.gameObject.SetActive(!containerChilden.gameObject.activeSelf);
		((Image)buttonCollapse.targetGraphic).sprite = (containerChilden.gameObject.activeSelf ? spriteCollapseOff : spriteCollapseOn);
	}
}
