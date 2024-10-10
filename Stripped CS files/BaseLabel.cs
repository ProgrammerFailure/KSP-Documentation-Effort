using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class BaseLabel : MonoBehaviour
{
	public Image icon;

	public TextMeshProUGUI text;

	public PointerEnterExitHandler ptrEnterExit;

	public PointerClickHandler ptrClick;

	public Image mouseHoverImage;

	public VesselLabels labels;

	public bool Hover => ptrEnterExit.IsOver;

	public VesselLabels.VesselLabelType labelType { get; set; }

	public BaseLabel()
	{
	}

	public void Awake()
	{
		ptrEnterExit = base.gameObject.AddComponent<PointerEnterExitHandler>();
		ptrClick = base.gameObject.AddComponent<PointerClickHandler>();
		mouseHoverImage = base.gameObject.AddComponent<Image>();
		mouseHoverImage.color = Color.clear;
		ptrClick.onPointerClick.AddListener(OnClick);
		GameEvents.onUIScaleChange.Add(ResetParent);
	}

	public void Setup(VesselLabels labels, VesselLabels.VesselLabelType labelType)
	{
		this.labels = labels;
		this.labelType = labelType;
		icon.color = labelType.labelColor;
		icon.sprite = labelType.sprite;
		icon.rectTransform.sizeDelta = new Vector2(labelType.iconSize, labelType.iconSize);
	}

	public void Enable()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void Disable()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public virtual void OnClick(PointerEventData ptrData)
	{
	}

	public void ResetParent()
	{
		if (base.transform.parent != null)
		{
			Transform parent = base.transform.parent;
			base.transform.SetParent(null);
			base.transform.localScale = Vector3.one;
			base.transform.SetParent(parent);
			base.transform.localPosition = Vector3.zero;
		}
	}

	public void OnDestroy()
	{
		GameEvents.onUIScaleChange.Remove(ResetParent);
	}
}
