using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

public class ModulePartVariants : PartModule, IModuleInfo, IMultipleDragCube, IPartMassModifier, IPartCostModifier
{
	[SerializeField]
	public List<PartVariant> variantList;

	[KSPField]
	public bool useMultipleDragCubes = true;

	[KSPField]
	public bool useProceduralDragCubes;

	public List<Material> partMaterials;

	[UI_VariantSelector(controlEnabled = true, scene = UI_Scene.Editor, affectSymCounterparts = UI_Scene.Editor)]
	[KSPField]
	public int variantIndex;

	[KSPField(isPersistant = true)]
	public bool useVariantMass;

	public ModuleJettison moduleJettison;

	public PartVariant SelectedVariant => variantList[variantIndex];

	public bool IsMultipleCubesActive => useMultipleDragCubes;

	public void onVariantChanged(BaseField field, object obj)
	{
		RefreshVariant();
		EditorLogic.fetch.SetBackup();
	}

	public void onVariantSymmetrycallyChanged(BaseField field, object obj)
	{
		RefreshVariant();
	}

	public static void ApplyVariant(Part part, Transform meshRoot, PartVariant variant, Material[] materials, bool skipShader)
	{
		ApplyVariant(part, meshRoot, variant, materials, skipShader, -1);
	}

	public static void ApplyVariant(Part part, Transform meshRoot, PartVariant variant, Material[] materials, bool skipShader, int variantIndex)
	{
		if (variant == null)
		{
			return;
		}
		if (EditorLogic.iconRequestedVariantChange != null && EditorLogic.iconRequestedVariantChange.inInventory && variantIndex != -1)
		{
			UIPartActionInventorySlot component = EditorLogic.iconRequestedVariantChange.gameObject.GetComponent<UIPartActionInventorySlot>();
			if (component.inventoryPartActionUI.inventoryPartModule.storedParts.TryGetValue(component.slotIndex, out var val))
			{
				if (variant != null)
				{
					val.variantName = variant.Name;
				}
				else
				{
					val.variantName = "";
				}
			}
		}
		if (meshRoot != null)
		{
			variant.UpdateModel(meshRoot.transform);
		}
		if (variant.Materials != null)
		{
			for (int i = 0; i < variant.Materials.Count; i++)
			{
				for (int j = 0; j < materials.Length; j++)
				{
					if (materials[j].name.StartsWith(variant.Materials[i].name))
					{
						if (skipShader)
						{
							Shader shader = materials[j].shader;
							materials[j].CopyPropertiesFromMaterial(variant.Materials[i]);
							materials[j].shader = shader;
						}
						else
						{
							materials[j].shader = variant.Materials[i].shader;
							materials[j].CopyPropertiesFromMaterial(variant.Materials[i]);
						}
					}
				}
			}
		}
		if (HighLogic.LoadedScene == GameScenes.EDITOR || HighLogic.LoadedScene == GameScenes.LOADING)
		{
			if (part != null && part.attachNodes != null && variant.AttachNodes.Count > 0)
			{
				for (int k = 0; k < part.attachNodes.Count; k++)
				{
					for (int l = 0; l < variant.AttachNodes.Count; l++)
					{
						if (part.attachNodes[k].id == variant.AttachNodes[l].id)
						{
							UpdateNode(part.attachNodes[k], variant.AttachNodes[l], part);
							break;
						}
					}
				}
			}
			if (part != null && part.srfAttachNode != null && variant.SrfAttachNode != null)
			{
				UpdateNode(part.srfAttachNode, variant.SrfAttachNode, part);
			}
		}
		if (part != null)
		{
			for (int m = 0; m < part.ModuleAnimateGenerics.Count; m++)
			{
				if (!part.ModuleAnimateGenerics[m].AnimationIsDisabledByVariant && variant.DisabledAnimations.Contains(part.ModuleAnimateGenerics[m].animationName))
				{
					part.ModuleAnimateGenerics[m].VariantToggleAnimationDisabled(disabled: true);
				}
				else if (part.ModuleAnimateGenerics[m].AnimationIsDisabledByVariant && !variant.DisabledAnimations.Contains(part.ModuleAnimateGenerics[m].animationName))
				{
					part.ModuleAnimateGenerics[m].VariantToggleAnimationDisabled(disabled: false);
				}
			}
			if (part.variants != null)
			{
				for (int n = 0; n < part.variants.variantList.Count; n++)
				{
					for (int num = 0; num < part.variants.variantList[n].DisabledEvents.Count; num++)
					{
						string[] array = part.variants.variantList[n].DisabledEvents[num].Split('.');
						PartModule partModule = part.Modules[array[0]];
						if (!(partModule != null))
						{
							continue;
						}
						BaseEvent baseEvent = partModule.Events[array[1]];
						if (baseEvent == null)
						{
							continue;
						}
						if (part.variants.variantList[n] == variant)
						{
							if (!baseEvent.EventIsDisabledByVariant)
							{
								baseEvent.VariantToggleEventDisabled(disabled: true);
							}
						}
						else if (baseEvent.EventIsDisabledByVariant)
						{
							baseEvent.VariantToggleEventDisabled(disabled: false);
						}
					}
				}
			}
			if (variant.AttachRules != null)
			{
				part.attachRules = variant.AttachRules;
			}
			else
			{
				part.attachRules = part.baseVariant.AttachRules;
			}
		}
		GameEvents.onVariantApplied.Fire(part, variant);
		if (HighLogic.LoadedSceneIsEditor)
		{
			GameEvents.onEditorVariantApplied.Fire(part, variant);
		}
	}

