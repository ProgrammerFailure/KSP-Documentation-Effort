using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommNet;

public class CommNetUIModeButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	public Button button;

	public UIStateImage stateImage;

	public TooltipController_Text tooltip;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CommNetUIModeButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnNetworkInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateUI()
	{
		throw null;
	}
}
