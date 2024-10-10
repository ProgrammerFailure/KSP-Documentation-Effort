using System.Collections.Generic;

namespace PreFlightTests;

public abstract class DesignConcernBase : IDesignConcern
{
	public bool lastPassed;

	public bool firstTest = true;

	public event Callback<bool> TestResultChanged;

	event Callback<bool> IDesignConcern.testResultChanged
	{
		add
		{
			TestResultChanged += value;
		}
		remove
		{
			TestResultChanged -= value;
		}
	}

	public DesignConcernBase()
	{
	}

	public abstract bool TestCondition();

	public abstract string GetConcernTitle();

	public abstract string GetConcernDescription();

	public abstract DesignConcernSeverity GetSeverity();

	public virtual EditorFacilities GetEditorFacilities()
	{
		return EditorFacilities.flag_1;
	}

	public virtual List<Part> GetAffectedParts()
	{
		return null;
	}

	public bool GetPreviousResult()
	{
		return lastPassed;
	}

	public bool Test()
	{
		bool flag = TestCondition();
		if (firstTest)
		{
			if (this.TestResultChanged != null)
			{
				this.TestResultChanged(flag);
			}
			firstTest = false;
			lastPassed = flag;
		}
		else if (flag)
		{
			if (!lastPassed && this.TestResultChanged != null)
			{
				this.TestResultChanged(arg1: true);
			}
			lastPassed = true;
		}
		else
		{
			if (lastPassed && this.TestResultChanged != null)
			{
				this.TestResultChanged(arg1: false);
			}
			lastPassed = false;
		}
		return lastPassed;
	}
}
