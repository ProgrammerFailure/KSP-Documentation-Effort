using System;
using System.Collections.Generic;
using ns2;
using ns4;
using ns9;
using TMPro;
using UnityEngine;

namespace ns11;

public class RDController : MonoBehaviour
{
	public static RDController Instance;

	public RDTechTree techTree;

	public UIStatePanel nodePanel;

	public RDGridArea gridArea;

	public RDNode node_inPanel;

	public TextMeshProUGUI node_name;

	public TextMeshProUGUI node_description;

	public UIStateButton actionButton;

	public TextMeshProUGUI actionButtonText;

	public RDPartList partList;

	public RDNodeList requiresList;

	public TextMeshProUGUI requiresCaption;

	public GameObject RDNodePrefab;

	public Transform RDNodeRoot;

	public List<RDNode> nodes = new List<RDNode>();

	[NonSerialized]
	public RDNode node_selected;

	public float scienceCap;

	public static EventData<RDController> OnRDTreeSpawn = new EventData<RDController>("OnRDTreeSpawn");

	public static EventData<RDController> OnRDTreeDespawn = new EventData<RDController>("OnRDTreeDespawn");

	[NonSerialized]
	public IconLoader iconLoader;

	[SerializeField]
	public IconLoader iconLoaderPrefab;

	public static HashSet<AvailablePart> otherPartHashes = new HashSet<AvailablePart>();

	public float ScienceCap
	{
		get
		{
			return scienceCap;
		}
		set
		{
			scienceCap = value;
		}
	}

	public void Awake()
	{
		if (Instance != null)
		{
			Debug.LogError("RDController: Instance already exist.");
			base.gameObject.DestroyGameObject();
		}
		else
		{
			Instance = this;
		}
	}

	public void OnEnable()
	{
		if (node_selected != null)
		{
			ShowNodePanel(node_selected);
		}
		else
		{
			ShowNothingPanel();
		}
	}

	public void Start()
	{
		iconLoader = UnityEngine.Object.Instantiate(iconLoaderPrefab);
		actionButton.onClickState.AddListener(ActionButtonClick);
		if (HighLogic.LoadedSceneIsGame)
		{
			scienceCap = GameVariables.Instance.GetScienceCostLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment));
		}
		else
		{
			scienceCap = float.MaxValue;
		}
		ShowNothingPanel();
		StartCoroutine(CallbackUtil.DelayedCallback(2, delegate
		{
			OnRDTreeSpawn.Fire(this);
			SnapToStartNode();
		}));
	}

	public void OnDestroy()
	{
		OnRDTreeDespawn.Fire(this);
		actionButton.onClickState.RemoveListener(ActionButtonClick);
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void ActionButtonClick(string state)
	{
		if (state == "research")
		{
			node_selected.tech.ResearchTech();
			techTree.RefreshUI();
		}
		else
		{
			if (!(state == "purchase"))
			{
				return;
			}
			int count = node_selected.tech.partsAssigned.Count;
			for (int i = 0; i < count; i++)
			{
				AvailablePart ap = node_selected.tech.partsAssigned[i];
				if (!node_selected.tech.PartIsPurchased(ap))
				{
					node_selected.tech.PurchasePart(ap);
				}
			}
			node_selected.UpdateGraphics();
			partList.SetupParts(node_selected);
			UpdatePanel();
		}
	}

	public void ShowNodePanel(RDNode node)
	{
		nodePanel.SetState("node");
		UpdatePanel();
		node_name.text = node.name;
		node_description.text = node.description;
		partList.SetupParts(node);
		if (node.parents.Length != 0)
		{
			if (node.parents.Length > 1)
			{
				if (node.AnyParentToUnlock)
				{
					requiresCaption.text = Localizer.Format("#autoLOC_469075");
				}
				else
				{
					requiresCaption.text = Localizer.Format("#autoLOC_469079");
				}
			}
			else
			{
				requiresCaption.text = Localizer.Format("#autoLOC_469084");
			}
			requiresList.AddNodes(node.parents);
		}
		else
		{
			requiresCaption.text = Localizer.Format("#autoLOC_469091");
			requiresList.ClearList(destroyItems: true);
		}
	}

	public void UpdatePanel()
	{
		if (node_selected.IsResearched)
		{
			node_inPanel.SetButtonState(RDNode.State.RESEARCHED);
			node_inPanel.SetIconState(node_selected.icon);
			actionButton.SetState("purchase");
			UpdatePurchaseButton();
			return;
		}
		if (node_selected.state == RDNode.State.FADED)
		{
			node_inPanel.SetButtonState(RDNode.State.FADED);
			node_inPanel.SetIconState(node_selected.icon);
			actionButton.gameObject.SetActive(value: false);
			return;
		}
		node_inPanel.SetButtonState(RDNode.State.RESEARCHABLE);
		node_inPanel.SetIconState(node_selected.icon);
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDTechResearch, 0f, -node_selected.tech.scienceCost, 0f);
		actionButtonText.text = Localizer.Format("#autoLOC_469120", currencyModifierQuery.GetCostLine(displayInverted: true, useCurrencyColors: false, useInsufficientCurrencyColors: false));
		actionButton.gameObject.SetActive(value: true);
		actionButton.Enable(enable: true);
		actionButton.SetState("research");
	}

	public void UpdatePurchaseButton()
	{
		if (!RUIutils.All(node_selected.tech.partsAssigned, (AvailablePart a) => node_selected.tech.partsPurchased.Contains(a)))
		{
			actionButton.gameObject.SetActive(value: true);
			int count = node_selected.tech.partsAssigned.Count;
			int num = 0;
			int num2 = 0;
			otherPartHashes.Clear();
			for (int i = 0; i < count; i++)
			{
				AvailablePart availablePart = node_selected.tech.partsAssigned[i];
				if (node_selected.tech.partsPurchased.Contains(availablePart) || otherPartHashes.Contains(availablePart))
				{
					continue;
				}
				if (!availablePart.TechHidden)
				{
					num++;
					num2 += availablePart.entryCost;
				}
				otherPartHashes.Add(availablePart);
				string[] array = availablePart.identicalParts.Split(',');
				int num3 = array.Length;
				while (num3-- > 0)
				{
					AvailablePart partInfoByName = PartLoader.getPartInfoByName(array[num3].Replace('_', '.').Trim());
					if (partInfoByName != null)
					{
						otherPartHashes.Add(partInfoByName);
					}
				}
			}
			CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.RnDPartPurchase, -num2, 0f, 0f);
			if (num > 0)
			{
				actionButtonText.text = Localizer.Format("#autoLOC_7003283", num, currencyModifierQuery.GetCostLine());
			}
			else
			{
				actionButton.gameObject.SetActive(value: false);
			}
			if (currencyModifierQuery.CanAfford())
			{
				actionButton.Enable(enable: true);
			}
			else
			{
				actionButton.Enable(enable: false);
			}
		}
		else
		{
			actionButton.gameObject.SetActive(value: false);
		}
	}

	public void ShowNothingPanel()
	{
		nodePanel.SetState("nothing");
	}

	public void RegisterNode(RDNode node)
	{
		nodes.AddUnique(node);
		int num = node.parents.Length;
		for (int i = 0; i < num; i++)
		{
			node.parents[i].parent.node.children.AddUnique(node);
		}
	}

	[ContextMenu("Snap to start node")]
	public void SnapToStartNode()
	{
		gridArea.SnapToNode(nodes[0], new Vector2((gridArea.scrollRect.transform as RectTransform).rect.width / 3f, 0f));
	}
}
