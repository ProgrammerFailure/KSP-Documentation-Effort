using System.Runtime.CompilerServices;
using Expansions.Serenity;
using TMPro;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class EditorActionControllerHeader : UISelectableGridLayoutGroupItem
{
	public TextMeshProUGUI text;

	public Button editButton;

	public TMP_InputField inputField;

	private string controllerName;

	private ModuleRoboticController controller;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorActionControllerHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Select()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void Deselect()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(string controllerName, ModuleRoboticController controller)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EditButtonClicked()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InputFieldDone(string newValue)
	{
		throw null;
	}
}
