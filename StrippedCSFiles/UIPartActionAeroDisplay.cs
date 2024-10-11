using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

[UI_Label]
public class UIPartActionAeroDisplay : UIPartActionItem
{
	public int rows;

	public int cubeRows;

	internal BasePAWGroup pawGroup;

	[SerializeField]
	private TextMeshProUGUI txtMach;

	[SerializeField]
	private TextMeshProUGUI txtDragVector;

	[SerializeField]
	private TextMeshProUGUI txtDragCube0;

	[SerializeField]
	private TextMeshProUGUI txtDragCube1;

	[SerializeField]
	private TextMeshProUGUI txtDragCube2;

	[SerializeField]
	private TextMeshProUGUI txtDragCube3;

	[SerializeField]
	private TextMeshProUGUI txtDragCube4;

	[SerializeField]
	private TextMeshProUGUI txtDragCube5;

	[SerializeField]
	private TextMeshProUGUI txtDragScalar;

	[SerializeField]
	private TextMeshProUGUI txtAreaDrag;

	[SerializeField]
	private TextMeshProUGUI txtCube0;

	[SerializeField]
	private TextMeshProUGUI txtCube1;

	[SerializeField]
	private TextMeshProUGUI txtCube2;

	[SerializeField]
	private TextMeshProUGUI txtCube3;

	[SerializeField]
	private TextMeshProUGUI txtCube4;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIPartActionAeroDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void UpdateItem()
	{
		throw null;
	}
}
