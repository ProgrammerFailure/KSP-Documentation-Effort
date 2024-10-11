using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DialogGUITextInput : DialogGUIBase
{
	public Func<string, string> onTextUpdated;

	public Func<string> GetString;

	public string text;

	public string placeholder;

	public bool multiLine;

	public int maxLength;

	private TMP_InputField field;

	private TMP_InputField.ContentType? contentType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, float hght = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, float w, float hght)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUITextInput(string txt, string placeHlder, bool multiline, int maxlength, Func<string, string> textSetFunc, float hght = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUITextInput(string txt, string placeHlder, bool multiline, int maxlength, Func<string, string> textSetFunc, float w, float hght)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUITextInput(string txt, bool multiline, int maxlength, Func<string, string> textSetFunc, Func<string> getString, TMP_InputField.ContentType contentType, float hght = -1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Update()
	{
		throw null;
	}
}
