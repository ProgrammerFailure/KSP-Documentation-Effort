using System;
using UnityEngine;

public class AngleRenderEject : MonoBehaviour
{
	public bool _isBecomingVisible;

	public bool _isBecomingVisible_LinesDone;

	public bool _isBecomingVisible_ArcDone;

	public bool _isBecomingVisible_VesselVectDone;

	public bool _isHiding;

	public DateTime StartDrawing;

	public Vector3d vectPosWorldPivot;

	public Vector3d vectPosWorldOrigin;

	public Vector3d vectPosWorldOrbitLabel;

	public Vector3d vectPosWorldEnd;

	public Vector3d vectPosPivotWorking;

	public Vector3d vectPosEndWorking;

	public GameObject objLineStart;

	public GameObject objLineStartArrow1;

	public GameObject objLineStartArrow2;

	public GameObject objLineEnd;

	public GameObject objLineArc;

	public GameObject objLineVesselVect;

	public GameObject objLineVesselVectArrow1;

	public GameObject objLineVesselVectArrow2;

	public LineRenderer lineStart;

	public LineRenderer lineStartArrow1;

	public LineRenderer lineStartArrow2;

	public LineRenderer lineVesselVect;

	public LineRenderer lineVesselVectArrow1;

	public LineRenderer lineVesselVectArrow2;

	public LineRenderer lineEnd;

	public LineRenderer lineArc;

	public PlanetariumCamera cam;

	public int ArcPoints = 72;

	public int StartWidth = 10;

	public int EndWidth = 10;

	public GUIStyle styleLabelEnd;

	public GUIStyle styleLabelTarget;

	public double time;

	public bool isDrawing { get; set; }

	public bool isVisible => isAngleVisible;

	public bool isAngleVisible { get; set; }

	public bool isBecomingVisible => _isBecomingVisible;

	public bool IsBecomingInvisible { get; set; }

	public CelestialBody bodyOrigin { get; set; }

	public Orbit VesselOrbit { get; set; }

	public double AngleTargetValue { get; set; }

	public bool DrawToRetrograde { get; set; }

	public void Start()
	{
		objLineStart = new GameObject("LineStart");
		objLineStartArrow1 = new GameObject("LineStartArrow1");
		objLineStartArrow2 = new GameObject("LineStartArrow2");
		objLineEnd = new GameObject("LineEnd");
		objLineArc = new GameObject("LineArc");
		objLineVesselVect = new GameObject("LineVesselVect");
		objLineVesselVectArrow1 = new GameObject("LineVesselVectArrow1");
		objLineVesselVectArrow2 = new GameObject("LineVesselVectArrow2");
		Material orbitLinesMaterial = ((MapView)UnityEngine.Object.FindObjectOfType(typeof(MapView))).orbitLinesMaterial;
		lineStart = InitLine(objLineStart, Color.blue, 2, 10, orbitLinesMaterial);
		lineStartArrow1 = InitLine(objLineStartArrow1, Color.blue, 2, 10, orbitLinesMaterial);
		lineStartArrow2 = InitLine(objLineStartArrow2, Color.blue, 2, 10, orbitLinesMaterial);
		lineEnd = InitLine(objLineEnd, Color.blue, 2, 10, orbitLinesMaterial);
		lineArc = InitLine(objLineArc, Color.blue, ArcPoints, 10, orbitLinesMaterial);
		lineVesselVect = InitLine(objLineVesselVect, Color.green, 2, 10, orbitLinesMaterial);
		lineVesselVectArrow1 = InitLine(objLineVesselVectArrow1, Color.green, 2, 10, orbitLinesMaterial);
		lineVesselVectArrow2 = InitLine(objLineVesselVectArrow2, Color.green, 2, 10, orbitLinesMaterial);
		styleLabelEnd = new GUIStyle();
		styleLabelEnd.normal.textColor = Color.white;
		styleLabelEnd.alignment = TextAnchor.MiddleCenter;
		styleLabelTarget = new GUIStyle();
		styleLabelTarget.normal.textColor = Color.white;
		styleLabelTarget.alignment = TextAnchor.MiddleCenter;
		cam = (PlanetariumCamera)UnityEngine.Object.FindObjectOfType(typeof(PlanetariumCamera));
	}

	public LineRenderer InitLine(GameObject objToAttach, Color lineColor, int VertexCount, int InitialWidth, Material linesMaterial)
	{
		objToAttach.layer = 9;
		LineRenderer lineRenderer = objToAttach.AddComponent<LineRenderer>();
		lineRenderer.material = linesMaterial;
		lineRenderer.startColor = lineColor;
		lineRenderer.endColor = lineColor;
		lineRenderer.transform.parent = null;
		lineRenderer.useWorldSpace = true;
		lineRenderer.startWidth = InitialWidth;
		lineRenderer.endWidth = InitialWidth;
		lineRenderer.positionCount = VertexCount;
		lineRenderer.enabled = false;
		return lineRenderer;
	}

