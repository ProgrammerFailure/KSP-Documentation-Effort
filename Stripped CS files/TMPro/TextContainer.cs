using UnityEngine;
using UnityEngine.EventSystems;

namespace TMPro;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[AddComponentMenu("Layout/Text Container")]
public class TextContainer : UIBehaviour
{
	public bool m_hasChanged;

	[SerializeField]
	public Vector2 m_pivot;

	[SerializeField]
	public TextContainerAnchors m_anchorPosition = TextContainerAnchors.Middle;

	[SerializeField]
	public Rect m_rect;

	public bool m_isDefaultWidth;

	public bool m_isDefaultHeight;

	public bool m_isAutoFitting;

	public Vector3[] m_corners = new Vector3[4];

	public Vector3[] m_worldCorners = new Vector3[4];

	[SerializeField]
	public Vector4 m_margins;

	public RectTransform m_rectTransform;

	public static Vector2 k_defaultSize = new Vector2(100f, 100f);

	public TextMeshPro m_textMeshPro;

	public bool hasChanged
	{
		get
		{
			return m_hasChanged;
		}
		set
		{
			m_hasChanged = value;
		}
	}

	public Vector2 pivot
	{
		get
		{
			return m_pivot;
		}
		set
		{
			if (m_pivot != value)
			{
				m_pivot = value;
				m_anchorPosition = GetAnchorPosition(m_pivot);
				m_hasChanged = true;
				OnContainerChanged();
			}
		}
	}

	public TextContainerAnchors anchorPosition
	{
		get
		{
			return m_anchorPosition;
		}
		set
		{
			if (m_anchorPosition != value)
			{
				m_anchorPosition = value;
				m_pivot = GetPivot(m_anchorPosition);
				m_hasChanged = true;
				OnContainerChanged();
			}
		}
	}

	public Rect rect
	{
		get
		{
			return m_rect;
		}
		set
		{
			if (m_rect != value)
			{
				m_rect = value;
				m_hasChanged = true;
				OnContainerChanged();
			}
		}
	}

	public Vector2 size
	{
		get
		{
			return new Vector2(m_rect.width, m_rect.height);
		}
		set
		{
			if (new Vector2(m_rect.width, m_rect.height) != value)
			{
				SetRect(value);
				m_hasChanged = true;
				m_isDefaultWidth = false;
				m_isDefaultHeight = false;
				OnContainerChanged();
			}
		}
	}

	public float width
	{
		get
		{
			return m_rect.width;
		}
		set
		{
			SetRect(new Vector2(value, m_rect.height));
			m_hasChanged = true;
			m_isDefaultWidth = false;
			OnContainerChanged();
		}
	}

	public float height
	{
		get
		{
			return m_rect.height;
		}
		set
		{
			SetRect(new Vector2(m_rect.width, value));
			m_hasChanged = true;
			m_isDefaultHeight = false;
			OnContainerChanged();
		}
	}

	public bool isDefaultWidth => m_isDefaultWidth;

	public bool isDefaultHeight => m_isDefaultHeight;

	public bool isAutoFitting
	{
		get
		{
			return m_isAutoFitting;
		}
		set
		{
			m_isAutoFitting = value;
		}
	}

	public Vector3[] corners => m_corners;

	public Vector3[] worldCorners => m_worldCorners;

	public Vector4 margins
	{
		get
		{
			return m_margins;
		}
		set
		{
			if (m_margins != value)
			{
				m_margins = value;
				m_hasChanged = true;
				OnContainerChanged();
			}
		}
	}

	public RectTransform rectTransform
	{
		get
		{
			if (m_rectTransform == null)
			{
				m_rectTransform = GetComponent<RectTransform>();
			}
			return m_rectTransform;
		}
	}

	public TextMeshPro textMeshPro
	{
		get
		{
			if (m_textMeshPro == null)
			{
				m_textMeshPro = GetComponent<TextMeshPro>();
			}
			return m_textMeshPro;
		}
	}

	public override void Awake()
	{
		Debug.LogWarning("The Text Container component is now Obsolete and can safely be removed from [" + base.gameObject.name + "].", this);
	}

