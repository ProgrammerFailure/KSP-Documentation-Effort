using System.Collections.Generic;

public class ModuleStructuralNodeToggle : PartModule
{
	[KSPField]
	public string MeshMenuName = "";

	[KSPField]
	public string NodeMenuName = "";

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001852")]
	public bool showMesh = true;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001853")]
	public bool showNodes = true;

	public List<ModuleStructuralNode> structNodes;

	public void Start()
	{
		if (!string.IsNullOrEmpty(MeshMenuName))
		{
			base.Fields["showMesh"].guiName = MeshMenuName;
		}
		if (!string.IsNullOrEmpty(NodeMenuName))
		{
			base.Fields["showNodes"].guiName = NodeMenuName;
		}
		base.Fields["showMesh"].uiControlEditor.onFieldChanged = OnFieldUpdated;
		base.Fields["showNodes"].uiControlEditor.onFieldChanged = OnFieldUpdated;
		UpdateNodes();
	}

	public void OnFieldUpdated(BaseField field, object obj)
	{
		UpdateNodes();
	}

	public void UpdateNodes()
	{
		if (structNodes == null)
		{
			structNodes = base.part.FindModulesImplementing<ModuleStructuralNode>();
		}
		int count = structNodes.Count;
		for (int i = 0; i < count; i++)
		{
			structNodes[i].visibilityState = showMesh;
			structNodes[i].SetNodeState(showNodes);
		}
	}
}
