using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using ns11;
using ns2;
using ns9;
using UnityEngine;

[Serializable]
public class TransferTypeSimple : TransferTypeBase
{
	public bool ControlsSetup;

	public bool topFrameSetup;

	public bool prefabsSetup;

	public ManeuverToolUIFrame appFrame;

	public bool currentSituationValid;

	public SafeAbortBackgroundWorker bw;

	public bool running;

	public TransferDataSimple transferDataSimple;

	public double earliestStartUT;

	public double porkChopWidth = 292.0;

	public double porkChopHeight = 292.0;

	public double lastTopFrameUpdate = -1.0;

	public ManeuverToolCBContainer activeSourceBody;

	public ManeuverToolCBContainer activeTargetBody;

	public ManeuverToolCBContainer activeParentBody;

	public GameObject ellipseOne;

	public GameObject ellipseTwo;

	public GameObject arcOne;

	public GameObject arcTwo;

	public ManeuverToolVisualTextBox departureBox;

	public ManeuverToolVisualTextBox departuredV;

	public ManeuverToolVisualTextBox arrivalBox;

	public ManeuverToolVisualTextBox arrivaldV;

	public ManeuverToolVisualTextBox durationBox;

	public Vector2 departureBoxPos = new Vector2(-123.5f, -30f);

	public Vector2 departuredVPos = new Vector2(-123.5f, -76.3f);

	public Vector2 arrivalBoxPos = new Vector2(-49f, 198f);

	public Vector2 arrivaldVPos = new Vector2(-49f, 152.4f);

	public Vector2 durationBoxPos = new Vector2(124f, -169.5f);

	public double workerRunningTime;

	public double workerStalledTime;

	public bool foundIntercept;

	public CelestialBody previousSourceBody;

	public CelestialBody previousTargetBody;

	public bool previousCircularize;

	public double previousCircPe;

	public bool restartCalcs;

	public bool completeDrawRequired;

	public double lastCalcTime;

	public int previousTargetBodyIndex = -1;

	public double topFrameUpdateIntervalSecs = 60.0;

	public new TransferDataSimple currentSelectedTransfer
	{
		get
		{
			return base.currentSelectedTransfer as TransferDataSimple;
		}
		set
		{
			base.currentSelectedTransfer = value;
			base.currentSelectedTransfer.dataChangedCallback = UpdateTransferData;
		}
	}

	public TransferDataTopDataBase currentSelectedTransferTopData
	{
		get
		{
			return currentSelectTopData;
		}
		set
		{
			currentSelectTopData = value;
			currentSelectTopData.dataChangedCallback = UpdateTransferData;
		}
	}

	public void OnVisualizer(bool state)
	{
		if (ManeuverTool.Instance.currentSelectedTransferType == this)
		{
			UpdateVisualPanel();
		}
	}

	~TransferTypeSimple()
	{
		appFrame.RemoveOnSidePanelClicked(OnVisualizer);
		AppUIMember control = appFrame.inputPanel.GetControl("targetBodyName");
		if (control != null)
		{
			AppUIMemberDropdown appUIMemberDropdown = control as AppUIMemberDropdown;
			if (appUIMemberDropdown != null)
			{
				appUIMemberDropdown.dropdown.onItemSelected.RemoveListener(OnTargetClicked);
			}
		}
	}

	public override string DisplayName()
	{
		return Localizer.Format("#autoLOC_6002648");
	}

	public override bool ChangePosition(ManeuverNode node, out string errorString)
	{
		errorString = "";
		if (currentSelectedTransfer == null)
		{
			return false;
		}
		if ((currentSelectedTransfer.currentPositionNode == null && node == null) || currentSelectedTransfer.currentPositionNode == node)
		{
			if (node == null)
			{
				currentSelectedTransfer.currentPositionNode = null;
			}
			return true;
		}
		currentSelectedTransfer.currentPositionNode = node;
		if (!ValidSituation(out errorString))
		{
			return false;
		}
		if (appFrame != null && currentSelectedTransfer.vessel != null && currentSelectedTransfer.startingOrbit != null)
		{
			DrawTypeControls(appFrame, DrawReason.TransferTypeChanged);
			RefreshUIControls();
		}
		return true;
	}

	public override void ChangeTime()
	{
		UpdateTransferData();
	}

	public override void DrawTypeControls(ManeuverToolUIFrame appFrame, DrawReason reason)
	{
		this.appFrame = appFrame;
		bool flag = !prefabsSetup;
		if (!prefabsSetup)
		{
			appFrame.AddOnSidePanelClicked(OnVisualizer);
			SetupVisualPrefabs();
		}
		if (currentSelectedTransfer == null)
		{
			Callback<TransferDataBase.ManeuverCalculationState, int> calculationStateChangedCallback = null;
			if (ManeuverTool.Instance != null && ManeuverTool.Instance.appFrame != null)
			{
				calculationStateChangedCallback = ManeuverTool.Instance.appFrame.SetCalculationState;
			}
			currentSelectedTransfer = new TransferDataSimple(UpdateTransferData, calculationStateChangedCallback);
			transferDataSimple = currentSelectedTransfer;
		}
		currentSelectedTransfer.vessel = FlightGlobals.ActiveVessel;
		if (currentSelectedTransfer.currentPositionNode != null)
		{
			currentSelectedTransfer.startUT = currentSelectedTransfer.currentPositionNode.nextPatch.StartUT;
			currentSelectedTransfer.positionNodeIdx = currentSelectedTransfer.vessel.patchedConicSolver.FindManeuverIndex(currentSelectedTransfer.currentPositionNode);
		}
		else
		{
			currentSelectedTransfer.startUT = 0.0;
			currentSelectedTransfer.positionNodeIdx = 0;
		}
		earliestStartUT = ((currentSelectedTransfer.startUT <= 0.0) ? Planetarium.GetUniversalTime() : currentSelectedTransfer.startUT);
		if (currentSelectedTransfer.currentPositionNode != null)
		{
			currentSelectedTransfer.startingOrbit = currentSelectedTransfer.currentPositionNode.nextPatch;
		}
		else
		{
			currentSelectedTransfer.startingOrbit = currentSelectedTransfer.vessel.patchedConicSolver.FindPatchContainingUT(earliestStartUT);
		}
		if (!topFrameSetup || currentSelectedTransferTopData == null)
		{
			currentSelectedTransferTopData = new TransferDataSimpleTopData(UpdateTopData);
			appFrame.SetTopPanelHeader(Localizer.Format("#autoLOC_6002693"), "", Localizer.Format("#autoLOC_6002691"), Localizer.Format("#autoLOC_6002692"));
			appFrame.topPanelData.Setup(currentSelectedTransferTopData, UpdateTopData);
			topFrameSetup = true;
		}
		if (reason == DrawReason.TransferTypeChanged || reason == DrawReason.VesselChanged || currentSelectedTransfer.cbItems == null || currentSelectedTransfer.cbItems.Count < 1)
		{
			SetupCBNames();
			previousSourceBody = null;
		}
		if (!(reason == DrawReason.AppShown || flag))
		{
			if (running)
			{
				restartCalcs = true;
				completeDrawRequired = true;
			}
			else
			{
				UpdateTransferData();
				lastTopFrameUpdate = -1.0;
				UpdateTopData();
			}
		}
	}

