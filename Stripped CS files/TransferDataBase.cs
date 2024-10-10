using System;
using ns11;
using ns2;

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

	public ManeuverCalculationState calculationState;

	public ManeuverCalculationState CalculationState
	{
		get
		{
			return calculationState;
		}
		set
		{
			calculationState = value;
			if (calculationStateChangedCallback != null)
			{
				calculationStateChangedCallback(value, calculationPercentage);
			}
		}
	}

	public TransferDataBase(Callback dataChangedCallback, Callback<ManeuverCalculationState, int> calculationStateChangedCallback)
	{
		this.dataChangedCallback = dataChangedCallback;
		this.calculationStateChangedCallback = calculationStateChangedCallback;
	}

	public abstract string GetAlarmTitle();

	public abstract string GetAlarmDescription();

	public abstract bool IsAnyDropdownOpen(AppUIInputPanel panel);
}
