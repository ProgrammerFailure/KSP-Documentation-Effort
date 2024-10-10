using System.Collections.Generic;
using ns9;
using PreFlightTests;
using UnityEngine;

public class PreFlightCheck
{
	public Callback OnComplete;

	public Callback OnAbort;

	public List<IPreFlightTest> tests;

	public int currentTest;

	public MultiOptionDialog warningDialog;

	public bool showWrongVesselTypeWarning;

	public bool runTestResult;

	public PreFlightCheck(Callback onComplete, Callback onAbort)
	{
		OnComplete = onComplete;
		OnAbort = onAbort;
		tests = new List<IPreFlightTest>();
		showWrongVesselTypeWarning = GameSettings.SHOW_WRONG_VESSEL_TYPE_CONFIRMATION;
	}

	public void AddTest(IPreFlightTest test)
	{
		tests.Add(test);
	}

	public bool RunTests()
	{
		currentTest = -1;
		runTestResult = tests.Count == 0;
		if (tests.Count > 0)
		{
			runNextTest();
		}
		return runTestResult;
	}

	public void runNextTest()
	{
		currentTest++;
		if (currentTest < tests.Count)
		{
			string name = tests[currentTest].GetType().Name;
			bool flag = name == typeof(WrongVesselTypeForLaunchSite).Name;
			if (tests[currentTest].Test())
			{
				Debug.Log("[Pre-Flight Check]: Checking for " + name + ": PASS!");
				runNextTest();
				return;
			}
			if (flag && !GameSettings.SHOW_WRONG_VESSEL_TYPE_CONFIRMATION)
			{
				Debug.Log("[Pre-Flight Check]: Checking for " + name + ": SKIPPED WARNING!");
				runNextTest();
				return;
			}
			if (tests[currentTest].GetProceedOption() != null)
			{
				if (flag)
				{
					warningDialog = new MultiOptionDialog("TestWarning", tests[currentTest].GetWarningDescription(), tests[currentTest].GetWarningTitle(), HighLogic.UISkin, 320f, new DialogGUISpace(5f), new DialogGUIToggle(() => !showWrongVesselTypeWarning, Localizer.Format("#autoLOC_6005014"), delegate
					{
						showWrongVesselTypeWarning = !showWrongVesselTypeWarning;
					}, 120f), new DialogGUIButton(tests[currentTest].GetProceedOption(), delegate
					{
						if (GameSettings.SHOW_WRONG_VESSEL_TYPE_CONFIRMATION != showWrongVesselTypeWarning)
						{
							GameSettings.SHOW_WRONG_VESSEL_TYPE_CONFIRMATION = showWrongVesselTypeWarning;
							GameSettings.SaveSettings();
						}
						runNextTest();
					}), new DialogGUIButton(tests[currentTest].GetAbortOption(), delegate
					{
						showWrongVesselTypeWarning = GameSettings.SHOW_WRONG_VESSEL_TYPE_CONFIRMATION;
						Abort();
					}));
				}
				else
				{
					warningDialog = new MultiOptionDialog("TestWarning", tests[currentTest].GetWarningDescription(), tests[currentTest].GetWarningTitle(), HighLogic.UISkin, 320f, new DialogGUISpace(5f), new DialogGUIButton(tests[currentTest].GetProceedOption(), runNextTest), new DialogGUIButton(tests[currentTest].GetAbortOption(), Abort));
				}
			}
			else
			{
				warningDialog = new MultiOptionDialog("TestWarning", tests[currentTest].GetWarningDescription(), tests[currentTest].GetWarningTitle(), HighLogic.UISkin, 320f, new DialogGUIButton(tests[currentTest].GetAbortOption(), Abort));
			}
			PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), warningDialog, persistAcrossScenes: false, HighLogic.UISkin).OnDismiss = Abort;
			Debug.Log("[Pre-Flight Check]: Checking for " + name + ": FAIL!");
		}
		else
		{
			Complete();
			runTestResult = true;
		}
	}

	public void Complete()
	{
		Debug.Log("[Pre-Flight Check]: All Checks Complete. Go for Launch!");
		OnComplete();
	}

	public void Abort()
	{
		Debug.Log("[Pre-Flight Check]: Launch Aborted.");
		OnAbort();
	}
}
