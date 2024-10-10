using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns36;

public class NavBall : MonoBehaviour
{
	public Transform navBall;

	public Transform progradeVector;

	public Transform retrogradeVector;

	public Transform normalVector;

	public Transform antiNormalVector;

	public Transform radialInVector;

	public Transform radialOutVector;

	public Transform progradeWaypoint;

	public Transform retrogradeWaypoint;

	public TextMeshProUGUI headingText;

	public Image sideGaugeGee;

	public Image sideGaugeThrottle;

	public Vector3 rotationOffset = new Vector3(90f, 0f, 0f);

	public Vector3 displayVelocity;

	public float displaySpeed;

	public Vector3 displayVelDir;

	public bool initialHeadingSet;

	[SerializeField]
	public float vectorUnitScale = 1f;

	[SerializeField]
	public float vectorUnitCutoff = 0.022f;

	[SerializeField]
	public float vectorVelocityThreshold = 0.1f;

	public Vector3 wCoM;

	public Vector3 obtVel;

	public Vector3 cbPos;

	public Vector3 normal;

	public Vector3 radial;

	public Transform target { get; set; }

	public Quaternion attitudeGymbal { get; set; }

	public Quaternion relativeGymbal { get; set; }

	public Quaternion offsetGymbal { get; set; }

	[SerializeField]
	public float VectorUnitScale => vectorUnitScale;

	[SerializeField]
	public float VectorUnitCutoff => vectorUnitCutoff;

	[SerializeField]
	public float VectorVelocityThreshold => vectorVelocityThreshold;

	public void Start()
	{
		Texture2D texture = GameDatabase.Instance.GetTexture("Squad/Props/NavBall/GaugeGee", asNormalMap: false);
		if (texture != null)
		{
			sideGaugeGee.sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
		}
		Texture2D texture2 = GameDatabase.Instance.GetTexture("Squad/Props/NavBall/GaugeThrottle", asNormalMap: false);
		if (texture2 != null)
		{
			sideGaugeThrottle.sprite = Sprite.Create(texture2, new Rect(0f, 0f, texture2.width, texture2.height), new Vector2(0.5f, 0.5f));
		}
	}

	public void Update()
	{
		if (!FlightGlobals.ready)
		{
			return;
		}
		CelestialBody currentMainBody = FlightGlobals.currentMainBody;
		target = FlightGlobals.ActiveVessel.ReferenceTransform;
		offsetGymbal = Quaternion.Euler(FlightGlobals.ActiveVessel.isEVA ? new Vector3(0f, 0f, 0f) : rotationOffset);
		if (FlightGlobals.ActiveVessel.isEVA && !MapView.MapIsEnabled && FlightCamera.fetch != null)
		{
			attitudeGymbal = Quaternion.Inverse(FlightCamera.fetch.getReferenceFrame() * Quaternion.AngleAxis(FlightCamera.fetch.camHdg * 57.29578f, Vector3.up) * Quaternion.AngleAxis(FlightCamera.fetch.camPitch * 57.29578f, Vector3.right));
		}
		else
		{
			attitudeGymbal = offsetGymbal * Quaternion.Inverse(target.rotation);
		}
		relativeGymbal = attitudeGymbal * Quaternion.LookRotation(Vector3.ProjectOnPlane(currentMainBody.position + (Vector3d)currentMainBody.transform.up * currentMainBody.Radius - target.position, (target.position - currentMainBody.position).normalized).normalized, (target.position - currentMainBody.position).normalized);
		navBall.rotation = relativeGymbal;
		switch (FlightGlobals.speedDisplayMode)
		{
		case FlightGlobals.SpeedDisplayModes.Orbit:
			displayVelocity = FlightGlobals.ship_obtVelocity;
			break;
		case FlightGlobals.SpeedDisplayModes.Surface:
			displayVelocity = FlightGlobals.ship_srfVelocity;
			break;
		case FlightGlobals.SpeedDisplayModes.Target:
			displayVelocity = FlightGlobals.ship_tgtVelocity;
			break;
		}
		if (FlightGlobals.ActiveVessel.isEVA && (FlightGlobals.ActiveVessel.LandedOrSplashed || (FlightGlobals.ActiveVessel.heightFromTerrain >= 0f && FlightGlobals.ActiveVessel.heightFromTerrain <= 1f)))
		{
			progradeVector.gameObject.SetActive(value: false);
			retrogradeVector.gameObject.SetActive(value: false);
			normalVector.gameObject.SetActive(value: false);
			antiNormalVector.gameObject.SetActive(value: false);
			radialInVector.gameObject.SetActive(value: false);
			radialOutVector.gameObject.SetActive(value: false);
			progradeWaypoint.gameObject.SetActive(value: false);
			retrogradeWaypoint.gameObject.SetActive(value: false);
		}
		else
		{
			DrawOrbitalCues(FlightGlobals.speedDisplayMode == FlightGlobals.SpeedDisplayModes.Orbit);
			displaySpeed = displayVelocity.magnitude;
			if (displaySpeed == 0f)
			{
				displaySpeed = 1E-06f;
			}
			displayVelDir = displayVelocity / displaySpeed;
			progradeVector.gameObject.SetActive(displaySpeed > vectorVelocityThreshold && progradeVector.transform.localPosition.z >= vectorUnitCutoff);
			progradeVector.localPosition = attitudeGymbal * (displayVelDir * vectorUnitScale);
			retrogradeVector.gameObject.SetActive(displaySpeed > vectorVelocityThreshold && retrogradeVector.transform.localPosition.z > vectorUnitCutoff);
			retrogradeVector.localPosition = attitudeGymbal * (-displayVelDir * vectorUnitScale);
			progradeWaypoint.gameObject.SetActive(FlightGlobals.fetch.vesselTargetTransform != null && progradeWaypoint.transform.localPosition.z >= vectorUnitCutoff);
			if (FlightGlobals.fetch.vesselTargetDirection != Vector3.zero)
			{
				progradeWaypoint.localPosition = attitudeGymbal * FlightGlobals.fetch.vesselTargetDirection * vectorUnitScale;
			}
			retrogradeWaypoint.gameObject.SetActive(FlightGlobals.fetch.vesselTargetTransform != null && retrogradeWaypoint.transform.localPosition.z > vectorUnitCutoff);
			if (FlightGlobals.fetch.vesselTargetDirection != Vector3.zero)
			{
				retrogradeWaypoint.localPosition = attitudeGymbal * -FlightGlobals.fetch.vesselTargetDirection * vectorUnitScale;
			}
			SetVectorAlphaTint(progradeVector);
			SetVectorAlphaTint(retrogradeVector);
			SetVectorAlphaTint(progradeWaypoint);
			SetVectorAlphaTint(retrogradeWaypoint);
		}
		if (FlightGlobals.ActiveVessel.situation == Vessel.Situations.PRELAUNCH)
		{
			if (FlightGlobals.ActiveVessel.ctrlState.pitch != 0f || FlightGlobals.ActiveVessel.ctrlState.yaw != 0f || FlightGlobals.ActiveVessel.ctrlState.roll != 0f || !initialHeadingSet)
			{
				headingText.text = KSPUtil.LocalizeNumber(Quaternion.Inverse(relativeGymbal).eulerAngles.y, "000") + "°";
				initialHeadingSet = true;
			}
		}
		else
		{
			headingText.text = KSPUtil.LocalizeNumber(Quaternion.Inverse(relativeGymbal).eulerAngles.y, "000") + "°";
			initialHeadingSet = true;
		}
	}

