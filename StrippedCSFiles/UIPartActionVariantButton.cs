using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPartActionVariantButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public Image imagePrimaryColor;

	public Image imageSecomdaryColor;

	public Image imageSelected;

	public Image imageInvalid;

	public Button buttonMain;

	private UIPartActionVariantSelector selectorReference;

	private bool locked;

	private int index;

	public bool Locked
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionVariantButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(UIPartActionVariantSelector selectorReference, int index, string hexPrimaryColor, string hexSecondaryColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetColor(string hexPrimaryColor, string hexSecondaryColor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonPressed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}
}
