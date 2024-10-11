using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleStructuralNodeToggle : PartModule
{
	[KSPField]
	public string MeshMenuName;

	[KSPField]
	public string NodeMenuName;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001852")]
	public bool showMesh;

	[UI_Toggle(disabledText = "#autoLOC_6001073", enabledText = "#autoLOC_6001074")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001853")]
	public bool showNodes;

	private List<ModuleStructuralNode> structNodes;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleStructuralNodeToggle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldUpdated(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateNodes()
	{
		throw null;
	}
}
