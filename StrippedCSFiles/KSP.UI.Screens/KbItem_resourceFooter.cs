using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KbItem_resourceFooter : KbItem
{
	public UIStateButton toggleColor;

	public UIStateButton toggleStyle;

	public Button btnPlus;

	public Button btnMinus;

	public TextMeshProUGUI cutoff;

	public TextMeshProUGUI cutoffValue;

	public RectTransform panelCutooff;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_resourceFooter()
	{
		throw null;
	}
}
