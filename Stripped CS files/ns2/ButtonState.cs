using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns2;

[Serializable]
public class ButtonState
{
	public string name;

	public Sprite normal;

	public Sprite highlight;

	public Sprite pressed;

	public Sprite disabled;

	public Color textColor = Color.white;

	public void Setup(Button button, Image image, TextMeshProUGUI text = null)
	{
		SpriteState spriteState = button.spriteState;
		spriteState.highlightedSprite = highlight;
		spriteState.pressedSprite = pressed;
		spriteState.disabledSprite = disabled;
		if (spriteState.selectedSprite == null)
		{
			spriteState.selectedSprite = highlight;
		}
		button.spriteState = spriteState;
		image.sprite = normal;
		if (text != null)
		{
			text.color = textColor;
		}
	}
}
