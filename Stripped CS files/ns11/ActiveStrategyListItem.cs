using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class ActiveStrategyListItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI description;

	public RawImage icon;

	public void Setup(string title, string description)
	{
		this.title.text = title;
		this.description.text = description;
	}

	public void Setup(string title, string description, Texture2D texture)
	{
		this.title.text = title;
		this.description.text = description;
		icon.texture = texture;
	}
}
