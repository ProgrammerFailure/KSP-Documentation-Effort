using TMPro;
using UnityEngine;

public class UIButtonTextColor : MonoBehaviour
{
	public Color idleColor;

	public Color overColor;

	public TextMeshProUGUI textLabel;

	public void SetIdleColor()
	{
		textLabel.color = idleColor;
	}

	public void SetOverColor()
	{
		textLabel.color = overColor;
	}
}
