using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro;

public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[Serializable]
	public class CharacterSelectionEvent : UnityEvent<char, int>
	{
	}

	[Serializable]
	public class SpriteSelectionEvent : UnityEvent<char, int>
	{
	}

	[Serializable]
	public class WordSelectionEvent : UnityEvent<string, int, int>
	{
	}

	[Serializable]
	public class LineSelectionEvent : UnityEvent<string, int, int>
	{
	}

	[Serializable]
	public class LinkSelectionEvent : UnityEvent<string, string, int>
	{
	}

	[SerializeField]
	public CharacterSelectionEvent m_OnCharacterSelection = new CharacterSelectionEvent();

	[SerializeField]
	public SpriteSelectionEvent m_OnSpriteSelection = new SpriteSelectionEvent();

	[SerializeField]
	public WordSelectionEvent m_OnWordSelection = new WordSelectionEvent();

	[SerializeField]
	public LineSelectionEvent m_OnLineSelection = new LineSelectionEvent();

	[SerializeField]
	public LinkSelectionEvent m_OnLinkSelection = new LinkSelectionEvent();

	public TMP_Text m_TextComponent;

	public Camera m_Camera;

	public Canvas m_Canvas;

	public int m_selectedLink = -1;

	public int m_lastCharIndex = -1;

	public int m_lastWordIndex = -1;

	public int m_lastLineIndex = -1;

	public CharacterSelectionEvent onCharacterSelection
	{
		get
		{
			return m_OnCharacterSelection;
		}
		set
		{
			m_OnCharacterSelection = value;
		}
	}

	public SpriteSelectionEvent onSpriteSelection
	{
		get
		{
			return m_OnSpriteSelection;
		}
		set
		{
			m_OnSpriteSelection = value;
		}
	}

	public WordSelectionEvent onWordSelection
	{
		get
		{
			return m_OnWordSelection;
		}
		set
		{
			m_OnWordSelection = value;
		}
	}

	public LineSelectionEvent onLineSelection
	{
		get
		{
			return m_OnLineSelection;
		}
		set
		{
			m_OnLineSelection = value;
		}
	}

	public LinkSelectionEvent onLinkSelection
	{
		get
		{
			return m_OnLinkSelection;
		}
		set
		{
			m_OnLinkSelection = value;
		}
	}

	public void Awake()
	{
		m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
		if (m_TextComponent.GetType() == typeof(TextMeshProUGUI))
		{
			m_Canvas = base.gameObject.GetComponentInParent<Canvas>();
			if (m_Canvas != null)
			{
				if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
				{
					m_Camera = null;
				}
				else
				{
					m_Camera = m_Canvas.worldCamera;
				}
			}
		}
		else
		{
			m_Camera = Camera.main;
		}
	}

	public void LateUpdate()
	{
		if (!TMP_TextUtilities.IsIntersectingRectTransform(m_TextComponent.rectTransform, Input.mousePosition, m_Camera))
		{
			return;
		}
		int num = TMP_TextUtilities.FindIntersectingCharacter(m_TextComponent, Input.mousePosition, m_Camera, visibleOnly: true);
		if (num != -1 && num != m_lastCharIndex)
		{
			m_lastCharIndex = num;
			switch (m_TextComponent.textInfo.characterInfo[num].elementType)
			{
			case TMP_TextElementType.Character:
				SendOnCharacterSelection(m_TextComponent.textInfo.characterInfo[num].character, num);
				break;
			case TMP_TextElementType.Sprite:
				SendOnSpriteSelection(m_TextComponent.textInfo.characterInfo[num].character, num);
				break;
			}
		}
		int num2 = TMP_TextUtilities.FindIntersectingWord(m_TextComponent, Input.mousePosition, m_Camera);
		if (num2 != -1 && num2 != m_lastWordIndex)
		{
			m_lastWordIndex = num2;
			TMP_WordInfo tMP_WordInfo = m_TextComponent.textInfo.wordInfo[num2];
			SendOnWordSelection(tMP_WordInfo.GetWord(), tMP_WordInfo.firstCharacterIndex, tMP_WordInfo.characterCount);
		}
		int num3 = TMP_TextUtilities.FindIntersectingLine(m_TextComponent, Input.mousePosition, m_Camera);
		if (num3 != -1 && num3 != m_lastLineIndex)
		{
			m_lastLineIndex = num3;
			TMP_LineInfo tMP_LineInfo = m_TextComponent.textInfo.lineInfo[num3];
			char[] array = new char[tMP_LineInfo.characterCount];
			for (int i = 0; i < tMP_LineInfo.characterCount && i < m_TextComponent.textInfo.characterInfo.Length; i++)
			{
				array[i] = m_TextComponent.textInfo.characterInfo[i + tMP_LineInfo.firstCharacterIndex].character;
			}
			string line = new string(array);
			SendOnLineSelection(line, tMP_LineInfo.firstCharacterIndex, tMP_LineInfo.characterCount);
		}
		int num4 = TMP_TextUtilities.FindIntersectingLink(m_TextComponent, Input.mousePosition, m_Camera);
		if (num4 != -1 && num4 != m_selectedLink)
		{
			m_selectedLink = num4;
			TMP_LinkInfo tMP_LinkInfo = m_TextComponent.textInfo.linkInfo[num4];
			SendOnLinkSelection(tMP_LinkInfo.GetLinkID(), tMP_LinkInfo.GetLinkText(), num4);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
	}

	public void OnPointerExit(PointerEventData eventData)
	{
	}

	public void SendOnCharacterSelection(char character, int characterIndex)
	{
		if (onCharacterSelection != null)
		{
			onCharacterSelection.Invoke(character, characterIndex);
		}
	}

	public void SendOnSpriteSelection(char character, int characterIndex)
	{
		if (onSpriteSelection != null)
		{
			onSpriteSelection.Invoke(character, characterIndex);
		}
	}

	public void SendOnWordSelection(string word, int charIndex, int length)
	{
		if (onWordSelection != null)
		{
			onWordSelection.Invoke(word, charIndex, length);
		}
	}

	public void SendOnLineSelection(string line, int charIndex, int length)
	{
		if (onLineSelection != null)
		{
			onLineSelection.Invoke(line, charIndex, length);
		}
	}

	public void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		if (onLinkSelection != null)
		{
			onLinkSelection.Invoke(linkID, linkText, linkIndex);
		}
	}
}
