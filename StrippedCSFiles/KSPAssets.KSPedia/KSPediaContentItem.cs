using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSPAssets.KSPedia;

public class KSPediaContentItem : MonoBehaviour
{
	public TextMeshProUGUI text;

	public Button btn;

	private string screenName;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPediaContentItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string text, KSPediaDatabase.Screen screen)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnClick()
	{
		throw null;
	}
}
