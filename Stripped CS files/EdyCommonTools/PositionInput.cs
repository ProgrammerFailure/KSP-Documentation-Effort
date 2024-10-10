using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(PositionController))]
public class PositionInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public enum OutputPlane
	{
		const_0,
		const_1,
		const_2
	}

	public InputSource source;

	public OutputPlane outputPlane;

	public bool swapCoordinates;

	public string inputAxisX = "Mouse X";

	public string inputAxisY = "Mouse Y";

	public int mouseButtonForDrag = -1;

	public bool mouseButtonExclusive;

	public Vector2 inputSensitivity = Vector2.one;

	public Vector2 defaultPosition = Vector2.zero;

	public PositionController m_pos;

	public void OnEnable()
	{
		m_pos = GetComponent<PositionController>();
	}

	public Vector3 MapToPlane(Vector2 v, Vector3 defaultPos)
	{
		if (swapCoordinates)
		{
			float x = v.x;
			v.x = v.y;
			v.y = x;
		}
		Vector3 zero = Vector3.zero;
		switch (outputPlane)
		{
		case OutputPlane.const_0:
			zero.x = v.x;
			zero.y = defaultPos.y;
			zero.z = v.y;
			break;
		case OutputPlane.const_1:
			zero.x = v.x;
			zero.y = v.y;
			zero.z = defaultPos.z;
			break;
		case OutputPlane.const_2:
			zero.x = defaultPos.x;
			zero.y = v.y;
			zero.z = v.x;
			break;
		}
		return zero;
	}

	public void Move(Vector2 delta)
	{
		if (base.enabled)
		{
			m_pos.position += MapToPlane(Vector3.Scale(delta, inputSensitivity), Vector2.zero);
		}
	}

	public void ResetDefaults()
	{
		if (base.enabled)
		{
			m_pos.ResetPosition(MapToPlane(defaultPosition, m_pos.position));
		}
	}

	public void ProcessStandardInput()
	{
		bool flag = true;
		if (mouseButtonForDrag != -1)
		{
			int num = 0;
			if (Input.GetMouseButton(mouseButtonForDrag))
			{
				if (mouseButtonExclusive)
				{
					if (Input.GetMouseButton(0))
					{
						num++;
					}
					if (Input.GetMouseButton(1))
					{
						num++;
					}
					if (Input.GetMouseButton(2))
					{
						num++;
					}
				}
				else
				{
					num = 1;
				}
			}
			flag = num == 1;
		}
		if (flag)
		{
			float x = (string.IsNullOrEmpty(inputAxisX) ? 0f : Input.GetAxis(inputAxisX));
			float y = (string.IsNullOrEmpty(inputAxisY) ? 0f : Input.GetAxis(inputAxisY));
			Move(new Vector2(x, y));
		}
	}

	public void Update()
	{
		if (source == InputSource.StandardInput)
		{
			ProcessStandardInput();
		}
	}

	public void OnMove(Vector2 delta)
	{
		if (source == InputSource.Messages)
		{
			Move(delta);
		}
	}
}
