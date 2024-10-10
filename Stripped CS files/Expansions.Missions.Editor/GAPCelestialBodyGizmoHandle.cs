using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyGizmoHandle : MonoBehaviour
{
	public MeshRenderer handleRenderer;

	[SerializeField]
	public Color normalColor;

	[SerializeField]
	public Color hoverColor;

	[SerializeField]
	public Color downColor;

	[SerializeField]
	public Color disabledColor;

	public Vector3 moveDirection;

	public Color currentColor;

	public void Start()
	{
		currentColor = normalColor;
	}

	public void OnHoverStart()
	{
		SetColor(hoverColor);
	}

	public void OnHoverEnd()
	{
		SetColor(normalColor);
	}

	public void SetAlpha(float newAlpha)
	{
		currentColor.a = newAlpha;
		handleRenderer.material.SetColor("_Color", currentColor);
	}

	public void SetColor(Color newColor)
	{
		currentColor = new Color(newColor.r, newColor.g, newColor.b, newColor.a);
		handleRenderer.material.SetColor("_Color", currentColor);
	}
}
