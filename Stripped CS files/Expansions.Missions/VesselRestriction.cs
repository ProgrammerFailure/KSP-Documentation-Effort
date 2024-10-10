using Expansions.Missions.Runtime;
using ns2;
using ns9;
using PreFlightTests;
using TMPro;
using UnityEngine;

namespace Expansions.Missions;

public class VesselRestriction : DynamicModule
{
	public UIListItem UIEntry;

	public UIStateImage UIEntryState;

	public static double epsilon = 1E-15;

	public VesselRestriction()
	{
	}

	public VesselRestriction(MENode node)
		: base(node)
	{
	}

	public virtual void SuscribeToEvents()
	{
	}

	public virtual void ClearEvents()
	{
	}

	public override string GetDisplayName()
	{
		return Localizer.Format("#autoLOC_8100211");
	}

	public void AddAppUIReference(ref UIListItem listItem, ref UIStateImage listItemState)
	{
		UIEntry = listItem;
		UIEntryState = listItemState;
		UpdateAppEntry();
	}

	public void ClearAppUIREference()
	{
		UIEntry = null;
		UIEntryState = null;
	}

	public virtual void UpdateAppEntry()
	{
		if (UIEntry != null && UIEntryState != null)
		{
			UIEntry.GetComponentInChildren<TextMeshProUGUI>().text = GetStateMessage();
			int state = ((!IsComplete()) ? 1 : 0);
			UIEntryState.SetState(state);
			if (!IsComplete() && UIEntry.Data is MissionsAppVesselInfo missionsAppVesselInfo)
			{
				missionsAppVesselInfo.vesselSituation.readyToLaunch = false;
				MissionsApp.Instance.SetVesselItemReadyIcon(missionsAppVesselInfo.vesselSituation, missionsAppVesselInfo);
			}
		}
	}

	public virtual string GetStateMessage()
	{
		return Localizer.Format("#autoLOC_8100212", Time.time);
	}

	public virtual bool IsComplete()
	{
		return true;
	}

	public bool SameComparatorWrapper(VesselRestriction otherRestriction)
	{
		if (GetType() == otherRestriction.GetType())
		{
			return SameComparator(otherRestriction);
		}
		return false;
	}

	public virtual bool SameComparator(VesselRestriction otherRestriction)
	{
		return true;
	}

	public virtual IPreFlightTest GetPreflightCheck()
	{
		return new MissionPreflightCheck(this);
	}

	public static bool TestFloat(float currentValue, float targetValue, TestComparisonLessGreaterEqual comparisonOperator)
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => currentValue <= targetValue, 
			TestComparisonLessGreaterEqual.Equal => (double)Mathf.Abs(targetValue - currentValue) < epsilon, 
			TestComparisonLessGreaterEqual.GreaterOrEqual => currentValue >= targetValue, 
			_ => true, 
		};
	}

	public static bool TestInt(int currentValue, int targetValue, TestComparisonLessGreaterEqual comparisonOperator)
	{
		return comparisonOperator switch
		{
			TestComparisonLessGreaterEqual.LessOrEqual => currentValue <= targetValue, 
			TestComparisonLessGreaterEqual.Equal => targetValue - currentValue == 0, 
			TestComparisonLessGreaterEqual.GreaterOrEqual => currentValue >= targetValue, 
			_ => true, 
		};
	}
}
