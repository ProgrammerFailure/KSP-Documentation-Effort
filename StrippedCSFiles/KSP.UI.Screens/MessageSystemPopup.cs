using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class MessageSystemPopup : MonoBehaviour
{
	public delegate void OnDiscard();

	public delegate void OnShowNext(bool discardSelected);

	public delegate void OnShowPrevious(bool discardSelected);

	public enum ButtonTypes
	{
		DISCARDCLOSE = 1
	}

	public TextMeshProUGUI textArea;

	public Button btnClose;

	public Button btnDiscard;

	public Button btnNext;

	public Button btnPrevious;

	public EventTrigger background;

	public TextMeshProUGUI title;

	public OnDiscard onDiscard;

	public OnShowNext onShowNext;

	public OnShowPrevious onShowPrevious;

	private bool showing;

	public bool Showing
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MessageSystemPopup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MessageSystemPopup InstantiateFromPrefab(MessageSystemPopup prefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoadRequest(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Show(string title, string text, OnDiscard onDiscard, OnShowNext onShowNext, OnShowPrevious onShowPrevious)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Hide()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputClose(BaseEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseinputDiscard()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShowPrevious()
	{
		throw null;
	}
}
