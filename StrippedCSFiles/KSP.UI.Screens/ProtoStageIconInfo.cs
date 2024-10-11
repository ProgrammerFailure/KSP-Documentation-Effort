using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class ProtoStageIconInfo
{
	protected ProtoStageIcon protoIcon;

	public StageIconInfoBox infoBoxRef;

	public Color msgBoxTextColor;

	public Color msgBoxBgColor;

	public string msg;

	public string pBarCaption;

	public Color pBarMainColor;

	public Color pBarBgColor;

	public float pBarValue;

	public ProtoStageIcon ProtoIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoStageIconInfo(ProtoStageIcon icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void StartInfoBox(StageIconInfoBox box)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLength(float l)
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
