using System;
using System.Runtime.CompilerServices;

[Serializable]
public class ExperimentResultDialogPage
{
	public string title;

	public string resultText;

	public float dataSize;

	public float refValue;

	public float scienceValueRatio;

	public float scienceValue;

	public float baseTransmitValue;

	public float TransmitBonus;

	public float remainingScience;

	public float valueAfterRecovery;

	public float valueAfterTransmit;

	public float xmitDataScalar;

	public bool showTransmitWarning;

	public string transmitWarningMessage;

	public bool showReset;

	public ScienceLabSearch labSearch;

	public float CommBonus;

	public Callback<ScienceData> OnDiscardData;

	public Callback<ScienceData> OnKeepData;

	public Callback<ScienceData> OnTransmitData;

	public Callback<ScienceData> OnSendToLab;

	public Part host;

	public ScienceData pageData;

	public static string[] sandboxResults;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperimentResultDialogPage(Part host, ScienceData experimentData, float xmitBase, float xmitBonus, bool showTransmitWarning, string transmitWarningMessage, bool showResetOption, ScienceLabSearch labSearch, Callback<ScienceData> onDiscardData, Callback<ScienceData> onKeepData, Callback<ScienceData> onTransmitData, Callback<ScienceData> onSendToLab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ExperimentResultDialogPage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float UpdatePageLabValue()
	{
		throw null;
	}
}
