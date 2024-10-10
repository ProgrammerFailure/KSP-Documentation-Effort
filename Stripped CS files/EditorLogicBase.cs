using System.Collections.Generic;
using ns11;
using ns2;
using ns9;
using UnityEngine;

public class EditorLogicBase : MonoBehaviour
{
	[SerializeField]
	public KerbalFSM fsm;

	public Vector3 selPartGrabOffset;

	public Vector3 dragPlaneCenter;

	public Vector3 srfAttachCursorOffset;

	public ScreenMessage modeMsg;

	public ScreenMessage interactMsg;

	[SerializeField]
	public GameObject attachNodePrefab;

	public Part selectedPart;

	public CompoundPart selectedCompoundPart;

	public Quaternion stackRotation;

	public bool partTweaked;

	public bool checkInputLocks = true;

	public EditorScreen editorScreen;

	public bool isCurrentPartFlag;

	public static string cacheAutoLOC_125488;

	public static string cacheAutoLOC_125583;

	public static string cacheAutoLOC_125724;

	public static string cacheAutoLOC_125784;

	public static string cacheAutoLOC_125798;

	public static string cacheAutoLOC_6001217;

	public static string cacheAutoLOC_6001218;

	public static string cacheAutoLOC_6001219;

	public static string cacheAutoLOC_6001220;

	public static string cacheAutoLOC_6001221;

	public static string cacheAutoLOC_6001222;

	public static string cacheAutoLOC_6001223;

	public static string cacheAutoLOC_6001224;

	public static string cacheAutoLOC_6001225;

	public static string cacheAutoLOC_6001226;

	public static string cacheAutoLOC_6004038;

	public static string cacheAutoLOC_6006095;

	public bool IsCurrentPartFlag => isCurrentPartFlag;

