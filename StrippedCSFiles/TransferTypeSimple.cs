using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using KSP.UI;
using UnityEngine;

[Serializable]
public class TransferTypeSimple : TransferTypeBase
{
	private bool ControlsSetup;

	private bool topFrameSetup;

	private bool prefabsSetup;

	private ManeuverToolUIFrame appFrame;

	private bool currentSituationValid;

	private SafeAbortBackgroundWorker bw;

	private bool running;

	private TransferDataSimple transferDataSimple;

	private double earliestStartUT;

	private double porkChopWidth;

	private double porkChopHeight;

	private double lastTopFrameUpdate;

	private ManeuverToolCBContainer activeSourceBody;

	private ManeuverToolCBContainer activeTargetBody;

	private ManeuverToolCBContainer activeParentBody;

	private GameObject ellipseOne;

	private GameObject ellipseTwo;

	private GameObject arcOne;

	private GameObject arcTwo;

	private ManeuverToolVisualTextBox departureBox;

	private ManeuverToolVisualTextBox departuredV;

	private ManeuverToolVisualTextBox arrivalBox;

	private ManeuverToolVisualTextBox arrivaldV;

	private ManeuverToolVisualTextBox durationBox;

	private Vector2 departureBoxPos;

	private Vector2 departuredVPos;

	private Vector2 arrivalBoxPos;

	private Vector2 arrivaldVPos;

	private Vector2 durationBoxPos;

	private double workerRunningTime;

	private double workerStalledTime;

	private bool foundIntercept;

	private CelestialBody previousSourceBody;

	private CelestialBody previousTargetBody;

	private bool previousCircularize;

	private double previousCircPe;

	private bool restartCalcs;

	private bool completeDrawRequired;

	private double lastCalcTime;

	private int previousTargetBodyIndex;

	public double topFrameUpdateIntervalSecs;

	public new TransferDataSimple currentSelectedTransfer
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

	public TransferDataTopDataBase currentSelectedTransferTopData
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
	public TransferTypeSimple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVisualizer(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~TransferTypeSimple()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string DisplayName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool ChangePosition(ManeuverNode node, out string errorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ChangeTime()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void DrawTypeControls(ManeuverToolUIFrame appFrame, DrawReason reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CompleteDrawControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateTransferData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DoMathCalcs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void bw_ProcessCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void bw_CalculateTransfer(object sender, DoWorkEventArgs e)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double FinalizeTransferDV(TransferDataSimple transferDataSimple, double startUT, SafeAbortBackgroundWorker bw)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool DidWeIntercept(SafeAbortBackgroundWorker bw, ref TransferDataSimple transferDataSimple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculationComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool CreateManeuver(out ManeuverNode node, out string alarmTitle, out string alarmDesc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateTopData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ValidSituation(out string errorString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool InclinationTooBig()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupCBNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private List<CelestialBody> ValidCelestialBodies()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetSourceBody(bool UpdateData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupVisualPrefabs()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateVisualPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshUIControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float ScalePlanetToAltitude(CelestialBody body, float altitude)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnTargetClicked(int value)
	{
		throw null;
	}
}
