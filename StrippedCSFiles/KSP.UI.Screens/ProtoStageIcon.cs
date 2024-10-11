using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

[Serializable]
public class ProtoStageIcon
{
	[SerializeField]
	protected Part part;

	[SerializeField]
	protected StageIcon stageIcon;

	protected Color iconColor;

	protected Color backgroundColor;

	protected Color borderColor;

	protected DefaultIcons iconType;

	protected string customIconFilename;

	protected int customIconX;

	protected int customIconY;

	protected bool blinkBorder;

	protected float blinkInterval;

	protected bool frozen;

	protected bool highlighted;

	public Vector3 homePos;

	private List<ProtoStageIconInfo> infoBoxes;

	public Callback onStageIconDestroy;

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public StageIcon StageIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Highlighted
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoStageIcon(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StageIcon CreateIcon(bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveIcon(bool alertStagingSequencer = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(DefaultIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIcon(string file, int x, int y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetIconColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBorderColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBackgroundColor(Color c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BlinkBorder(float interval)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Highlight(bool highlightState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Freeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unfreeze()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoStageIconInfo DisplayInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveInfo(ProtoStageIconInfo iBox)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearInfoBoxes()
	{
		throw null;
	}
}
