using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PreFlightTests;

public abstract class DesignConcernBase : IDesignConcern
{
	private bool lastPassed;

	private bool firstTest;

	public event Callback<bool> TestResultChanged
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	private event Callback<bool> _0003
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected DesignConcernBase()
	{
		throw null;
	}

	public abstract bool TestCondition();

	public abstract string GetConcernTitle();

	public abstract string GetConcernDescription();

	public abstract DesignConcernSeverity GetSeverity();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual EditorFacilities GetEditorFacilities()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual List<Part> GetAffectedParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetPreviousResult()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool Test()
	{
		throw null;
	}
}
