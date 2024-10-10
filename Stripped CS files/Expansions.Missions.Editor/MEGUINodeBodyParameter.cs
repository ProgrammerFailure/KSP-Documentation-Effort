using TMPro;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUINodeBodyParameter : MonoBehaviour
{
	public TextMeshProUGUI text;

	public MEGUINode myNode;

	public void Setup(MEGUINode guiNodeReference, string displayString, int parameterPosition)
	{
		myNode = guiNodeReference;
		text.text = displayString;
		if (parameterPosition < 1)
		{
			text.rectTransform.offsetMin = new Vector2((myNode.inputConnectorButton.transform as RectTransform).rect.width, 0f);
			text.rectTransform.offsetMax = new Vector2(0f - (myNode.outputConnectorButton.transform as RectTransform).rect.width, 0f);
		}
		Vector3 localPosition = base.transform.localPosition;
		localPosition.z = 0f;
		base.transform.localPosition = localPosition;
	}
}
