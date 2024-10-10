using UnityEngine;

namespace TMPro.Examples;

public class TMPro_InstructionOverlay : MonoBehaviour
{
	public enum FpsCounterAnchorPositions
	{
		TopLeft,
		BottomLeft,
		TopRight,
		BottomRight
	}

	public FpsCounterAnchorPositions AnchorPosition = FpsCounterAnchorPositions.BottomLeft;

	public const string instructions = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

	public TextMeshPro m_TextMeshPro;

	public TextContainer m_textContainer;

	public Transform m_frameCounter_transform;

	public Camera m_camera;

	public void Awake()
	{
		if (base.enabled)
		{
			m_camera = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			m_frameCounter_transform = gameObject.transform;
			m_frameCounter_transform.parent = m_camera.transform;
			m_frameCounter_transform.localRotation = Quaternion.identity;
			m_TextMeshPro = gameObject.AddComponent<TextMeshPro>();
			m_TextMeshPro.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/LiberationSans SDF");
			m_TextMeshPro.fontSharedMaterial = Resources.Load<Material>("Fonts & Materials/LiberationSans SDF - Overlay");
			m_TextMeshPro.fontSize = 30f;
			m_TextMeshPro.isOverlay = true;
			m_textContainer = gameObject.GetComponent<TextContainer>();
			Set_FrameCounter_Position(AnchorPosition);
			m_TextMeshPro.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}
	}

	public void Set_FrameCounter_Position(FpsCounterAnchorPositions anchor_position)
	{
		switch (anchor_position)
		{
		case FpsCounterAnchorPositions.TopLeft:
			m_textContainer.anchorPosition = TextContainerAnchors.TopLeft;
			m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
			break;
		case FpsCounterAnchorPositions.BottomLeft:
			m_textContainer.anchorPosition = TextContainerAnchors.BottomLeft;
			m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
			break;
		case FpsCounterAnchorPositions.TopRight:
			m_textContainer.anchorPosition = TextContainerAnchors.TopRight;
			m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
			break;
		case FpsCounterAnchorPositions.BottomRight:
			m_textContainer.anchorPosition = TextContainerAnchors.BottomRight;
			m_frameCounter_transform.position = m_camera.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
			break;
		}
	}
}
