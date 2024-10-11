using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

[RequireComponent(typeof(UIListItem))]
public class RDDropDownListItem : MonoBehaviour
{
	public TextMeshProUGUI header;

	public TextMeshProUGUI description;

	public UIRadioButton radioButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDDropDownListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string header, string description)
	{
		throw null;
	}
}