	public void SetVectorAlphaTint(Transform vector)
	{
		float num = Mathf.Clamp01(Vector3.Dot(vector.localPosition.normalized, Vector3.forward));
		float num2 = Vector3.Dot(vector.localPosition.normalized, Vector3.up);
		float num3 = num;
		if (num2 >= 0.65f)
		{
			num3 *= Mathf.Clamp01(Mathf.InverseLerp(0.9f, 0.65f, num2));
		}
		else if (num2 <= -0.75f)
		{
			num3 *= Mathf.Clamp01(Mathf.InverseLerp(-0.95f, -0.75f, num2));
		}
		vector.GetComponent<MeshRenderer>().materials[0].SetFloat("_Opacity", num3);
	}

	public void DrawOrbitalCues(bool drawCondition)
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (activeVessel.orbit != null && activeVessel.orbit.referenceBody != null && drawCondition)
		{
			wCoM = activeVessel.CurrentCoM;
			obtVel = activeVessel.orbit.GetVel();
			cbPos = activeVessel.mainBody.position;
			radial = Vector3.ProjectOnPlane((wCoM - cbPos).normalized, obtVel).normalized;
			normal = Vector3.Cross(radial, obtVel.normalized);
			radial = attitudeGymbal * radial * vectorUnitScale;
			normal = attitudeGymbal * normal * vectorUnitScale;
			antiNormalVector.gameObject.SetActive(normal.z > vectorUnitCutoff);
			normalVector.gameObject.SetActive(normal.z < 0f - vectorUnitCutoff);
			antiNormalVector.localPosition = normal;
			normalVector.localPosition = -normal;
			SetVectorAlphaTint(antiNormalVector);
			SetVectorAlphaTint(normalVector);
			radialInVector.gameObject.SetActive(radial.z < 0f - vectorUnitCutoff);
			radialOutVector.gameObject.SetActive(radial.z > vectorUnitCutoff);
			radialInVector.localPosition = -radial;
			radialOutVector.localPosition = radial;
			SetVectorAlphaTint(radialInVector);
			SetVectorAlphaTint(radialOutVector);
		}
		else
		{
			if (radialInVector.gameObject.activeSelf)
			{
				radialInVector.gameObject.SetActive(value: false);
			}
			if (radialOutVector.gameObject.activeSelf)
			{
				radialOutVector.gameObject.SetActive(value: false);
			}
			if (normalVector.gameObject.activeSelf)
			{
				normalVector.gameObject.SetActive(value: false);
			}
			if (antiNormalVector.gameObject.activeSelf)
			{
				antiNormalVector.gameObject.SetActive(value: false);
			}
		}
	}

	public void SetWaypoint(Transform target)
	{
	}

	public void ClearWaypoint()
	{
	}
}
