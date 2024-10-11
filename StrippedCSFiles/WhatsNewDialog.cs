using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhatsNewDialog : MonoBehaviour
{
	[SerializeField]
	private RawImage bannerIcon;

	[SerializeField]
	private Button moreInfoButton;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private Toggle dontShowAgainToggle;

	[SerializeField]
	private TMP_Text textBody;

	[SerializeField]
	private ScrollRect bodyScroll;

	[SerializeField]
	private float scrollStep;

	[SerializeField]
	private Button merchButton;

	public WhatsNewModes currentMode;

	public string whatsNewText;

	private bool dontShowAgain;

	private TMP_Text moreInfoText;

	[SerializeField]
	private string merchandiseURL;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WhatsNewDialog()
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public WhatsNewDialog Create()
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMerchButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetChangeLogText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ParseReadMeFile(string readmePath, string relevantVersion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ParseReadMeFile(string readmePath, string relevantVersion, string relevantDLCVersion)
	{
		throw null;
	}
}