	public void OnDestroy()
	{
		_isBecomingVisible = false;
		_isBecomingVisible_LinesDone = false;
		_isBecomingVisible_ArcDone = false;
		_isBecomingVisible_VesselVectDone = false;
		_isHiding = false;
		isDrawing = false;
		lineStart = null;
		lineStartArrow1 = null;
		lineStartArrow2 = null;
		lineEnd = null;
		lineArc = null;
		lineVesselVect = null;
		lineVesselVectArrow1 = null;
		lineVesselVectArrow2 = null;
		objLineStart.DestroyGameObject();
		objLineEnd.DestroyGameObject();
		objLineArc.DestroyGameObject();
	}

	public void DrawAngle(CelestialBody bodyOrigin, double angleTarget, bool ToRetrograde, double time)
	{
		VesselOrbit = null;
		this.bodyOrigin = bodyOrigin;
		AngleTargetValue = angleTarget;
		DrawToRetrograde = ToRetrograde;
		this.time = time;
		isDrawing = true;
		StartDrawing = KSPUtil.SystemDateTime.DateTimeNow();
		_isBecomingVisible = true;
		_isBecomingVisible_LinesDone = false;
		_isBecomingVisible_ArcDone = false;
		_isBecomingVisible_VesselVectDone = false;
		_isHiding = false;
	}

	public void HideAngle()
	{
		StartDrawing = KSPUtil.SystemDateTime.DateTimeNow();
		_isHiding = true;
	}

	public static double ClampDegrees360(double angle)
	{
		angle %= 360.0;
		if (angle < 0.0)
		{
			return angle + 360.0;
		}
		return angle;
	}

	public static double ClampDegrees180(double angle)
	{
		angle = ClampDegrees360(angle);
		if (angle > 180.0)
		{
			angle -= 360.0;
		}
		return angle;
	}

