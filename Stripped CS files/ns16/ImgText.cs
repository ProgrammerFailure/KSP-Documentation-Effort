using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns16;

public class ImgText : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI textComponent;

	[SerializeField]
	public Image imgComponent;

	public string text
	{
		get
		{
			return textComponent.text;
		}
		set
		{
			textComponent.text = value;
		}
	}

	public Sprite sprite
	{
		get
		{
			return imgComponent.sprite;
		}
		set
		{
			imgComponent.sprite = value;
		}
	}
}
