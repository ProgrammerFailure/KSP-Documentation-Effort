using UnityEngine;

namespace EdyCommonTools;

[RequireComponent(typeof(RotationController))]
public class RotationInput : MonoBehaviour
{
	public enum InputSource
	{
		StandardInput,
		Messages
	}

	public InputSource source;

	public string horizontalAxis = "Mouse X";

	public string verticalAxis = "Mouse Y";

	public int mouseButtonForDrag = -1;

	public bool mouseButtonExclusive;

	public float horizontalSensitivity = 4f;

	public float verticalSensitivity = 4f;

	public float horizontalDefault;

	public float verticalDefault;

	public RotationController m_rot;

	public void OnEnable()
	{
		m_rot = GetComponent<RotationController>();
	}

	public void Move(Vector2 delta)
	{
		if (base.enabled)
		{
			m_rot.horizontal.angle += delta.x * horizontalSensitivity;
			m_rot.vertical.angle += delta.y * verticalSensitivity;
		}
	}

	public void ResetDefaults()
	{
		if (base.enabled)
		{
			m_rot.horizontal.ResetAngle(MathUtility.ClampAngle(m_rot.horizontal.angle));
			m_rot.vertical.ResetAngle(MathUtility.ClampAngle(m_rot.vertical.angle));
			m_rot.horizontal.angle = MathUtility.ClampAngle(horizontalDefault);
			m_rot.vertical.angle = MathUtility.ClampAngle(verticalDefault);
		}
	}

	public void ResetDefaultsImmediate()
	{
		if (base.enabled)
		{
			m_rot.horizontal.ResetAngle(MathUtility.ClampAngle(horizontalDefault));
			m_rot.vertical.ResetAngle(MathUtility.ClampAngle(verticalDefault));
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
			Move(new Vector2(Input.GetAxis(horizontalAxis), Input.GetAxis(verticalAxis)));
		}
	}

	public void Update()
	{
		if (source == InputSource.StandardInput)
		{
			ProcessStandardInput();
		}
	}

	public void OnDrag(Vector2 delta)
	{
		if (source == InputSource.Messages)
		{
			Move(delta);
		}
	}
}
