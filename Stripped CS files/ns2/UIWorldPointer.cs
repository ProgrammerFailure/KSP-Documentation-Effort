using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

namespace ns2;

public class UIWorldPointer : MonoBehaviour
{
	public enum UISnapType
	{
		Anchor,
		Dynamic
	}

	public enum LineType
	{
		Direct,
		Kinked
	}

	public RectTransform uiTransform;

	public Transform worldTransform;

	public Camera worldCam;

	public UISnapType uiSnapType = UISnapType.Dynamic;

	public Vector2 uiSnapAnchor = new Vector2(1f, 0.5f);

	public LineType lineType = LineType.Kinked;

	public int chamferSubdivisions = 2;

	public float chamferDistance = 20f;

	public float kinkPercentage = 10f;

	public bool continuousUpdate = true;

	public Color lineColor = Color.white;

	public float lineWidth = 1f;

	public VectorLine line;

	public Material lineMat;

	public Vector3[] localCorners = new Vector3[4];

	public static Texture2D lineTexture;

	public static UIWorldPointer Create(RectTransform uiTransform, Transform worldTransform, Camera worldCam, Material mat)
	{
		GameObject obj = new GameObject("UI2WorldPointer");
		obj.AddComponent<RectTransform>();
		obj.transform.SetParent(uiTransform, worldPositionStays: false);
		UIWorldPointer uIWorldPointer = obj.AddComponent<UIWorldPointer>();
		uIWorldPointer.Setup(uiTransform, worldTransform, worldCam, mat);
		return uIWorldPointer;
	}

	public void Terminate()
	{
		Object.Destroy(base.gameObject);
	}

	public void Setup(RectTransform uiTransform, Transform worldTransform, Camera worldCam, Material mat)
	{
		this.uiTransform = uiTransform;
		this.worldTransform = worldTransform;
		this.worldCam = worldCam;
		lineMat = mat;
	}

	public void LateUpdate()
	{
		if (continuousUpdate || line == null)
		{
			UpdateLine();
		}
	}

	public void OnDestroy()
	{
		if (line != null)
		{
			VectorLine.Destroy(ref line);
		}
	}

	public void OnEnable()
	{
		if (line != null)
		{
			line.active = true;
		}
	}

	public void OnDisable()
	{
		if (line != null)
		{
			line.active = false;
		}
	}

	public void CreateLine()
	{
		List<Vector2> list = new List<Vector2>();
		if (lineType == LineType.Direct)
		{
			list = new List<Vector2>(new Vector2[2]
			{
				Vector2.zero,
				Vector2.one
			});
		}
		else
		{
			int num = 3 + 2 * (int)Mathf.Pow(2f, Mathf.Max(chamferSubdivisions, 1));
			for (int i = 0; i < num; i++)
			{
				list.Add(Vector2.zero);
			}
		}
		line = new VectorLine(base.gameObject.name, list, lineWidth);
		line.lineType = Vectrosity.LineType.Continuous;
		if (lineMat != null)
		{
			line.material = lineMat;
		}
		else
		{
			line.texture = GetTexture();
		}
		line.SetColor(lineColor);
		line.rectTransform.gameObject.layer = LayerMask.NameToLayer("UI");
	}

	public void UpdateLine()
	{
		if (!(worldTransform == null) && !(uiTransform == null))
		{
			if (line == null)
			{
				CreateLine();
			}
			if (lineType == LineType.Direct)
			{
				UpdateDirect();
			}
			else
			{
				UpdateKinked();
			}
			line.Draw();
			line.rectTransform.SetParent(uiTransform, worldPositionStays: false);
		}
		else
		{
			Terminate();
		}
	}

	public void UpdateDirect()
	{
		Vector3 vector = worldCam.WorldToScreenPoint(worldTransform.position);
		if (RectTransformUtility.RectangleContainsScreenPoint(uiTransform, vector, UIMasterController.Instance.uiCamera))
		{
			line.points2[0] = Vector2.zero;
			line.points2[1] = Vector2.zero;
		}
		else
		{
			line.points2[0] = uiTransform.InverseTransformPoint(UIMasterController.Instance.uiCamera.ScreenToWorldPoint(GetUIPoint(vector)));
			line.points2[1] = uiTransform.InverseTransformPoint(UIMasterController.Instance.uiCamera.ScreenToWorldPoint(vector));
		}
	}

