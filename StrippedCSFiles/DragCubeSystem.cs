using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DragCubeSystem : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CRenderDragCubesCoroutine_003Ed__31 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part p;

		public DragCubeSystem _003C_003E4__this;

		public ConfigNode dragConfig;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CRenderDragCubesCoroutine_003Ed__31(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CRenderDragCubes_003Ed__34 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public List<IMultipleDragCube> multipleInterfaces;

		public IMultipleDragCube moduleJettison;

		public ModulePartVariants modulePartVariants;

		public Part partPrefab;

		public DragCubeSystem _003C_003E4__this;

		public GameObject partObject;

		public ModuleDragAreaModifier[] areaModifiers;

		public ModuleDragModifier[] dragModifiers;

		public ConfigNode node;

		public bool destroyObject;

		private string[] _003CpositionNames_003E5__2;

		private int _003CinterfaceCount_003E5__3;

		private int _003CpositionCount_003E5__4;

		private int _003CinterfaceIdx_003E5__5;

		private int _003Cj_003E5__6;

		private DragCube _003Ccube_003E5__7;

		private Bounds _003CpartBounds_003E5__8;

		private int _003Ci_003E5__9;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CRenderDragCubes_003Ed__34(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSetupDragCubeCoroutine_003Ed__46 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Part p;

		public DragCubeSystem _003C_003E4__this;

		private ConfigNode _003CdragConfig_003E5__2;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CSetupDragCubeCoroutine_003Ed__46(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CSetupDragCubeCoroutine_003Ed__47 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ConfigNode dragConfig;

		public DragCubeSystem _003C_003E4__this;

		public Part p;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CSetupDragCubeCoroutine_003Ed__47(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[SerializeField]
	private int resolution;

	[SerializeField]
	private AnimationCurve dragCurve;

	[SerializeField]
	private string cameraLayer;

	[SerializeField]
	private float proceduralDragUpdateInterval;

	[SerializeField]
	private Shader dragShader;

	[SerializeField]
	private Shader dragShaderBumped;

	[SerializeField]
	private bool debugOutputTextures;

	[SerializeField]
	private string debugHaltOnPartName;

	[SerializeField]
	private string debugHaltOnPartCubeName;

	[SerializeField]
	private GameObject testPart;

	private Camera aeroCamera;

	private float aeroCameraSize;

	private int cameraLayerInt;

	private RenderTexture renderTexture;

	private Texture2D aeroTexture;

	private static float aeroCameraOffset;

	private static float aeroCameraDoubleOffset;

	private Coroutine testRoutine;

	[SerializeField]
	private Part testPartInstance;

	private static int debugProceduralTextureID;

	[SerializeField]
	private bool debugProcedural;

	public static DragCubeSystem Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public float ProceduralDragUpdateInterval
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragCubeSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static DragCubeSystem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAeroCamera()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateAeroTextures()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Test Render")]
	private void TestRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRenderDragCubesCoroutine_003Ed__31))]
	private IEnumerator RenderDragCubesCoroutine(Part p, ConfigNode dragConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetCdModifier(string name, ModuleDragModifier[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetAreaModifier(string name, ModuleDragAreaModifier[] modifiers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CRenderDragCubes_003Ed__34))]
	private IEnumerator RenderDragCubes(GameObject partObject, Part partPrefab, ConfigNode node, List<IMultipleDragCube> multipleInterfaces, IMultipleDragCube moduleJettison, ModulePartVariants modulePartVariants, ModuleDragModifier[] dragModifiers, ModuleDragAreaModifier[] areaModifiers, bool destroyObject = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Bounds CalculatePartBounds(GameObject part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupPartForRender(Part part, GameObject partObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool SetMainTexture(Material mat)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CleanUpPartMaterials(GameObject part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float SetAeroCamera(DragCube.DragFace direction, Bounds partBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetPlanarSize(DragCube.DragFace direction, Bounds partBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private float GetPlanarDepth(DragCube.DragFace direction, Bounds partBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetPlanarCenter(DragCube.DragFace direction, Bounds partBounds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Vector3 GetPlanarDirection(DragCube.DragFace direction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateAeroTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CalculateAerodynamics(Camera aeroCamera, out float area, out float drag, out float depth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetupDragCubeCoroutine_003Ed__46))]
	public IEnumerator SetupDragCubeCoroutine(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetupDragCubeCoroutine_003Ed__47))]
	public IEnumerator SetupDragCubeCoroutine(Part p, ConfigNode dragConfig)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DragCube RenderProceduralDragCube(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadDragCubes(Part p)
	{
		throw null;
	}
}
