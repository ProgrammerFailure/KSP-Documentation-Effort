using ns12;
using ns2;
using ns4;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ns11;

public class RDNodePrefab : MonoBehaviour
{
	public Image arrowT;

	public Image arrowB;

	public Image arrowL;

	public Image arrowR;

	public Image searchHighlight;

	public Image selection;

	public Image circle;

	public TextMeshProUGUI circle_label;

	public GameObject scalar;

	public UIStateButton button;

	public Image techIcon;

	public RectOffset offset;

	public TooltipController_TitleAndText tooltip;

	public float width => button.Image.rectTransform.sizeDelta.x;

	public float height => button.Image.rectTransform.sizeDelta.y;

	public void SetIcon(Icon icon, Color color)
	{
		Sprite sprite = Sprite.Create(icon.texture as Texture2D, new Rect(0f, 0f, icon.texture.width, icon.texture.height), new Vector2(0f, 0f), 100f);
		techIcon.sprite = sprite;
		techIcon.color = color;
	}

	public void SetIcon(Icon icon)
	{
		Sprite sprite = Sprite.Create(icon.texture as Texture2D, new Rect(0f, 0f, icon.texture.width, icon.texture.height), new Vector2(0f, 0f), 100f);
		techIcon.sprite = sprite;
	}

	public void SetIconColor(Color color)
	{
		techIcon.color = color;
	}

	public Color GetIconColor()
	{
		return techIcon.color;
	}

	public void Setup()
	{
		offset = new RectOffset(0, 0, 5, 5);
		arrowT.enabled = false;
		arrowB.enabled = false;
		arrowL.enabled = false;
		arrowR.enabled = false;
		circle.enabled = false;
		circle_label.enabled = false;
		UnselectNode();
	}

	public void SetViewable(bool show)
	{
		scalar.SetActive(show);
		circle_label.gameObject.SetActive(show);
	}

	public void SetScale(float scale)
	{
		scalar.transform.localScale = Vector3.one * scale;
	}

	public void AddInputDelegate(UnityAction action)
	{
		button.onClick.AddListener(action);
	}

	public void SetAvailablePartsCircle(int parts)
	{
		circle.enabled = true;
		circle_label.enabled = true;
		if (parts == 0)
		{
			HideAvailablePartsCircle();
		}
		else if (parts <= 9)
		{
			circle_label.text = parts.ToString();
		}
		else
		{
			circle_label.text = "9+";
		}
	}

	public void HideAvailablePartsCircle()
	{
		circle.enabled = false;
		circle_label.enabled = false;
	}

	public void SelectNode()
	{
		selection.enabled = true;
	}

	public void UnselectNode()
	{
		selection.enabled = false;
	}

	public void SetArrowHeadState(RDNode.Parent parent, bool show, Material mat)
	{
		parent.arrowHead.enabled = show;
		parent.arrowHead.color = mat.color;
		parent.line.color = mat.color;
	}

	public Image GetArrowHeadPrefab(RDNode.Anchor anchor)
	{
		return anchor switch
		{
			RDNode.Anchor.const_0 => arrowT, 
			RDNode.Anchor.BOTTOM => arrowB, 
			RDNode.Anchor.RIGHT => arrowR, 
			RDNode.Anchor.LEFT => arrowL, 
			_ => null, 
		};
	}

	public void InstantiateArrowHeadAtPos(RDNode.Parent parent, Vector3 pos, Material mat)
	{
		parent.arrowHead = Object.Instantiate(GetArrowHeadPrefab(parent.anchor));
		parent.arrowHead.transform.position = pos;
		parent.arrowHead.enabled = true;
		parent.arrowHead.gameObject.layer = 5;
		parent.arrowHead.transform.SetParent(parent.parent.node.graphics.scalar.transform);
		parent.arrowHead.transform.localScale = Vector3.one;
		parent.arrowHead.transform.localPosition = new Vector3(parent.arrowHead.transform.localPosition.x, parent.arrowHead.transform.localPosition.y, 0f);
	}
}
