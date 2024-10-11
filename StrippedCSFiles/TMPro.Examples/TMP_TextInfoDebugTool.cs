using System.Runtime.CompilerServices;
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

	private TMP_Text m_TextComponent;

	private Transform m_Transform;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_TextInfoDebugTool()
	{
		throw null;
	}
}
