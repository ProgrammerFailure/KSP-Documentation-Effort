using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public class GenericCascadingList : MonoBehaviour
{
	public class CollapsableBody
	{
		public TextMeshProUGUI text;

		public string label;

		public string label2;

		public bool collapsed = true;

		public CollapsableBody(TextMeshProUGUI text, string label, string label2)
		{
			this.text = text;
			this.label = label;
			this.label2 = label2;
		}
	}

	public UIListItem cascadeHeader;

	public UIListItem cascadeFooter;

	public UIListItem cascadeBody;

	public UIListItem cascadeBody_keyValue;

	public UICascadingList ruiList;

	public void Setup(UIList scrollList)
	{
		ruiList.cascadingList = scrollList;
		base.gameObject.SetActive(value: false);
	}

	public UIListItem CreateHeader(string label, out Button button, bool scaleBg = false)
	{
		UIListItem uIListItem = Object.Instantiate(cascadeHeader);
		uIListItem.GetComponentInChildren<TextMeshProUGUI>().text = label;
		button = uIListItem.GetComponentInChildren<Button>();
		return uIListItem;
	}

	public UIListItem_spacer CreateBody_spacer(UIListItem_spacer prefab, string label, int space)
	{
		UIListItem_spacer uIListItem_spacer = Object.Instantiate(prefab);
		uIListItem_spacer.GetComponentInChildren<TextMeshProUGUI>().text = label;
		uIListItem_spacer.spacer.minWidth = space;
		return uIListItem_spacer;
	}

	public UIListItem_spacer CreateBodyCollapsable_spacer(UIListItem_spacer prefab, string label, string label2, int maxWidth)
	{
		UIListItem_spacer uIListItem_spacer = Object.Instantiate(prefab);
		TextMeshProUGUI componentInChildren = uIListItem_spacer.GetComponentInChildren<TextMeshProUGUI>();
		componentInChildren.text = label;
		PointerClickHandler component = uIListItem_spacer.GetComponent<PointerClickHandler>();
		component.onPointerClick.AddListener(MouseInputCollapsableBody_spacer);
		component.Data = new CollapsableBody(componentInChildren, label, label2);
		return uIListItem_spacer;
	}

	public void MouseInputCollapsableBody_spacer(PointerEventData eventData)
	{
		CollapsableBody collapsableBody = eventData.pointerPress.GetComponent<PointerClickHandler>().Data as CollapsableBody;
		collapsableBody.collapsed = !collapsableBody.collapsed;
		if (collapsableBody.collapsed)
		{
			collapsableBody.text.text = collapsableBody.label;
		}
		else
		{
			collapsableBody.text.text = collapsableBody.label2;
		}
	}

	public UIListItem CreateBodyKeyValueAutofit(string key, string value)
	{
		UIListItem uIListItem = Object.Instantiate(cascadeBody_keyValue);
		TextMeshProUGUI textElement = uIListItem.GetTextElement("keyRich");
		TextMeshProUGUI textElement2 = uIListItem.GetTextElement("valueRich");
		textElement.text = key;
		textElement2.text = value;
		return uIListItem;
	}

	public UIListItem CreateBody(string label)
	{
		return CreateBody(cascadeBody, label);
	}

	public UIListItem CreateBody(UIListItem prefab, string label)
	{
		UIListItem uIListItem = Object.Instantiate(prefab);
		uIListItem.GetComponentInChildren<TextMeshProUGUI>().text = label;
		return uIListItem;
	}

	public UIListItem CreateBody(string key, string value)
	{
		UIListItem uIListItem = Object.Instantiate(cascadeBody_keyValue);
		TextMeshProUGUI textElement = uIListItem.GetTextElement("keyRich");
		TextMeshProUGUI textElement2 = uIListItem.GetTextElement("valueRich");
		textElement.text = key;
		textElement2.text = value;
		return uIListItem;
	}

	public UIListItem CreateBodyCollapsable(string label, string label2)
	{
		return CreateBodyCollapsable(cascadeBody, label, label2);
	}

	public UIListItem CreateBodyCollapsable(UIListItem prefab, string label, string label2)
	{
		UIListItem uIListItem = Object.Instantiate(prefab);
		TextMeshProUGUI componentInChildren = uIListItem.GetComponentInChildren<TextMeshProUGUI>();
		componentInChildren.text = label;
		PointerClickHandler component = uIListItem.GetComponent<PointerClickHandler>();
		component.onPointerClick.AddListener(MouseInputCollapsableBody);
		component.Data = new CollapsableBody(componentInChildren, label, label2);
		return uIListItem;
	}

	public void UpdateBodyCollapsable(UIListItem lic, string label, string label2)
	{
		CollapsableBody collapsableBody = lic.GetComponent<PointerClickHandler>().Data as CollapsableBody;
		collapsableBody.label = label;
		collapsableBody.label2 = label2;
		if (collapsableBody.collapsed)
		{
			collapsableBody.text.text = collapsableBody.label;
		}
		else
		{
			collapsableBody.text.text = collapsableBody.label2;
		}
	}

	public void MouseInputCollapsableBody(PointerEventData eventData)
	{
		CollapsableBody collapsableBody = eventData.pointerPress.GetComponent<PointerClickHandler>().Data as CollapsableBody;
		collapsableBody.collapsed = !collapsableBody.collapsed;
		if (collapsableBody.collapsed)
		{
			collapsableBody.text.text = collapsableBody.label;
		}
		else
		{
			collapsableBody.text.text = collapsableBody.label2;
		}
	}

	public UIListItem CreateFooter()
	{
		return Object.Instantiate(cascadeFooter);
	}
}
