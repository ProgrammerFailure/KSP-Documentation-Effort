using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class AppUIInputPanel : MonoBehaviour
{
	public Transform rowParent;

	public AppUI_Data data;

	public TextMeshProUGUI textTargetForHover;

	public ScrollRect scrollTargetForMemberEvents;

	private bool setupComplete;

	private List<AppUIMember> rows;

	public bool SetupComplete
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIInputPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(AppUI_Data data, Callback onDataChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ReleaseData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetErrorState(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyChildren()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Refresh Members")]
	public void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AppUIMember GetControl(string fieldName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool AnyTextFieldHasFocus()
	{
		throw null;
	}
}
