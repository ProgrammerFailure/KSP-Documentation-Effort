using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ComboSelector : MonoBehaviour
{
	[SerializeField]
	private Button buttonPrevious;

	[SerializeField]
	private Button buttonNext;

	[SerializeField]
	private ScrollRect scrollMain;

	public int comboEntryIndex;

	public List<ComboButton> comboList;

	public int fieldValue;

	public Material previewBodyMaterial;

	public Material previewHelmetMaterial;

	public Material previewNeckringMaterial;

	public SuitButton suitButton;

	public ScrollRect ScrollMain
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ComboSelector()
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
	private void OnButtonPrev()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonNext()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectVariant(int newIndex)
	{
		throw null;
	}
}
