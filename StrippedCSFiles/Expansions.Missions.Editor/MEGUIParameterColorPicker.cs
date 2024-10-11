using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ColorPicker]
public class MEGUIParameterColorPicker : MEGUIParameter
{
	public ColorPicker colorPicker;

	public Canvas colorControls;

	public Button colorPickerButton;

	protected GameObject blocker;

	public override bool IsInteractable
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

	public Color FieldValue
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
	public MEGUIParameterColorPicker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ResetDefaultValue(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPickColorButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnColorChange(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}
