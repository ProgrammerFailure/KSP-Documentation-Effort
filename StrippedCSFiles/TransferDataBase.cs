using System;
using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;

[Serializable]
public abstract class TransferDataBase : AppUI_Data
{
	public enum ManeuverCalculationState
	{
		waiting,
		calculating,
		failed,
		complete
	}

	public Callback dataChangedCallback;

	public Callback<ManeuverCalculationState, int> calculationStateChangedCallback;

	public double startUT;

	public int positionNodeIdx;

	public ManeuverNode currentPositionNode;

	public Orbit startingOrbit;

	public Vessel vessel;

	public double startBurnTime;

	public int calculationPercentage;

	private ManeuverCalculationState calculationState;

	public ManeuverCalculationState CalculationState
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
	public TransferDataBase(Callback dataChangedCallback, Callback<ManeuverCalculationState, int> calculationStateChangedCallback)
	{
		throw null;
	}

	public abstract string GetAlarmTitle();

	public abstract string GetAlarmDescription();

	public abstract bool IsAnyDropdownOpen(AppUIInputPanel panel);
}