	public void UpdateKinked()
	{
		Vector3 vector = worldCam.WorldToScreenPoint(worldTransform.position);
		if (RectTransformUtility.RectangleContainsScreenPoint(uiTransform, vector, UIMasterController.Instance.uiCamera))
		{
			int count = line.points2.Count;
			while (count-- > 0)
			{
				line.points2[count] = Vector2.zero;
			}
			return;
		}
		Vector2 vector2 = uiTransform.InverseTransformPoint((Vector2)UIMasterController.Instance.uiCamera.ScreenToWorldPoint(GetUIPoint(vector)));
		Vector2 vector3 = uiTransform.InverseTransformPoint((Vector2)UIMasterController.Instance.uiCamera.ScreenToWorldPoint(vector));
		vector2.x += uiTransform.sizeDelta.x * uiTransform.pivot.x;
		vector2.y += uiTransform.sizeDelta.y * uiTransform.pivot.y;
		vector3.x += uiTransform.sizeDelta.x * uiTransform.pivot.x;
		vector3.y += uiTransform.sizeDelta.y * uiTransform.pivot.y;
		uiTransform.GetLocalCorners(localCorners);
		Vector2 vector4 = (localCorners[0] + localCorners[2]) * 0.5f;
		vector4.x = Mathf.Abs(vector4.x);
		vector4.y = Mathf.Abs(vector4.y);
		float num = vector3.x - vector2.x;
		float num2 = vector3.y - vector2.y;
		float num3 = Mathf.Abs(num);
		float num4 = Mathf.Abs(num2);
		if (num3 + num4 < 20f)
		{
			int count2 = line.points2.Count;
			while (count2-- > 0)
			{
				line.points2[count2] = Vector2.zero;
			}
		}
		else if (!(num3 < chamferDistance * 2f) && num4 >= chamferDistance * 2f)
		{
			float f = ((num3 > num4) ? num : num2) / 100f * kinkPercentage;
			Vector2 vector5 = (vector2 - vector4).normalized * Mathf.Abs(f);
			Vector2 vector6 = vector2 + vector5;
			Vector2 vector7 = vector3 - vector5;
			Vector2 vector8 = (vector7 + vector6) * 0.5f;
			int num5 = Mathf.FloorToInt(Mathf.Pow(2f, Mathf.Max(chamferSubdivisions, 1)));
			line.points2[0] = vector2;
			line.points2.SetRange(ChamferCorner(vector2, vector6, vector8, chamferDistance, chamferSubdivisions), 1);
			line.points2[1 + num5] = vector8;
			line.points2.SetRange(ChamferCorner(vector8, vector7, vector3, chamferDistance, chamferSubdivisions), 2 + num5);
			line.points2[2 + num5 + num5] = vector3;
		}
		else
		{
			Vector2 vector9 = (vector3 - vector2) / (line.points2.Count - 1);
			int count3 = line.points2.Count;
			while (count3-- > 0)
			{
				line.points2[count3] = vector2 + vector9 * count3;
			}
		}
	}

	public Vector2[] ChamferCorner(Vector2 inPoint, Vector2 cornerPoint, Vector2 outPoint, float chamferDist, int subdivs)
	{
		Vector2[] array = new Vector2[1] { cornerPoint };
		for (int i = 0; i < subdivs; i++)
		{
			Vector2[] array2 = new Vector2[array.Length * 2];
			int j = 0;
			for (int num = array2.Length; j < num; j += 2)
			{
				int num2 = j / 2;
				Vector2 vector = array[num2];
				Vector2 normalized = (((num2 > 0) ? array[num2 - 1] : inPoint) - vector).normalized;
				Vector2 normalized2 = (((num2 < array.Length - 1) ? array[num2 + 1] : outPoint) - vector).normalized;
				array2[j] = vector + normalized * (chamferDist / Mathf.Pow(2.5f, i + 1));
				array2[j + 1] = vector + normalized2 * (chamferDist / Mathf.Pow(2.5f, i + 1));
			}
			array = array2;
		}
		return array;
	}

	public static Texture2D GetTexture()
	{
		if (lineTexture == null)
		{
			lineTexture = new Texture2D(1, 1, TextureFormat.RGBA32, mipChain: false);
			lineTexture.SetPixel(0, 0, Color.white);
			lineTexture.Apply(updateMipmaps: false, makeNoLongerReadable: true);
		}
		return lineTexture;
	}

	public Vector2 GetUIPoint(Vector3 worldPoint)
	{
		if (uiSnapType == UISnapType.Anchor)
		{
			return GetAnchoredPoint(worldPoint);
		}
		return GetClosestCornerOrMidpoint(worldPoint, 1f);
	}

	public Vector3 GetClosestCornerOrMidpoint(Vector3 worldPoint, float midPointPreference)
	{
		Vector3[] array = new Vector3[4];
		Vector3[] array2 = new Vector3[4];
		uiTransform.GetWorldCorners(array);
		for (int num = 3; num >= 0; num--)
		{
			array[num] = UIMasterController.Instance.uiCamera.WorldToScreenPoint(array[num]);
		}
		int i = 0;
		for (int num2 = array.Length; i < num2; i++)
		{
			if (i != array.Length - 1)
			{
				array2[i] = (array[i] + array[i + 1]) * 0.5f;
			}
			else
			{
				array2[i] = (array[i] + array[0]) * 0.5f;
			}
		}
		Vector3 vector = UtilMath.MinFrom((Vector3 p) => Vector3.SqrMagnitude(p - worldPoint), array);
		Vector3 vector2 = UtilMath.MinFrom((Vector3 p) => Vector3.SqrMagnitude(p - worldPoint), array2);
		if (Vector3.SqrMagnitude(vector2 - worldPoint) < Vector3.SqrMagnitude(vector - worldPoint) * (1f + midPointPreference))
		{
			return vector2;
		}
		return vector;
	}

	public Vector3 GetAnchoredPoint(Vector3 worldPoint)
	{
		Vector3[] array = new Vector3[4];
		uiTransform.GetWorldCorners(array);
		Vector3 vector = UIMasterController.Instance.uiCamera.WorldToScreenPoint(array[0]);
		Vector3 vector2 = UIMasterController.Instance.uiCamera.WorldToScreenPoint(array[2]);
		float x = Mathf.Lerp(vector.x, vector2.x, uiSnapAnchor.x);
		float y = Mathf.Lerp(vector.y, vector2.y, uiSnapAnchor.y);
		return new Vector3(x, y, vector.z);
	}
}
