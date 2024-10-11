using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

internal class EditorLaunchPadItem : MonoBehaviour
{
	public Toggle toggleSetDefault;

	public string siteName;

	public TextMeshProUGUI textName;

	public Button buttonLaunch;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorLaunchPadItem()
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
	public void Create(UILaunchsiteController controller, string siteName, string textName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_Launch()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_SelectDefault(bool value)
	{
		throw null;
	}
}
