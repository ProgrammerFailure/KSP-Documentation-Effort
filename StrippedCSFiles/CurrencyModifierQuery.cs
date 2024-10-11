using System;
using System.Runtime.CompilerServices;

public class CurrencyModifierQuery
{
	public enum TextStyling
	{
		None,
		OnGUI,
		EzGUIRichText,
		OnGUI_LessIsGood,
		EzGUIRichText_LessIsGood
	}

	public TransactionReasons reason;

	private float inputFunds;

	private float inputScience;

	private float inputRep;

	private float deltaFunds;

	private float deltaScience;

	private float deltaRep;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CurrencyModifierQuery(TransactionReasons reason, float f0, float s0, float r0)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetInput(Currency c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddDelta(Currency c, float val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetEffectDelta(Currency c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetTotal(Currency c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetCostLine(bool displayInverted = true, bool useCurrencyColors = false, bool useInsufficientCurrencyColors = true, bool includePercentage = false, string seperator = ", ")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanAfford(Action<Currency> onInsufficientCurrency = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool CanAfford(Currency c)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetEffectDeltaText(Currency c, string format, TextStyling textStyle = TextStyling.None)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetEffectPercentageText(Currency c, string format, TextStyling textStyle = TextStyling.None)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CurrencyModifierQuery RunQuery(TransactionReasons reason, float f0, float s0, float r0)
	{
		throw null;
	}
}
