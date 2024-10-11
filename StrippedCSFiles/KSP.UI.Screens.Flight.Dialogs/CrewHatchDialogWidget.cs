using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight.Dialogs;

public class CrewHatchDialogWidget : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textCrewName;

	[SerializeField]
	private Button btnEVA;

	[SerializeField]
	private Button btnTransfer;

	[NonSerialized]
	public ProtoCrewMember protoCrewMember;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewHatchDialogWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Init(ProtoCrewMember crew, Callback<ProtoCrewMember> onBtnEVA, Callback<ProtoCrewMember> onBtnTransfer, UIAvailability evaBtnAvail, UIAvailability transferBtnAvail)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void BtnInit(Button btn, UIAvailability btnAvail, UnityAction btnEvtHandler)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}
}
