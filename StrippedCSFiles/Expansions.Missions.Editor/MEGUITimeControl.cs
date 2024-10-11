using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUITimeControl : MonoBehaviour
{
	[Serializable]
	public class TimeControlValueChange : UnityEvent<double>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public TimeControlValueChange()
		{
			throw null;
		}
	}

	public Button timeButton;

	private TextMeshProUGUI timeButtonText;

	public TMP_InputField timeYears;

	public TMP_InputField timeDays;

	public TMP_InputField timeHours;

	public TMP_InputField timeMins;

	public TMP_InputField timeSecs;

	public GameObject timeEntrySection;

	[SerializeField]
	private LayoutElement paramLayoutElement;

	private float initialParamheight;

	public bool isDate;

	private KSPUtil.DefaultDateTimeFormatter dateFormatter;

	private double years;

	private double days;

	private double hours;

	private double minutes;

	private double seconds;

	[SerializeField]
	public TimeControlValueChange onValueChange;

	public double time
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUITimeControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateButtonText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTextValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateTimeFromInput(string value)
	{
		throw null;
	}
}