	public override void OnEnable()
	{
		OnContainerChanged();
	}

	public override void OnDisable()
	{
	}

	public void OnContainerChanged()
	{
		UpdateCorners();
		if (m_rectTransform != null)
		{
			m_rectTransform.sizeDelta = size;
			m_rectTransform.hasChanged = true;
		}
		if (textMeshPro != null)
		{
			m_textMeshPro.SetVerticesDirty();
			m_textMeshPro.margin = m_margins;
		}
	}

	public override void OnRectTransformDimensionsChange()
	{
		if (rectTransform == null)
		{
			m_rectTransform = base.gameObject.AddComponent<RectTransform>();
		}
		if (m_rectTransform.sizeDelta != k_defaultSize)
		{
			size = m_rectTransform.sizeDelta;
		}
		pivot = m_rectTransform.pivot;
		m_hasChanged = true;
		OnContainerChanged();
	}

	public void SetRect(Vector2 size)
	{
		m_rect = new Rect(m_rect.x, m_rect.y, size.x, size.y);
	}

	public void UpdateCorners()
	{
		m_corners[0] = new Vector3((0f - m_pivot.x) * m_rect.width, (0f - m_pivot.y) * m_rect.height);
		m_corners[1] = new Vector3((0f - m_pivot.x) * m_rect.width, (1f - m_pivot.y) * m_rect.height);
		m_corners[2] = new Vector3((1f - m_pivot.x) * m_rect.width, (1f - m_pivot.y) * m_rect.height);
		m_corners[3] = new Vector3((1f - m_pivot.x) * m_rect.width, (0f - m_pivot.y) * m_rect.height);
		if (m_rectTransform != null)
		{
			m_rectTransform.pivot = m_pivot;
		}
	}

	public Vector2 GetPivot(TextContainerAnchors anchor)
	{
		Vector2 result = Vector2.zero;
		switch (anchor)
		{
		case TextContainerAnchors.TopLeft:
			result = new Vector2(0f, 1f);
			break;
		case TextContainerAnchors.Top:
			result = new Vector2(0.5f, 1f);
			break;
		case TextContainerAnchors.TopRight:
			result = new Vector2(1f, 1f);
			break;
		case TextContainerAnchors.Left:
			result = new Vector2(0f, 0.5f);
			break;
		case TextContainerAnchors.Middle:
			result = new Vector2(0.5f, 0.5f);
			break;
		case TextContainerAnchors.Right:
			result = new Vector2(1f, 0.5f);
			break;
		case TextContainerAnchors.BottomLeft:
			result = new Vector2(0f, 0f);
			break;
		case TextContainerAnchors.Bottom:
			result = new Vector2(0.5f, 0f);
			break;
		case TextContainerAnchors.BottomRight:
			result = new Vector2(1f, 0f);
			break;
		}
		return result;
	}

	public TextContainerAnchors GetAnchorPosition(Vector2 pivot)
	{
		if (pivot == new Vector2(0f, 1f))
		{
			return TextContainerAnchors.TopLeft;
		}
		if (pivot == new Vector2(0.5f, 1f))
		{
			return TextContainerAnchors.Top;
		}
		if (pivot == new Vector2(1f, 1f))
		{
			return TextContainerAnchors.TopRight;
		}
		if (pivot == new Vector2(0f, 0.5f))
		{
			return TextContainerAnchors.Left;
		}
		if (pivot == new Vector2(0.5f, 0.5f))
		{
			return TextContainerAnchors.Middle;
		}
		if (pivot == new Vector2(1f, 0.5f))
		{
			return TextContainerAnchors.Right;
		}
		if (pivot == new Vector2(0f, 0f))
		{
			return TextContainerAnchors.BottomLeft;
		}
		if (pivot == new Vector2(0.5f, 0f))
		{
			return TextContainerAnchors.Bottom;
		}
		if (pivot == new Vector2(1f, 0f))
		{
			return TextContainerAnchors.BottomRight;
		}
		return TextContainerAnchors.Custom;
	}
}