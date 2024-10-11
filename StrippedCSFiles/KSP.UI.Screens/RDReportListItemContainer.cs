using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class RDReportListItemContainer : MonoBehaviour
{
	public UIStateButton button_report;

	public TextMeshProUGUI label_description;

	public TextMeshProUGUI label_science;

	public TextMeshProUGUI label_value;

	public Slider slider_value;

	public TextMeshProUGUI label_data;

	public GUISkin popupSkin;

	[NonSerialized]
	public string id;

	[NonSerialized]
	public string description;

	[NonSerialized]
	public float science;

	[NonSerialized]
	public float data;

	[NonSerialized]
	public float value;

	private PopupDialog scienceReport;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDReportListItemContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClosePopup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDescriptionLabel(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScienceLabel(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDataLabel(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValueSlider(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PushDataToGUI()
	{
		throw null;
	}
}