	public void CompleteDrawControls()
	{
		workerRunningTime = 0.0;
		running = false;
		UpdateTransferData();
		lastTopFrameUpdate = -1.0;
		UpdateTopData();
	}

	public override void UpdateTransferData()
	{
		if (currentSelectedTransfer == null)
		{
			Callback<TransferDataBase.ManeuverCalculationState, int> calculationStateChangedCallback = null;
			if (ManeuverTool.Instance != null && ManeuverTool.Instance.appFrame != null)
			{
				calculationStateChangedCallback = ManeuverTool.Instance.appFrame.SetCalculationState;
			}
			currentSelectedTransfer = new TransferDataSimple(UpdateTransferData, calculationStateChangedCallback);
		}
		if (previousSourceBody != null && currentSelectedTransfer.SourceBody != null && previousSourceBody.name == currentSelectedTransfer.SourceBody.name && previousTargetBody != null && currentSelectedTransfer.TargetBody != null && previousTargetBody.name == currentSelectedTransfer.TargetBody.name && Planetarium.GetUniversalTime() - lastCalcTime < 5.0)
		{
			return;
		}
		previousSourceBody = currentSelectedTransfer.SourceBody;
		previousTargetBody = currentSelectedTransfer.TargetBody;
		lastCalcTime = Planetarium.GetUniversalTime();
		string errorString = "";
		currentSituationValid = ValidSituation(out errorString);
		if (currentSituationValid)
		{
			SetSourceBody(UpdateData: false);
			if (currentSelectedTransfer.TargetBody != null && ControlsSetup)
			{
				currentSelectedTransfer.transferPe = TransferMath.SafeOrbitRadius(currentSelectedTransfer.TargetBody) * 2.0 - currentSelectedTransfer.TargetBody.Radius;
			}
			UpdateVisualPanel();
			DoMathCalcs();
		}
	}

	public void DoMathCalcs()
	{
		string errorString = "";
		if (ValidSituation(out errorString) && !appFrame.ErrorState && bw == null && !running && FlightGlobals.ActiveVessel != null && currentSelectedTransfer.CalculationState != TransferDataBase.ManeuverCalculationState.calculating)
		{
			transferDataSimple = currentSelectedTransfer;
			transferDataSimple.vessel = FlightGlobals.ActiveVessel;
			earliestStartUT = ((currentSelectedTransfer.startUT <= 0.0) ? Planetarium.GetUniversalTime() : currentSelectedTransfer.startUT);
			if (currentSelectedTransfer.currentPositionNode != null)
			{
				currentSelectedTransfer.startingOrbit = currentSelectedTransfer.currentPositionNode.nextPatch;
			}
			else
			{
				currentSelectedTransfer.startingOrbit = currentSelectedTransfer.vessel.patchedConicSolver.FindPatchContainingUT(earliestStartUT);
			}
			running = true;
			transferDataSimple.calculationPercentage = 0;
			transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.waiting;
			workerRunningTime = Planetarium.GetUniversalTime();
			appFrame.SetCreateManeuverButton(interactable: false);
			bw = new SafeAbortBackgroundWorker();
			bw.WorkerReportsProgress = true;
			bw.WorkerSupportsCancellation = true;
			bw.DoWork += bw_CalculateTransfer;
			bw.RunWorkerCompleted += bw_ProcessCompleted;
			bw.ProgressChanged += bw_ProgressChanged;
			bw.RunWorkerAsync();
		}
	}

