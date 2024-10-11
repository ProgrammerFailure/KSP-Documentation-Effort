using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TextMeshQueue : MonoBehaviour
{
	private class TextMeshQueueItem
	{
		private TextMeshQueue textMeshQueue;

		private TextMeshProUGUI textObject;

		private List<string> lines;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextMeshQueueItem(TextMeshQueue textMeshQueue)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool AddLine(string line)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool RemoveLine()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool DestroyIfEmpty()
		{
			throw null;
		}
	}

	[SerializeField]
	protected TextMeshProUGUI prefab;

	[SerializeField]
	protected RectTransform parent;

	[SerializeField]
	protected int characterLimit;

	private List<TextMeshQueueItem> items;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextMeshQueue()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddLine(string line)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveAllLines()
	{
		throw null;
	}
}
