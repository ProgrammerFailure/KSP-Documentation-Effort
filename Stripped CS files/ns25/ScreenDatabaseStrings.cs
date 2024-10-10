using System.Collections.Generic;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns25;

public class ScreenDatabaseStrings : MonoBehaviour
{
	public Button SearchButton;

	public TMP_InputField SearchKeyBox;

	public TMP_InputField SearchValueBox;

	public Toggle ExactMatch;

	public TextMeshQueue textMeshQueue;

	public ScrollRect scrollRect;

	public void Start()
	{
		SearchButton.onClick.AddListener(OnSearchClicked);
		SearchKeyBox.isRichTextEditingAllowed = false;
		SearchKeyBox.inputType = TMP_InputField.InputType.Standard;
		SearchValueBox.isRichTextEditingAllowed = false;
		SearchValueBox.inputType = TMP_InputField.InputType.Standard;
	}

	public void OnDestroy()
	{
		SearchButton.onClick.RemoveAllListeners();
	}

	public void OnSearchClicked()
	{
		textMeshQueue.RemoveAllLines();
		string text = SearchKeyBox.text.ToLower();
		string text2 = SearchValueBox.text.ToLower();
		Dictionary<string, string> tags = Localizer.Tags;
		int num = 0;
		int num2 = 0;
		List<string> list = new List<string>();
		textMeshQueue.AddLine(Localizer.Format("#autoLOC_901076"));
		Dictionary<string, string>.Enumerator enumerator = tags.GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, string> current = enumerator.Current;
			if ((current.Value.ToLower() == text2 && text == "") || (current.Key.ToLower() == text && text2 == "") || (current.Value.ToLower() == text2 && current.Key.ToLower() == text))
			{
				num2++;
				list.Add(current.Key);
				textMeshQueue.AddLine(FormatKeyValue(current));
			}
		}
		enumerator.Dispose();
		if (num2 < 1)
		{
			textMeshQueue.AddLine(Localizer.Format("#autoLOC_901077", text, text2));
		}
		if (!ExactMatch.isOn)
		{
			textMeshQueue.AddLine(Localizer.Format("#autoLOC_901079"));
			enumerator = tags.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current2 = enumerator.Current;
				if (current2.Key.ToLower().Contains(text) && current2.Value.ToLower().Contains(text2) && !list.Contains(current2.Key))
				{
					textMeshQueue.AddLine(FormatKeyValue(current2));
					if (++num >= 50)
					{
						textMeshQueue.AddLine(Localizer.Format("#autoLOC_901080"));
						Debug.Log(Localizer.Format("#autoLOC_901081"));
						break;
					}
				}
			}
			enumerator.Dispose();
		}
		if (num < 1)
		{
			textMeshQueue.AddLine(Localizer.Format("#autoLOC_901078", text, text2));
		}
		scrollRect.verticalScrollbar.value = 1f;
	}

	public string FormatKeyValue(KeyValuePair<string, string> keyValuePair)
	{
		return Localizer.Format("#autoLOC_901082") + ": " + keyValuePair.Key + "\n " + Localizer.Format("#autoLOC_901083") + "  : " + keyValuePair.Value;
	}
}
