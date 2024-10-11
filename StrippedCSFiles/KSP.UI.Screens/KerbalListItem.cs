using System.Runtime.CompilerServices;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class KerbalListItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public Image border;

	public RawImage kerbalImage;

	public TooltipController_TitleAndText tooltip;

	private GameObject avatar;

	private RenderTexture avatarRT;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalListItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(string headName, string headDesc, GameObject avatarPrefab)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