	public static void UpdateNode(AttachNode partNode, AttachNode variantNode, Part part)
	{
		UpdatePartPosition(partNode, variantNode);
		partNode.originalPosition = variantNode.originalPosition;
		partNode.position = variantNode.position;
		partNode.orientation = variantNode.orientation;
		partNode.originalOrientation = variantNode.originalOrientation;
		partNode.size = variantNode.size;
		if (partNode.icon != null)
		{
			partNode.icon.transform.localScale = Vector3.one * partNode.radius * ((partNode.size == 0) ? ((float)partNode.size + 0.5f) : ((float)partNode.size));
			partNode.icon.transform.up = part.transform.TransformDirection(partNode.orientation);
		}
	}

	public static void UpdatePartPosition(AttachNode currentNode, AttachNode newNode)
	{
		if (currentNode.attachedPart == null)
		{
			return;
		}
		Part part = ((currentNode.owner != null) ? currentNode.owner.potentialParent : null);
		if (currentNode.attachedPart != EditorLogic.RootPart && currentNode.attachedPart != part)
		{
			if (currentNode.owner != null)
			{
				Vector3 vector = currentNode.owner.transform.TransformPoint(newNode.originalPosition);
				Vector3 vector2 = currentNode.owner.transform.TransformPoint(currentNode.originalPosition);
				Vector3 translation = vector - vector2;
				currentNode.attachedPart.transform.Translate(translation, Space.World);
			}
		}
		else if (part != null)
		{
			Vector3 vector3 = currentNode.owner.transform.TransformPoint(newNode.originalPosition);
			Vector3 translation2 = currentNode.owner.transform.TransformPoint(currentNode.originalPosition) - vector3;
			currentNode.owner.transform.Translate(translation2, Space.World);
		}
	}

	public int GetVariantIndex(string variantName)
	{
		int result = 0;
		for (int i = 0; i < variantList.Count; i++)
		{
			if (variantList[i].Name == variantName)
			{
				_ = variantList[i];
				result = i;
				break;
			}
		}
		return result;
	}

	public int GetCurrentVariantIndex()
	{
		return variantIndex;
	}

	public int GetVariantThemeIndex(string variantThemeName)
	{
		int result = 0;
		for (int i = 0; i < variantList.Count; i++)
		{
			if (variantList[i].themeName == variantThemeName)
			{
				_ = variantList[i];
				result = i;
				break;
			}
		}
		return result;
	}

