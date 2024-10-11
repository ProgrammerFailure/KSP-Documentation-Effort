using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_ScoreRangeList]
public class MEGUIParameterScoreRangeList : MEGUIParameter
{
	public MEGUIScoreRangeItem scoreRangeItemPrefab;

	public MEGUIScoreRangeTimeItem scoreRangeTimeItemPrefab;

	public Button addButton;

	public TextMeshProUGUI minTitleLabel;

	public TextMeshProUGUI maxTitleLabel;

	public TextMeshProUGUI valueTitleLabel;

	protected List<MEGUIScoreRangeItem> scoreRangeList;

	private MEGUI_ScoreRangeList attrib;

	public List<ScoreRange> FieldValue
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
	public MEGUIParameterScoreRangeList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void AddScoreRange()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAddButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnRemoveScoreRange(MEGUIScoreRangeItem scoreRangeItem, ScoreRange scoreRange)
	{
		throw null;
	}
}
