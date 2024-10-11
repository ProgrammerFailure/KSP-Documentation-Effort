using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageGroupInfoItem : MonoBehaviour
{
	public HorizontalLayoutGroup hLayout;

	public TextMeshProUGUI infoName;

	public TextMeshProUGUI infoValue;

	public LayoutElement infoNameLayout;

	public LayoutElement infoValueLayout;

	private bool updateFunctionIsNull;

	private Func<DeltaVStageInfo, DeltaVSituationOptions, string> updateFunction;

	public string valueSuffix;

	public bool OnUpdateInEditor;

	public Func<DeltaVStageInfo, DeltaVSituationOptions, string> UpdateFunction
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
	public StageGroupInfoItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(DeltaVAppValues.InfoLine info, float panelWidth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateValue(DeltaVStageInfo stage, DeltaVSituationOptions situation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnUpdate(DeltaVStageInfo stage, DeltaVSituationOptions situation)
	{
		throw null;
	}
}
