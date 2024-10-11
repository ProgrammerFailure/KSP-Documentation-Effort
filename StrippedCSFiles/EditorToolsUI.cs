using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;

public class EditorToolsUI : MonoBehaviour
{
	[SerializeField]
	private Toggle placeButton;

	[SerializeField]
	private Toggle moveButton;

	[SerializeField]
	private Toggle rotateButton;

	[SerializeField]
	private Toggle rootButton;

	private Color placeBtnColor0;

	private Color moveBtnColor0;

	private Color rotateBtnColor0;

	private Color rootBtnColor0;

	private ConstructionMode constructionMode;

	public UIPanelTransition panelTransition;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorToolsUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorRestart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorLoad(ShipConstruct ct, CraftBrowserDialog.LoadType loadType)
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
	private void onPlaceButtonInput(bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onMoveButtonInput(bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRotateButtonInput(bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onRootButtonInput(bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetMode(ConstructionMode mode, bool updateUI = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorScreenChange(EditorScreen scr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorPartEvent(ConstructionEventType evt, Part part)
	{
		throw null;
	}
}
