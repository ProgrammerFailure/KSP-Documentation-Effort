using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

[ExecuteInEditMode]
public class InteractableCtrlUIStates : MonoBehaviour
{
	[Serializable]
	public class State
	{
		[SerializeField]
		private Graphic[] tgtGraphics;

		[SerializeField]
		private Color color;

		[SerializeField]
		private bool toggleEnabled;

		[SerializeField]
		private TextMeshProUGUI tgtText;

		[SerializeField]
		private bool changeColorOnly;

		[SerializeField]
		private string text;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public State()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void SetActive()
		{
			throw null;
		}
	}

	[SerializeField]
	private Selectable tgtCtrl;

	[SerializeField]
	private State stInteractable;

	[SerializeField]
	private State stNonInteractable;

	private bool ctrlFlagLast;

	private bool firstRun;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InteractableCtrlUIStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
