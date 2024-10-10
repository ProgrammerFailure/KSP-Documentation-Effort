using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogGUIContentSizer : DialogGUIBase
{
	public ContentSizeFitter.FitMode widthMode;

	public ContentSizeFitter.FitMode heightMode;

	public bool useParentSize;

	public ContentSizeFitter fitter;

	public DialogGUIContentSizer(ContentSizeFitter.FitMode widthMode, ContentSizeFitter.FitMode heightMode, bool useParentSize = false)
	{
		this.widthMode = widthMode;
		this.heightMode = heightMode;
		this.useParentSize = useParentSize;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		uiItem = layouts.Peek().gameObject;
		fitter = uiItem.AddComponent<ContentSizeFitter>();
		fitter.enabled = false;
		fitter.horizontalFit = widthMode;
		fitter.verticalFit = heightMode;
		return uiItem;
	}

	public override void Update()
	{
		base.Update();
		RectTransform rectTransform = ((!useParentSize) ? uiItem.GetComponent<RectTransform>() : uiItem.transform.parent.GetComponent<RectTransform>());
		RectTransform component = uiItem.GetComponent<RectTransform>();
		Vector2 vector = new Vector2(LayoutUtility.GetMinWidth(component), LayoutUtility.GetMinHeight(component));
		Vector2 vector2 = new Vector2(LayoutUtility.GetPreferredWidth(component), LayoutUtility.GetPreferredHeight(component));
		if ((widthMode == ContentSizeFitter.FitMode.MinSize && vector.x > rectTransform.rect.size.x) || (heightMode == ContentSizeFitter.FitMode.MinSize && vector.y > rectTransform.rect.size.y) || (widthMode == ContentSizeFitter.FitMode.PreferredSize && vector2.x > rectTransform.rect.size.x) || (heightMode == ContentSizeFitter.FitMode.PreferredSize && vector2.y > rectTransform.rect.size.y))
		{
			if (!fitter.enabled)
			{
				fitter.enabled = true;
				base.Dirty = true;
			}
		}
		else
		{
			fitter.enabled = false;
		}
	}

	public override void Resize()
	{
		base.Resize();
	}
}
