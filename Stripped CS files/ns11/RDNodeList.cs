using ns2;
using UnityEngine;

namespace ns11;

[RequireComponent(typeof(UIList))]
public class RDNodeList : MonoBehaviour
{
	public RDController controller;

	public RDNodeListItem nodeListItemPrefab;

	public UIList scrollList;

	public void Awake()
	{
		scrollList = GetComponent<UIList>();
	}

	public void AddNodes(RDNode.Parent[] arrows)
	{
		scrollList.Clear(destroyElements: true);
		int i = 0;
		for (int num = arrows.Length; i < num; i++)
		{
			RDNodeListItem rDNodeListItem = Object.Instantiate(nodeListItemPrefab);
			RDNode node = rDNodeListItem.node;
			node.controller = controller;
			rDNodeListItem.text.text = arrows[i].parent.node.name;
			node.Setup();
			node.SetIconState(arrows[i].parent.node.icon);
			node.graphics.arrowB.gameObject.SetActive(value: false);
			node.graphics.arrowL.gameObject.SetActive(value: false);
			node.graphics.arrowR.gameObject.SetActive(value: false);
			node.graphics.arrowT.gameObject.SetActive(value: false);
			node.graphics.arrowB.gameObject.SetActive(value: false);
			node.graphics.circle.gameObject.SetActive(value: false);
			node.graphics.circle_label.gameObject.SetActive(value: false);
			node.graphics.selection.gameObject.SetActive(value: false);
			if (arrows[i].parent.node.IsResearched)
			{
				node.SetButtonState(RDNode.State.RESEARCHED);
			}
			else if (arrows[i].parent.node.state != RDNode.State.FADED && arrows[i].parent.node.state != RDNode.State.HIDDEN)
			{
				node.SetButtonState(RDNode.State.RESEARCHABLE);
			}
			else
			{
				node.SetButtonState(RDNode.State.FADED);
			}
			scrollList.AddItem(rDNodeListItem.GetComponent<UIListItem>());
		}
	}

	public void ClearList(bool destroyItems)
	{
		scrollList.Clear(destroyItems);
	}
}
