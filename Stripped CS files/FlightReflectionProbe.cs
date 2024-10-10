using UnityEngine;
using UnityEngine.Rendering;

public class FlightReflectionProbe : MonoBehaviour
{
	public ReflectionProbe probeComponent;

	public float angleDivisor = 2f;

	public AtmosphereFromGround afg;

	public Renderer afgRenderer;

	public int renderID = -1;

	public bool forceRender = true;

	public bool turnOff;

	public bool rendering;

	public static FlightReflectionProbe Spawn()
	{
		return new GameObject("Reflection Probe").AddComponent<FlightReflectionProbe>();
	}

	public void Start()
	{
		probeComponent = base.gameObject.AddComponent<ReflectionProbe>();
		probeComponent.mode = ReflectionProbeMode.Realtime;
		probeComponent.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
		OnSettingsUpdate();
		probeComponent.shadowDistance = 0f;
		probeComponent.hdr = false;
		probeComponent.cullingMask = (1 << LayerMask.NameToLayer("Water")) | (1 << LayerMask.NameToLayer("Local Scenery")) | (1 << LayerMask.NameToLayer("Scaled Scenery"));
		probeComponent.clearFlags = ReflectionProbeClearFlags.SolidColor;
		probeComponent.nearClipPlane = 0.1f;
		probeComponent.farClipPlane = 110000f;
		probeComponent.size = new Vector3(500f, 500f, 500f);
		probeComponent.enabled = false;
		GameEvents.OnGameSettingsApplied.Add(OnSettingsUpdate);
	}

	public void Update()
	{
		if (!(probeComponent != null))
		{
			return;
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (!forceRender && !probeComponent.IsFinishedRendering(renderID))
			{
				return;
			}
			if (turnOff)
			{
				rendering = false;
				turnOff = false;
				probeComponent.enabled = false;
			}
			else if (rendering)
			{
				if (!probeComponent.enabled)
				{
					probeComponent.enabled = true;
				}
				probeComponent.backgroundColor = GetSkyColor();
				renderID = probeComponent.RenderProbe();
				forceRender = false;
			}
		}
		else if (probeComponent.enabled && probeComponent.IsFinishedRendering(renderID))
		{
			probeComponent.enabled = false;
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(OnSettingsUpdate);
	}

	public void ForceRender()
	{
		forceRender = true;
	}

	public void Enable(bool enable)
	{
		if (enable)
		{
			rendering = true;
		}
		else
		{
			turnOff = true;
		}
	}

	public virtual Color GetSkyColor()
	{
		Color result = Color.black;
		if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.mainBody != null && FlightGlobals.ActiveVessel.mainBody.afg != null)
		{
			if (afg != FlightGlobals.ActiveVessel.mainBody.afg)
			{
				afg = FlightGlobals.ActiveVessel.mainBody.afg;
				afgRenderer = afg.GetComponent<Renderer>();
			}
			Vector3 vector = afg.mainCamera.position - afg.transform.position;
			if (afg != null && afgRenderer != null && vector.magnitude <= afg.outerRadius)
			{
				float maxRadiansDelta = Mathf.Acos(vector.magnitude / afg.outerRadius) / angleDivisor;
				Vector3 vector2 = Vector3.RotateTowards(vector.normalized, -vector.normalized, maxRadiansDelta, 0f);
				Vector3 position = afg.transform.position + vector2 * afg.outerRadius;
				Vector3 vector3 = afg.transform.InverseTransformPoint(position);
				Matrix4x4 localToWorldMatrix = afgRenderer.localToWorldMatrix;
				Vector3 vector4 = VectorMatrixMultiplication(vector3, localToWorldMatrix) - vector;
				float magnitude = vector4.magnitude;
				vector4 /= magnitude;
				Vector3 vector5 = vector;
				float cos = Vector3.Dot(vector4, vector5) / vector5.magnitude;
				float num = Mathf.Exp(afg.scaleOverScaleDepth * Mathf.Abs(afg.innerRadius - afg.cameraHeight)) * expScale(cos, afg.scaleDepth);
				float num2 = magnitude / afg.samples;
				float num3 = num2 * afg.scale;
				Vector3 vector6 = vector4 * num2;
				Vector3 rhs = vector5 + vector6 * 0.5f;
				Vector3 zero = Vector3.zero;
				Vector3 vector7 = new Vector3(afg.invWaveLength.r, afg.invWaveLength.g, afg.invWaveLength.b);
				Vector3 vector9 = default(Vector3);
				for (int i = 0; i < 3; i++)
				{
					float magnitude2 = rhs.magnitude;
					float num4 = Mathf.Exp(afg.scaleOverScaleDepth * Mathf.Abs(afg.innerRadius - magnitude2));
					float cos2 = Vector3.Dot(afg.sunLightDirection, rhs) / magnitude2;
					float cos3 = Vector3.Dot(vector4, rhs) / magnitude2;
					Vector3 vector8 = (0f - (num + num4 * (expScale(cos2, afg.scaleDepth) - expScale(cos3, afg.scaleDepth)))) * (vector7 * afg.Kr4PI + new Vector3(afg.Km4PI, afg.Km4PI, afg.Km4PI));
					vector9.x = Mathf.Exp(vector8.x);
					vector9.y = Mathf.Exp(vector8.y);
					vector9.z = Mathf.Exp(vector8.z);
					zero += vector9 * (num4 * num3);
					rhs += vector6;
				}
				Vector3 vector10 = vector - vector3;
				Vector3 vector11 = vector7 * afg.KrESun;
				Vector3 vector12 = new Vector3(zero.x * vector11.x, zero.y * vector11.y, zero.z * vector11.z);
				Vector3 vector13 = zero * afg.KmESun;
				float num5 = Vector3.Dot(afg.sunLightDirection, vector10) / vector10.magnitude;
				float num6 = num5 * num5;
				float num7 = 1.5f * ((1f - afg.g2) / (2f + afg.g2)) * (1f + num6) / Mathf.Pow(1f + afg.g2 - 2f * afg.g * num5, 1.5f);
				float num8 = 0.75f * (1f + num6);
				Vector4 vector14 = (0f - afg.exposure) * (num8 * vector12) + num7 * vector13;
				vector14.x = 1f - Mathf.Pow(2f, vector14.x);
				vector14.y = 1f - Mathf.Pow(2f, vector14.y);
				vector14.z = 1f - Mathf.Pow(2f, vector14.z);
				vector14.w = vector14.z;
				Vector4 vector15 = default(Vector4);
				vector15.x = Mathf.Lerp(afg.underwaterColorStart.r, afg.underwaterColorEnd.r, afg.camHeightUnderwater);
				vector15.y = Mathf.Lerp(afg.underwaterColorStart.g, afg.underwaterColorEnd.g, afg.camHeightUnderwater);
				vector15.z = Mathf.Lerp(afg.underwaterColorStart.b, afg.underwaterColorEnd.b, afg.camHeightUnderwater);
				vector15.w = Mathf.Lerp(afg.underwaterColorStart.a, afg.underwaterColorEnd.a, afg.camHeightUnderwater);
				vector15.x = Mathf.Lerp(0f, vector15.x, afg.lightDot);
				vector15.y = Mathf.Lerp(0f, vector15.y, afg.lightDot);
				vector15.z = Mathf.Lerp(0f, vector15.z, afg.lightDot);
				vector15.w = Mathf.Lerp(1f, vector15.w, afg.lightDot);
				Vector3 vector16 = VectorMatrixMultiplication(vector - vector10, localToWorldMatrix);
				float num9 = VectorMatrixMultiplication(vector, localToWorldMatrix).magnitude - vector16.magnitude;
				float t = Mathf.Clamp01(Sign(afg.camHeightUnderwater) * (afg.underwaterOpacityAltBase * (1f + Sign(num9) * num9 * 0.03f) + afg.camHeightUnderwater * afg.underwaterOpacityAltMult));
				Vector4 vector17 = default(Vector4);
				vector17.x = Mathf.Lerp(vector14.x, vector15.x, t);
				vector17.y = Mathf.Lerp(vector14.y, vector15.y, t);
				vector17.z = Mathf.Lerp(vector14.z, vector15.z, t);
				vector17.w = Mathf.Lerp(vector14.w, vector15.w, t);
				result = new Color(vector17.x, vector17.y, vector17.z, vector17.z);
			}
		}
		return result;
	}

