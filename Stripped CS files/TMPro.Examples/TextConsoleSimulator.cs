using System.Collections;
using UnityEngine;

namespace TMPro.Examples;

public class TextConsoleSimulator : MonoBehaviour
{
	public TMP_Text m_TextComponent;

	public bool hasTextChanged;

	public void Awake()
	{
		m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
	}

	public void Start()
	{
		StartCoroutine(RevealCharacters(m_TextComponent));
	}

	public void OnEnable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Add(ON_TEXT_CHANGED);
	}

	public void OnDisable()
	{
		TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(ON_TEXT_CHANGED);
	}

	public void ON_TEXT_CHANGED(Object obj)
	{
		hasTextChanged = true;
	}

	public IEnumerator RevealCharacters(TMP_Text textComponent)
	{
		textComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = textComponent.textInfo;
		int totalVisibleCharacters = textInfo.characterCount;
		int visibleCount = 0;
		while (true)
		{
			if (hasTextChanged)
			{
				totalVisibleCharacters = textInfo.characterCount;
				hasTextChanged = false;
			}
			if (visibleCount > totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
				visibleCount = 0;
			}
			textComponent.maxVisibleCharacters = visibleCount;
			visibleCount++;
			yield return null;
		}
	}

	public IEnumerator RevealWords(TMP_Text textComponent)
	{
		textComponent.ForceMeshUpdate();
		int totalWordCount = textComponent.textInfo.wordCount;
		int totalVisibleCharacters = textComponent.textInfo.characterCount;
		int counter = 0;
		int visibleCount = 0;
		while (true)
		{
			int num = counter % (totalWordCount + 1);
			if (num == 0)
			{
				visibleCount = 0;
			}
			else if (num < totalWordCount)
			{
				visibleCount = textComponent.textInfo.wordInfo[num - 1].lastCharacterIndex + 1;
			}
			else if (num == totalWordCount)
			{
				visibleCount = totalVisibleCharacters;
			}
			textComponent.maxVisibleCharacters = visibleCount;
			if (visibleCount >= totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
			}
			counter++;
			yield return new WaitForSeconds(0.1f);
		}
	}
}
