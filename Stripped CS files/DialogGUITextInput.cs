using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUITextInput : DialogGUIBase
{
	public Func<string, string> onTextUpdated;

	public Func<string> GetString;

	public string text;

	public string placeholder;

	public bool multiLine;

	public int maxLength;

	public TMP_InputField field;

	public TMP_InputField.ContentType? contentType;

	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, float hght = -1f)
	{
		text = txt;
		multiLine = multiline;
		maxLength = maxlength;
		onTextUpdated = textSetFunc;
		height = hght;
		size = new Vector2(-1f, hght);
	}

	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, float w, float hght)
	{
		text = txt;
		multiLine = multiline;
		maxLength = maxlength;
		onTextUpdated = textSetFunc;
		height = hght;
		width = w;
		size = new Vector2(w, hght);
	}

	public DialogGUITextInput(string txt, string placeHlder, bool multiline, int maxlength, Func<string, string> textSetFunc, float hght = -1f)
	{
		text = txt;
		placeholder = placeHlder;
		multiLine = multiline;
		maxLength = maxlength;
		onTextUpdated = textSetFunc;
		height = hght;
		size = new Vector2(-1f, hght);
	}

	public DialogGUITextInput(string txt, string placeHlder, bool multiline, int maxlength, Func<string, string> textSetFunc, float w, float hght)
	{
		text = txt;
		placeholder = placeHlder;
		multiLine = multiline;
		maxLength = maxlength;
		onTextUpdated = textSetFunc;
		height = hght;
		width = w;
		size = new Vector2(w, hght);
	}

	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, Func<string> getString, TMP_InputField.ContentType contentType, float hght = -1f)
	{
		text = txt;
		multiLine = multiline;
		maxLength = maxlength;
		onTextUpdated = textSetFunc;
		GetString = getString;
		height = hght;
		size = new Vector2(-1f, hght);
		this.contentType = contentType;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = UnityEngine.Object.Instantiate(UISkinManager.GetPrefab("UITextInputPrefab"));
		uiItem.SetActive(value: true);
		uiItem.transform.SetParent(layouts.Peek(), worldPositionStays: false);
		SetupTransformAndLayout();
		field = uiItem.GetComponent<TMP_InputField>();
		field.text = text;
		field.onEndEdit.AddListener(delegate(string n)
		{
			onTextUpdated(n);
		});
		field.lineType = (multiLine ? TMP_InputField.LineType.MultiLineNewline : TMP_InputField.LineType.SingleLine);
		field.characterLimit = maxLength;
		if (multiLine)
		{
			field.textComponent.enableWordWrapping = true;
			field.textComponent.overflowMode = TextOverflowModes.Truncate;
			uiItem.GetComponent<LayoutElement>().minHeight = height;
		}
		if (contentType.HasValue)
		{
			field.contentType = contentType.Value;
		}
		if (string.IsNullOrEmpty(placeholder))
		{
			placeholder = text;
		}
		uiItem.GetComponent<Image>().sprite = skin.box.normal.background;
		DialogGUIBase.SetUpTextObject(uiItem.GetChild("Text").GetComponent<TextMeshProUGUI>(), text, skin.label, skin);
		DialogGUIBase.SetUpTextObject(uiItem.GetChild("Placeholder").GetComponent<TextMeshProUGUI>(), placeholder, skin.label, skin);
		return base.Create(ref layouts, skin);
	}

	public override void Update()
	{
		base.Update();
		if (GetString != null && !field.isFocused)
		{
			field.text = GetString();
		}
	}
}
