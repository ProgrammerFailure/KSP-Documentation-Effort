using UnityEngine;

namespace TMPro.Examples;

[ExecuteInEditMode]
public class TMP_TextInfoDebugTool : MonoBehaviour
{
	public bool ShowCharacters;

	public bool ShowWords;

	public bool ShowLinks;

	public bool ShowLines;

	public bool ShowMeshBounds;

	public bool ShowTextBounds;

	[TextArea(2, 2)]
	[Space(10f)]
	public string ObjectStats;

	public TMP_Text m_TextComponent;

	public Transform m_Transform;
}
