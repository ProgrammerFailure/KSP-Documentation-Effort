using System.Runtime.CompilerServices;
using TMPro;

namespace KSP.UI.Screens;

public class KbItem_resourceItem : KbItem
{
	public TextMeshProUGUI keyRich;

	public TextMeshProUGUI valueRich;

	public UIRadioButton radioButton;

	public PointerEnterExitHandler hoverHandler;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbItem_resourceItem()
	{
		throw null;
	}
}
