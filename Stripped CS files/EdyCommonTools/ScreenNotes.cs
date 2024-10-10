using UnityEngine;

namespace EdyCommonTools;

[ExecuteInEditMode]
public class ScreenNotes : MonoBehaviour
{
	[TextArea(2, 40)]
	public string text;

	[Range(6f, 100f)]
	public int size = 10;

	public Color color = Color.white;

	public Vector2 screenPosition = new Vector2(8f, 8f);

	[Space(5f)]
	public Font font;

	public GUIStyle m_smallStyle = new GUIStyle();

	public void OnEnable()
	{
	}

	public void OnValidate()
	{
	}

	public void Update()
	{
		UpdateTextProperties();
	}

	public void UpdateTextProperties()
	{
		m_smallStyle.font = font;
		m_smallStyle.fontSize = size;
		m_smallStyle.normal.textColor = color;
	}

	public void OnGUI()
	{
		float num = ((screenPosition.x < 0f) ? ((float)Screen.width + screenPosition.x - 20f) : screenPosition.x);
		float num2 = ((screenPosition.y < 0f) ? ((float)Screen.height + screenPosition.y - 20f) : screenPosition.y);
		GUI.Label(new Rect(num + 8f, num2 + 8f, Screen.width, Screen.height), text, m_smallStyle);
	}
}