	public bool SetVariant(string variantName)
	{
		if (!GetVariantNames().Contains(variantName))
		{
			Debug.LogErrorFormat("Variant name does not exist to be set. Part:{0} , VariantName: {1}", base.part.name, variantName);
			return false;
		}
		variantIndex = GetVariantIndex(variantName);
		RefreshVariant();
		return true;
	}

	public List<string> GetVariantNames()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < variantList.Count; i++)
		{
			list.Add(variantList[i].Name);
		}
		return list;
	}

	public bool HasVariant(string variantName)
	{
		return GetVariantNames().Contains(variantName);
	}

	public bool SetVariantTheme(string variantThemeName)
	{
		if (!GetVariantThemeNames().Contains(variantThemeName))
		{
			Debug.LogErrorFormat("Variant theme does not exist to be set. Part:{0} , VariantName: {1}", base.part.name, variantThemeName);
			return false;
		}
		variantIndex = GetVariantThemeIndex(variantThemeName);
		RefreshVariant();
		EditorLogic.fetch.SetBackup();
		return true;
	}

	public List<string> GetVariantThemeNames()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < variantList.Count; i++)
		{
			list.Add(variantList[i].themeName);
		}
		return list;
	}

	public bool HasVariantTheme(string variantThemeName)
	{
		return GetVariantThemeNames().Contains(variantThemeName);
	}

	public void RefreshVariant()
	{
		if (variantIndex < 0 || variantIndex > variantList.Count)
		{
			return;
		}
		ApplyVariant(base.part, base.part.transform.Find("model"), variantList[variantIndex], partMaterials.ToArray(), skipShader: false, variantIndex);
		if (!(moduleJettison == null) && (!(moduleJettison != null) || !moduleJettison.isJettisoned))
		{
			if (moduleJettison != null && !moduleJettison.isJettisoned)
			{
				SetCubes(state: false);
			}
		}
		else
		{
			SetCubes(state: true);
		}
	}

	public void SetCubes(bool state)
	{
		if (useMultipleDragCubes && !base.part.DragCubes.Procedural)
		{
			string text = variantIndex.ToString();
			if (base.part.DragCubes.GetCube(text) != null)
			{
				base.part.DragCubes.ResetCubeWeights(0f);
				if (state)
				{
					base.part.DragCubes.SetCubeWeight(text, 1f);
				}
			}
			else if (HighLogic.LoadedSceneIsFlight)
			{
				Debug.LogError("[ModulePartVariants] Drag cube named " + text + " not found for multiple drag cubes for part " + base.part.name + " !");
			}
		}
		base.part.DragCubes.ForceUpdate(weights: true, occlusion: true, resetProcTiming: true);
	}

	public override void OnAwake()
	{
		base.OnAwake();
		List<Renderer> list = base.part.FindModelComponents<Renderer>();
		base.part.baseVariant = new PartVariant(Localizer.Format("#autoLOC_8007005"), Localizer.Format("#autoLOC_8007005"), base.part.attachNodes, base.part.srfAttachNode);
		partMaterials = new List<Material>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].material != null && !list[i].material.name.ToLower().Contains("flag") && !list[i].material.name.ToLower().Contains("auto truss"))
			{
				partMaterials.Add(list[i].material);
			}
			base.part.baseVariant.TryCopyMaterial(list[i].material);
		}
	}

	public override void OnStart(StartState state)
	{
		if (state == StartState.Editor)
		{
			UI_VariantSelector obj = base.Fields["variantIndex"].uiControlEditor as UI_VariantSelector;
			obj.variants = variantList;
			obj.onFieldChanged = (Callback<BaseField, object>)Delegate.Combine(obj.onFieldChanged, new Callback<BaseField, object>(onVariantChanged));
			obj.onSymmetryFieldChanged = (Callback<BaseField, object>)Delegate.Combine(obj.onSymmetryFieldChanged, new Callback<BaseField, object>(onVariantSymmetrycallyChanged));
		}
		if (base.part != null)
		{
			moduleJettison = base.part.Modules.GetModule<ModuleJettison>();
		}
		RefreshVariant();
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		if (variantList == null)
		{
			variantList = new List<PartVariant>();
			string value = node.GetValue("primaryColor");
			if (!string.IsNullOrEmpty(value))
			{
				base.part.baseVariant.PrimaryColor = value;
			}
			string value2 = node.GetValue("secondaryColor");
			if (!string.IsNullOrEmpty(value2))
			{
				base.part.baseVariant.SecondaryColor = value2;
			}
			string value3 = node.GetValue("sizeGroup");
			if (!string.IsNullOrEmpty(value3))
			{
				base.part.baseVariant.SizeGroup = value3;
			}
			string value4 = node.GetValue("baseName");
			if (!string.IsNullOrEmpty(value4))
			{
				base.part.baseVariant.Name = value4;
				base.part.baseVariant.DisplayName = value4;
			}
			string value5 = node.GetValue("baseDisplayName");
			if (!string.IsNullOrEmpty(value5))
			{
				base.part.baseVariant.DisplayName = value5;
			}
			string value6 = node.GetValue("baseMass");
			if (!string.IsNullOrEmpty(value6))
			{
				float result = 0f;
				float.TryParse(value6, out result);
				base.part.baseVariant.Mass = result;
			}
			string value7 = node.GetValue("baseCost");
			if (!string.IsNullOrEmpty(value7))
			{
				float result2 = 0f;
				float.TryParse(value7, out result2);
				base.part.baseVariant.Cost = result2;
			}
			string value8 = node.GetValue("baseThemeName");
			if (!string.IsNullOrEmpty(value8))
			{
				base.part.baseVariant.themeName = value8;
			}
			base.part.baseVariant.AttachRules = base.part.attachRules;
			ConfigNode[] nodes = node.GetNodes("VARIANT");
			int i = 0;
			for (int num = nodes.Length; i < num; i++)
			{
				ConfigNode node2 = nodes[i];
				PartVariant partVariant = new PartVariant(base.part.baseVariant);
				partVariant.Load(node2);
				variantList.Add(partVariant);
			}
			string value9 = string.Empty;
			if (node.TryGetValue("baseVariant", ref value9))
			{
				int index = GetVariantIndex(value9);
				PartVariant partVariant2 = variantList[index];
				if (partVariant2 != null)
				{
					base.part.baseVariant = partVariant2;
					variantIndex = index;
				}
			}
			else
			{
				variantList.Insert(0, base.part.baseVariant);
			}
		}
		string value10 = string.Empty;
		node.TryGetValue("selectedVariant", ref value10);
		if (!string.IsNullOrEmpty(value10))
		{
			variantIndex = GetVariantIndex(value10);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		if (variantIndex > 0 && variantIndex < variantList.Count)
		{
			node.AddValue("selectedVariant", SelectedVariant.Name);
		}
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_8007000");
	}

	public string GetModuleTitle()
	{
		return GetTitle();
	}

	public override string GetInfo()
	{
		string text = string.Empty;
		for (int i = 0; i < variantList.Count; i++)
		{
			text = text + Localizer.Format(variantList[i].DisplayName) + "\n";
		}
		return text;
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return Localizer.Format("#autoLOC_8007002", variantList.Count);
	}

	public static string GetTitle()
	{
		return "PartVariants";
	}

	public string[] GetDragCubeNames()
	{
		int count = variantList.Count;
		string[] array = new string[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = i.ToString();
		}
		return array;
	}

	public void AssumeDragCubePosition(string name)
	{
		int result = 0;
		if (int.TryParse(name, out result))
		{
			variantIndex = result;
			RefreshVariant();
		}
	}

	public bool UsesProceduralDragCubes()
	{
		return useProceduralDragCubes;
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		if (HighLogic.LoadedSceneIsEditor)
		{
			useVariantMass = true;
		}
		if (useVariantMass)
		{
			return SelectedVariant.Mass;
		}
		return 0f;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}

	public float GetModuleCost(float defaultCost, ModifierStagingSituation sit)
	{
		return SelectedVariant.Cost;
	}

	public ModifierChangeWhen GetModuleCostChangeWhen()
	{
		return ModifierChangeWhen.FIXED;
	}
}
