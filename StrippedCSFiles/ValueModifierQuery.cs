using System.Runtime.CompilerServices;

public class ValueModifierQuery
{
	public enum TextStyling
	{
		None,
		OnGUI,
		EzGUIRichText,
		OnGUI_LessIsGood,
		EzGUIRichText_LessIsGood
	}

	private string valueId;

	private float input;

	private float delta;

	public string ValueId
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValueModifierQuery(string valueId, float v0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetInput()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddDelta(float val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetEffectDelta()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetEffectDeltaText(string format, TextStyling textStyle = TextStyling.None)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetEffectPercentageText(string format, TextStyling textStyle = TextStyling.None)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ValueModifierQuery RunQuery(string valueId, float input)
	{
		throw null;
	}
}
