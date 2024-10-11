using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleJettison : PartModule, IActivateOnDecouple, IScalarModule, IMultipleDragCube, IPartMassModifier
{
	[SerializeField]
	private Transform _jettisonTransform;

	[KSPField]
	public string bottomNodeName;

	[KSPField]
	public bool checkBottomNode;

	[KSPField]
	public bool decoupleEnabled;

	[KSPField]
	public bool isFairing;

	[KSPField(isPersistant = true)]
	public string activejettisonName;

	[KSPField]
	public string jettisonName;

	[KSPField]
	public string menuName;

	[KSPField]
	public string actionSuffix;

	[KSPField]
	public bool useMultipleDragCubes;

	[KSPField]
	public bool useProceduralDragCubes;

	[KSPField(isPersistant = true)]
	public bool isJettisoned;

	[UI_Toggle(disabledText = "#autoLOC_6001072", enabledText = "#autoLOC_6001071")]
	[KSPField(isPersistant = true, guiActiveEditor = true, guiName = "#autoLOC_6001455")]
	public bool shroudHideOverride;

	[KSPField]
	public bool hideJettisonMenu;

	[KSPField]
	public bool allowShroudToggle;

	[KSPField]
	public bool useCalculatedMass;

	[KSPField]
	public bool modifyJettisonedMass;

	[KSPField]
	public float jettisonedObjectMass;

	[KSPField]
	public float jettisonForce;

	[KSPField]
	public bool ignoreNodes;

	[KSPField]
	public Vector3 jettisonDirection;

	[KSPField]
	public bool manualJettison;

	public float forceScalar;

	private bool inEditor;

	private Vector3 partVolume;

	private float partDensity;

	private MaterialColorUpdater jettisonTemperatureRenderer;

	private bool hasVariantDragCubes;

	private bool setupComplete;

	private EventData<float, float> OnJettisonStart;

	private EventData<float> OnJettisonEnd;

	[KSPField]
	public string moduleID;

	public Transform jettisonTransform
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

	public bool IsMultipleCubesActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleJettison()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction("#autoLOC_6001456", activeEditor = false)]
	public void JettisonAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001456")]
	public void Jettison()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onVariantApplied(Part eventPart, PartVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Transform FetchActiveMeshTransform(string nameList)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadJettisonMeshState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetVariants()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetActionsSuffix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupJettisonState(bool isEditor)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetIgnoreCollisionFlags(List<Collider> set1, List<Collider> set2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckAttachNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFairingDragCube(bool fairingState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string[] GetDragCubeNames()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AssumeDragCubePosition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool UsesProceduralDragCubes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DecoupleAction(string nodeName, bool weDecouple)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override bool IsStageable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingEnableText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetStagingDisableText()
	{
		throw null;
	}
}