	public void OnPreCull()
	{
		if (MapView.MapIsEnabled && isDrawing)
		{
			Vector3d vector3d = bodyOrigin.orbit.getOrbitalVelocityAtUT((time != 0.0) ? time : Planetarium.GetUniversalTime()).xzy.normalized * bodyOrigin.Radius * 5.0;
			Vector3d vector3d2 = ((!DrawToRetrograde) ? vector3d : (-vector3d));
			double magnitude = vector3d2.magnitude;
			Vector3d vector3d3 = Quaternion.AngleAxis((float)AngleTargetValue, bodyOrigin.orbit.GetOrbitNormal().xzy) * vector3d2;
			double magnitude2 = vector3d3.magnitude;
			vectPosWorldPivot = bodyOrigin.transform.position;
			vectPosWorldOrigin = bodyOrigin.transform.position + vector3d2;
			vectPosWorldEnd = bodyOrigin.transform.position + vector3d3;
			Vector3d pointEnd = bodyOrigin.transform.position + vector3d;
			vectPosWorldOrbitLabel = bodyOrigin.transform.position + vector3d * 3.0 / 4.0;
			_ = VesselOrbit;
			if (_isHiding)
			{
				float num = (float)(KSPUtil.SystemDateTime.DateTimeNow() - StartDrawing).TotalSeconds / 0.25f;
				if (num >= 1f)
				{
					_isHiding = false;
					isDrawing = false;
				}
				vectPosPivotWorking = bodyOrigin.transform.position - Mathf.Lerp(0f, (float)magnitude, Mathf.Clamp01(num)) * vector3d2.normalized;
				DrawLine(lineStart, vectPosWorldPivot + (DrawToRetrograde ? (vector3d.normalized * Mathf.Lerp((float)magnitude, 0f, num)) : default(Vector3d)), vectPosWorldPivot + (vectPosWorldOrigin - vectPosWorldPivot).normalized * Mathf.Lerp((float)magnitude, 0f, num));
				DrawLine(lineEnd, vectPosWorldPivot, vectPosWorldPivot + (vectPosWorldEnd - vectPosWorldPivot).normalized * Mathf.Lerp((float)magnitude2, 0f, num));
				DrawArc(lineArc, vector3d2, AngleTargetValue, Mathf.Lerp((float)bodyOrigin.Radius * 3f, 0f, num), Mathf.Lerp((float)bodyOrigin.Radius * 3f, 0f, num));
				Vector3d vector3d4 = bodyOrigin.transform.position + vector3d3 * 3.0 / 4.0;
				Vector3d pointEnd2 = (Vector3d)(Quaternion.AngleAxis(-90f, bodyOrigin.orbit.GetOrbitNormal().xzy) * vector3d3).normalized * (double)Mathf.Lerp((float)bodyOrigin.Radius * 3f, 0f, Mathf.Clamp01(num));
				pointEnd2 += vector3d4;
				DrawLine(lineVesselVect, vector3d4, pointEnd2);
				lineStartArrow1.enabled = false;
				lineStartArrow2.enabled = false;
				lineVesselVectArrow1.enabled = false;
				lineVesselVectArrow2.enabled = false;
			}
			else if (isBecomingVisible)
			{
				if (!_isBecomingVisible_LinesDone)
				{
					float num2 = (float)(KSPUtil.SystemDateTime.DateTimeNow() - StartDrawing).TotalSeconds / 0.5f;
					if (num2 >= 1f)
					{
						_isBecomingVisible_LinesDone = true;
						StartDrawing = KSPUtil.SystemDateTime.DateTimeNow();
					}
					vectPosPivotWorking = vectPosWorldPivot + (DrawToRetrograde ? (vector3d.normalized * Mathf.Lerp(0f, (float)magnitude, num2)) : default(Vector3d)) + Mathf.Lerp((float)magnitude, 0f, Mathf.Clamp01(num2)) * vector3d2.normalized;
					DrawLine(lineStart, vectPosPivotWorking, vectPosWorldOrigin);
				}
				else if (!_isBecomingVisible_ArcDone)
				{
					float num3 = (float)(KSPUtil.SystemDateTime.DateTimeNow() - StartDrawing).TotalSeconds / 0.5f;
					if (num3 >= 1f)
					{
						_isBecomingVisible_ArcDone = true;
						StartDrawing = KSPUtil.SystemDateTime.DateTimeNow();
					}
					double num4 = Mathf.Lerp((float)magnitude, (float)magnitude2, Mathf.Clamp01(num3));
					double num5 = ClampDegrees180(Mathf.Lerp(0f, (float)AngleTargetValue, Mathf.Clamp01(num3)));
					vectPosEndWorking = (Vector3d)(Quaternion.AngleAxis((float)num5, bodyOrigin.orbit.GetOrbitNormal().xzy) * vector3d2).normalized * num4;
					vectPosEndWorking += bodyOrigin.transform.position;
					DrawLine(lineStart, vectPosWorldPivot + (DrawToRetrograde ? vector3d : default(Vector3d)), vectPosWorldOrigin);
					DrawLine(lineEnd, vectPosWorldPivot, vectPosEndWorking);
					DrawArc(lineArc, vector3d2, num5, bodyOrigin.Radius * 3.0, bodyOrigin.Radius * 3.0);
				}
				else if (!_isBecomingVisible_VesselVectDone)
				{
					float num6 = (float)(KSPUtil.SystemDateTime.DateTimeNow() - StartDrawing).TotalSeconds / 0.5f;
					if (num6 >= 1f)
					{
						_isBecomingVisible_VesselVectDone = true;
						_isBecomingVisible = false;
					}
					DrawLine(lineStart, vectPosWorldPivot + (DrawToRetrograde ? vector3d : default(Vector3d)), vectPosWorldOrigin);
					DrawLineArrow(lineStartArrow1, lineStartArrow2, vectPosWorldPivot, pointEnd, bodyOrigin.orbit.GetOrbitNormal().xzy, bodyOrigin.Radius * 2.0 / 3.0);
					DrawLine(lineEnd, vectPosWorldPivot, vectPosWorldEnd);
					DrawArc(lineArc, vector3d2, AngleTargetValue, bodyOrigin.Radius * 3.0, bodyOrigin.Radius * 3.0);
					Vector3d vector3d5 = bodyOrigin.transform.position + vector3d3 * 3.0 / 4.0;
					Vector3d pointEnd3 = (Vector3d)(Quaternion.AngleAxis(-90f, bodyOrigin.orbit.GetOrbitNormal().xzy) * vector3d3).normalized * (double)Mathf.Lerp(0f, (float)bodyOrigin.Radius * 3f, Mathf.Clamp01(num6));
					pointEnd3 += vector3d5;
					DrawLine(lineVesselVect, vector3d5, pointEnd3);
				}
			}
			else
			{
				DrawLine(lineStart, vectPosWorldPivot + (DrawToRetrograde ? vector3d : default(Vector3d)), vectPosWorldOrigin);
				DrawLineArrow(lineStartArrow1, lineStartArrow2, vectPosWorldPivot, pointEnd, bodyOrigin.orbit.GetOrbitNormal().xzy, bodyOrigin.Radius * 2.0 / 3.0);
				DrawLine(lineEnd, vectPosWorldPivot, vectPosWorldEnd);
				DrawArc(lineArc, vector3d2, AngleTargetValue, bodyOrigin.Radius * 3.0, bodyOrigin.Radius * 3.0);
				Vector3d vector3d6 = bodyOrigin.transform.position + vector3d3 * 3.0 / 4.0;
				Vector3d pointEnd4 = (Vector3d)(Quaternion.AngleAxis(-90f, bodyOrigin.orbit.GetOrbitNormal().xzy) * vector3d3).normalized * bodyOrigin.Radius * 3.0;
				pointEnd4 += vector3d6;
				DrawLine(lineVesselVect, vector3d6, pointEnd4);
				DrawLineArrow(lineVesselVectArrow1, lineVesselVectArrow2, vector3d6, pointEnd4, bodyOrigin.orbit.GetOrbitNormal().xzy, bodyOrigin.Radius * 2.0 / 3.0);
			}
		}
		else
		{
			lineStart.enabled = false;
			lineStartArrow1.enabled = false;
			lineStartArrow2.enabled = false;
			lineEnd.enabled = false;
			lineArc.enabled = false;
			lineVesselVect.enabled = false;
			lineVesselVectArrow1.enabled = false;
			lineVesselVectArrow2.enabled = false;
		}
	}

