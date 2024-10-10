using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Telemetry/Suspension Chart", 1)]
public class VPSuspensionGraph : VehicleBehaviour
{
	public enum Mode
	{
		TotalForce,
		SpringOnly,
		DamperOnly
	}

	public int wheel;

	public bool autoRange = true;

	public float manualForceRange = 10000f;

	public float manualDepthRange = 0.25f;

	[Range(0f, 1f)]
	public float negativeRangeFactor = 0.2f;

	public Color color = Color.green;

	public Color clampedColor = Color.red;

	public Mode mode;

	public int width = 256;

	public int height = 256;

	public Rect rect = new Rect(-5f, -2.5f, 10f, 5f);

	public float gridX = 1f;

	public float gridY = 1f;

	public bool autoClearBackground;

	public int positionX = 8;

	public int positionY = 8;

	public Font font;

	public TextureCanvas m_canvas;

	public GUIStyle m_style = new GUIStyle();

	public string m_text = "";

	public float m_lastGridX = 1f;

	public float m_lastGridY = 1f;

	public TextureCanvas canvas => m_canvas;

	public override void OnEnableVehicle()
	{
		gridX = 0.05f;
		gridY = 1000f;
		SetupCanvas();
	}

	public override void FixedUpdateVehicle()
	{
		UpdateCanvas();
	}

	public void GetSuspensionLimits(VehicleBase.WheelState wheelState, out float maxContactDepth, out float maxForce)
	{
		if (autoRange)
		{
			maxContactDepth = wheelState.wheelCol.suspensionDistance;
			maxForce = wheelState.wheelCol.springRate * maxContactDepth;
		}
		else
		{
			maxForce = manualForceRange;
			maxContactDepth = manualDepthRange;
		}
	}

	public void OnDrawCanvas()
	{
		VehicleBase.WheelState wheelState = base.vehicle.wheelState[wheel];
		GetSuspensionLimits(wheelState, out var maxContactDepth, out var maxForce);
		rect = new Rect((0f - maxContactDepth) * 0.1f, (0f - maxForce) * negativeRangeFactor, maxContactDepth * 1.3f, maxForce * (1.1f + negativeRangeFactor));
		float num = wheelState.hit.force;
		float value = wheelState.contactDepth * wheelState.wheelCol.runtimeSpringRate;
		value = Mathf.Clamp(value, 0f, wheelState.wheelCol.runtimeSuspensionTravel * wheelState.wheelCol.runtimeSpringRate);
		switch (mode)
		{
		case Mode.DamperOnly:
			num -= value;
			break;
		case Mode.SpringOnly:
			num = value;
			break;
		}
		Vector2 point = new Vector2(wheelState.contactDepth, num);
		if (rect.Contains(point))
		{
			m_canvas.color = color;
		}
		else
		{
			point.x = Mathf.Clamp(point.x, rect.xMin, rect.xMax);
			point.y = Mathf.Clamp(point.y, rect.yMin, rect.yMax);
			if (point.y == rect.yMax)
			{
				point.y -= m_canvas.Pixels2CanvasY(1);
			}
			if (point.x == rect.xMax)
			{
				point.x -= m_canvas.Pixels2CanvasX(1);
			}
			m_canvas.color = clampedColor;
		}
		m_canvas.Dot(point.x, point.y);
	}

	public void OnDrawCanvasBackground()
	{
		GetSuspensionLimits(base.vehicle.wheelState[wheel], out var maxContactDepth, out var maxForce);
		m_canvas.color = Color.white;
		m_canvas.VerticalLine(maxContactDepth);
		m_canvas.color = Color.white * 0.5f;
		m_canvas.HorizontalLine(maxForce);
	}

	public void OnValidate()
	{
		SetupText();
	}

	public void UpdateCanvas()
	{
		if (rect != m_canvas.rect || gridX != m_lastGridX || gridY != m_lastGridY || width != m_canvas.width || height != m_canvas.height)
		{
			SetupCanvas();
		}
		if (autoClearBackground)
		{
			m_canvas.Restore();
			m_canvas.alpha = 0.65f;
			m_canvas.alphaBlend = false;
		}
		OnDrawCanvas();
	}

	public void SetupCanvas()
	{
		if (m_canvas != null)
		{
			m_canvas.DestroyTexture();
		}
		m_canvas = new TextureCanvas(width, height, rect);
		m_canvas.alpha = 0.5f;
		m_canvas.color = Color.black;
		m_canvas.Clear();
		m_canvas.alpha = 0.65f;
		m_canvas.color = Color.green * 0.25f;
		m_canvas.Grid(gridX, gridY);
		m_lastGridX = gridX;
		m_lastGridY = gridY;
		m_canvas.color = Color.white * 0.6f;
		m_canvas.HorizontalLine(0f);
		m_canvas.VerticalLine(0f);
		OnDrawCanvasBackground();
		m_canvas.Save();
	}

	public void SetupText()
	{
		m_style.font = font;
		m_style.fontSize = 10;
		m_style.normal.textColor = Color.white;
	}

	public void OnGUI()
	{
		int num = ((positionX < 0) ? (Screen.width + positionX - width + 1) : positionX);
		int num2 = ((positionY < 0) ? (Screen.height + positionY - height + 1) : positionY);
		m_canvas.GUIDraw(num, num2);
		GUI.Label(new Rect(num + 8, num2 + 8, width, height), m_text, m_style);
		if (GUI.Button(new Rect(num - 24, num2, 24f, 22f), "C"))
		{
			SetupCanvas();
		}
	}
}
