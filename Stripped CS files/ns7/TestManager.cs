using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns7;

public class TestManager
{
	public static List<UnitTest> tests = new List<UnitTest>();

	public static HashSet<Type> types = new HashSet<Type>();

	public static bool HaveUnitTests => tests.Count > 0;

	public static void AddTest(UnitTest test)
	{
		tests.Add(test);
	}

	public static void ClearUnitTests()
	{
		tests.Clear();
		types.Clear();
	}

	public static TestResults RunTests()
	{
		if (HaveUnitTests)
		{
			TestResults testResults = new TestResults();
			PDebug.Log("RUNNING " + tests.Count + " UNIT TESTS");
			foreach (UnitTest test in tests)
			{
				foreach (TestState item in test.PerformTest())
				{
					if (item.Succeeded)
					{
						testResults.failed++;
					}
					else
					{
						testResults.success++;
					}
					testResults.states.Add(item);
				}
			}
			PDebug.Log($"UNIT TESTING COMPLETE: {testResults.failed} failures, {testResults.success} passes ({Math.Round((double)testResults.failed / (double)(testResults.failed + testResults.success) * 100.0)}% failed)");
			return testResults;
		}
		return new TestResults();
	}

	public static void AddTest(Type t)
	{
		if (types.Contains(t))
		{
			return;
		}
		PDebug.Log("TestManager: Adding unit test: " + t.FullName);
		types.Add(t);
		try
		{
			AddTest((UnitTest)t.GetConstructor(new Type[0]).Invoke(new object[0]));
		}
		catch (Exception message)
		{
			MonoBehaviour.print(message);
		}
	}
}