	public void OnGUI()
	{
		if (MapView.MapIsEnabled && isDrawing && !_isBecomingVisible && !_isHiding)
		{
			GUI.Label(new Rect(PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(vectPosWorldEnd)).x - 50f, (float)Screen.height - PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(vectPosWorldEnd)).y - 15f, 100f, 30f), string.Format("{0:0.00}°\r\n{1}", AngleTargetValue, DrawToRetrograde ? "to retrograde" : "to prograde"), styleLabelEnd);
			GUI.Label(new Rect(PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(vectPosWorldOrbitLabel)).x - 50f, (float)Screen.height - PlanetariumCamera.Camera.WorldToScreenPoint(ScaledSpace.LocalToScaledSpace(vectPosWorldOrbitLabel)).y - 15f, 100f, 30f), "Orbit", styleLabelTarget);
			_ = VesselOrbit;
		}
	}

	public void DrawArc(LineRenderer line, Vector3d vectStart, double Angle, double StartLength, double EndLength)
	{
		double num = Math.Min(StartLength, EndLength);
		for (int i = 0; i < ArcPoints; i++)
		{
			Vector3d vector3d = ((Vector3d)(Quaternion.AngleAxis((float)Angle / (float)(ArcPoints - 1) * (float)i, bodyOrigin.orbit.GetOrbitNormal().xzy) * vectStart)).normalized * num;
			vector3d = bodyOrigin.transform.position + vector3d;
			line.SetPosition(i, ScaledSpace.LocalToScaledSpace(vector3d));
		}
		line.startWidth = 0.01f * cam.Distance;
		line.endWidth = 0.01f * cam.Distance;
		line.enabled = true;
	}

	public void DrawLine(LineRenderer line, Vector3d pointStart, Vector3d pointEnd)
	{
		line.SetPosition(0, ScaledSpace.LocalToScaledSpace(pointStart));
		line.SetPosition(1, ScaledSpace.LocalToScaledSpace(pointEnd));
		line.startWidth = 0.01f * cam.Distance;
		line.endWidth = 0.01f * cam.Distance;
		line.enabled = true;
	}

	public void DrawLineArrow(LineRenderer line1, LineRenderer line2, Vector3d pointStart, Vector3d pointEnd, Vector3d vectPlaneNormal, double ArrowArmLength)
	{
		Vector3d vector3d = (pointEnd - pointStart).normalized * ArrowArmLength;
		Vector3d vector3d2 = Quaternion.AngleAxis(30f, vectPlaneNormal) * vector3d;
		Vector3d vector3d3 = Quaternion.AngleAxis(-30f, vectPlaneNormal) * vector3d;
		line1.SetPosition(0, ScaledSpace.LocalToScaledSpace(pointEnd - vector3d2));
		line1.SetPosition(1, ScaledSpace.LocalToScaledSpace(pointEnd));
		line2.SetPosition(0, ScaledSpace.LocalToScaledSpace(pointEnd - vector3d3));
		line2.SetPosition(1, ScaledSpace.LocalToScaledSpace(pointEnd));
		line1.startWidth = 0.01f * cam.Distance;
		line1.endWidth = 0.01f * cam.Distance;
		line2.startWidth = 0.01f * cam.Distance;
		line2.endWidth = 0.01f * cam.Distance;
		line1.enabled = true;
		line2.enabled = true;
	}
}