	public float expScale(float cos, float scaleDepth)
	{
		float num = 1f - cos;
		return (0f - scaleDepth) * Mathf.Exp(-0.00287f + num * (0.459f + num * (3.83f + num * (-6.8f + num * 5.25f))));
	}

	public Vector3 VectorMatrixMultiplication(Vector3 vec, Matrix4x4 mat)
	{
		Vector3 result = default(Vector3);
		result.x = vec.x * mat.m00 + vec.y * mat.m01 + vec.z * mat.m02;
		result.y = vec.x * mat.m10 + vec.y * mat.m11 + vec.z * mat.m12;
		result.z = vec.x * mat.m20 + vec.y * mat.m21 + vec.z * mat.m22;
		return result;
	}

	public float Sign(float number)
	{
		if (number == 0f)
		{
			return 0f;
		}
		return Mathf.Sign(number);
	}

	public virtual void OnSettingsUpdate()
	{
		switch (GameSettings.REFLECTION_PROBE_REFRESH_MODE)
		{
		default:
			Enable(enable: false);
			break;
		case 1:
			Enable(enable: true);
			probeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.IndividualFaces;
			break;
		case 2:
			Enable(enable: true);
			probeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
			break;
		case 3:
			Enable(enable: true);
			probeComponent.timeSlicingMode = ReflectionProbeTimeSlicingMode.NoTimeSlicing;
			break;
		}
		int a = 4;
		if (SystemInfo.maxCubemapSize < 256)
		{
			a = 0;
		}
		else if (SystemInfo.maxCubemapSize < 512)
		{
			a = 1;
		}
		else if (SystemInfo.maxCubemapSize < 1024)
		{
			a = 2;
		}
		else if (SystemInfo.maxCubemapSize < 2048)
		{
			a = 3;
		}
		switch (Mathf.Min(a, GameSettings.REFLECTION_PROBE_TEXTURE_RESOLUTION))
		{
		default:
			probeComponent.resolution = 128;
			break;
		case 1:
			probeComponent.resolution = 256;
			break;
		case 2:
			probeComponent.resolution = 512;
			break;
		case 3:
			probeComponent.resolution = 1024;
			break;
		case 4:
			probeComponent.resolution = 2048;
			break;
		}
	}
}
