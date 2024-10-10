using UnityEngine;

namespace TMPro.Examples;

public class TMP_TextEventCheck : MonoBehaviour
{
	public TMP_TextEventHandler TextEventHandler;

	public void OnEnable()
	{
		if (TextEventHandler != null)
		{
			TextEventHandler.onCharacterSelection.AddListener(OnCharacterSelection);
			TextEventHandler.onSpriteSelection.AddListener(OnSpriteSelection);
			TextEventHandler.onWordSelection.AddListener(OnWordSelection);
			TextEventHandler.onLineSelection.AddListener(OnLineSelection);
			TextEventHandler.onLinkSelection.AddListener(OnLinkSelection);
		}
	}

	public void OnDisable()
	{
		if (TextEventHandler != null)
		{
			TextEventHandler.onCharacterSelection.RemoveListener(OnCharacterSelection);
			TextEventHandler.onSpriteSelection.RemoveListener(OnSpriteSelection);
			TextEventHandler.onWordSelection.RemoveListener(OnWordSelection);
			TextEventHandler.onLineSelection.RemoveListener(OnLineSelection);
			TextEventHandler.onLinkSelection.RemoveListener(OnLinkSelection);
		}
	}

	public void OnCharacterSelection(char c, int index)
	{
		Debug.Log("Character [" + c.ToString() + "] at Index: " + index + " has been selected.");
	}

	public void OnSpriteSelection(char c, int index)
	{
		Debug.Log("Sprite [" + c.ToString() + "] at Index: " + index + " has been selected.");
	}

	public void OnWordSelection(string word, int firstCharacterIndex, int length)
	{
		Debug.Log("Word [" + word + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
	}

	public void OnLineSelection(string lineText, int firstCharacterIndex, int length)
	{
		Debug.Log("Line [" + lineText + "] with first character index of " + firstCharacterIndex + " and length of " + length + " has been selected.");
	}

	public void OnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		Debug.Log("Link Index: " + linkIndex + " with ID [" + linkID + "] and Text \"" + linkText + "\" has been selected.");
	}
}
