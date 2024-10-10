using UnityEngine;

public class InternalLabel : InternalModule
{
	[KSPField]
	public string transformName = "anim";

	[KSPField]
	public string text = "Label";

	[KSPField]
	public string textFont = "Arial";

	[KSPField]
	public float textSize = 1f;

	[KSPField]
	public bool textWrapping;

	[KSPField]
	public Color textColor = new Color(0.8627f, 0.8627f, 0.8627f, 1f);

	[KSPField]
	public string textAlign = "TopLeft";

	public Transform labelTransform;

	public InternalText textObj;

	public override void OnAwake()
	{
		if (labelTransform == null)
		{
			labelTransform = internalProp.FindModelTransform(transformName);
			if (labelTransform == null)
			{
				Debug.Log("InternalSeat: Cannot find InternalLabel transformName '" + transformName + "'");
				return;
			}
		}
		if (InternalComponents.Instance != null)
		{
			textObj = InternalComponents.Instance.CreateText(textFont, textSize, labelTransform, text, textColor, textWrapping, textAlign);
		}
	}

	public void SetText(string text)
	{
		if (!(textObj == null))
		{
			textObj.text.text = text;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("textAlign"))
		{
			textAlign = node.GetValue("textAlign");
		}
		if (node.HasValue("textWrapping"))
		{
			node.TryGetValue("textWrapping", ref textWrapping);
		}
		if (node.HasValue("textColor"))
		{
			node.TryGetValue("textColor", ref textColor);
		}
	}
}
