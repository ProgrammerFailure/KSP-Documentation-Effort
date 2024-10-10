using System;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionAeroDisplay : UIPartActionItem
{
	public int rows = 10;

	public int cubeRows;

	public BasePAWGroup pawGroup;

	[SerializeField]
	public TextMeshProUGUI txtMach;

	[SerializeField]
	public TextMeshProUGUI txtDragVector;

	[SerializeField]
	public TextMeshProUGUI txtDragCube0;

	[SerializeField]
	public TextMeshProUGUI txtDragCube1;

	[SerializeField]
	public TextMeshProUGUI txtDragCube2;

	[SerializeField]
	public TextMeshProUGUI txtDragCube3;

	[SerializeField]
	public TextMeshProUGUI txtDragCube4;

	[SerializeField]
	public TextMeshProUGUI txtDragCube5;

	[SerializeField]
	public TextMeshProUGUI txtDragScalar;

	[SerializeField]
	public TextMeshProUGUI txtAreaDrag;

	[SerializeField]
	public TextMeshProUGUI txtCube0;

	[SerializeField]
	public TextMeshProUGUI txtCube1;

	[SerializeField]
	public TextMeshProUGUI txtCube2;

	[SerializeField]
	public TextMeshProUGUI txtCube3;

	[SerializeField]
	public TextMeshProUGUI txtCube4;

	public virtual void Setup(UIPartActionWindow window, Part part, UI_Scene scene)
	{
		SetupItem(window, part, null, scene, null);
		cubeRows = Math.Min(part.DragCubes.Cubes.Count, 5);
		if (part.DragCubes.Procedural)
		{
			cubeRows = 1;
		}
		if (cubeRows > 0)
		{
			txtCube0.gameObject.SetActive(value: true);
		}
		if (cubeRows > 1)
		{
			txtCube1.gameObject.SetActive(value: true);
		}
		if (cubeRows > 2)
		{
			txtCube2.gameObject.SetActive(value: true);
		}
		if (cubeRows > 3)
		{
			txtCube3.gameObject.SetActive(value: true);
		}
		if (cubeRows > 4)
		{
			txtCube4.gameObject.SetActive(value: true);
		}
		LayoutElement component = base.transform.GetComponent<LayoutElement>();
		if (component != null)
		{
			component.preferredHeight = 140f + (float)cubeRows * 14f;
		}
	}

	public void Awake()
	{
		pawGroup = new BasePAWGroup("Debug", "#autoLOC_8320010", startCollapsed: false);
	}

	public override void UpdateItem()
	{
		txtMach.text = Localizer.Format("#autoLOC_357350", part.machNumber.ToString("F2"));
		txtDragCube0.text = "XP :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[0], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[0], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[0], "F2");
		txtDragCube1.text = "XN :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[1], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[1], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[1], "F2");
		txtDragCube2.text = "YP :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[2], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[2], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[2], "F2");
		txtDragCube3.text = "YN :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[3], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[3], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[3], "F2");
		txtDragCube4.text = "ZP :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[4], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[4], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[4], "F2");
		txtDragCube5.text = "ZN :OccA:" + KSPUtil.LocalizeNumber(part.DragCubes.AreaOccluded[5], "F2") + " WDrg:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedDrag[5], "F2") + " WArea:" + KSPUtil.LocalizeNumber(part.DragCubes.WeightedArea[5], "F2");
		txtDragVector.text = Localizer.Format("#autoLOC_6001917", part.DragCubes.DragVector.ToString());
		txtAreaDrag.text = "A.Cd: " + KSPUtil.LocalizeNumber(part.DragCubes.AreaDrag, "F2");
		txtDragScalar.text = Localizer.Format("#autoLOC_6001916", part.dragScalar.ToString("F2"));
		if (part.DragCubes.Procedural)
		{
			txtCube0.text = Localizer.Format("#autoLOC_6002516");
			return;
		}
		if (cubeRows > 0)
		{
			txtCube0.text = Localizer.Format("#autoLOC_6002515", part.DragCubes.Cubes[0].Name, part.DragCubes.Cubes[0].Weight);
		}
		if (cubeRows > 1)
		{
			txtCube1.text = Localizer.Format("#autoLOC_6002515", part.DragCubes.Cubes[1].Name, part.DragCubes.Cubes[1].Weight);
		}
		if (cubeRows > 2)
		{
			txtCube2.text = Localizer.Format("#autoLOC_6002515", part.DragCubes.Cubes[2].Name, part.DragCubes.Cubes[2].Weight);
		}
		if (cubeRows > 3)
		{
			txtCube3.text = Localizer.Format("#autoLOC_6002515", part.DragCubes.Cubes[3].Name, part.DragCubes.Cubes[3].Weight);
		}
		if (cubeRows > 4)
		{
			txtCube4.text = Localizer.Format("#autoLOC_6002515", part.DragCubes.Cubes[4].Name, part.DragCubes.Cubes[4].Weight);
		}
	}
}
