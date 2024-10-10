using UnityEngine;

namespace TMPro.Examples;

public class SimpleScript : MonoBehaviour
{
	public TextMeshPro m_textMeshPro;

	public const string label = "The <#0050FF>count is: </color>{0:2}";

	public float m_frame;

	public void Start()
	{
		m_textMeshPro = base.gameObject.AddComponent<TextMeshPro>();
		m_textMeshPro.autoSizeTextContainer = true;
		m_textMeshPro.fontSize = 48f;
		m_textMeshPro.alignment = TextAlignmentOptions.Center;
		m_textMeshPro.enableWordWrapping = false;
	}

	public void Update()
	{
		m_textMeshPro.SetText("The <#0050FF>count is: </color>{0:2}", m_frame % 1000f);
		m_frame += 1f * Time.deltaTime;
	}
}
