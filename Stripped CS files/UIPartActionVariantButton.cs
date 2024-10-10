using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionVariantButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Image imagePrimaryColor;

	public Image imageSecomdaryColor;

	public Image imageSelected;

	public Image imageInvalid;

	public Button buttonMain;

	public UIPartActionVariantSelector selectorReference;

	public bool locked;

	public int index;

	public bool Locked
	{
		get
		{
			return locked;
		}
		set
		{
			imageInvalid.gameObject.SetActive(value);
			locked = value;
		}
	}

	public void Setup(UIPartActionVariantSelector selectorReference, int index, string hexPrimaryColor, string hexSecondaryColor)
	{
		this.selectorReference = selectorReference;
		this.index = index;
		buttonMain.onClick.AddListener(OnButtonPressed);
		SetColor(hexPrimaryColor, hexSecondaryColor);
	}

	public void SetColor(string hexPrimaryColor, string hexSecondaryColor)
	{
		Color color = Color.white;
		ColorUtility.TryParseHtmlString(hexPrimaryColor, out color);
		Color color2;
		Color color3 = ((!ColorUtility.TryParseHtmlString(hexSecondaryColor, out color2)) ? color : color2);
		imagePrimaryColor.color = color;
		imageSecomdaryColor.color = color3;
	}

	public void OnButtonPressed()
	{
		if (!locked)
		{
			selectorReference.OnButtonPressed(index);
		}
	}

	public void Select()
	{
		imageSelected.gameObject.SetActive(value: true);
	}

	public void Reset()
	{
		imageSelected.gameObject.SetActive(value: false);
	}

	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		selectorReference.SetButtonOverText(index);
	}

	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		selectorReference.ResetButtonOverText();
	}
}
