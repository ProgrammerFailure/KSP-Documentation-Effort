using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

[MEGUI_GameParameters]
internal class MEGUIParameterGameParameters : MEGUIParameter, IMEHistoryTarget
{
	public Button openDifficultySettings;

	protected DifficultyOptionsMenu difficultyPrefab;

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

	public GameParameters FieldValue
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
	public MEGUIParameterGameParameters()
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
	protected void OnDifficultySettingsOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OpenDifficultySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnGameParametersChanged(GameParameters settings, bool changed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CopyDeleteStockCraft(string folder, bool copy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FileInfo[] getStockFiles(string stockPath, string fileType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool craftFileInUse(List<VesselSituation> vesselSituations, string fileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistoryValueChanged(ConfigNode data, HistoryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override ConfigNode GetState()
	{
		throw null;
	}
}
