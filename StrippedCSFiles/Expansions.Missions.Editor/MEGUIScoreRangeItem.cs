using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIScoreRangeItem : MonoBehaviour
{
	public TMP_InputField minRangeInput;

	public TMP_InputField maxRangeInput;

	public TMP_InputField scoreInput;

	public Button removeButton;

	protected ScoreRange scoreRange;

	protected bool isDirty;

	protected MEGUI_ScoreRangeList attrib;

	protected Action<MEGUIScoreRangeItem, ScoreRange> OnDeleteItem;

	protected Action onUpdateValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIScoreRangeItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIScoreRangeItem Create(ScoreRange scoreRange, MEGUI_ScoreRangeList attrib, Transform parent, Action<MEGUIScoreRangeItem, ScoreRange> onDeleteItem, Action onUpdateValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Setup(ScoreRange scoreRange)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnInputValueChange(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnScoreInputEndEdit(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMaxRangeInputEndEdit(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMinRangeInputEndEdit(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private char OnValidatePercentageInput(string text, int charIndex, char addedChar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectMinRangeInput(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectMaxRangeInput(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnRemoveButton()
	{
		throw null;
	}
}
