using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions;

public class MEGUINodeIconGroup : MonoBehaviour
{
	public new string name;

	public TextMeshProUGUI title;

	public RectTransform containerChildren;

	public Button buttonCollapse;

	public Sprite spriteCollapseOn;

	public Sprite spriteCollapseOff;

	[SerializeField]
	private Image headerImage;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUINodeIconGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEGUINodeIconGroup Create(string name, string displayName, string htmlColorCode = "")
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CollapseGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonCollapsePressed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHeaderColor(Color newColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetHeaderColor(string htmlColorCode)
	{
		throw null;
	}
}
