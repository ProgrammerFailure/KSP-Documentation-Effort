using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VesselSpawnSearch : MonoBehaviour
{
	public delegate void HasFiltered(bool filtered);

	[CompilerGenerated]
	private sealed class _003CSearchRoutine_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public VesselSpawnSearch _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CSearchRoutine_003Ed__26(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[Header("Inspector assigned")]
	[Tooltip("The input field for search terms")]
	[SerializeField]
	private TMP_InputField searchField;

	[SerializeField]
	[Tooltip("The content area where the searchable widgets appear")]
	private RectTransform scrollListContent;

	[SerializeField]
	[Tooltip("(seconds) Delay in checks for keystrokes")]
	private float searchKeystrokeDelay;

	[SerializeField]
	[Tooltip("The full tab background for when the Steam tab is visible")]
	private RectTransform fullTabBackground;

	[SerializeField]
	[Tooltip("The toggle for the stock craft / player craft list")]
	private Toggle tabVesselList;

	[SerializeField]
	[Tooltip("Items the search filter will ignore")]
	private List<GameObject> filterIgnoreItems;

	public HasFiltered hasFiltered;

	[NonSerialized]
	public const string gameSearchFieldControlLockId = "GameSearchFieldTextInput";

	private Image searchFieldBackground;

	protected PointerClickHandler searchFieldClickHandler;

	protected float searchTimer;

	protected Coroutine searchRoutine;

	private string previousSearch;

	public bool HasSearchText
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselSpawnSearch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SearchField_OnEndEdit(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SearchField_OnValueChange(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SearchField_OnClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SteamCraftListReceived()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SearchRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SearchStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSearchRoutine_003Ed__26))]
	protected IEnumerator SearchRoutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool ItemMatchesSearch(Transform item, string searchTerm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SearchStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustSearchInputSize()
	{
		throw null;
	}
}