	public void bw_ProcessCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error == null && !e.Cancelled)
		{
			transferDataSimple.calculationPercentage = 100;
			transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.complete;
			CalculationComplete();
		}
		else
		{
			if (transferDataSimple != null)
			{
				transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
			}
			appFrame.SetCreateManeuverButton(interactable: false);
			appFrame.SetHelpString("#autoLOC_6002697");
		}
		bw = null;
		running = false;
		workerRunningTime = 0.0;
		lastCalcTime = Planetarium.GetUniversalTime();
	}

	public void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		if (e.ProgressPercentage == -1)
		{
			if ((bool)ManeuverTool.Instance && e.UserState is string)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.Log(e.UserState as string);
				});
			}
		}
		else if (e.ProgressPercentage != transferDataSimple.calculationPercentage)
		{
			transferDataSimple.calculationPercentage = e.ProgressPercentage;
			transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.calculating;
		}
	}

	public void bw_CalculateTransfer(object sender, DoWorkEventArgs e)
	{
		try
		{
			running = true;
			if (transferDataSimple != null && !(transferDataSimple.vessel == null) && transferDataSimple.startingOrbit != null && !(transferDataSimple.TargetBody == null) && !(transferDataSimple.SourceBody == null))
			{
				transferDataSimple.calculationPercentage = 0;
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.calculating;
				});
				Orbit startingOrbit = transferDataSimple.startingOrbit;
				Orbit orbit = transferDataSimple.TargetBody.orbit;
				transferDataSimple.optimalPhaseAngleTime = TransferMath.AlignmentTime(transferDataSimple, (startingOrbit.referenceBody.orbit != null) ? startingOrbit.referenceBody.orbit : startingOrbit, orbit, earliestStartUT);
				double num = earliestStartUT;
				double num2 = Math.Abs(1.0 / (1.0 / orbit.period - 1.0 / ((startingOrbit.referenceBody.orbit != null) ? startingOrbit.referenceBody.orbit.period : startingOrbit.period)));
				double num3 = Math.Min(2.0 * num2, 2.0 * ((startingOrbit.referenceBody.orbit != null) ? startingOrbit.referenceBody.orbit.period : startingOrbit.period));
				if (TransferMath.SameSOITransfer(transferDataSimple))
				{
					transferDataSimple.startBurnTime = num + transferDataSimple.optimalPhaseAngleTime;
					if (TransferMath.CalcSameSOITransferDV(transferDataSimple, startingOrbit) == 0.0)
					{
						foundIntercept = false;
						e.Cancel = true;
					}
					else if (!DidWeIntercept(bw, ref transferDataSimple))
					{
						foundIntercept = false;
					}
					else
					{
						foundIntercept = true;
						TransferMath.NoBurnRequired(transferDataSimple);
					}
					return;
				}
				double num4 = num + transferDataSimple.optimalPhaseAngleTime;
				double num5 = double.MaxValue;
				double transferTime = double.MaxValue;
				double num6 = TransferMath.HohmannTimeOfFlight(startingOrbit.referenceBody.isStar ? startingOrbit : startingOrbit.referenceBody.orbit, orbit);
				double num7 = Math.Max(num6 - orbit.period, num6 / 2.0);
				double num8 = num7 + Math.Min(2.0 * orbit.period, num6);
				double num9 = num8 - num7;
				double num10 = num3 / (porkChopWidth - 1.0);
				double num11 = num9 / (porkChopHeight - 1.0);
				for (int i = 0; (double)i < porkChopWidth; i++)
				{
					for (double num12 = 0.0; num12 < porkChopHeight; num12 += 1.0)
					{
						transferDataSimple.transferTime = num8 - num12 * num11;
						double num13 = num + (double)i * num10;
						double num14 = TransferMath.TransferDV(transferDataSimple, num13);
						if (!num14.Equals(-1.0) && num14 < num5)
						{
							num4 = num13;
							num5 = num14;
							transferTime = transferDataSimple.transferTime;
						}
						if (bw == null || bw.CancellationPending)
						{
							e.Cancel = true;
							return;
						}
					}
					bw.ReportProgress((int)Math.Truncate((double)i / porkChopWidth * 100.0));
				}
				if (num5.Equals(double.MaxValue))
				{
					num4 = num + transferDataSimple.optimalPhaseAngleTime;
				}
				transferDataSimple.transferTime = transferTime;
				transferDataSimple.startBurnTime = num4;
				TransferMath.TransferDV(transferDataSimple, num4);
				if (FinalizeTransferDV(transferDataSimple, num4, bw).Equals(-1.0))
				{
					e.Cancel = true;
				}
			}
			else
			{
				e.Cancel = true;
			}
		}
		catch (ThreadAbortException)
		{
		}
		catch (Exception ex2)
		{
			e.Result = ex2.Message + "\n" + ex2.StackTrace;
			e.Cancel = true;
		}
	}

	public double FinalizeTransferDV(TransferDataSimple transferDataSimple, double startUT, SafeAbortBackgroundWorker bw)
	{
		try
		{
			Orbit startingOrbit = transferDataSimple.startingOrbit;
			Orbit orbit = transferDataSimple.TargetBody.GetOrbit();
			if (TransferMath.FinalizeLambert(transferDataSimple, startUT, startingOrbit, orbit).Equals(-1.0))
			{
				return -1.0;
			}
			if (transferDataSimple.ejectAngleRetrograde)
			{
				transferDataSimple.ejectAngle = -Math.PI + transferDataSimple.ejectAngle;
			}
			double num = double.MaxValue;
			double num2 = startUT;
			double num3 = startUT;
			OrbitUtil.CurrentEjectionAngle(transferDataSimple.startingOrbit, num3, startingOrbit.referenceBody.orbit.getOrbitalVelocityAtUT(num3));
			OrbitUtil.CurrentEjectionAngle(transferDataSimple.startingOrbit, num3 + (double)(int)transferDataSimple.startingOrbit.period, startingOrbit.referenceBody.orbit.getOrbitalVelocityAtUT(num3));
			int num4 = 0;
			while (true)
			{
				if (num4 < (int)transferDataSimple.startingOrbit.period)
				{
					double num5 = num3 + (double)num4;
					double num6 = OrbitUtil.CurrentEjectionAngle(transferDataSimple.startingOrbit, num5, startingOrbit.referenceBody.orbit.getOrbitalVelocityAtUT(num3)) * (Math.PI / 180.0);
					if (Math.Abs(num6 - transferDataSimple.ejectAngle) < num)
					{
						num = Math.Abs(num6 - transferDataSimple.ejectAngle);
						num2 = num5;
					}
					if (bw == null || bw.CancellationPending)
					{
						break;
					}
					num4++;
					continue;
				}
				startUT = num2;
				if (TransferMath.FinalizeLambert(transferDataSimple, startUT, startingOrbit, orbit).Equals(-1.0))
				{
					return -1.0;
				}
				if (!DidWeIntercept(bw, ref transferDataSimple))
				{
					if (bw == null || bw.CancellationPending)
					{
						return -1.0;
					}
					if (!TransferMath.CalculateCorrection(transferDataSimple, startUT, bw))
					{
						foundIntercept = false;
					}
					else
					{
						foundIntercept = true;
						TransferMath.NoBurnRequired(transferDataSimple);
					}
				}
				else
				{
					foundIntercept = true;
					TransferMath.NoBurnRequired(transferDataSimple);
				}
				transferDataSimple.circularizedV = 0.0;
				return transferDataSimple.transferdV.magnitude + transferDataSimple.circularizedV + transferDataSimple.correctiondV.magnitude;
			}
			return -1.0;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return -1.0;
		}
	}

	public bool DidWeIntercept(SafeAbortBackgroundWorker bw, ref TransferDataSimple transferDataSimple)
	{
		try
		{
			Orbit intercept = new Orbit();
			double closestApproach = double.MaxValue;
			Vector3d vector3d = transferDataSimple.transferdV;
			Orbit orbit = ((transferDataSimple.currentPositionNode == null) ? transferDataSimple.vessel.patchedConicSolver.FindPatchContainingUT(transferDataSimple.startBurnTime) : transferDataSimple.currentPositionNode.nextPatch);
			bool flag;
			if (flag = TransferMath.SameSOITransfer(transferDataSimple))
			{
				vector3d = new Vector3d(0.0, 0.0, transferDataSimple.transferdV.magnitude);
			}
			bool flag2 = TransferMath.InterceptBody(bw, orbit, transferDataSimple.TargetBody, vector3d, transferDataSimple.startBurnTime, 5, out intercept, out closestApproach);
			if (bw != null && !bw.CancellationPending)
			{
				if (!flag2 && flag)
				{
					TransferMath.CalcSameSOITransferDV(transferDataSimple, orbit, transferDataSimple.otherNodeTime);
					vector3d = new Vector3d(0.0, 0.0, transferDataSimple.transferdV.magnitude);
					flag2 = TransferMath.InterceptBody(bw, orbit, transferDataSimple.TargetBody, vector3d, transferDataSimple.startBurnTime, 5, out intercept, out closestApproach);
				}
				double num = TransferMath.SafeOrbitRadius(transferDataSimple.TargetBody);
				if (flag2 && closestApproach < num)
				{
					int num2 = 10;
					while (closestApproach < num && num2 > 0)
					{
						vector3d = new Vector3d(vector3d.x, vector3d.y, vector3d.z - (double)GameSettings.MANEUVER_TOOL_CB_COLLISION_ADJUSTMENT);
						flag2 = TransferMath.InterceptBody(bw, orbit, transferDataSimple.TargetBody, vector3d, transferDataSimple.startBurnTime, 5, out intercept, out closestApproach);
						num2--;
						if (bw == null || bw.CancellationPending)
						{
							return false;
						}
					}
					transferDataSimple.transferdV = vector3d;
				}
				if (!flag2 && !orbit.referenceBody.isStar && (flag2 = TransferMath.FindIntercept(transferDataSimple, orbit, transferDataSimple.TargetBody, vector3d, transferDataSimple.startBurnTime, flag, closestApproach, bw, out var adjustedDV)))
				{
					transferDataSimple.transferdV = adjustedDV;
				}
				return flag2;
			}
			return false;
		}
		catch (Exception ex)
		{
			Exception ex2 = ex;
			Exception e = ex2;
			if ((bool)ManeuverTool.Instance)
			{
				ManeuverTool.Instance.InvokeAsync(delegate
				{
					Debug.LogWarning("[TransferMath]: Exception: " + e.Message + "\n" + e.StackTrace);
				});
			}
			return false;
		}
	}

	public void CalculationComplete()
	{
		if (currentSelectedTransfer.CalculationState == TransferDataBase.ManeuverCalculationState.complete)
		{
			ManeuverTool.Instance.appFrame.UpdateDVText((currentSelectedTransfer.correctiondV.magnitude + currentSelectedTransfer.transferdV.magnitude).ToString("N0") + Localizer.Format("#autoLOC_7001415"));
			if (departuredV != null)
			{
				departuredV.SetValueText((currentSelectedTransfer.correctiondV.magnitude + currentSelectedTransfer.transferdV.magnitude).ToString("N0") + Localizer.Format("#autoLOC_7001415"));
			}
			double num = TransferMath.CalcCircularizeDV(currentSelectedTransfer, currentSelectedTransfer.transferPe);
			currentSelectedTransfer.lowCircularizationDv = Localizer.Format("#autoLOC_6002667", num.ToString("N2"), (currentSelectedTransfer.transferPe / 1000.0).ToString("N0"));
			double num2 = TransferMath.SafeOrbitRadius(currentSelectedTransfer.TargetBody) * 100.0;
			if (num2 > currentSelectedTransfer.TargetBody.sphereOfInfluence)
			{
				num2 = currentSelectedTransfer.TargetBody.sphereOfInfluence / 2.0;
			}
			num = TransferMath.CalcCircularizeDV(currentSelectedTransfer, num2);
			currentSelectedTransfer.highCircularizationDv = Localizer.Format("#autoLOC_6002667", num.ToString("N2"), (num2 / 1000.0).ToString("N0"));
			if (arrivaldV != null)
			{
				arrivaldV.gameObject.SetActive(value: true);
				arrivaldV.SetValueText(currentSelectedTransfer.circularizedV.ToString("N0") + Localizer.Format("#autoLOC_7001415"));
			}
			appFrame.SetCreateManeuverButton(interactable: true);
			if (foundIntercept)
			{
				appFrame.SetHelpString("");
			}
			else
			{
				appFrame.SetHelpString("#autoLOC_6002698");
			}
		}
		else
		{
			ManeuverTool.Instance.appFrame.UpdateDVText("");
			if (departuredV != null)
			{
				departuredV.SetValueText("");
			}
			appFrame.SetCreateManeuverButton(interactable: false);
			appFrame.SetHelpString("#autoLOC_6002697");
		}
		transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.complete;
		ManeuverTool.Instance.StartCoroutine(CallbackUtil.DelayedCallback(2, RefreshUIControls));
	}

	public override bool CreateManeuver(out ManeuverNode node, out string alarmTitle, out string alarmDesc)
	{
		node = null;
		alarmTitle = "";
		alarmDesc = "";
		if (FlightGlobals.ActiveVessel != null && currentSelectedTransfer != null)
		{
			if (currentSelectedTransfer.CalculationState != TransferDataBase.ManeuverCalculationState.complete)
			{
				return false;
			}
			alarmTitle = currentSelectedTransfer.GetAlarmTitle();
			alarmDesc = currentSelectedTransfer.GetAlarmDescription();
			if (FlightGlobals.ActiveVessel.patchedConicSolver != null)
			{
				node = FlightGlobals.ActiveVessel.patchedConicSolver.AddManeuverNode(currentSelectedTransfer?.startBurnTime ?? 0.0);
			}
			if (node != null)
			{
				if (TransferMath.SameSOITransfer(currentSelectedTransfer))
				{
					if (TransferMath.ChangingPeriapsis(currentSelectedTransfer.startingOrbit, currentSelectedTransfer.TargetBody.orbit, node.double_0))
					{
						node.DeltaV = new Vector3d(0.0, 0.0, 0.0 - currentSelectedTransfer.transferdV.magnitude);
					}
					else
					{
						node.DeltaV = new Vector3d(0.0, 0.0, currentSelectedTransfer.transferdV.magnitude);
					}
				}
				else
				{
					node.DeltaV = new Vector3d(currentSelectedTransfer.transferdV.x, currentSelectedTransfer.transferdV.y, currentSelectedTransfer.transferdV.z);
				}
				FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
				if (currentSelectedTransfer.correctionBurnRequired)
				{
					FlightGlobals.ActiveVessel.patchedConicSolver.AddManeuverNode(currentSelectedTransfer.correctionBurnTime).DeltaV = new Vector3d(currentSelectedTransfer.correctiondV.x, currentSelectedTransfer.correctiondV.y, currentSelectedTransfer.correctiondV.z);
					FlightGlobals.ActiveVessel.patchedConicSolver.UpdateFlightPlan();
				}
			}
			return true;
		}
		return false;
	}

	public override void OnUpdate()
	{
		string errorString = "";
		bool flag = false;
		if (restartCalcs)
		{
			if (bw != null && !bw.Abort())
			{
				bw = null;
				workerRunningTime = 0.0;
				running = false;
				transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
			}
			if (completeDrawRequired)
			{
				ManeuverTool.Instance.StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
				{
					CompleteDrawControls();
					completeDrawRequired = false;
				}));
			}
			restartCalcs = false;
			return;
		}
		if (workerRunningTime > 0.0 && Planetarium.GetUniversalTime() - workerRunningTime > (double)GameSettings.MANEUVER_TOOL_CALC_TIMEOUT && !completeDrawRequired)
		{
			if (currentSelectedTransfer != null)
			{
				currentSelectedTransfer.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
			}
			Debug.LogFormat("[TransferTypeSimple]: Calculation for Simple Maneuver Transfer ran too long. Cancelling.");
			appFrame.SetCreateManeuverButton(interactable: false);
			appFrame.SetHelpString("#autoLOC_6002697");
			if (bw != null)
			{
				if (!bw.Abort())
				{
					bw = null;
					workerRunningTime = 0.0;
					running = false;
					transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
				}
			}
			else
			{
				workerRunningTime = 0.0;
			}
			return;
		}
		if (currentSelectedTransfer != null)
		{
			currentSituationValid = ValidSituation(out errorString);
		}
		if (currentSelectedTransfer != null && currentSituationValid && (currentSelectedTransfer.CalculationState == TransferDataBase.ManeuverCalculationState.waiting || currentSelectedTransfer.CalculationState == TransferDataBase.ManeuverCalculationState.calculating))
		{
			if (workerStalledTime <= 0.0)
			{
				workerStalledTime = Planetarium.GetUniversalTime();
			}
			else if (Planetarium.GetUniversalTime() - workerStalledTime > 3.0)
			{
				workerStalledTime = 0.0;
				running = false;
				currentSelectedTransfer.CalculationState = TransferDataBase.ManeuverCalculationState.waiting;
				DoMathCalcs();
			}
		}
		else
		{
			workerStalledTime = 0.0;
		}
		if (currentSelectedTransfer != null)
		{
			if (!currentSituationValid)
			{
				flag = true;
			}
			else
			{
				if (!running)
				{
					SetSourceBody(UpdateData: true);
				}
				if (currentSelectedTransfer.CalculationState == TransferDataBase.ManeuverCalculationState.complete)
				{
					if (ManeuverTool.Instance.currentTimeDisplayUT)
					{
						if (ManeuverTool.Instance.currentTimeDisplaySeconds)
						{
							ManeuverTool.Instance.appFrame.UpdateTimeText(currentSelectedTransfer.startBurnTime + Localizer.Format("#autoLOC_6002317"));
							if (departureBox != null)
							{
								departureBox.SetValueText(currentSelectedTransfer.startBurnTime + Localizer.Format("#autoLOC_6002317"));
							}
							if (arrivalBox != null)
							{
								arrivalBox.SetValueText(currentSelectedTransfer.arrivalTime + Localizer.Format("#autoLOC_6002317"));
							}
							if (durationBox != null && !double.IsNaN(currentSelectedTransfer.startBurnTime) && !double.IsNaN(currentSelectedTransfer.arrivalTime))
							{
								durationBox.SetValueText(currentSelectedTransfer.transferTime + Localizer.Format("#autoLOC_6002317"));
							}
						}
						else
						{
							ManeuverTool.Instance.appFrame.UpdateTimeText(KSPUtil.PrintDateCompact(currentSelectedTransfer.startBurnTime, includeTime: true, includeSeconds: true));
							if (departureBox != null)
							{
								departureBox.SetValueText(KSPUtil.PrintDateCompact(currentSelectedTransfer.startBurnTime, includeTime: true, includeSeconds: true));
							}
							if (arrivalBox != null)
							{
								arrivalBox.SetValueText(KSPUtil.PrintDateCompact(currentSelectedTransfer.arrivalTime, includeTime: true, includeSeconds: true));
							}
							if (durationBox != null && !double.IsNaN(currentSelectedTransfer.startBurnTime) && !double.IsNaN(currentSelectedTransfer.arrivalTime))
							{
								durationBox.SetValueText(KSPUtil.PrintTime(currentSelectedTransfer.transferTime, 3, explicitPositive: false));
							}
						}
					}
					else
					{
						double startBurnTime = currentSelectedTransfer.startBurnTime;
						startBurnTime = Planetarium.GetUniversalTime() - startBurnTime;
						if (ManeuverTool.Instance.currentTimeDisplaySeconds)
						{
							ManeuverTool.Instance.appFrame.UpdateTimeText(currentSelectedTransfer.startBurnTime + Localizer.Format("#autoLOC_6002317"));
							if (departureBox != null)
							{
								departureBox.SetValueText(currentSelectedTransfer.startBurnTime + Localizer.Format("#autoLOC_6002317"));
							}
							if (arrivalBox != null)
							{
								arrivalBox.SetValueText(currentSelectedTransfer.arrivalTime + Localizer.Format("#autoLOC_6002317"));
							}
							if (durationBox != null)
							{
								durationBox.SetValueText(currentSelectedTransfer.transferTime + Localizer.Format("#autoLOC_6002317"));
							}
						}
						else
						{
							ManeuverTool.Instance.appFrame.UpdateTimeText(KSPUtil.PrintTimeCompact(currentSelectedTransfer.startBurnTime, explicitPositive: true));
							if (departureBox != null)
							{
								departureBox.SetValueText(KSPUtil.PrintTimeCompact(currentSelectedTransfer.startBurnTime, explicitPositive: true));
							}
							if (arrivalBox != null)
							{
								arrivalBox.SetValueText(KSPUtil.PrintTimeCompact(currentSelectedTransfer.arrivalTime, explicitPositive: true));
							}
							if (durationBox != null && !double.IsNaN(currentSelectedTransfer.startBurnTime) && !double.IsNaN(currentSelectedTransfer.arrivalTime))
							{
								durationBox.SetValueText(KSPUtil.PrintTime(currentSelectedTransfer.transferTime, 3, explicitPositive: false));
							}
						}
					}
				}
				else
				{
					ManeuverTool.Instance.appFrame.UpdateTimeText("");
					if (departureBox != null)
					{
						departureBox.SetValueText("");
					}
					if (arrivalBox != null)
					{
						arrivalBox.SetValueText("");
					}
					if (durationBox != null)
					{
						durationBox.SetValueText("");
					}
				}
			}
		}
		else
		{
			ManeuverTool.Instance.appFrame.UpdateTimeText("");
			ManeuverTool.Instance.appFrame.UpdateDVText("");
			if (departureBox != null)
			{
				departureBox.SetValueText("");
			}
			if (arrivalBox != null)
			{
				arrivalBox.SetValueText("");
			}
			if (durationBox != null)
			{
				durationBox.SetValueText("");
			}
		}
		UpdateTopData();
		if (!InputLockManager.IsUnlocked(ControlTypes.MANNODE_ADDEDIT) && Application.isFocused)
		{
			flag = true;
			errorString = "#autoLOC_465476";
		}
		else
		{
			flag = flag;
		}
		appFrame.SetErrorState(flag, flag ? errorString : "", currentSelectedTransfer, toggleInputPanel: false, UpdateTransferData);
	}

	public override void HideApp()
	{
		if (bw != null)
		{
			if (currentSelectedTransfer != null)
			{
				currentSelectedTransfer.CalculationState = TransferDataBase.ManeuverCalculationState.waiting;
			}
			if (!bw.Abort())
			{
				bw = null;
				workerRunningTime = 0.0;
				running = false;
				transferDataSimple.CalculationState = TransferDataBase.ManeuverCalculationState.failed;
			}
		}
	}

	public void UpdateTopData()
	{
		if (appFrame == null)
		{
			return;
		}
		if (!appFrame.TopPanelActive)
		{
			appFrame.SetTopPanelState(state: true);
		}
		if (!topFrameSetup || !(appFrame.topPanelData != null))
		{
			return;
		}
		double universalTime = Planetarium.GetUniversalTime();
		if (!(lastTopFrameUpdate < 0.0) && !(universalTime > lastTopFrameUpdate + topFrameUpdateIntervalSecs))
		{
			return;
		}
		string text = "";
		string text2 = "";
		string text3 = "";
		CelestialBody celestialBody = null;
		if (currentSelectedTransfer != null)
		{
			if (currentSelectedTransfer.vessel == null && FlightGlobals.ActiveVessel != null)
			{
				currentSelectedTransfer.vessel = FlightGlobals.ActiveVessel;
			}
			if (currentSelectedTransfer.SourceBody == null && currentSelectedTransfer.vessel != null)
			{
				currentSelectedTransfer.SourceBody = currentSelectedTransfer.vessel.mainBody;
			}
			celestialBody = currentSelectedTransfer.SourceBody;
		}
		if (celestialBody != null && currentSelectedTransfer.vessel != null)
		{
			for (int i = 0; i < currentSelectedTransfer.cbItems.Count; i++)
			{
				CelestialBody bodyByName = FlightGlobals.GetBodyByName((string)currentSelectedTransfer.cbItems[i].key);
				if (bodyByName != null)
				{
					double afterUT = ((currentSelectedTransfer.startUT <= 0.0) ? Planetarium.GetUniversalTime() : currentSelectedTransfer.startUT);
					TransferMath.CalculateStartEndXferTimes(celestialBody, bodyByName, afterUT, GameSettings.MANEUVER_TOOL_TRANSFER_DEGREES, out var startTime, out var endTime);
					string text4 = KSPUtil.PrintDateDeltaCompact(startTime, includeTime: true, includeSeconds: false, 2);
					string text5 = KSPUtil.PrintDateDeltaCompact(endTime, includeTime: true, includeSeconds: false, 2);
					text += currentSelectedTransfer.cbItems[i].text;
					text2 += text4;
					text3 += text5;
					if (i < currentSelectedTransfer.cbItems.Count - 1)
					{
						text += "\n";
						text2 += "\n";
						text3 += "\n";
					}
				}
			}
		}
		if (currentSelectedTransferTopData is TransferDataSimpleTopData transferDataSimpleTopData)
		{
			transferDataSimpleTopData.transferListData.column1 = text;
			transferDataSimpleTopData.transferListData.column2 = text2;
			transferDataSimpleTopData.transferListData.column3 = text3;
		}
		appFrame.topPanelData.RefreshUI();
		lastTopFrameUpdate = universalTime;
	}

	public bool ValidSituation(out string errorString)
	{
		currentSelectedTransfer.startUT = 0.0;
		errorString = "";
		if (FlightGlobals.ActiveVessel == null)
		{
			errorString = "#autoLOC_6002663";
			currentSelectedTransfer.SourceBody = null;
			return false;
		}
		if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.FLYING || FlightGlobals.ActiveVessel.situation == Vessel.Situations.LANDED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SPLASHED || FlightGlobals.ActiveVessel.situation == Vessel.Situations.SUB_ORBITAL)
		{
			errorString = "#autoLOC_6002663";
			currentSelectedTransfer.SourceBody = null;
			return false;
		}
		if (currentSelectedTransfer.currentPositionNode == null)
		{
			if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.orbit != null)
			{
				if (FlightGlobals.ActiveVessel.orbit.eccentricity > 1.0)
				{
					errorString = "#autoLOC_6002664";
					currentSelectedTransfer.SourceBody = null;
					return false;
				}
				if (InclinationTooBig())
				{
					errorString = "#autoLOC_6002684";
					currentSelectedTransfer.SourceBody = null;
					return false;
				}
				if (Math.Abs(FlightGlobals.ActiveVessel.orbit.eccentricity) > 0.2)
				{
					errorString = "#autoLOC_6002685";
					currentSelectedTransfer.SourceBody = null;
					return false;
				}
			}
		}
		else
		{
			if (currentSelectedTransfer.currentPositionNode.nextPatch != null && currentSelectedTransfer.currentPositionNode.nextPatch.eccentricity > 1.0)
			{
				errorString = "#autoLOC_6002664";
				currentSelectedTransfer.SourceBody = null;
				return false;
			}
			if (InclinationTooBig())
			{
				errorString = "#autoLOC_6002684";
				currentSelectedTransfer.SourceBody = null;
				return false;
			}
			if (currentSelectedTransfer.currentPositionNode.nextPatch != null && currentSelectedTransfer.currentPositionNode.nextPatch.eccentricity > 0.2)
			{
				errorString = "#autoLOC_6002685";
				currentSelectedTransfer.SourceBody = null;
				return false;
			}
			if (currentSelectedTransfer.currentPositionNode.nextPatch != null)
			{
				currentSelectedTransfer.startUT = currentSelectedTransfer.currentPositionNode.nextPatch.StartUT;
			}
		}
		return true;
	}

	public bool InclinationTooBig()
	{
		if (currentSelectedTransfer != null && currentSelectedTransfer.startingOrbit != null && currentSelectedTransfer.TargetBody != null && currentSelectedTransfer.TargetBody.orbit != null)
		{
			if (Math.Abs(currentSelectedTransfer.startingOrbit.GetRelativeInclination(currentSelectedTransfer.TargetBody.orbit)) > 5.0 && Math.Abs(FlightGlobals.ActiveVessel.orbit.inclination) > 5.0)
			{
				return Math.Abs(FlightGlobals.ActiveVessel.orbit.inclination - 180.0) > 5.0;
			}
			return false;
		}
		if (Math.Abs(FlightGlobals.ActiveVessel.orbit.inclination) > 5.0)
		{
			return Math.Abs(FlightGlobals.ActiveVessel.orbit.inclination - 180.0) > 5.0;
		}
		return false;
	}

	public void SetupCBNames()
	{
		List<CelestialBody> list = ValidCelestialBodies();
		currentSelectedTransfer.cbItems = new List<AppUIMemberDropdown.AppUIDropdownItem>();
		for (int i = 0; i < list.Count; i++)
		{
			if (transferDataSimple != null && transferDataSimple.startingOrbit != null && transferDataSimple.startingOrbit.referenceBody != null && transferDataSimple.startingOrbit.referenceBody.HasChild(list[i]))
			{
				currentSelectedTransfer.cbItems.Add(new AppUIMemberDropdown.AppUIDropdownItem
				{
					key = list[i].name,
					text = "* " + list[i].displayName.LocalizeRemoveGender()
				});
			}
			else
			{
				currentSelectedTransfer.cbItems.Add(new AppUIMemberDropdown.AppUIDropdownItem
				{
					key = list[i].name,
					text = list[i].displayName.LocalizeRemoveGender()
				});
			}
		}
		if (currentSelectedTransfer.TargetBody == null)
		{
			currentSelectedTransfer.TargetBody = FlightGlobals.GetBodyByName((string)currentSelectedTransfer.cbItems[0].key);
			previousTargetBodyIndex = -1;
			return;
		}
		bool flag = false;
		for (int j = 0; j < currentSelectedTransfer.cbItems.Count; j++)
		{
			if (currentSelectedTransfer.cbItems[j].key.Equals(currentSelectedTransfer.TargetBody.name))
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			currentSelectedTransfer.TargetBody = FlightGlobals.GetBodyByName((string)currentSelectedTransfer.cbItems[0].key);
			previousTargetBodyIndex = -1;
		}
	}

	public List<CelestialBody> ValidCelestialBodies()
	{
		List<CelestialBody> list = new List<CelestialBody>();
		if (transferDataSimple != null && transferDataSimple.vessel != null && transferDataSimple.startingOrbit != null)
		{
			for (int i = 0; i < transferDataSimple.startingOrbit.referenceBody.orbitingBodies.Count; i++)
			{
				list.Add(transferDataSimple.startingOrbit.referenceBody.orbitingBodies[i]);
			}
			if (transferDataSimple.startingOrbit.referenceBody.referenceBody != null)
			{
				if (transferDataSimple.startingOrbit.referenceBody.referenceBody != null && !transferDataSimple.startingOrbit.referenceBody.referenceBody.isStar)
				{
					list.Add(transferDataSimple.startingOrbit.referenceBody.referenceBody);
				}
				for (int j = 0; j < transferDataSimple.startingOrbit.referenceBody.referenceBody.orbitingBodies.Count; j++)
				{
					if (transferDataSimple.startingOrbit.referenceBody.referenceBody.orbitingBodies[j] != transferDataSimple.startingOrbit.referenceBody)
					{
						list.Add(transferDataSimple.startingOrbit.referenceBody.referenceBody.orbitingBodies[j]);
					}
				}
			}
		}
		else
		{
			list = FlightGlobals.Bodies;
		}
		return list;
	}

	public void SetSourceBody(bool UpdateData)
	{
		if (currentSelectedTransfer == null)
		{
			return;
		}
		string errorString = "";
		if (ValidSituation(out errorString) && currentSelectedTransfer.startingOrbit != null && currentSelectedTransfer.startingOrbit.referenceBody != null && (currentSelectedTransfer.SourceBody == null || currentSelectedTransfer.SourceBody != currentSelectedTransfer.startingOrbit.referenceBody))
		{
			currentSelectedTransfer.SourceBody = currentSelectedTransfer.startingOrbit.referenceBody;
			SetupCBNames();
			if (UpdateData)
			{
				UpdateTransferData();
			}
			lastTopFrameUpdate = -1.0;
			UpdateTopData();
			UpdateVisualPanel();
		}
	}

	public void SetupVisualPrefabs()
	{
		GameObject transferVisualPrefab = ManeuverTool.Instance.GetTransferVisualPrefab("ManeuverToolVisualEllipse1");
		ellipseOne = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab);
		ellipseOne.SetActive(value: false);
		GameObject transferVisualPrefab2 = ManeuverTool.Instance.GetTransferVisualPrefab("ManeuverToolVisualEllipse2");
		ellipseTwo = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab2);
		ellipseTwo.SetActive(value: false);
		GameObject transferVisualPrefab3 = ManeuverTool.Instance.GetTransferVisualPrefab("ManeuverToolVisualCurve1");
		arcOne = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab3);
		arcOne.SetActive(value: false);
		GameObject transferVisualPrefab4 = ManeuverTool.Instance.GetTransferVisualPrefab("ManeuverToolVisualCurve2");
		arcTwo = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab4);
		arcTwo.SetActive(value: false);
		GameObject transferVisualPrefab5 = ManeuverTool.Instance.GetTransferVisualPrefab("ManeuverToolVisualTextBox");
		departureBox = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab5, departureBoxPos, Vector3.one).GetComponent<ManeuverToolVisualTextBox>();
		departureBox.SetNameText(Localizer.Format("#autoLOC_6002686"));
		departureBox.gameObject.SetActive(value: false);
		departuredV = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab5, departuredVPos, Vector3.one).GetComponent<ManeuverToolVisualTextBox>();
		departuredV.SetNameText(Localizer.Format("#autoLOC_6002687"));
		departuredV.gameObject.SetActive(value: false);
		arrivalBox = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab5, arrivalBoxPos, Vector3.one).GetComponent<ManeuverToolVisualTextBox>();
		arrivalBox.SetNameText(Localizer.Format("#autoLOC_6002688"));
		arrivalBox.gameObject.SetActive(value: false);
		arrivaldV = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab5, arrivaldVPos, Vector3.one).GetComponent<ManeuverToolVisualTextBox>();
		arrivaldV.SetNameText(Localizer.Format("#autoLOC_6002689"));
		arrivaldV.gameObject.SetActive(value: false);
		durationBox = appFrame.AddObjectToVisualizerWindow(transferVisualPrefab5, durationBoxPos, Vector3.one).GetComponent<ManeuverToolVisualTextBox>();
		durationBox.SetNameText(Localizer.Format("#autoLOC_6002690"));
		durationBox.gameObject.SetActive(value: false);
		prefabsSetup = true;
	}

	public void UpdateVisualPanel()
	{
		if (appFrame == null)
		{
			return;
		}
		if (!appFrame.VisualPanelActive)
		{
			appFrame.SetVisualPanelState(state: true);
		}
		if (appFrame.VisualPanelExtended)
		{
			if (transferDataSimple.SourceBody != null)
			{
				if (activeSourceBody != null)
				{
					appFrame.DeactivatePlanet(activeSourceBody);
				}
				activeSourceBody = appFrame.ActivatePlanet(transferDataSimple.SourceBody.name, appFrame.sourceBodyLocalPos, ScalePlanetToAltitude(transferDataSimple.SourceBody, (float)transferDataSimple.altitudeAtTransferTime));
				if (activeParentBody != null)
				{
					appFrame.DeactivatePlanet(activeParentBody);
				}
				if (transferDataSimple.SourceBody.referenceBody != null && !TransferMath.SameSOITransfer(transferDataSimple))
				{
					activeParentBody = appFrame.ActivatePlanet(transferDataSimple.SourceBody.referenceBody.name, appFrame.parentBodyLocalPos, 0.7f);
				}
			}
			if (transferDataSimple.TargetBody != null)
			{
				if (activeTargetBody != null)
				{
					appFrame.DeactivatePlanet(activeTargetBody);
				}
				activeTargetBody = appFrame.ActivatePlanet(transferDataSimple.TargetBody.name, appFrame.targetBodyLocalPos, ScalePlanetToAltitude(transferDataSimple.TargetBody, (float)transferDataSimple.transferPe));
			}
			if (!prefabsSetup)
			{
				SetupVisualPrefabs();
			}
			if (prefabsSetup)
			{
				ellipseOne.SetActive(value: true);
				ellipseTwo.SetActive(currentSelectedTransfer.transferCircularize);
				departureBox.gameObject.SetActive(value: true);
				departuredV.gameObject.SetActive(value: true);
				arrivalBox.gameObject.SetActive(value: true);
				arrivaldV.gameObject.SetActive(currentSelectedTransfer.transferCircularize);
				durationBox.gameObject.SetActive(value: true);
				arcOne.SetActive(currentSelectedTransfer.transferCircularize);
				arcTwo.SetActive(!currentSelectedTransfer.transferCircularize);
			}
		}
		else
		{
			if (activeSourceBody != null)
			{
				appFrame.DeactivatePlanet(activeSourceBody);
			}
			if (activeParentBody != null)
			{
				appFrame.DeactivatePlanet(activeParentBody);
			}
			if (activeTargetBody != null)
			{
				appFrame.DeactivatePlanet(activeTargetBody);
			}
			if (prefabsSetup)
			{
				ellipseOne.SetActive(value: false);
				ellipseTwo.SetActive(value: false);
				departureBox.gameObject.SetActive(value: false);
				departuredV.gameObject.SetActive(value: false);
				arrivalBox.gameObject.SetActive(value: false);
				arrivaldV.gameObject.SetActive(value: false);
				durationBox.gameObject.SetActive(value: false);
				arcOne.SetActive(value: false);
				arcTwo.SetActive(value: false);
			}
		}
	}

	public void RefreshUIControls()
	{
		if (!ControlsSetup)
		{
			appFrame.inputPanel.Setup(currentSelectedTransfer, UpdateTransferData);
			AppUIMember control = appFrame.inputPanel.GetControl("targetBodyName");
			if (control != null)
			{
				AppUIMemberDropdown appUIMemberDropdown = control as AppUIMemberDropdown;
				if (appUIMemberDropdown != null)
				{
					appUIMemberDropdown.dropdown.onItemSelected.AddListener(OnTargetClicked);
				}
			}
			ControlsSetup = true;
		}
		else if (!running)
		{
			appFrame.inputPanel.RefreshUI();
		}
	}

	public float ScalePlanetToAltitude(CelestialBody body, float altitude)
	{
		float t = Mathf.InverseLerp((float)TransferMath.SafeOrbitRadius(body) - (float)body.Radius, (float)body.sphereOfInfluence, altitude);
		return Mathf.Lerp(1f, 0.5f, t);
	}

	public void OnTargetClicked(int value)
	{
		if (value == previousTargetBodyIndex)
		{
			previousTargetBody = null;
			UpdateTransferData();
		}
		previousTargetBodyIndex = value;
	}
}
