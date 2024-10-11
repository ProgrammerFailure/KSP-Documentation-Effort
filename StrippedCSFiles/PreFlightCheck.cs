using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PreFlightTests;

public class PreFlightCheck
{
	private Callback OnComplete;

	private Callback OnAbort;

	private List<IPreFlightTest> tests;

	private int currentTest;

	private MultiOptionDialog warningDialog;

	private bool showWrongVesselTypeWarning;

	private bool runTestResult;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreFlightCheck(Callback onComplete, Callback onAbort)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddTest(IPreFlightTest test)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool RunTests()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void runNextTest()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Complete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Abort()
	{
		throw null;
	}
}
