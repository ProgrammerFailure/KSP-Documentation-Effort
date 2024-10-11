using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight.Dialogs;

public class FlagPlaqueDialog : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI textHeader;

	[SerializeField]
	private TextMeshProUGUI textContent;

	[SerializeField]
	private Button buttonClose;

	private Callback onDismiss;

	private string plaqueText;

	private string siteName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagPlaqueDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FlagPlaqueDialog Spawn(string siteName, string plaqueText, Callback onDismiss)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onBtnClose()
	{
		throw null;
	}
}
