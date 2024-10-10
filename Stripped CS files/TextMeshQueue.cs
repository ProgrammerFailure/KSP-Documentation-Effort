using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshQueue : MonoBehaviour
{
	public class TextMeshQueueItem
	{
		public TextMeshQueue textMeshQueue;

		public TextMeshProUGUI textObject;

		public List<string> lines = new List<string>();

		public TextMeshQueueItem(TextMeshQueue textMeshQueue)
		{
			this.textMeshQueue = textMeshQueue;
		}

		public bool AddLine(string line)
		{
			if (textObject == null)
			{
				textObject = UnityEngine.Object.Instantiate(textMeshQueue.prefab);
				textObject.transform.SetParent(textMeshQueue.parent, worldPositionStays: false);
			}
			if (textObject.text.Length + line.Length > textMeshQueue.characterLimit)
			{
				return false;
			}
			if (lines.Count > 0)
			{
				textObject.text += Environment.NewLine;
			}
			textObject.text += line;
			lines.Add(line);
			return true;
		}

		public bool RemoveLine()
		{
			if (DestroyIfEmpty())
			{
				return true;
			}
			string text = lines[0];
			int num = textObject.text.IndexOf(text, StringComparison.CurrentCulture);
			if (num >= 0)
			{
				int num2 = num + text.Length;
				int num3 = textObject.text.IndexOf(Environment.NewLine, num2);
				int num4 = num3 + Environment.NewLine.Length;
				textObject.text = textObject.text.Substring((num3 >= 0) ? num4 : num2);
			}
			lines.RemoveAt(0);
			if (DestroyIfEmpty())
			{
				return true;
			}
			return false;
		}

		public bool DestroyIfEmpty()
		{
			if (lines.Count > 0)
			{
				return false;
			}
			UnityEngine.Object.Destroy(textObject.gameObject);
			textObject = null;
			return true;
		}
	}

	[SerializeField]
	public TextMeshProUGUI prefab;

	[SerializeField]
	public RectTransform parent;

	[SerializeField]
	public int characterLimit = 10000;

	public List<TextMeshQueueItem> items = new List<TextMeshQueueItem>();

	public void AddLine(string line)
	{
		if (!string.IsNullOrEmpty(line))
		{
			if (line.Length > characterLimit)
			{
				line = line.Substring(0, characterLimit);
			}
			if (items.Count <= 0)
			{
				items.Add(new TextMeshQueueItem(this));
			}
			TextMeshQueueItem textMeshQueueItem = items[items.Count - 1];
			while (!textMeshQueueItem.AddLine(line))
			{
				items.Add(new TextMeshQueueItem(this));
				textMeshQueueItem = items[items.Count - 1];
			}
		}
	}

	public void RemoveLine()
	{
		if (items.Count > 0 && items[0].RemoveLine())
		{
			items.RemoveAt(0);
		}
	}

	public void RemoveAllLines()
	{
		while (items.Count > 0)
		{
			if (items[0].RemoveLine())
			{
				items.RemoveAt(0);
			}
		}
	}
}
