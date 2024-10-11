using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

[AppUI_InputDateTime]
public class AppUIMemberDateTime : AppUIMember
{
	public enum DisplayModes
	{
		datetime,
		ut
	}

	public enum DateTimeModes
	{
		date,
		timespan
	}

	public TMP_InputField yInput;

	public TMP_InputField dInput;

	public TMP_InputField hInput;

	public TMP_InputField mInput;

	public TMP_InputField sInput;

	public TMP_InputField utInput;

	public GameObject dateTimeComponents;

	public GameObject utComponents;

	internal ITypeParser parser;

	private DisplayModes displayMode;

	private DateTimeModes datetimeMode;

	private IDateTimeFormatter dateFormatter;

	private int year;

	private int day;

	private int hour;

	private int min;

	private int sec;

	public DisplayModes DisplayMode
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

	public DateTimeModes DatetimeMode
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

	public IDateTimeFormatter DateFormatter
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
	public AppUIMemberDateTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnRefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMode(DisplayModes newMode)
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