	public void partRotationInputUpdate()
	{
		partTweaked = false;
		if (!checkInputLocks || InputLockManager.IsUnlocked(ControlTypes.EDITOR_GIZMO_TOOLS) || editorScreen == EditorScreen.Cargo)
		{
			if (fsm == null || fsm.currentStateName != "st_rotate_tweak")
			{
				if (GameSettings.Editor_yawLeft.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? 5 : 90, Quaternion.Inverse(selectedPart.initRotation) * Vector3.forward) * selectedPart.attRotation;
					partTweaked = true;
				}
				else if (GameSettings.Editor_yawRight.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? (-5) : (-90), Quaternion.Inverse(selectedPart.initRotation) * Vector3.forward) * selectedPart.attRotation;
					partTweaked = true;
				}
				if (GameSettings.Editor_rollLeft.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? 5 : 90, Quaternion.Inverse(selectedPart.initRotation) * Vector3.up) * selectedPart.attRotation;
					partTweaked = true;
				}
				else if (GameSettings.Editor_rollRight.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? (-5) : (-90), Quaternion.Inverse(selectedPart.initRotation) * Vector3.up) * selectedPart.attRotation;
					partTweaked = true;
				}
				else if (GameSettings.Editor_pitchUp.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? (-5) : (-90), Quaternion.Inverse(selectedPart.initRotation) * Vector3.right) * selectedPart.attRotation;
					partTweaked = true;
				}
				else if (GameSettings.Editor_pitchDown.GetKeyDown())
				{
					selectedPart.attRotation = Quaternion.AngleAxis(GameSettings.Editor_fineTweak.GetKey() ? 5 : 90, Quaternion.Inverse(selectedPart.initRotation) * Vector3.right) * selectedPart.attRotation;
					partTweaked = true;
				}
			}
			partRotationResetUpdate();
		}
		if (partTweaked)
		{
			GameEvents.onEditorPartEvent.Fire(ConstructionEventType.PartRotated, selectedPart);
			partTweaked = false;
		}
	}

	public virtual void partRotationResetUpdate()
	{
	}

	public void displayAttachNodeIcons(List<Part> parts, Part selectedPart, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		int i = 0;
		for (int count = parts.Count; i < count; i++)
		{
			handleAttachNodeIcons(parts[i], stackNodes, srfNodes, dockNodes);
		}
		handleAttachNodeIcons(selectedPart, stackNodes, srfNodes, dockNodes);
		handleChildNodeIcons(selectedPart);
	}

	public void handleChildNodeIcons(Part part)
	{
		int i = 0;
		for (int count = part.children.Count; i < count; i++)
		{
			Part part2 = part.children[i];
			handleAttachNodeIcons(part2, stackNodes: false, srfNodes: false, dockNodes: false);
			handleChildNodeIcons(part2);
		}
	}

	public void handleAttachNodeIcons(Part part, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		int i = 0;
		for (int count = part.attachNodes.Count; i < count; i++)
		{
			AssignAttachIcon(part, part.attachNodes[i], stackNodes, srfNodes, dockNodes);
		}
	}

	public void AssignAttachIcon(Part part, AttachNode node, bool stackNodes, bool srfNodes, bool dockNodes)
	{
		if ((stackNodes || node.nodeType != 0) && (dockNodes || node.nodeType != AttachNode.NodeType.Dock) && node.nodeType != AttachNode.NodeType.Surface && (!(node.attachedPart != null) || CheatOptions.AllowPartClipping || !(part != null)))
		{
			if (node.icon == null)
			{
				node.icon = Object.Instantiate(attachNodePrefab);
				node.icon.gameObject.SetActive(value: true);
				node.icon.transform.localScale = Vector3.one * node.radius * ((node.size == 0) ? ((float)node.size + 0.5f) : ((float)node.size));
			}
			node.icon.transform.position = part.transform.TransformPoint(node.position);
			node.icon.transform.up = part.transform.TransformDirection(node.orientation);
			switch (node.nodeType)
			{
			case AttachNode.NodeType.Dock:
			{
				Color grassyGreen = XKCDColors.AquaBlue;
				grassyGreen.a = 0.5f;
				node.icon.GetComponent<Renderer>().material.color = grassyGreen;
				break;
			}
			case AttachNode.NodeType.Stack:
			{
				Color grassyGreen = XKCDColors.GrassyGreen;
				grassyGreen.a = 0.5f;
				node.icon.GetComponent<Renderer>().material.color = grassyGreen;
				break;
			}
			}
		}
		else if (node.icon != null)
		{
			node.DestroyNodeIcon();
			node.icon = null;
		}
	}

	public static void clearAttachNodes(Part part, Part otherPart)
	{
		if (!otherPart)
		{
			return;
		}
		AttachNode attachNode = null;
		int i = 0;
		for (int count = otherPart.attachNodes.Count; i < count; i++)
		{
			attachNode = otherPart.attachNodes[i];
			if (!(attachNode.attachedPart != part))
			{
				attachNode.attachedPart = null;
				break;
			}
		}
		if (otherPart.srfAttachNode.attachedPart == part)
		{
			otherPart.srfAttachNode.attachedPart = null;
		}
		int j = 0;
		for (int count2 = part.attachNodes.Count; j < count2; j++)
		{
			attachNode = part.attachNodes[j];
			if (!(attachNode.attachedPart != otherPart))
			{
				attachNode.attachedPart = null;
				break;
			}
		}
		if (part.srfAttachNode.attachedPart == otherPart)
		{
			part.srfAttachNode.attachedPart = null;
		}
	}

	public bool PartInHierarchy(Part part, Part tgtPart)
	{
		if (part.children.Contains(tgtPart))
		{
			return true;
		}
		int num = 0;
		int count = part.children.Count;
		while (true)
		{
			if (num < count)
			{
				if (PartInHierarchy(part.children[num], tgtPart))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public float FindPartSurface(Part p, Vector3 origin, Vector3 direction, Vector3 fromParent, RaycastHit hit, out Vector3 srfNormal)
	{
		return FindPartSurface(p, origin, direction, fromParent, hit, out srfNormal, 0);
	}

	public float FindPartSurface(Part p, Vector3 origin, Vector3 direction, Vector3 fromParent, RaycastHit hit, out Vector3 srfNormal, int layerMask)
	{
		List<Collider> list = p.FindModelComponents<Collider>();
		float num = 0f;
		srfNormal = hit.normal;
		int count = list.Count;
		while (count-- > 0)
		{
			if ((layerMask <= 0 || ((1 << list[count].gameObject.layer) & layerMask) <= 0) && list[count].Raycast(new Ray(origin + fromParent * 100f, -fromParent), out hit, 110f))
			{
				Vector3 vector = Vector3.ProjectOnPlane(hit.point - origin, direction);
				if (vector.sqrMagnitude > num)
				{
					num = vector.sqrMagnitude;
					srfNormal = hit.normal;
				}
			}
		}
		if (num != 0f)
		{
			return Mathf.Sqrt(num);
		}
		return 1f;
	}

	public static List<Part> FindPartsInChildren(Part part)
	{
		List<Part> parts = new List<Part>();
		FindPartsInChildren(ref parts, part);
		return parts;
	}

	public static void FindPartsInChildren(ref List<Part> parts, Part part)
	{
		parts.Add(part);
		int i = 0;
		for (int count = part.children.Count; i < count; i++)
		{
			FindPartsInChildren(ref parts, part.children[i]);
		}
	}

	public void CreateSelectedPartIcon()
	{
		if (!(selectedPart == null))
		{
			int variantIdx = -1;
			string partIconTexturePath = CraftThumbnail.GetPartIconTexturePath(selectedPart, out variantIdx);
			if (!string.IsNullOrEmpty(partIconTexturePath))
			{
				string fullFileName = "";
				Texture2D iconTexture = CraftThumbnail.TakePartSnapshot(selectedPart.name, null, selectedPart, 256, partIconTexturePath.Substring(0, partIconTexturePath.LastIndexOf(selectedPart.name)), out fullFileName, 15f, 25f, 15f, 25f, 18f, variantIdx);
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon = Object.Instantiate(UIPartActionControllerInventory.Instance.InventoryOnlyIconPrefab, UIMasterController.Instance.actionCanvas.transform).GetComponent<EditorInventoryOnlyIcon>();
				EditorInventoryOnlyIcon currentInventoryOnlyIcon = UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon;
				currentInventoryOnlyIcon.SetIconTexture(iconTexture);
				currentInventoryOnlyIcon.SetStackAmount(UIPartActionControllerInventory.stackSize);
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.ToggleBackgroundVisibility(UIPartActionControllerInventory.stackSize > 1);
				UIPartActionControllerInventory.Instance.CurrentInventoryOnlyIcon.gameObject.SetActive(value: false);
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_125488 = Localizer.Format("#autoLOC_125488");
		cacheAutoLOC_125583 = Localizer.Format("#autoLOC_125583");
		cacheAutoLOC_125724 = Localizer.Format("#autoLOC_125724");
		cacheAutoLOC_125784 = Localizer.Format("#autoLOC_125784");
		cacheAutoLOC_125798 = Localizer.Format("#autoLOC_125798");
		cacheAutoLOC_6001217 = Localizer.Format("#autoLOC_6001217");
		cacheAutoLOC_6001218 = Localizer.Format("#autoLOC_6001218");
		cacheAutoLOC_6001219 = Localizer.Format("#autoLOC_6001219");
		cacheAutoLOC_6001220 = Localizer.Format("#autoLOC_6001220");
		cacheAutoLOC_6001221 = Localizer.Format("#autoLOC_6001221");
		cacheAutoLOC_6001222 = Localizer.Format("#autoLOC_6001222");
		cacheAutoLOC_6001223 = Localizer.Format("#autoLOC_6001223");
		cacheAutoLOC_6001224 = Localizer.Format("#autoLOC_6001224");
		cacheAutoLOC_6001225 = Localizer.Format("#autoLOC_6001225");
		cacheAutoLOC_6001226 = Localizer.Format("#autoLOC_6001226");
		cacheAutoLOC_6004038 = Localizer.Format("#autoLOC_6004038");
		cacheAutoLOC_6006095 = Localizer.Format("#autoLOC_6006095");
	}
}
