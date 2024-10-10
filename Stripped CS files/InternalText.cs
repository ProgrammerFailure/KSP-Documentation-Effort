using TMPro;
using UnityEngine;

public class InternalText : MonoBehaviour
{
	public string fontName;

	public float fontSize;

	[HideInInspector]
	public TextMeshPro text;

	public void Awake()
	{
		text = GetComponent<TextMeshPro>();
	}
}
