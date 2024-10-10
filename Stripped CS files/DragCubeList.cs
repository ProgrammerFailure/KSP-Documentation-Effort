using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DragCubeList
{
	public struct CubeData
	{
		public Vector3 dragVector;

		public Vector3 liftForce;

		public float area;

		public float areaDrag;

		public float depth;

		public float crossSectionalArea;

		public float exposedArea;

		public float dragCoeff;

		public float taperDot;

		public CubeData(CubeData oldCube)
		{
			dragVector = oldCube.dragVector;
			liftForce = oldCube.liftForce;
			area = oldCube.area;
			areaDrag = oldCube.areaDrag;
			depth = oldCube.depth;
			crossSectionalArea = oldCube.crossSectionalArea;
			exposedArea = oldCube.exposedArea;
			dragCoeff = oldCube.dragCoeff;
			taperDot = oldCube.taperDot;
		}

		public void Reset()
		{
			dragVector = (liftForce = Vector3.zero);
			float num = 0f;
			taperDot = 0f;
			float num2 = num;
			num = 0f;
			dragCoeff = num2;
			float num3 = num;
			num = 0f;
			exposedArea = num3;
			float num4 = num;
			num = 0f;
			crossSectionalArea = num4;
			float num5 = num;
			num = 0f;
			depth = num5;
			float num6 = num;
			num = 0f;
			areaDrag = num6;
			area = num;
		}
	}

	[SerializeField]
	public List<DragCube> cubes = new List<DragCube>();

	[SerializeField]
	public float postOcclusionArea;

	[SerializeField]
	public bool none;

	[SerializeField]
	public bool procedural;

	public Part part;

	[SerializeField]
	public CubeData cubeData;

	[SerializeField]
	public bool debugDragCube;

	public bool weightsRequireUpdate = true;

	public bool occlusionRequireUpdate = true;

	public DragCube proceduralDragCube;

	public float proceduralDragNextUpdate;

	public float occlusionMultiplier = 1f;

	public float[] weightedArea = new float[6];

	public float[] weightedDrag = new float[6];

	public float[] weightedDragOrig = new float[6];

	public float[] weightedDepth = new float[6];

	public float[] areaOccluded = new float[6];

	public Vector3 weightedCenter = Vector3.zero;

	public Vector3 weightedSize = Vector3.zero;

	public bool rotateDragVector;

	public Quaternion dragVectorRotation = Quaternion.identity;

	[NonSerialized]
	public FloatCurve DragCurveCd;

	[NonSerialized]
	public FloatCurve DragCurveCdPower;

	[NonSerialized]
	public FloatCurve DragCurveMultiplier;

	[NonSerialized]
	public PhysicsGlobals.LiftingSurfaceCurve BodyLiftCurve;

	[NonSerialized]
	public PhysicsGlobals.SurfaceCurvesList SurfaceCurves;

	public static Vector3[] faceDirections = new Vector3[6]
	{
		Vector3.right,
		Vector3.left,
		Vector3.up,
		Vector3.down,
		Vector3.forward,
		Vector3.back
	};

	public List<DragCube> Cubes => cubes;

	public float AreaDrag => cubeData.areaDrag;

	public float Area => cubeData.area;

	public float Depth => cubeData.depth;

	public float CrossSectionalArea => cubeData.crossSectionalArea;

	public float ExposedArea => cubeData.exposedArea;

	public float PostOcclusionArea => postOcclusionArea;

	public Vector3 LiftForce => cubeData.liftForce;

	public float DragCoeff => cubeData.dragCoeff;

	public float TaperDot => cubeData.taperDot;

	public bool None
	{
		get
		{
			return none;
		}
		set
		{
			none = value;
		}
	}

	public bool Procedural
	{
		get
		{
			return procedural;
		}
		set
		{
			procedural = value;
		}
	}

	public Vector3 DragVector => cubeData.dragVector;

	public Part Part => part;

	public float OcclusionMultiplier => occlusionMultiplier;

	public float[] WeightedArea => weightedArea;

	public float[] WeightedDrag => weightedDrag;

	public float[] WeightedDepth => weightedDepth;

	public float[] AreaOccluded => areaOccluded;

	public Vector3 WeightedCenter => weightedCenter;

	public Vector3 WeightedSize => weightedSize;

	public bool RotateDragVector => rotateDragVector;

	public Quaternion DragVectorRotation => dragVectorRotation;

	public void SetPart(Part part)
	{
		this.part = part;
		DragCurveCd = PhysicsGlobals.DragCurveCd;
		DragCurveCdPower = PhysicsGlobals.DragCurveCdPower;
		DragCurveMultiplier = PhysicsGlobals.DragCurveMultiplier;
		BodyLiftCurve = PhysicsGlobals.BodyLiftCurve;
		SurfaceCurves = PhysicsGlobals.SurfaceCurves;
	}

	public void SetDragWeights()
	{
		if (!weightsRequireUpdate || none)
		{
			return;
		}
		if (procedural)
		{
			if (SetDragWeights_Procedural())
			{
				weightsRequireUpdate = false;
			}
		}
		else
		{
			SetDragWeights_List();
			weightsRequireUpdate = false;
		}
	}

	public void SetDragWeights_List()
	{
		ResetCubeArray(weightedArea);
		ResetCubeArray(weightedDrag);
		ResetCubeArray(weightedDragOrig);
		ResetCubeArray(weightedDepth);
		weightedCenter = Vector3.zero;
		weightedSize = Vector3.zero;
		int count = cubes.Count;
		if (count == 0)
		{
			return;
		}
		float num = 0f;
		int index = count;
		while (index-- > 0)
		{
			DragCube dragCube = cubes[index];
			if (dragCube.Weight != 0f)
			{
				num += dragCube.Weight;
				AddCubeArray(weightedArea, dragCube.Area, dragCube.Weight);
				AddCubeArray(weightedDrag, dragCube.Drag, dragCube.Weight);
				AddCubeArray(weightedDragOrig, dragCube.Drag, dragCube.Weight);
				AddCubeArray(weightedDepth, dragCube.Depth, dragCube.Weight);
				weightedCenter += dragCube.Center * dragCube.Weight;
				weightedSize += dragCube.Size * dragCube.Weight;
			}
		}
		if (num == 0f)
		{
			ResetCubeArray(weightedArea);
			ResetCubeArray(weightedDrag);
			ResetCubeArray(weightedDragOrig);
			ResetCubeArray(weightedDepth);
			weightedCenter = Vector3.zero;
			weightedSize = Vector3.zero;
		}
		else
		{
			float num2 = 1f / num;
			MultiplyCubeArray(weightedArea, num2);
			MultiplyCubeArray(weightedDrag, num2);
			MultiplyCubeArray(weightedDragOrig, num2);
			MultiplyCubeArray(weightedDepth, num2);
			weightedCenter *= num2;
			weightedSize *= num2;
		}
	}

	public bool SetDragWeights_Procedural()
	{
		if (Time.realtimeSinceStartup > proceduralDragNextUpdate)
		{
			proceduralDragNextUpdate = Time.realtimeSinceStartup + DragCubeSystem.Instance.ProceduralDragUpdateInterval;
			proceduralDragCube = DragCubeSystem.Instance.RenderProceduralDragCube(part);
			SetCubeArray(weightedArea, proceduralDragCube.Area);
			SetCubeArray(weightedDrag, proceduralDragCube.Drag);
			SetCubeArray(weightedDragOrig, proceduralDragCube.Drag);
			SetCubeArray(weightedDepth, proceduralDragCube.Depth);
			weightedCenter = proceduralDragCube.Center;
			weightedSize = proceduralDragCube.Size;
			return true;
		}
		return false;
	}

	public void SetPartOcclusion()
	{
		if (!occlusionRequireUpdate || none)
		{
			return;
		}
		occlusionRequireUpdate = false;
		SetCubeArray(areaOccluded, weightedArea);
		SetCubeArray(weightedDrag, weightedDragOrig);
		int count = part.attachNodes.Count;
		while (count-- > 0)
		{
			AttachNode attachNode = part.attachNodes[count];
			if (attachNode == null)
			{
				continue;
			}
			Part attachedPart = attachNode.attachedPart;
			if (attachedPart == null)
			{
				continue;
			}
			AttachNode attachNode2 = attachNode.attachedPart.FindAttachNodeByPart(part);
			if (attachedPart.dragModel != Part.DragModel.CUBE)
			{
				continue;
			}
			Vector3 normalized = attachNode.orientation.normalized;
			Vector3 vector = Quaternion.Inverse(attachedPart.partTransform.rotation) * part.partTransform.rotation * normalized;
			float num = ((attachNode.overrideDragArea >= 0f) ? attachNode.overrideDragArea : GetFacingAreaSum(-vector, attachedPart.DragCubes.weightedArea));
			if (debugDragCube)
			{
				Debug.Log(part.gameObject.name + " Node: " + attachNode.id);
				Debug.Log("Facing Area: " + num);
			}
			AreaToCubeOperation(normalized, num * occlusionMultiplier, (float fArea, float cArea) => Mathf.Max(0f, cArea - fArea));
			if (debugDragCube)
			{
				Debug.Log("[DragCube]: Occlusion of " + attachedPart.name + " against " + part.name + ". Remote node: " + ((attachNode2 != null) ? attachNode2.id : "N/A") + ", Local node: " + attachNode.id + ", Att Direction " + KSPUtil.WriteVector(-vector) + ", Local Direction " + KSPUtil.WriteVector(normalized) + ", Facing Area: " + num.ToString("0.0") + ", OccludedArea: " + KSPUtil.PrintCollection(areaOccluded, ", ", (float f) => f.ToString("0.0")));
			}
			attachNode.contactArea = num;
		}
		postOcclusionArea = SumCubeArray(areaOccluded);
		if (part.srfAttachNode != null && part.srfAttachNode.attachedPart != null)
		{
			AttachNode attachNode = part.srfAttachNode;
			float facingAreaSum = GetFacingAreaSum(attachNode.orientation.normalized, part.DragCubes.weightedArea);
			attachNode.contactArea = facingAreaSum;
		}
	}

	public void SetDrag(Vector3 vector, float machNumber)
	{
		if (!none)
		{
			if (rotateDragVector)
			{
				AddSurfaceDragDirection(dragVectorRotation * -vector, machNumber, ref cubeData);
			}
			else
			{
				AddSurfaceDragDirection(-vector, machNumber, ref cubeData);
			}
		}
	}

	public void AddSurfaceDragDirection(Vector3 direction, float machNumber, ref CubeData retData)
	{
		float num = 0f;
		retData.Reset();
		retData.dragVector = direction;
		for (int i = 0; i < 6; i++)
		{
			Vector3 vector = faceDirections[i];
			float num2 = Vector3.Dot(direction, vector);
			float dotNormalized = (num2 + 1f) * 0.5f;
			float num3 = PhysicsGlobals.DragCurveValue(SurfaceCurves, dotNormalized, machNumber);
			float num4 = areaOccluded[i] * num3;
			float num5 = weightedDrag[i];
			retData.area += num4;
			float num6 = num5;
			if (num6 < 1f)
			{
				num6 = Mathf.Pow(DragCurveCd.Evaluate(num5), DragCurveCdPower.Evaluate(machNumber));
			}
			retData.areaDrag += num4 * num6;
			retData.crossSectionalArea += areaOccluded[i] * Mathf.Clamp01(num2);
			num5 = ((!(num5 < 1f) || !(num5 > 0.01f)) ? 1f : (1f / num5));
			retData.exposedArea += num4 / DragCurveMultiplier.Evaluate(machNumber) * num5;
			if (num2 > 0f)
			{
				num += num2;
				double num7 = BodyLiftCurve.liftCurve.Evaluate(num2);
				if (!double.IsNaN(num7))
				{
					retData.liftForce += -vector * (num2 * areaOccluded[i] * weightedDrag[i] * (float)num7);
				}
				retData.depth += num2 * weightedDepth[i];
				retData.taperDot += num2 * num5;
			}
		}
		if (num > 0f)
		{
			float num8 = 1f / num;
			retData.depth *= num8;
			retData.taperDot *= num8;
		}
		if (retData.area > 0f)
		{
			retData.dragCoeff = retData.areaDrag / retData.area;
			retData.areaDrag = retData.area * retData.dragCoeff;
		}
		else
		{
			float dragCoeff = 0f;
			retData.areaDrag = 0f;
			retData.dragCoeff = dragCoeff;
		}
	}

	public float GetCubeAreaDir(Vector3 dir, float[] areas)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			Vector3 rhs = faceDirections[i];
			float num2 = Vector3.Dot(dir, rhs);
			if (num2 > 0f)
			{
				num += areas[i] * num2;
			}
		}
		return num;
	}

	public float GetCubeAreaDir(Vector3 dir)
	{
		return GetCubeAreaDir(dir, areaOccluded);
	}

	public float GetCubeCoeffDir(Vector3 dir)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 6; i++)
		{
			Vector3 rhs = faceDirections[i];
			float num3 = Vector3.Dot(dir, rhs);
			if (num3 > 0f)
			{
				num += weightedDrag[i] * num3;
				num2 += num3;
			}
		}
		return num / num2;
	}

	public float GetCubeDepthDir(Vector3 dir)
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 6; i++)
		{
			Vector3 rhs = faceDirections[i];
			float num3 = Vector3.Dot(dir, rhs);
			if (num3 > 0f)
			{
				num += weightedDepth[i] * num3;
				num2 += num3;
			}
		}
		return num / num2;
	}

	public static Vector3 GetFaceDirection(DragCube.DragFace direction)
	{
		return direction switch
		{
			DragCube.DragFace.const_0 => Vector3.right, 
			DragCube.DragFace.const_1 => Vector3.left, 
			DragCube.DragFace.const_2 => Vector3.up, 
			DragCube.DragFace.const_3 => Vector3.down, 
			DragCube.DragFace.const_4 => Vector3.forward, 
			_ => Vector3.back, 
		};
	}

	public static float SetFacedDotProducts(Vector3 direction, float[] cubeArray)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			float num2 = Vector3.Dot(direction, faceDirections[i]);
			if (num2 <= 0f)
			{
				cubeArray[i] = 0f;
				continue;
			}
			cubeArray[i] = num2;
			num += num2;
		}
		return num;
	}

	public static float SetFacedDotProducts(Vector3 direction, float[] outArray, float[] faceArray)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			float num2 = Vector3.Dot(direction, faceDirections[i]);
			if (num2 <= 0f)
			{
				outArray[i] = 0f;
				continue;
			}
			outArray[i] = faceArray[i] * num2;
			num += num2;
		}
		return num;
	}

	public static float SetFacedDotProductsNormalized(Vector3 direction, float[] outArray, float[] faceArray)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			float num2 = Vector3.Dot(direction, faceDirections[i]);
			if (num2 <= 0f)
			{
				outArray[i] = 0f;
				continue;
			}
			outArray[i] = faceArray[i] * num2;
			num += num2;
		}
		DivideCubeArray(outArray, num);
		return num;
	}

	public static float AddFacedDotProducts(Vector3 direction, float[] outArray, float[] faceArray)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			float num2 = Vector3.Dot(direction, faceDirections[i]);
			if (!(num2 <= 0f))
			{
				outArray[i] += faceArray[i] * num2;
				num += num2;
			}
		}
		return num;
	}

	public static void ResetCubeArray(float[] arr)
	{
		for (int i = 0; i < 6; i++)
		{
			arr[i] = 0f;
		}
	}

	public static void ResetCubeArray(Vector3[] arr)
	{
		for (int i = 0; i < 6; i++)
		{
			arr[i].x = 0f;
			arr[i].y = 0f;
			arr[i].z = 0f;
		}
	}

	public static void DivideCubeArray(float[] arr, float divisor)
	{
		if (divisor == 0f)
		{
			ResetCubeArray(arr);
			return;
		}
		float num = 1f / divisor;
		for (int i = 0; i < 6; i++)
		{
			arr[i] *= num;
		}
	}

	public static void DivideCubeArray(float[] arr, float[] divisor)
	{
		for (int i = 0; i < 6; i++)
		{
			if (divisor[i] == 0f)
			{
				arr[i] = 0f;
			}
			else
			{
				arr[i] /= divisor[i];
			}
		}
	}

	public static void MultiplyCubeArray(float[] arr, float multiply)
	{
		for (int i = 0; i < 6; i++)
		{
			arr[i] *= multiply;
		}
	}

	public static void MultiplyCubeArray(float[] arr, float[] multiply)
	{
		for (int i = 0; i < 6; i++)
		{
			arr[i] *= multiply[i];
		}
	}

	public static void AddCubeArray(float[] outputArray, float[] inputArray, float multiply = 1f)
	{
		for (int i = 0; i < 6; i++)
		{
			outputArray[i] += inputArray[i] * multiply;
		}
	}

	public static void SetCubeArray(float[] outputArray, float[] inputArray, float multiply = 1f)
	{
		for (int i = 0; i < 6; i++)
		{
			outputArray[i] = inputArray[i] * multiply;
		}
	}

	public static float SumCubeArray(float[] arr)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			num += arr[i];
		}
		return num;
	}

	public static Vector3 CreateVectorFromArray(float[] arr)
	{
		return new Vector3((arr[0] > arr[1]) ? arr[0] : (0f - arr[1]), (arr[2] > arr[3]) ? arr[2] : (0f - arr[3]), (arr[4] > arr[5]) ? arr[4] : (0f - arr[5]));
	}

	public static void AddVectorToArray(Vector3 vector, float[] arr)
	{
		if (vector.x >= 0f)
		{
			arr[0] += vector.x;
		}
		else
		{
			arr[1] -= vector.x;
		}
		if (vector.y >= 0f)
		{
			arr[2] += vector.y;
		}
		else
		{
			arr[3] -= vector.y;
		}
		if (vector.z >= 0f)
		{
			arr[4] += vector.z;
		}
		else
		{
			arr[5] -= vector.z;
		}
	}

	public static void SubtractVectorFromArray(Vector3 vector, float[] arr)
	{
		if (vector.x >= 0f)
		{
			arr[0] -= vector.x;
		}
		else
		{
			arr[1] += vector.x;
		}
		if (vector.y >= 0f)
		{
			arr[2] -= vector.y;
		}
		else
		{
			arr[3] += vector.y;
		}
		if (vector.z >= 0f)
		{
			arr[4] -= vector.z;
		}
		else
		{
			arr[5] += vector.z;
		}
	}

	public static void SubtractVectorFromArrayClamp(Vector3 vector, float[] arr)
	{
		if (vector.x >= 0f)
		{
			arr[0] = Mathf.Max(arr[0] - vector.x, 0f);
		}
		else
		{
			arr[1] = Mathf.Max(arr[1] + vector.x, 0f);
		}
		if (vector.y >= 0f)
		{
			arr[2] = Mathf.Max(arr[2] - vector.y, 0f);
		}
		else
		{
			arr[3] = Mathf.Max(arr[3] + vector.y, 0f);
		}
		if (vector.z >= 0f)
		{
			arr[4] = Mathf.Max(arr[4] - vector.z, 0f);
		}
		else
		{
			arr[5] = Mathf.Max(arr[5] + vector.z, 0f);
		}
	}

	public static float GetFacingAreaSum(Vector3 direction, float[] faceAreaArray)
	{
		float num = 0f;
		for (int i = 0; i < 6; i++)
		{
			float num2 = Mathf.Clamp01(Vector3.Dot(direction, faceDirections[i]));
			num += faceAreaArray[i] * num2;
		}
		return num;
	}

	public void AreaToCubeOperation(Vector3 direction, float area, Func<float, float, float> operation)
	{
		for (int i = 0; i < 6; i++)
		{
			float num = Mathf.Clamp01(Vector3.Dot(direction, faceDirections[i]));
			if (num > 0f)
			{
				if (debugDragCube)
				{
					DragCube.DragFace dragFace = (DragCube.DragFace)i;
					Debug.Log(dragFace.ToString() + " LocalDot: " + num);
				}
				float num2 = areaOccluded[i];
				areaOccluded[i] = operation(area * num, areaOccluded[i]);
				float num3 = weightedArea[i] - areaOccluded[i];
				if (debugDragCube)
				{
					Debug.Log("AreaOccluded: " + areaOccluded[i] + " = prevAreaOccluded: " + num2 + " - Facing area * localDot: " + area * num);
					Debug.Log("occArea: " + num3);
				}
				if (areaOccluded[i] > 0f)
				{
					weightedDrag[i] = Mathf.Max(0f, (weightedDragOrig[i] * weightedArea[i] - num3) / areaOccluded[i]);
				}
				else
				{
					weightedDrag[i] = 1E-05f;
				}
				if (debugDragCube)
				{
					Debug.Log("weightedDrag: " + weightedDrag[i]);
				}
			}
		}
	}

	public void SetOcclusionMultiplier(float multiplier)
	{
		occlusionMultiplier = multiplier;
		ForceUpdate(weights: false, occlusion: true);
	}

	public DragCube GetCube(string name)
	{
		int count = cubes.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (!(cubes[count].Name == name));
		return cubes[count];
	}

	public void SetCubeWeight(string name, float weight)
	{
		bool flag = false;
		int count = cubes.Count;
		while (count-- > 0)
		{
			if (cubes[count].Name == name && cubes[count].Weight != weight)
			{
				cubes[count].Weight = weight;
				flag = true;
			}
		}
		if (flag)
		{
			ForceUpdate(weights: true, occlusion: true);
		}
	}

	public void SetDragVectorRotation(Quaternion rotation)
	{
		rotateDragVector = true;
		dragVectorRotation = rotation;
	}

	public void SetDragVectorRotation(bool rotateDragVector)
	{
		this.rotateDragVector = rotateDragVector;
	}

	public void ResetCubeWeights(float newWeight)
	{
		bool flag = false;
		int count = cubes.Count;
		while (count-- > 0)
		{
			if (cubes[count].Weight != newWeight)
			{
				cubes[count].Weight = newWeight;
				flag = true;
			}
		}
		if (flag)
		{
			ForceUpdate(weights: true, occlusion: true);
		}
	}

	public void ResetCubeWeights()
	{
		ResetCubeWeights(1f);
	}

	public void ClearCubes()
	{
		cubes.Clear();
		procedural = false;
	}

	public ConfigNode SaveCubes()
	{
		ConfigNode configNode = new ConfigNode("DRAG_CUBE");
		if (none)
		{
			configNode.AddValue("none", true.ToString());
			return configNode;
		}
		if (procedural)
		{
			configNode.AddValue("procedural", true.ToString());
			return configNode;
		}
		int count = cubes.Count;
		while (count-- > 0)
		{
			configNode.AddValue("cube", cubes[count].SaveToString());
		}
		return configNode;
	}

	public bool LoadCubes(ConfigNode node)
	{
		ClearCubes();
		if (node.HasValue("none"))
		{
			none = true;
			return true;
		}
		if (node.HasValue("procedural"))
		{
			procedural = true;
			return true;
		}
		string[] values = node.GetValues("cube");
		int num = values.Length;
		while (true)
		{
			if (num-- > 0)
			{
				string[] data = values[num].Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				DragCube dragCube = new DragCube();
				if (!dragCube.Load(data))
				{
					break;
				}
				cubes.Add(dragCube);
				continue;
			}
			return true;
		}
		Debug.LogError("DragCube: Invalid cube line " + (num + 1));
		return false;
	}

	public bool LoadCubes(DragCubeList cubes)
	{
		bool result = false;
		bool flag = procedural;
		ClearCubes();
		if (cubes.cubes.Count < 1)
		{
			procedural = flag;
			return false;
		}
		for (int i = 0; i < cubes.cubes.Count; i++)
		{
			DragCube item = cubes.cubes[i];
			this.cubes.Add(item);
			result = true;
		}
		procedural = flag;
		return result;
	}

	public bool LoadCube(DragCubeList cubes, string whichOne)
	{
		bool result = false;
		bool flag = procedural;
		ClearCubes();
		if (cubes.cubes.Count < 1)
		{
			procedural = flag;
			return false;
		}
		for (int i = 0; i < cubes.cubes.Count; i++)
		{
			if (cubes.cubes[i].Name == whichOne)
			{
				DragCube item = cubes.cubes[i];
				this.cubes.Add(item);
				result = true;
			}
		}
		procedural = flag;
		return result;
	}

	public void RequestOcclusionUpdate()
	{
		occlusionRequireUpdate = true;
	}

	public void ForceUpdate(bool weights, bool occlusion, bool resetProcTiming = false)
	{
		if (resetProcTiming)
		{
			proceduralDragNextUpdate = -1f;
		}
		if (weights)
		{
			weightsRequireUpdate = true;
			occlusionRequireUpdate = true;
		}
		else if (occlusion)
		{
			occlusionRequireUpdate = true;
		}
		if (!occlusionRequireUpdate)
		{
			return;
		}
		int count = part.attachNodes.Count;
		while (count-- > 0)
		{
			if (!(part.attachNodes[count].attachedPart == null) && part.attachNodes[count].attachedPart.dragModel == Part.DragModel.CUBE)
			{
				part.attachNodes[count].attachedPart.DragCubes.occlusionRequireUpdate = true;
			}
		}
	}

	public void DrawDragGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(part.partTransform.position, part.partTransform.position + part.partTransform.up * 1.5f);
		Gizmos.DrawRay(part.partTransform.position, -part.partTransform.up);
		Gizmos.color = Color.red;
		Gizmos.DrawLine(part.partTransform.position, part.partTransform.position + part.partTransform.right * 1.5f);
		Gizmos.DrawRay(part.partTransform.position, -part.partTransform.right);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(part.partTransform.position, part.partTransform.position + part.partTransform.forward * 1.5f);
		Gizmos.DrawRay(part.partTransform.position, -part.partTransform.forward);
		Gizmos.color = Color.cyan;
		Gizmos.DrawRay(part.partTransform.position, part.partTransform.TransformDirection(cubeData.dragVector.normalized));
	}
}
