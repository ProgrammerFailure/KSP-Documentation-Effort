using System;
using UnityEngine;

public class FlightCtrlState : IConfigNode
{
	public float mainThrottle;

	public float roll;

	public float yaw;

	public float pitch;

	public float rollTrim;

	public float yawTrim;

	public float pitchTrim;

	public float wheelSteer;

	public float wheelSteerTrim;

	public float wheelThrottle;

	public float wheelThrottleTrim;

	public float float_0;

	public float float_1;

	public float float_2;

	public bool killRot;

	public bool gearUp;

	public bool gearDown;

	public bool headlight;

	public float[] custom_axes;

	public bool isIdle
	{
		get
		{
			if ((double)roll == 0.0 && (double)pitch == 0.0 && (double)yaw == 0.0 && (double)float_0 == 0.0 && (double)float_1 == 0.0 && (double)float_2 == 0.0)
			{
				return (double)wheelSteer == 0.0;
			}
			return false;
		}
	}

	public bool isNeutral
	{
		get
		{
			if ((double)mainThrottle == 0.0 && (double)roll == 0.0 && (double)pitch == 0.0 && (double)yaw == 0.0 && (double)rollTrim == 0.0 && (double)pitchTrim == 0.0 && (double)yawTrim == 0.0 && (double)float_0 == 0.0 && (double)float_1 == 0.0 && (double)float_2 == 0.0 && (double)wheelSteer == 0.0 && (double)wheelSteerTrim == 0.0 && (double)wheelThrottle == 0.0)
			{
				return (double)wheelThrottleTrim == 0.0;
			}
			return false;
		}
	}

	public FlightCtrlState()
	{
		custom_axes = new float[GameSettings.AXIS_CUSTOM.Length];
	}

	public FlightCtrlState(float[] custom_axes)
	{
		this.custom_axes = custom_axes;
	}

	public void CopyFrom(FlightCtrlState st)
	{
		mainThrottle = st.mainThrottle;
		roll = st.roll;
		yaw = st.yaw;
		pitch = st.pitch;
		rollTrim = st.rollTrim;
		yawTrim = st.yawTrim;
		pitchTrim = st.pitchTrim;
		wheelSteer = st.wheelSteer;
		wheelSteerTrim = st.wheelSteerTrim;
		wheelThrottle = st.wheelThrottle;
		wheelThrottleTrim = st.wheelThrottleTrim;
		float_0 = st.float_0;
		float_1 = st.float_1;
		float_2 = st.float_2;
		killRot = st.killRot;
		gearUp = st.gearUp;
		gearDown = st.gearDown;
		headlight = st.headlight;
		if (custom_axes != null && st.custom_axes != null)
		{
			int num = Math.Min(custom_axes.Length, st.custom_axes.Length);
			for (int i = 0; i < num; i++)
			{
				custom_axes[i] = st.custom_axes[i];
			}
		}
	}

	public void MaskedCopyFrom(FlightCtrlState st, KSPAxisGroup mask)
	{
		if ((mask & KSPAxisGroup.MainThrottle) != 0)
		{
			mainThrottle = st.mainThrottle;
		}
		if ((mask & KSPAxisGroup.Roll) != 0)
		{
			roll = st.roll;
			rollTrim = st.rollTrim;
		}
		if ((mask & KSPAxisGroup.Yaw) != 0)
		{
			yaw = st.yaw;
			yawTrim = st.yawTrim;
		}
		if ((mask & KSPAxisGroup.Pitch) != 0)
		{
			pitch = st.pitch;
			pitchTrim = st.pitchTrim;
		}
		if ((mask & KSPAxisGroup.WheelSteer) != 0)
		{
			wheelSteer = st.wheelSteer;
			wheelSteerTrim = st.wheelSteerTrim;
		}
		if ((mask & KSPAxisGroup.WheelThrottle) != 0)
		{
			wheelThrottle = st.wheelThrottle;
			wheelThrottleTrim = st.wheelThrottleTrim;
		}
		if ((mask & KSPAxisGroup.TranslateX) != 0)
		{
			float_0 = st.float_0;
		}
		if ((mask & KSPAxisGroup.TranslateZ) != 0)
		{
			float_1 = st.float_1;
		}
		if ((mask & KSPAxisGroup.TranslateZ) != 0)
		{
			float_2 = st.float_2;
		}
	}

	public void Neutralize()
	{
		mainThrottle = 0f;
		roll = 0f;
		yaw = 0f;
		pitch = 0f;
		wheelSteer = 0f;
		wheelThrottle = 0f;
		float_0 = 0f;
		float_1 = 0f;
		float_2 = 0f;
		killRot = false;
		gearUp = false;
		gearDown = false;
		headlight = false;
	}

	public void NeutralizeStick()
	{
		roll = 0f;
		yaw = 0f;
		pitch = 0f;
		wheelSteer = 0f;
	}

	public void ResetTrim()
	{
		rollTrim = 0f;
		yawTrim = 0f;
		pitchTrim = 0f;
		wheelSteerTrim = 0f;
		wheelThrottleTrim = 0f;
	}

	public void NeutralizeAll()
	{
		Neutralize();
		ResetTrim();
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("pitch"))
		{
			pitch = float.Parse(node.GetValue("pitch"));
		}
		if (node.HasValue("yaw"))
		{
			yaw = float.Parse(node.GetValue("yaw"));
		}
		if (node.HasValue("roll"))
		{
			roll = float.Parse(node.GetValue("roll"));
		}
		if (node.HasValue("trimPitch"))
		{
			pitchTrim = float.Parse(node.GetValue("trimPitch"));
		}
		if (node.HasValue("trimYaw"))
		{
			yawTrim = float.Parse(node.GetValue("trimYaw"));
		}
		if (node.HasValue("trimRoll"))
		{
			rollTrim = float.Parse(node.GetValue("trimRoll"));
		}
		if (node.HasValue("mainThrottle"))
		{
			mainThrottle = float.Parse(node.GetValue("mainThrottle"));
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("pitch", pitch);
		node.AddValue("yaw", yaw);
		node.AddValue("roll", roll);
		node.AddValue("trimPitch", pitchTrim);
		node.AddValue("trimYaw", yawTrim);
		node.AddValue("trimRoll", rollTrim);
		node.AddValue("mainThrottle", mainThrottle);
	}

	public Vector3 GetPYR()
	{
		return new Vector3(pitch, roll, yaw);
	}

	public Vector3 GetXYZ()
	{
		return new Vector3(float_0, float_1, float_2);
	}

	public Vector2 GetWheels()
	{
		return new Vector2(wheelSteer, wheelThrottle);
	}
}
