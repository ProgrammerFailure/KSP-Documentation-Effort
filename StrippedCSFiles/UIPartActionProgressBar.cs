using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine.UI;

[UI_ProgressBar]
public class UIPartActionProgressBar : UIPartActionFieldItem
{
	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldAmount;

	public Slider progBar;

	private float fieldValue;

	protected UI_ProgressBar progBarControl
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionProgressBar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}
}
