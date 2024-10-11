using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPartActionResourceToggle : MonoBehaviour
{
	public delegate void PartStatus(PartResource pR, bool b);

	public TextMeshProUGUI fieldName;

	public TextMeshProUGUI fieldStatus;

	public Toggle toggle;

	public PartResource partResource;

	public PartStatus sendPartStatus;

	private static string cacheAutoLOC_439839;

	private static string cacheAutoLOC_439840;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionResourceToggle()
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
	public void InitializeItem(string displayName, PartResource resource, bool active)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateItemName(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateItemNameAndStatus(string name, bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStatusText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStatusTextAndStatus(bool status)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PartStatusToggle(bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CacheLocalStrings()
	{
		throw null;
	}
}
