using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class StageIconInfoBox : MonoBehaviour
{
	[SerializeField]
	protected Image titleBg;

	[SerializeField]
	protected TextMeshProUGUI title;

	[SerializeField]
	protected Slider slider;

	[SerializeField]
	protected Image sliderBg;

	[SerializeField]
	protected Image sliderFill;

	[SerializeField]
	protected TextMeshProUGUI caption;

	public float valueDifferenceBeforeSliderUpdate;

	private float lastSliderValue;

	[HideInInspector]
	public bool expanded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIconInfoBox()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Expand()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Collapse()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMsgTextColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMsgBgColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetProgressBarColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetProgressBarBgColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMessage(string m)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetValue(float value, float min, float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCaption(string cap)
	{
		throw null;
	}
}
