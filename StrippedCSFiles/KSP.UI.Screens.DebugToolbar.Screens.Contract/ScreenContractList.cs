using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Contracts;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.Contract;

public class ScreenContractList : MonoBehaviour
{
	public enum ScreenContractListMode
	{
		Active,
		Offered,
		Archived
	}

	public ScreenContractExistingItem itemPrefab;

	public RectTransform listParent;

	protected List<ScreenContractExistingItem> items;

	public ScreenContractListMode mode;

	private bool dirty;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenContractList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RefreshItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateItem(Contracts.Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateError(string errorText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameStateCreated(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnGameStateLoaded(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnContractsModified()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnContractsModified(Contracts.Contract contract)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDirty()
	{
		throw null;
	}
}
