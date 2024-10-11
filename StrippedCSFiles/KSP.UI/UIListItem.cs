using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class UIListItem : MonoBehaviour
{
	public object Data;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextMeshProUGUI GetTextElement(string elementName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Button GetButtonElement(string elementName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIStateButton GetStateButtonElement(string elementName)
	{
		throw null;
	}
}
