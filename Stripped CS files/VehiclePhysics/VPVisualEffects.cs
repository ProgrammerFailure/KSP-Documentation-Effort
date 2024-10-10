using EdyCommonTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

[AddComponentMenu("Vehicle Physics/Effects/Visual Effects", 3)]
public class VPVisualEffects : VehicleBehaviour
{
	[Header("Steering wheel")]
	public Transform steeringWheel;

	public float degreesOfRotation = 420f;

	[Header("Lights")]
	[FormerlySerializedAs("brakeLightsGlow")]
	public GameObject[] brakeLightsOn = new GameObject[0];

	public GameObject[] brakeLightsOff = new GameObject[0];

	[FormerlySerializedAs("reverseLightsGlow")]
	public GameObject[] reverseLightsOn = new GameObject[0];

	public GameObject[] reverseLightsOff = new GameObject[0];

	[FormerlySerializedAs("headLightsGlow")]
	[FormerlySerializedAs("headLights")]
	public GameObject[] headLightsOn = new GameObject[0];

	public GameObject[] headLightsOff = new GameObject[0];

	public bool headLightsEnabled;

	public KeyCode headLightsToggleKey = KeyCode.L;

	[Header("Dashboard")]
	public Transform rpmGauge;

	public float rpmMax = 7000f;

	public float rpmMinAngle;

	public float rpmMaxAngle = -270f;

	[Space(5f)]
	public Transform speedGauge;

	public float speedMaxKph = 220f;

	public float speedMinAngle;

	public float speedMaxAngle = -270f;

	[Space(5f)]
	public GameObject[] dashboardOn = new GameObject[0];

	public GameObject[] dashboardOff = new GameObject[0];

	[Space(5f)]
	public GameObject stalledLightsOn;

	public GameObject stalledLightsOff;

	public GameObject handbrakeLightsOn;

	public GameObject handbrakeLightsOff;

	public InterpolatedFloat m_steerInput = new InterpolatedFloat();

	public InterpolatedFloat m_speedMs = new InterpolatedFloat();

	public InterpolatedFloat m_engineRpm = new InterpolatedFloat();

	public override void FixedUpdateVehicle()
	{
		m_steerInput.Set((float)base.vehicle.data.Get(1, 21) / 10000f);
		m_speedMs.Set((float)base.vehicle.data.Get(1, 0) / 1000f);
		m_engineRpm.Set((float)base.vehicle.data.Get(1, 1) / 1000f);
	}

	public override void UpdateVehicle()
	{
		int[] array = base.vehicle.data.Get(0);
		int[] array2 = base.vehicle.data.Get(1);
		bool flag = array[9] >= 0;
		float frameRatio = InterpolatedFloat.GetFrameRatio();
		if (steeringWheel != null)
		{
			Vector3 localEulerAngles = steeringWheel.localEulerAngles;
			localEulerAngles.z = -0.5f * degreesOfRotation * m_steerInput.GetInterpolated(frameRatio);
			steeringWheel.localEulerAngles = localEulerAngles;
		}
		float num = (float)array[2] / 10000f;
		bool flag2 = flag && num > 0.1f;
		if (brakeLightsOn.Length != 0)
		{
			SetGameObjectsActive(brakeLightsOn, flag2);
		}
		if (brakeLightsOff.Length != 0)
		{
			SetGameObjectsActive(brakeLightsOff, !flag2);
		}
		bool flag3 = flag && array[5] < 0;
		if (reverseLightsOn.Length != 0)
		{
			SetGameObjectsActive(reverseLightsOn, flag3);
		}
		if (reverseLightsOff.Length != 0)
		{
			SetGameObjectsActive(reverseLightsOff, !flag3);
		}
		if (Input.GetKeyDown(headLightsToggleKey))
		{
			headLightsEnabled = !headLightsEnabled;
		}
		if (headLightsOn.Length != 0)
		{
			SetGameObjectsActive(headLightsOn, headLightsEnabled);
		}
		if (headLightsOff.Length != 0)
		{
			SetGameObjectsActive(headLightsOff, !headLightsEnabled);
		}
		bool flag4 = flag && array2[2] != 0;
		if (speedGauge != null)
		{
			float num2 = m_speedMs.GetInterpolated(frameRatio) * 3.6f;
			Vector3 localEulerAngles2 = speedGauge.localEulerAngles;
			localEulerAngles2.z = Mathf.Lerp(speedMinAngle, speedMaxAngle, Mathf.Clamp01(num2 / speedMaxKph));
			speedGauge.localEulerAngles = localEulerAngles2;
		}
		if (rpmGauge != null)
		{
			float interpolated = m_engineRpm.GetInterpolated(frameRatio);
			Vector3 localEulerAngles3 = rpmGauge.localEulerAngles;
			localEulerAngles3.z = Mathf.Lerp(rpmMinAngle, rpmMaxAngle, Mathf.Clamp01(interpolated / rpmMax));
			rpmGauge.localEulerAngles = localEulerAngles3;
		}
		if (dashboardOn.Length != 0)
		{
			SetGameObjectsActive(dashboardOn, flag);
		}
		if (dashboardOff.Length != 0)
		{
			SetGameObjectsActive(dashboardOff, !flag);
		}
		if (stalledLightsOn != null)
		{
			stalledLightsOn.SetActive(flag4);
		}
		if (stalledLightsOff != null)
		{
			stalledLightsOff.SetActive(!flag4);
		}
		float num3 = (float)array[3] / 10000f;
		bool flag5 = flag && num3 > 0.1f;
		if (handbrakeLightsOn != null)
		{
			handbrakeLightsOn.SetActive(flag5);
		}
		if (handbrakeLightsOff != null)
		{
			handbrakeLightsOff.SetActive(!flag5);
		}
	}

	public void SetGameObjectsActive(GameObject[] gameObjects, bool active)
	{
		foreach (GameObject gameObject in gameObjects)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(active);
			}
		}
	}
}
