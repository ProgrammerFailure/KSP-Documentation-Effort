using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight.Dialogs;

public class ExperimentsResultDialog : MonoBehaviour
{
	public static ExperimentsResultDialog Instance;

	[SerializeField]
	private TextMeshProUGUI textHeaderTitle;

	[SerializeField]
	private TextMeshProUGUI textResults;

	[SerializeField]
	private TextMeshProUGUI textDataField;

	[SerializeField]
	private TextMeshProUGUI textRecoveryField;

	[SerializeField]
	private TextMeshProUGUI textTransmitField;

	[SerializeField]
	private TextMeshProUGUI textTransmitBtnPercent;

	[SerializeField]
	private TextMeshProUGUI textTransmitBtnTotalPercent;

	[SerializeField]
	private TextMeshProUGUI textTransmitBtnSignalStrength;

	[SerializeField]
	private TooltipController_Text tooltipTransmitBtn;

	[SerializeField]
	private TooltipController_Text tooltipTransmitCommsBtn;

	[SerializeField]
	private TextMeshProUGUI textLabBtnPercent;

	[SerializeField]
	private TooltipController_Text tooltipLabBtn;

	[SerializeField]
	private TextMeshProUGUI textInopWarning;

	[SerializeField]
	private Slider sldDataPrimary;

	[SerializeField]
	private Slider sldDataSecondary;

	[SerializeField]
	private Slider sldXmitPrimary;

	[SerializeField]
	private Slider sldXmitSecondary;

	[SerializeField]
	private Button btnKeep;

	[SerializeField]
	private Button btnDiscard;

	[SerializeField]
	private Button btnReset;

	[SerializeField]
	private Button btnXmit;

	[SerializeField]
	private Button btnXmitComms;

	[SerializeField]
	private Button btnLab;

	[SerializeField]
	private Button btnPagePrev;

	[SerializeField]
	private Button btnPageNext;

	public List<ExperimentResultDialogPage> pages;

	public ExperimentResultDialogPage currentPage;

	private PopupDialog warningDialog;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ExperimentsResultDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ExperimentsResultDialog DisplayResult(ExperimentResultDialogPage resultPage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUnpause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onPartDie(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselFocusChange(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVesselModified(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnDiscard()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnReset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnKeep()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnTransmit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnLab()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnPageNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onBtnPagePrev()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnPagesModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetPage(ExperimentResultDialogPage page)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void addPage(ExperimentResultDialogPage page)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void dismissCurrentPage(ScienceData pageData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void removePage(ExperimentResultDialogPage page)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NextPage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrevPage()
	{
		throw null;
	}
}
