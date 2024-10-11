using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.FX.Fireworks;
using UnityEngine;

public class ModulePartFirework : PartModule, IPartMassModifier
{
	[CompilerGenerated]
	private sealed class _003CwaitAndRefreshColors_003Ed__74 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ModulePartFirework _003C_003E4__this;

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
		public _003CwaitAndRefreshColors_003Ed__74(int _003C_003E1__state)
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

	[KSPField]
	public string shellName;

	[KSPField]
	public float shellDrag;

	[KSPField]
	public float shellMass;

	[KSPField]
	public float maxShots;

	[KSPField]
	public float shellRBMassScaleValue;

	[KSPField]
	public string cannonName;

	[UI_ChooseOption(affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "trail", groupDisplayName = "#autoLOC_6005108", isPersistant = true, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005105")]
	public int trailType;

	[KSPField(groupName = "trail", groupDisplayName = "#autoLOC_6005108", isPersistant = false, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005101")]
	[UI_ColorPicker(useFieldNameForColor = true)]
	public string primaryTrailColorChanger;

	[UI_ColorPicker(useFieldNameForColor = true)]
	[KSPField(groupName = "trail", groupDisplayName = "#autoLOC_6005108", isPersistant = false, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005102")]
	public string secondaryTrailColorChanger;

	[UI_ChooseOption(affectSymCounterparts = UI_Scene.All)]
	[KSPField(groupName = "burst", groupDisplayName = "#autoLOC_6005107", isPersistant = true, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6005105")]
	public int burstType;

	[KSPField(groupName = "burst", groupDisplayName = "#autoLOC_6005107", isPersistant = false, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "Primary color")]
	[UI_ColorPicker(useFieldNameForColor = true)]
	public string primaryBurstColorChanger;

	[KSPField(groupName = "burst", groupDisplayName = "#autoLOC_6005107", isPersistant = false, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "Secondary color")]
	[UI_ColorPicker(useFieldNameForColor = true)]
	public string secondaryBurstColorChanger;

	[UI_ColorPicker(useFieldNameForColor = true)]
	[KSPField(groupName = "burst", groupDisplayName = "#autoLOC_6005107", isPersistant = false, groupStartCollapsed = false, guiActive = true, guiActiveEditor = true, guiName = "Tertiary color")]
	public string tertiaryBurstColorChanger;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 5f, minValue = 0.25f)]
	[KSPAxisField(guiFormat = "F1", isPersistant = true, groupStartCollapsed = false, maxValue = 5f, groupDisplayName = "#autoLOC_6005107", minValue = 0.25f, groupName = "burst", guiActive = true, guiName = "#autoLOC_6005111")]
	public float burstSpread;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, groupStartCollapsed = false, maxValue = 5f, groupDisplayName = "#autoLOC_6005107", minValue = 0.5f, groupName = "burst", guiActive = true, guiName = "#autoLOC_6005112")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 5f, minValue = 0.5f)]
	public float burstDuration;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 5f, minValue = 0.25f)]
	[KSPAxisField(guiFormat = "F1", isPersistant = true, groupStartCollapsed = false, maxValue = 5f, groupDisplayName = "#autoLOC_6005107", minValue = 0.25f, groupName = "burst", guiActive = true, guiName = "#autoLOC_6005113")]
	public float burstFlareSize;

	[KSPAxisField(minValue = 10f, guiFormat = "F1", isPersistant = true, maxValue = 100f, guiActive = true, guiName = "#autoLOC_6005099")]
	[UI_FloatRange(maxValue = 100f, minValue = 10f)]
	public float shellVelocity;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 20f, minValue = 1f)]
	[KSPAxisField(minValue = 1f, guiFormat = "F1", isPersistant = true, maxValue = 20f, guiActive = true, guiName = "#autoLOC_6005100")]
	public float shellDuration;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6005104")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 32f, minValue = 1f)]
	public float fireworkShots;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6005104")]
	[UI_Label]
	public string fireworkShotsDisplay;

	public GameObject shellPrefab;

	public FireworkFX fxController;

	[KSPField(isPersistant = true)]
	public bool variationOnShellDirection;

	[KSPField(isPersistant = true)]
	private float variationOnShellDirMultiplier;

	private Vector3 shellForceDir;

	private List<FireworkFXDefinition> availableFX;

	private UI_ChooseOption trailChooseOption;

	private UI_ChooseOption burstChooseOption;

	private UI_FloatRange fireworkShotsFloatRange;

	private UI_ColorPicker tertiaryaryBurstColorChangerField;

	[SerializeField]
	private Color[] fireworkColors;

	private UIPartActionColorPicker burstColorPicker1;

	private UIPartActionColorPicker burstColorPicker2;

	private UIPartActionColorPicker burstColorPicker3;

	private UIPartActionColorPicker trailColorPicker1;

	private UIPartActionColorPicker trailColorPicker2;

	private WaitForSeconds wfs;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 R")]
	public float trailColor1R;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 G")]
	public float trailColor1G;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 B")]
	public float trailColor1B;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 R")]
	public float trailColor2R;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 G")]
	public float trailColor2G;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 B")]
	public float trailColor2B;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 R")]
	public float burstColor1R;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 G")]
	public float burstColor1G;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 B")]
	public float burstColor1B;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 R")]
	public float burstColor2R;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 G")]
	public float burstColor2G;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 B")]
	public float burstColor2B;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 R")]
	public float burstColor3R;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 G")]
	public float burstColor3G;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 B")]
	public float burstColor3B;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModulePartFirework()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override List<Color> PresetColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnColorChanged(Color color, string pickerName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPAction(guiName = "#autoLOC_6005103")]
	public void LaunchShellAction(KSPActionParam param)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = true, guiName = "#autoLOC_6005103")]
	public void LaunchShellEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPAWShown(UIPartActionWindow window, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPAWInit(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateFireworksColors(object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updatePickerColor(FireworkEffectType type, int number)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LaunchShell()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void configureSoundFX()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject getCurrentPSByType(FireworkEffectType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FireworkFXDefinition getCurrentFXByType(FireworkEffectType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private GameObject getFXFromPrefab(string prefabName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void updateFireworkShotLabel(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void trailTypeModified(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void configureColorFields(object field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIPartActionColorPicker fetchColorPickerFromID(int pickerID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CwaitAndRefreshColors_003Ed__74))]
	private IEnumerator waitAndRefreshColors()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string pickerID2FieldName(int pickerID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void matchColorPickers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void setAllColors(Color colB1, Color colB2, Color colB3, Color colT1, Color colT2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override Color GetCurrentColor(string fieldName)
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
}
