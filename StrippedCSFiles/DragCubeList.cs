using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.NoInlining)]
		public CubeData(CubeData oldCube)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Reset()
		{
			throw null;
		}
	}

	[SerializeField]
	private List<DragCube> cubes;

	[SerializeField]
	private float postOcclusionArea;

	[SerializeField]
	private bool none;

	[SerializeField]
	private bool procedural;

	private Part part;

	[SerializeField]
	private CubeData cubeData;

	[SerializeField]
	private bool debugDragCube;

	private bool weightsRequireUpdate;

	private bool occlusionRequireUpdate;

	private DragCube proceduralDragCube;

	private float proceduralDragNextUpdate;

	private float occlusionMultiplier;

	private float[] weightedArea;

	private float[] weightedDrag;

	private float[] weightedDragOrig;

	private float[] weightedDepth;

	private float[] areaOccluded;

	private Vector3 weightedCenter;

	private Vector3 weightedSize;

	private bool rotateDragVector;

	private Quaternion dragVectorRotation;

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

	internal static Vector3[] faceDirections;

	public List<DragCube> Cubes
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float AreaDrag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Area
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float Depth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float CrossSectionalArea
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float ExposedArea
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float PostOcclusionArea
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 LiftForce
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float DragCoeff
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float TaperDot
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool None
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Procedural
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public Vector3 DragVector
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Part Part
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float OcclusionMultiplier
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] WeightedArea
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] WeightedDrag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] WeightedDepth
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float[] AreaOccluded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WeightedCenter
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Vector3 WeightedSize
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool RotateDragVector
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Quaternion DragVectorRotation
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragCubeList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DragCubeList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDragWeights()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDragWeights_List()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetDragWeights_Procedural()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPartOcclusion()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDrag(Vector3 vector, float machNumber)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSurfaceDragDirection(Vector3 direction, float machNumber, ref CubeData retData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCubeAreaDir(Vector3 dir, float[] areas)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCubeAreaDir(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCubeCoeffDir(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetCubeDepthDir(Vector3 dir)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 GetFaceDirection(DragCube.DragFace direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float SetFacedDotProducts(Vector3 direction, float[] cubeArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float SetFacedDotProducts(Vector3 direction, float[] outArray, float[] faceArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float SetFacedDotProductsNormalized(Vector3 direction, float[] outArray, float[] faceArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float AddFacedDotProducts(Vector3 direction, float[] outArray, float[] faceArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ResetCubeArray(float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ResetCubeArray(Vector3[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DivideCubeArray(float[] arr, float divisor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void DivideCubeArray(float[] arr, float[] divisor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MultiplyCubeArray(float[] arr, float multiply)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void MultiplyCubeArray(float[] arr, float[] multiply)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddCubeArray(float[] outputArray, float[] inputArray, float multiply = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetCubeArray(float[] outputArray, float[] inputArray, float multiply = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float SumCubeArray(float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector3 CreateVectorFromArray(float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AddVectorToArray(Vector3 vector, float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SubtractVectorFromArray(Vector3 vector, float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SubtractVectorFromArrayClamp(Vector3 vector, float[] arr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static float GetFacingAreaSum(Vector3 direction, float[] faceAreaArray)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AreaToCubeOperation(Vector3 direction, float area, Func<float, float, float> operation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetOcclusionMultiplier(float multiplier)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragCube GetCube(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetCubeWeight(string name, float weight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDragVectorRotation(Quaternion rotation)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDragVectorRotation(bool rotateDragVector)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCubeWeights(float newWeight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ResetCubeWeights()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ClearCubes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ConfigNode SaveCubes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool LoadCubes(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool LoadCubes(DragCubeList cubes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool LoadCube(DragCubeList cubes, string whichOne)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RequestOcclusionUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ForceUpdate(bool weights, bool occlusion, bool resetProcTiming = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawDragGizmos()
	{
		throw null;
	}
}
