using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIListOrderItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public Button upButton;

	public Button downButton;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIListOrderItem()
	{
		throw null;
	}
}
