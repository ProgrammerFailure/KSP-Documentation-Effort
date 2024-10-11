using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Expansions.Missions.Editor;

namespace Expansions.Missions;

public class ScoreModule_Modifier : ScoreModule
{
	[MEGUI_Dropdown(canBePinned = false, resetValue = "Addition", guiName = "#autoLOC_8100123")]
	public ScoreModifierType modifierType;

	[MEGUI_InputField(CharacterLimit = 7, ContentType = MEGUI_Control.InputContentType.DecimalNumber, resetValue = "0", guiName = "#autoLOC_8100124")]
	public float modifierScore;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule_Modifier()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScoreModule_Modifier(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetDisplayToolTip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override float AwardScore(float currentScore)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetNodeBodyParameterString(BaseAPField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ScoreDescription()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<string> GetDefaultPinnedParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool Equals(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetHashCode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Save(ConfigNode node)
	{
		throw null;
	}
}
