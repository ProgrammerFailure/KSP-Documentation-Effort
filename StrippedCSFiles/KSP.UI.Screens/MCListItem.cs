using System.Runtime.CompilerServices;
using Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class MCListItem : MonoBehaviour
{
	public UIListItem container;

	public UIRadioButton radioButton;

	public TextMeshProUGUI title;

	public RawImage logoSprite;

	public UIStateImage difficulty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MCListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(Contract contract, string label)
	{
		throw null;
	}
}
