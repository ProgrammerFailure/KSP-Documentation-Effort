using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class EditorInventoryOnlyIcon : MonoBehaviour
{
	public TextMeshProUGUI StackAmountText;

	public RawImage InventoryItemThumbnail;

	public Image Background;

	public Transform StackParent;

	public int currentStack = 1;

	public float StackSeparation = 3f;

	public int MaxStackImages = 10;

	public void Awake()
	{
		SetStackAmount(0);
	}

	public void SetIconTexture(Texture texture)
	{
		if (InventoryItemThumbnail != null)
		{
			InventoryItemThumbnail.texture = texture;
			InventoryItemThumbnail.gameObject.SetActive(value: true);
		}
	}

	public void SetStackAmount(int stackAmount)
	{
		if (StackAmountText != null)
		{
			StackAmountText.gameObject.SetActive(stackAmount > 1);
			StackAmountText.text = stackAmount.ToString();
		}
		if (StackParent != null)
		{
			StackAmountText.gameObject.SetActive(stackAmount > 1);
			Image component = Background.GetComponent<Image>();
			ClearStackImages();
			for (int i = 1; i < stackAmount && i < MaxStackImages; i++)
			{
				GameObject obj = new GameObject("StackImage");
				Image image = obj.AddComponent<Image>();
				image.sprite = component.sprite;
				image.raycastTarget = false;
				obj.transform.SetParent(StackParent);
				obj.transform.SetAsFirstSibling();
				(obj.transform as RectTransform).localPosition = new Vector3((float)i * StackSeparation, (float)(-i) * StackSeparation, 0f);
				(obj.transform as RectTransform).sizeDelta = Background.rectTransform.sizeDelta;
				obj.transform.localScale = Background.transform.localScale;
			}
		}
		currentStack = stackAmount;
	}

	public void ClearStackImages()
	{
		for (int i = 0; i < StackParent.childCount; i++)
		{
			Object.Destroy(StackParent.GetChild(i).gameObject);
		}
	}

	public void ToggleBackgroundVisibility(bool visible)
	{
		if (Background != null)
		{
			Background.enabled = visible;
		}
	}
}
