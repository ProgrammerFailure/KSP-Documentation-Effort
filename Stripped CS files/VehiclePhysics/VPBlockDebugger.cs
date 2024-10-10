using System;
using System.Collections.Generic;
using EdyCommonTools;
using UnityEngine;

namespace VehiclePhysics;

public class VPBlockDebugger : VehicleBehaviour
{
	public enum AngularVelocityUnits
	{
		RadsPerSec,
		RevsPerMin
	}

	public enum ChangeDetection
	{
		Exact,
		Tolerance
	}

	public AngularVelocityUnits angularVelocityUnits = AngularVelocityUnits.RevsPerMin;

	public ChangeDetection changeDetection;

	public float changeTolerance = 0.0001f;

	[Space(5f)]
	public Font font;

	public Texture2D boxTexture;

	public Vector2 screenPosition = new Vector2(8f, 8f);

	public List<VehicleBase.SolverState> m_states;

	public GUIStyle m_style = new GUIStyle();

	public GUIStyle m_boxStyle;

	public string m_text;

	public int m_charWidth;

	public int m_lineLength;

	public int m_lines;

	public override void OnEnableComponent()
	{
		UpdateTextProperties();
	}

	public void OnValidate()
	{
		UpdateTextProperties();
	}

	public override void OnEnableVehicle()
	{
		m_states = new List<VehicleBase.SolverState>();
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onBeforeIntegrationStep = (Action)Delegate.Combine(vehicleBase.onBeforeIntegrationStep, new Action(RecordSolverState));
	}

	public override void OnDisableVehicle()
	{
		VehicleBase vehicleBase = base.vehicle;
		vehicleBase.onBeforeIntegrationStep = (Action)Delegate.Remove(vehicleBase.onBeforeIntegrationStep, new Action(RecordSolverState));
		m_states.Clear();
		UpdateTelemetryData();
	}

	public void OnGUI()
	{
		int num = m_lines * (int)m_style.lineHeight;
		int num2 = m_lineLength * m_charWidth + 16;
		if (m_boxStyle == null && boxTexture != null)
		{
			m_boxStyle = new GUIStyle(GUI.skin.box);
			m_boxStyle.normal.background = boxTexture;
		}
		GUI.Box(new Rect(screenPosition.x, screenPosition.y, num2, num + 40), "Block Diagnosis", (m_boxStyle != null) ? m_boxStyle : GUI.skin.box);
		GUI.Label(new Rect(screenPosition.x + 8f, screenPosition.y + 10f, Screen.width, Screen.height), m_text, m_style);
	}

	public void RecordSolverState()
	{
		m_states.Add(base.vehicle.GetSolverState());
		UpdateTelemetryData();
	}

	public void UpdateTextProperties()
	{
		m_style.font = font;
		m_style.fontSize = 10;
		m_style.normal.textColor = Color.white;
		m_style.richText = true;
	}

	public void UpdateTelemetryData()
	{
		if (m_states.Count == 0)
		{
			m_text = "No data";
			return;
		}
		if (font != null && font.GetCharacterInfo('8', out var info))
		{
			m_charWidth = info.advance;
		}
		else
		{
			m_charWidth = 8;
		}
		VehicleBase.SolverState solverState = m_states[m_states.Count - 1];
		VehicleBase.SolverState solverState2 = ((m_states.Count > 1) ? m_states[m_states.Count - 2] : m_states[m_states.Count - 1]);
		m_lines = 2;
		m_text = "\n\nBlock";
		for (int i = 0; i < solverState.blockStates.Length; i++)
		{
			m_text += GetIntString(i);
		}
		m_lineLength = m_text.Length;
		m_lines++;
		m_text += "\n     ";
		Type[] solverBlockTypes = base.vehicle.GetSolverBlockTypes();
		for (int j = 0; j < solverState.blockStates.Length; j++)
		{
			m_text += GetTypeString(solverBlockTypes[j]);
		}
		m_lines += 2;
		m_text += "\n\nw    ";
		for (int k = 0; k < solverState.blockStates.Length; k++)
		{
			m_text += GetFloatString(GetAngularVelocity(solverState.blockStates[k]), GetAngularVelocity(solverState2.blockStates[k]));
		}
		m_lines++;
		m_text += "\nTd   ";
		for (int l = 0; l < solverState.blockStates.Length; l++)
		{
			m_text += GetFloatString(solverState.blockStates[l].Td, solverState2.blockStates[l].Td);
		}
		m_lines++;
		m_text += "\nTr   ";
		for (int m = 0; m < solverState.blockStates.Length; m++)
		{
			m_text += GetFloatString(solverState.blockStates[m].Tr, solverState2.blockStates[m].Tr);
		}
		m_lines += 2;
		m_text += "\n\nI    ";
		for (int n = 0; n < solverState.blockStates.Length; n++)
		{
			m_text += GetFloatString(solverState.blockStates[n].float_1, solverState2.blockStates[n].float_1);
		}
	}

	public float GetAngularVelocity(VehicleBase.BlockState blockState)
	{
		float num = blockState.float_0 / blockState.float_1;
		if (angularVelocityUnits == AngularVelocityUnits.RevsPerMin)
		{
			num *= Block.WToRpm;
		}
		return num;
	}

	public string GetIntString(int value)
	{
		return $"{value,9}";
	}

	public string GetFloatString(float value, float prevValue)
	{
		float num = MathUtility.FastAbs(value);
		string text = ((num == 0f) ? string.Format("{0,9}", "0") : ((num < 1f) ? $"{value,9:0.00000}" : ((num < 1000f) ? $"{value,9:0.000}" : ((!(num < 100000f)) ? $" {value,8:0.}" : $"{value,9:0.0}"))));
		if (Mathf.Sign(value) != Mathf.Sign(prevValue))
		{
			text = "<color=fuchsia>" + text + "</color>";
		}
		else if (changeDetection == ChangeDetection.Exact)
		{
			if (value > prevValue)
			{
				text = "<color=lime>" + text + "</color>";
			}
			else if (value < prevValue)
			{
				text = "<color=red>" + text + "</color>";
			}
		}
		else if (value - prevValue > changeTolerance)
		{
			text = "<color=lime>" + text + "</color>";
		}
		else if (prevValue - value > changeTolerance)
		{
			text = "<color=red>" + text + "</color>";
		}
		return text;
	}

	public string GetFloatString(float value)
	{
		return GetFloatString(value, value);
	}

	public string GetTypeString(Type type)
	{
		string text = type.Name;
		if (text.Length > 8)
		{
			text = text.Substring(0, 8);
		}
		return $" {text,8}";
	}
}
