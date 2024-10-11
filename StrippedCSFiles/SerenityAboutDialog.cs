using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SerenityAboutDialog : MonoBehaviour
{
	[SerializeField]
	private RawImage bannerIcon;

	[SerializeField]
	private Button moreInfoButton;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Toggle dontShowAgainToggle;

	public Texture[] banners;

	private bool dontShowAgain;

	private string serenityExpansionURL;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SerenityAboutDialog()
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
	public SerenityAboutDialog Create(string expansionURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Dismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDontShowAgainToggle(bool value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCloseButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMoreInfoButton()
	{
		throw null;
	}
}
