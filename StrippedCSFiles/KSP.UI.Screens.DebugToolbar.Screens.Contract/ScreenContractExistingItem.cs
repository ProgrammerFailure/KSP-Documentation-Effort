using System.Runtime.CompilerServices;
using Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Contract;

public class ScreenContractExistingItem : MonoBehaviour
{
	public TextMeshProUGUI titleText;

	public TextMeshProUGUI stateText;

	public TextMeshProUGUI prestigeText;

	public Button leftButton;

	public TextMeshProUGUI leftButtonText;

	public Button rightButton;

	public TextMeshProUGUI rightButtonText;

	public TextMeshProUGUI errorText;

	private ScreenContractList contractList;

	public Contracts.Contract contract
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenContractExistingItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Contracts.Contract contract, ScreenContractList contractList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupError(string errorText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLeftButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnRightButtonClicked()
	{
		throw null;
	}
}
