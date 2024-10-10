using UnityEngine;

public class ButtonHighlighter : MonoBehaviour
{
	[SerializeField]
	public GameObject Highlighter;

	public void Enable(bool value)
	{
		Highlighter.SetActive(value);
	}
}
