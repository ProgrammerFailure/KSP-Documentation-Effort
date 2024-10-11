using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TMPro;

public class TMP_TextEventHandler : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	[Serializable]
	public class CharacterSelectionEvent : UnityEvent<char, int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public CharacterSelectionEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class SpriteSelectionEvent : UnityEvent<char, int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public SpriteSelectionEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class WordSelectionEvent : UnityEvent<string, int, int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public WordSelectionEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class LineSelectionEvent : UnityEvent<string, int, int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LineSelectionEvent()
		{
			throw null;
		}
	}

	[Serializable]
	public class LinkSelectionEvent : UnityEvent<string, string, int>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public LinkSelectionEvent()
		{
			throw null;
		}
	}

	[SerializeField]
	private CharacterSelectionEvent m_OnCharacterSelection;

	[SerializeField]
	private SpriteSelectionEvent m_OnSpriteSelection;

	[SerializeField]
	private WordSelectionEvent m_OnWordSelection;

	[SerializeField]
	private LineSelectionEvent m_OnLineSelection;

	[SerializeField]
	private LinkSelectionEvent m_OnLinkSelection;

	private TMP_Text m_TextComponent;

	private Camera m_Camera;

	private Canvas m_Canvas;

	private int m_selectedLink;

	private int m_lastCharIndex;

	private int m_lastWordIndex;

	private int m_lastLineIndex;

	public CharacterSelectionEvent onCharacterSelection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public SpriteSelectionEvent onSpriteSelection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public WordSelectionEvent onWordSelection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public LineSelectionEvent onLineSelection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public LinkSelectionEvent onLinkSelection
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_TextEventHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendOnCharacterSelection(char character, int characterIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendOnSpriteSelection(char character, int characterIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendOnWordSelection(string word, int charIndex, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendOnLineSelection(string line, int charIndex, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SendOnLinkSelection(string linkID, string linkText, int linkIndex)
	{
		throw null;
	}
}
