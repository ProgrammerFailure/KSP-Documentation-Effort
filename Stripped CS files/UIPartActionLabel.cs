using TMPro;
using UnityEngine.UI;

[UI_Label]
public class UIPartActionLabel : UIPartActionFieldItem
{
	public TextMeshProUGUI labelText;

	public LayoutElement layoutElement;

	public float baseHeight;

	public void Awake()
	{
		if (labelText == null)
		{
			labelText = base.gameObject.GetComponentInChildren<TextMeshProUGUI>();
		}
		layoutElement = base.gameObject.GetComponent<LayoutElement>();
		if (layoutElement != null)
		{
			baseHeight = layoutElement.preferredHeight;
		}
	}

	public override void UpdateItem()
	{
		string text = field.GuiString(field.host);
		labelText.text = text;
		int num = 1;
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == '\n')
			{
				num++;
			}
		}
		if (layoutElement != null)
		{
			layoutElement.preferredHeight = (float)num * baseHeight;
		}
	}
}
