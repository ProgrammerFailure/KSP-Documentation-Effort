using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro.Examples;

public class TMP_TextSelector_B : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler
{
	public RectTransform TextPopup_Prefab_01;

	private RectTransform m_TextPopup_RectTransform;

	private TextMeshProUGUI m_TextPopup_TMPComponent;

	private const string k_LinkText = "You have selected link <#ffff00>";

	private const string k_WordText = "Word Index: <#ffff00>";

	private TextMeshProUGUI m_TextMeshPro;

	private Canvas m_Canvas;

	private Camera m_Camera;

	private bool isHoveringObject;

	private int m_selectedWord;

	private int m_selectedLink;

	private int m_lastIndex;

	private Matrix4x4 m_matrix;

	private TMP_MeshInfo[] m_cachedMeshInfoVertexData;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_TextSelector_B()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ON_TEXT_CHANGED(Object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerEnter(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnPointerUp(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void RestoreCachedVertexAttributes(int index)
	{
		throw null;
	}
}
