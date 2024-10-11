using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

[AppUI_Input]
public class AppUIMemberInput : AppUIMember
{
	public TMP_InputField input;

	internal ITypeParser parser;

	public TextMeshProUGUI suffixLabel;

	private RectTransform inputRect;

	private RectTransform suffixRect;

	private Vector2 inputOffsetMax;

	public bool showSuffix;

	[SerializeField]
	private string _suffixText;

	public float suffixWidth;

	public float suffixPadding;

	public string suffixText
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
	public AppUIMemberInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void RefreshUIInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputEdited(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override bool AnyTextFieldHasFocus()
	{
		throw null;
	}
}
