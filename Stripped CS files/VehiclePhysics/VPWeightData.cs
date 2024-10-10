using System;
using UnityEngine;

namespace VehiclePhysics;

public class VPWeightData : VehicleBehaviour
{
	[Serializable]
	public class AxleGroup
	{
		public int[] axles = new int[0];

		public float specification;
	}

	public bool showAxleGroups = true;

	public AxleGroup[] axleGroups = new AxleGroup[0];

	public Vector2 screenPosition = new Vector2(8f, 8f);

	public Font font;

	public GUIStyle m_textStyle = new GUIStyle();

	public string m_text;

	public string m_groups;

	public override void OnEnableComponent()
	{
		UpdateTextStyle();
	}

	public void OnValidate()
	{
		UpdateTextStyle();
	}

	public override void FixedUpdateVehicle()
	{
		UpdateTelemetry();
	}

	public void OnGUI()
	{
		float num = ((!showAxleGroups || axleGroups.Length == 0) ? 120 : 248);
		float num2 = (float)(2 + base.vehicle.GetAxleCount()) * m_textStyle.lineHeight + 24f + 8f;
		float x = ((screenPosition.x < 0f) ? ((float)Screen.width + screenPosition.x - num) : screenPosition.x);
		float y = ((screenPosition.y < 0f) ? ((float)Screen.height + screenPosition.y - num2) : screenPosition.y);
		Rect position = new Rect(x, y, num, num2);
		GUI.Box(position, "Weight Data");
		position.x += 8f;
		position.y += 24f;
		GUI.Label(position, m_text, m_textStyle);
		if (showAxleGroups && axleGroups.Length != 0)
		{
			position.x += 120f;
			GUI.Label(position, m_groups, m_textStyle);
		}
	}

	public void UpdateTextStyle()
	{
		m_textStyle.font = font;
		m_textStyle.fontSize = 10;
		m_textStyle.normal.textColor = Color.white;
	}

	public void UpdateTelemetry()
	{
		m_text = "";
		int axleCount = base.vehicle.GetAxleCount();
		float num = 0f;
		for (int i = 0; i < axleCount; i++)
		{
			float weightInAxle = GetWeightInAxle(i);
			m_text += $"Axle {i}: {weightInAxle,6:0.} Kg\n";
			num += weightInAxle;
		}
		m_text += $"\nTotal: {num,7:0.} Kg";
		m_groups = "";
		if (!showAxleGroups)
		{
			return;
		}
		for (int j = 0; j < axleGroups.Length; j++)
		{
			AxleGroup axleGroup = axleGroups[j];
			float num2 = 0f;
			for (int k = 0; k < axleGroup.axles.Length; k++)
			{
				num2 += GetWeightInAxle(axleGroup.axles[k]);
			}
			m_groups += $"Group {j}:{num2,7:0.} Kg\n";
			if (axleGroup.specification > 0f)
			{
				Mathf.Abs(num2 - axleGroup.specification);
				m_groups += $"{axleGroup.specification,7:0.} {num2 - axleGroup.specification,7:+0.0;-0.0;-}\n";
			}
		}
	}

	public float GetWeightInAxle(int axle)
	{
		float num = 0f;
		int wheelsInAxle = base.vehicle.GetWheelsInAxle(axle);
		for (int i = 0; i < wheelsInAxle; i++)
		{
			int wheelIndex = base.vehicle.GetWheelIndex(axle, (VehicleBase.WheelPos)i);
			if (wheelIndex >= 0)
			{
				num += base.vehicle.wheelState[wheelIndex].downforce * (1f / base.vehicle.gravity.Magnitude);
			}
		}
		return num;
	}
}
