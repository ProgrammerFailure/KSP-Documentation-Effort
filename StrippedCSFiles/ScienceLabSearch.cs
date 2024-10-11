using System.Runtime.CompilerServices;

public class ScienceLabSearch
{
	public enum SearchError
	{
		NONE,
		BUG,
		MIXED,
		NO_LABS,
		NO_MANNED,
		NO_SPACE,
		ALL_RESEARCHED,
		ALL_PROCESSING
	}

	public Vessel Vessel;

	public ScienceData Data;

	public ModuleScienceLab NextLabForData;

	public SearchError Error;

	public int LabsTotal;

	public int LabsManned;

	public int LabsWithSpace;

	public int LabsNotResearched;

	public int LabsNotProcessing;

	public bool HasAnyLabs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasMultipleLabs
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool NextLabForDataFound
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ErrorString
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double ScienceExpectation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double TimeExpectation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string DataExpectationSummary
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScienceLabSearch(Vessel vessel, ScienceData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PostErrorToScreen()
	{
		throw null;
	}
}
