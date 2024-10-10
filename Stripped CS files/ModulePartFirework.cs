using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ns10;
using ns9;
using UnityEngine;

public class ModulePartFirework : PartModule, IPartMassModifier
{
	[KSPField]
	public string shellName = "FireworkShell";

	[KSPField]
	public float shellDrag = 0.001f;

	[KSPField]
	public float shellMass = 0.1f;

	[KSPField]
	public float maxShots = 10f;

	[KSPField]
	public float shellRBMassScaleValue = 0.05f;

	[KSPField]
	public string cannonName = "COL2";

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
	public float burstSpread = 2f;

	[KSPAxisField(guiFormat = "F1", isPersistant = true, groupStartCollapsed = false, maxValue = 5f, groupDisplayName = "#autoLOC_6005107", minValue = 0.5f, groupName = "burst", guiActive = true, guiName = "#autoLOC_6005112")]
	[UI_FloatRange(stepIncrement = 0.1f, maxValue = 5f, minValue = 0.5f)]
	public float burstDuration = 2f;

	[UI_FloatRange(stepIncrement = 0.05f, maxValue = 5f, minValue = 0.25f)]
	[KSPAxisField(guiFormat = "F1", isPersistant = true, groupStartCollapsed = false, maxValue = 5f, groupDisplayName = "#autoLOC_6005107", minValue = 0.25f, groupName = "burst", guiActive = true, guiName = "#autoLOC_6005113")]
	public float burstFlareSize = 2f;

	[KSPAxisField(minValue = 10f, guiFormat = "F1", isPersistant = true, maxValue = 100f, guiActive = true, guiName = "#autoLOC_6005099")]
	[UI_FloatRange(maxValue = 100f, minValue = 10f)]
	public float shellVelocity = 50f;

	[UI_FloatRange(stepIncrement = 1f, maxValue = 20f, minValue = 1f)]
	[KSPAxisField(minValue = 1f, guiFormat = "F1", isPersistant = true, maxValue = 20f, guiActive = true, guiName = "#autoLOC_6005100")]
	public float shellDuration = 5f;

	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6005104")]
	[UI_FloatRange(stepIncrement = 1f, maxValue = 32f, minValue = 1f)]
	public float fireworkShots = 32f;

	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6005104")]
	[UI_Label]
	public string fireworkShotsDisplay = "";

	public GameObject shellPrefab;

	public FireworkFX fxController;

	[KSPField(isPersistant = true)]
	public bool variationOnShellDirection = true;

	[KSPField(isPersistant = true)]
	public float variationOnShellDirMultiplier = 0.02f;

	public Vector3 shellForceDir = Vector3.forward;

	public List<FireworkFXDefinition> availableFX;

	public UI_ChooseOption trailChooseOption;

	public UI_ChooseOption burstChooseOption;

	public UI_FloatRange fireworkShotsFloatRange;

	public UI_ColorPicker tertiaryaryBurstColorChangerField;

	[SerializeField]
	public Color[] fireworkColors;

	public UIPartActionColorPicker burstColorPicker1;

	public UIPartActionColorPicker burstColorPicker2;

	public UIPartActionColorPicker burstColorPicker3;

	public UIPartActionColorPicker trailColorPicker1;

	public UIPartActionColorPicker trailColorPicker2;

	public WaitForSeconds wfs;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 R")]
	public float trailColor1R = 1f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 G")]
	public float trailColor1G = 0.6705883f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 1 B")]
	public float trailColor1B = 1f / 17f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 R")]
	public float trailColor2R = 1f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 G")]
	public float trailColor2G = 0.6705883f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Trail Color 2 B")]
	public float trailColor2B = 1f / 17f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 R")]
	public float burstColor1R = 1f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 G")]
	public float burstColor1G = 0.6705883f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 1 B")]
	public float burstColor1B = 1f / 17f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 R")]
	public float burstColor2R = 1f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 G")]
	public float burstColor2G = 0.6705883f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 2 B")]
	public float burstColor2B = 1f / 17f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 R")]
	public float burstColor3R = 1f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 G")]
	public float burstColor3G = 0.6705883f;

	[KSPAxisField(isPersistant = true, guiActive = false, guiName = "Burst Color 3 B")]
	public float burstColor3B = 1f / 17f;

	public override void OnAwake()
	{
		base.OnAwake();
		if (HighLogic.LoadedSceneIsEditor)
		{
			trailChooseOption = base.Fields["trailType"].uiControlEditor as UI_ChooseOption;
			burstChooseOption = base.Fields["burstType"].uiControlEditor as UI_ChooseOption;
			fireworkShotsFloatRange = base.Fields["fireworkShots"].uiControlEditor as UI_FloatRange;
			tertiaryaryBurstColorChangerField = base.Fields["tertiaryBurstColorChanger"].uiControlEditor as UI_ColorPicker;
			base.Fields["fireworkShotsDisplay"].guiActiveEditor = false;
		}
		else if (HighLogic.LoadedSceneIsFlight)
		{
			trailChooseOption = base.Fields["trailType"].uiControlFlight as UI_ChooseOption;
			burstChooseOption = base.Fields["burstType"].uiControlFlight as UI_ChooseOption;
			fireworkShotsFloatRange = base.Fields["fireworkShots"].uiControlFlight as UI_FloatRange;
			tertiaryaryBurstColorChangerField = base.Fields["tertiaryBurstColorChanger"].uiControlFlight as UI_ColorPicker;
		}
		fireworkColors = new Color[5];
		wfs = new WaitForSeconds(1f);
		base.Fields["fireworkShots"].OnValueModified += updateFireworkShotLabel;
		base.Fields["trailColor1R"].guiName = Localizer.Format("#autoLOC_6005129", "1");
		base.Fields["trailColor1G"].guiName = Localizer.Format("#autoLOC_6005130", "1");
		base.Fields["trailColor1B"].guiName = Localizer.Format("#autoLOC_6005131", "1");
		base.Fields["trailColor2R"].guiName = Localizer.Format("#autoLOC_6005129", "2");
		base.Fields["trailColor2G"].guiName = Localizer.Format("#autoLOC_6005130", "2");
		base.Fields["trailColor2B"].guiName = Localizer.Format("#autoLOC_6005131", "2");
		base.Fields["burstColor1R"].guiName = Localizer.Format("#autoLOC_6005126", "1");
		base.Fields["burstColor1G"].guiName = Localizer.Format("#autoLOC_6005127", "1");
		base.Fields["burstColor1B"].guiName = Localizer.Format("#autoLOC_6005128", "1");
		base.Fields["burstColor2R"].guiName = Localizer.Format("#autoLOC_6005126", "2");
		base.Fields["burstColor2G"].guiName = Localizer.Format("#autoLOC_6005127", "2");
		base.Fields["burstColor2B"].guiName = Localizer.Format("#autoLOC_6005128", "2");
		base.Fields["burstColor3R"].guiName = Localizer.Format("#autoLOC_6005126", "3");
		base.Fields["burstColor3G"].guiName = Localizer.Format("#autoLOC_6005127", "3");
		base.Fields["burstColor3B"].guiName = Localizer.Format("#autoLOC_6005128", "3");
		base.Fields["trailColor1R"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailColor1G"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailColor1B"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailColor2R"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailColor2G"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailColor2B"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor1R"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor1G"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor1B"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor2R"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor2G"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor2B"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor3R"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor3G"].OnValueModified += UpdateFireworksColors;
		base.Fields["burstColor3B"].OnValueModified += UpdateFireworksColors;
		base.Fields["trailType"].guiName = Localizer.Format("#autoLOC_6005105", "#autoLOC_6005108");
		base.Fields["burstType"].guiName = Localizer.Format("#autoLOC_6005105", "#autoLOC_6005107");
		shellPrefab = AssetBase.GetPrefab(shellName);
		if (shellPrefab == null)
		{
			base.Events["LaunchShell"].guiActive = false;
			return;
		}
		fxController = shellPrefab.GetComponent<FireworkFX>();
		availableFX = new List<FireworkFXDefinition>();
	}

	public override void OnSave(ConfigNode node)
	{
		base.OnSave(node);
		node.SetValue("burstColor1R", fireworkColors[0].r, createIfNotFound: true);
		node.SetValue("burstColor1G", fireworkColors[0].g, createIfNotFound: true);
		node.SetValue("burstColor1B", fireworkColors[0].b, createIfNotFound: true);
		node.SetValue("burstColor2R", fireworkColors[1].r, createIfNotFound: true);
		node.SetValue("burstColor2G", fireworkColors[1].g, createIfNotFound: true);
		node.SetValue("burstColor2B", fireworkColors[1].b, createIfNotFound: true);
		node.SetValue("burstColor3R", fireworkColors[2].r, createIfNotFound: true);
		node.SetValue("burstColor3G", fireworkColors[2].g, createIfNotFound: true);
		node.SetValue("burstColor3B", fireworkColors[2].b, createIfNotFound: true);
		node.SetValue("trailColor1R", fireworkColors[3].r, createIfNotFound: true);
		node.SetValue("trailColor1G", fireworkColors[3].g, createIfNotFound: true);
		node.SetValue("trailColor1B", fireworkColors[3].b, createIfNotFound: true);
		node.SetValue("trailColor2R", fireworkColors[4].r, createIfNotFound: true);
		node.SetValue("trailColor2G", fireworkColors[4].g, createIfNotFound: true);
		node.SetValue("trailColor2B", fireworkColors[4].b, createIfNotFound: true);
	}

	public override void OnLoad(ConfigNode node)
	{
		base.OnLoad(node);
		fireworkColors = new Color[5];
		fireworkColors[0] = new Color(burstColor1R, burstColor1G, burstColor1B);
		fireworkColors[1] = new Color(burstColor2R, burstColor2G, burstColor2B);
		fireworkColors[2] = new Color(burstColor3R, burstColor3G, burstColor3B);
		fireworkColors[3] = new Color(trailColor1R, trailColor1G, trailColor1B);
		fireworkColors[4] = new Color(trailColor2R, trailColor2G, trailColor2B);
	}

	public override void OnStart(StartState state)
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		base.OnStart(state);
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
		{
			foreach (FireworkFXDefinition value in DatabaseGameObject.fwFXList.fireworkFX.Values)
			{
				availableFX.Add(value);
				if (value.fwType == FireworkEffectType.TRAIL)
				{
					list.Add(value.displayName);
				}
				else
				{
					list2.Add(value.displayName);
				}
			}
		}
		trailChooseOption.options = (trailChooseOption.display = list.ToArray());
		burstChooseOption.options = (burstChooseOption.display = list2.ToArray());
		fireworkShotsFloatRange.maxValue = maxShots;
		if (fireworkShots > maxShots)
		{
			fireworkShots = maxShots;
		}
		updateFireworkShotLabel(null);
		if (availableFX != null && availableFX.Count > 0)
		{
			configureColorFields(null);
		}
		if (HighLogic.LoadedSceneIsEditor || HighLogic.LoadedSceneIsFlight)
		{
			fireworkColors[0] = new Color(burstColor1R, burstColor1G, burstColor1B);
			fireworkColors[1] = new Color(burstColor2R, burstColor2G, burstColor2B);
			fireworkColors[2] = new Color(burstColor3R, burstColor3G, burstColor3B);
			fireworkColors[3] = new Color(trailColor1R, trailColor1G, trailColor1B);
			fireworkColors[4] = new Color(trailColor2R, trailColor2G, trailColor2B);
		}
		base.Fields["burstType"].OnValueModified += configureColorFields;
		GameEvents.onPartActionUIShown.Add(OnPAWShown);
		GameEvents.onPartActionInitialized.Add(OnPAWInit);
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUIShown.Remove(OnPAWShown);
		GameEvents.onPartActionInitialized.Remove(OnPAWInit);
	}

	public override List<Color> PresetColors()
	{
		return GameSettings.GetFireworkPresetColors();
	}

	public override void OnColorChanged(Color color, string pickerName)
	{
		if (pickerName != null && pickerName.Length == 0)
		{
			return;
		}
		switch (pickerName)
		{
		case "secondaryTrailColorChanger":
			fireworkColors[4] = color;
			trailColor2R = color.r;
			trailColor2G = color.g;
			trailColor2B = color.b;
			if (trailColorPicker2 != null)
			{
				trailColorPicker2.colorPicker.SetColor(color);
				trailColorPicker2.currentColorImage.color = color;
			}
			break;
		case "primaryTrailColorChanger":
			fireworkColors[3] = color;
			trailColor1R = color.r;
			trailColor1G = color.g;
			trailColor1B = color.b;
			if (trailColorPicker1 != null)
			{
				trailColorPicker1.colorPicker.SetColor(color);
				trailColorPicker1.currentColorImage.color = color;
			}
			break;
		case "tertiaryBurstColorChanger":
			fireworkColors[2] = color;
			burstColor3R = color.r;
			burstColor3G = color.g;
			burstColor3B = color.b;
			if (burstColorPicker3 != null)
			{
				burstColorPicker3.colorPicker.SetColor(color);
				burstColorPicker3.currentColorImage.color = color;
			}
			break;
		case "secondaryBurstColorChanger":
			fireworkColors[1] = color;
			burstColor2R = color.r;
			burstColor2G = color.g;
			burstColor2B = color.b;
			if (burstColorPicker2 != null)
			{
				burstColorPicker2.colorPicker.SetColor(color);
				burstColorPicker2.currentColorImage.color = color;
			}
			break;
		case "primaryBurstColorChanger":
			fireworkColors[0] = color;
			burstColor1R = color.r;
			burstColor1G = color.g;
			burstColor1B = color.b;
			if (burstColorPicker1 != null)
			{
				burstColorPicker1.colorPicker.SetColor(color);
				burstColorPicker1.currentColorImage.color = color;
			}
			break;
		}
	}

	[KSPAction(guiName = "#autoLOC_6005103")]
	public void LaunchShellAction(KSPActionParam param)
	{
		LaunchShell();
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6005103")]
	public void LaunchShellEvent()
	{
		LaunchShell();
		int count = base.part.symmetryCounterparts.Count;
		while (count-- > 0)
		{
			if (base.part.symmetryCounterparts[count] != base.part)
			{
				base.part.symmetryCounterparts[count].Modules.GetModule<ModulePartFirework>().LaunchShell();
			}
		}
	}

	public void OnPAWShown(UIPartActionWindow window, Part p)
	{
		if (p.persistentId == base.part.persistentId)
		{
			matchColorPickers();
		}
	}

	public void OnPAWInit(Part p)
	{
		if (p.persistentId.Equals(base.part.persistentId))
		{
			matchColorPickers();
			setAllColors(XKCDColors.YellowishOrange, XKCDColors.YellowishOrange, XKCDColors.YellowishOrange, XKCDColors.YellowishOrange, XKCDColors.YellowishOrange);
		}
	}

	public void UpdateFireworksColors(object obj)
	{
		matchColorPickers();
		updatePickerColor(FireworkEffectType.TRAIL, 1);
		updatePickerColor(FireworkEffectType.TRAIL, 2);
		updatePickerColor(FireworkEffectType.BURST, 1);
		updatePickerColor(FireworkEffectType.BURST, 2);
		updatePickerColor(FireworkEffectType.BURST, 3);
	}

	public void updatePickerColor(FireworkEffectType type, int number)
	{
		Color white = Color.white;
		if (type != 0)
		{
			if (number.Equals(1) && burstColorPicker1 != null)
			{
				white.r = burstColor1R;
				white.g = burstColor1G;
				white.b = burstColor1B;
				OnColorChanged(white, burstColorPicker1.id);
			}
			else if (number.Equals(2) && burstColorPicker2 != null)
			{
				white.r = burstColor2R;
				white.g = burstColor2G;
				white.b = burstColor2B;
				OnColorChanged(white, burstColorPicker2.id);
			}
			else if (number.Equals(3) && burstColorPicker3 != null)
			{
				white.r = burstColor3R;
				white.g = burstColor3G;
				white.b = burstColor3B;
				OnColorChanged(white, burstColorPicker3.id);
			}
		}
		else if (number.Equals(1) && trailColorPicker1 != null)
		{
			white.r = trailColor1R;
			white.g = trailColor1G;
			white.b = trailColor1B;
			OnColorChanged(white, trailColorPicker1.id);
		}
		else if (number.Equals(2) && trailColorPicker2 != null)
		{
			white.r = trailColor2R;
			white.g = trailColor2G;
			white.b = trailColor2B;
			OnColorChanged(white, trailColorPicker2.id);
		}
	}

	public void LaunchShell()
	{
		if (shellPrefab == null)
		{
			return;
		}
		if (!CheatOptions.InfinitePropellant)
		{
			if (!(fireworkShots > 0f))
			{
				return;
			}
			fireworkShots -= 1f;
			updateFireworkShotLabel(null);
		}
		Transform transform = base.gameObject.GetChild(cannonName).transform;
		Vector3 position = ((transform != null) ? transform.position : Vector3.zero);
		GameObject gameObject = UnityEngine.Object.Instantiate(shellPrefab, position, Quaternion.identity);
		Renderer componentInChildren = gameObject.GetComponentInChildren<Renderer>();
		if (componentInChildren != null && componentInChildren.material.mainTexture == null)
		{
			componentInChildren.material.SetTexture("_MainTex", base.part.GetPartRenderers()[0].material.mainTexture);
		}
		fxController = gameObject.GetComponent<FireworkFX>();
		if (FlightGlobals.ActiveVessel.orbit.referenceBody.atmosphere && FlightGlobals.ActiveVessel.altitude < FlightGlobals.ActiveVessel.orbit.referenceBody.atmosphereDepth)
		{
			configureSoundFX();
		}
		physicalObject obj = physicalObject.ConvertToPhysicalObject(base.part, gameObject);
		Rigidbody rb = obj.rb;
		obj.maxDistance = 10000f;
		obj.origDrag = shellDrag;
		rb.mass = shellMass * shellRBMassScaleValue;
		rb.maxAngularVelocity = PhysicsGlobals.MaxAngularVelocity;
		rb.angularVelocity = base.part.Rigidbody.angularVelocity;
		gameObject.transform.rotation = base.transform.rotation;
		shellForceDir = transform.up;
		if (variationOnShellDirection)
		{
			shellForceDir = (shellForceDir + UnityEngine.Random.onUnitSphere * variationOnShellDirMultiplier).normalized;
		}
		rb.drag = shellDrag;
		rb.useGravity = false;
		Vector3 force = shellForceDir * shellMass * (shellVelocity / Time.fixedDeltaTime) * shellRBMassScaleValue;
		rb.AddForce(force, ForceMode.Force);
		if (Krakensbane.GetFrameVelocity().magnitude <= 0.0)
		{
			rb.AddForce(base.vessel.rb_velocityD.normalized * shellMass * (base.vessel.rb_velocityD.magnitude / (double)Time.fixedDeltaTime) * shellRBMassScaleValue, ForceMode.Force);
		}
		base.part.AddForce(force.normalized * shellMass * shellVelocity * shellRBMassScaleValue * -1f);
		matchColorPickers();
		fxController.Setup(shellDuration, getCurrentPSByType(FireworkEffectType.TRAIL), getCurrentPSByType(FireworkEffectType.BURST), fireworkColors[0], fireworkColors[1], fireworkColors[3], fireworkColors[2], fireworkColors[4], shellVelocity, burstSpread, burstDuration, burstFlareSize, getCurrentFXByType(FireworkEffectType.BURST).crackleSFX, getCurrentFXByType(FireworkEffectType.BURST).randomizeBurstOrientation, getCurrentFXByType(FireworkEffectType.TRAIL).minTrailLifetime, getCurrentFXByType(FireworkEffectType.TRAIL).maxTrailLifetime);
	}

	public void configureSoundFX()
	{
		string value = "";
		AudioFX[] components = base.part.GetComponents<AudioFX>();
		int num = components.Length;
		while (num-- > 0)
		{
			switch (components[num].effectName)
			{
			case "Explosion0":
			case "Explosion1":
			case "Explosion2":
				if (string.IsNullOrEmpty(value))
				{
					string[] array = base.part.Effects.EffectsStartingWith("Explosion").ToArray();
					value = array[UnityEngine.Random.Range(0, array.Length - 1)];
				}
				if (components[num].effectName.Equals(value))
				{
					fxController.explosionSound = GameDatabase.Instance.GetAudioClip(Path.ChangeExtension(components[num].clip, null));
				}
				break;
			case "Thump":
				base.part.Effect("Thump");
				break;
			default:
				fxController.whistleSound = GameDatabase.Instance.GetAudioClip(Path.ChangeExtension(components[num].clip, null));
				break;
			}
		}
	}

	public GameObject getCurrentPSByType(FireworkEffectType type)
	{
		GameObject result = null;
		if (type != 0 && type == FireworkEffectType.BURST)
		{
			int count = availableFX.Count;
			while (count-- > 0)
			{
				if (availableFX[count].fwType == FireworkEffectType.BURST && availableFX[count].displayName.Equals(burstChooseOption.options[burstType]))
				{
					result = getFXFromPrefab(availableFX[count].prefabName);
					break;
				}
			}
		}
		else
		{
			int count2 = availableFX.Count;
			while (count2-- > 0)
			{
				if (availableFX[count2].fwType == FireworkEffectType.TRAIL && availableFX[count2].displayName.Equals(trailChooseOption.options[trailType]))
				{
					result = getFXFromPrefab(availableFX[count2].prefabName);
					break;
				}
			}
		}
		return result;
	}

	public FireworkFXDefinition getCurrentFXByType(FireworkEffectType type)
	{
		FireworkFXDefinition result = null;
		if (type != 0 && type == FireworkEffectType.BURST)
		{
			int count = availableFX.Count;
			while (count-- > 0)
			{
				if (availableFX[count].fwType == FireworkEffectType.BURST && availableFX[count].displayName.Equals(burstChooseOption.options[burstType]))
				{
					result = availableFX[count];
					break;
				}
			}
		}
		else
		{
			int count2 = availableFX.Count;
			while (count2-- > 0)
			{
				if (availableFX[count2].fwType == FireworkEffectType.TRAIL && availableFX[count2].displayName.Equals(trailChooseOption.options[trailType]))
				{
					result = availableFX[count2];
					break;
				}
			}
		}
		return result;
	}

	public GameObject getFXFromPrefab(string prefabName)
	{
		return Resources.Load("Effects/Fireworks/" + prefabName) as GameObject;
	}

	public void updateFireworkShotLabel(object field)
	{
		fireworkShotsDisplay = fireworkShots.ToString("F0");
	}

	public void trailTypeModified(object field)
	{
		trailType = (int)Convert.ToUInt32(field);
		UIPartActionWindow uIPartActionWindow = UIPartActionController.Instance?.GetItem(base.part);
		if (uIPartActionWindow != null)
		{
			uIPartActionWindow.displayDirty = true;
		}
	}

	public void configureColorFields(object field)
	{
		UI_Scene scene = (HighLogic.LoadedSceneIsEditor ? UI_Scene.Editor : UI_Scene.Flight);
		FireworkFXDefinition currentFXByType = getCurrentFXByType(FireworkEffectType.BURST);
		base.Fields["primaryBurstColorChanger"].guiName = currentFXByType.color1Name;
		base.Fields["secondaryBurstColorChanger"].guiName = currentFXByType.color2Name;
		if (!currentFXByType.color3Name.Equals("none"))
		{
			tertiaryaryBurstColorChangerField.SetSceneVisibility(scene, state: true);
			base.Fields["tertiaryBurstColorChanger"].guiName = currentFXByType.color3Name;
		}
		else
		{
			tertiaryaryBurstColorChangerField.SetSceneVisibility(scene, state: false);
		}
		UIPartActionWindow uIPartActionWindow = UIPartActionController.Instance?.GetItem(base.part);
		if (uIPartActionWindow != null)
		{
			uIPartActionWindow.displayDirty = true;
		}
		StartCoroutine("waitAndRefreshColors");
	}

	public UIPartActionColorPicker fetchColorPickerFromID(int pickerID)
	{
		UIPartActionColorPicker uIPartActionColorPicker = null;
		if (base.part.PartActionWindow != null)
		{
			int count = base.part.PartActionWindow.colorPickers.Count;
			while (count-- > 0)
			{
				uIPartActionColorPicker = base.part.PartActionWindow.colorPickers[count];
				if (uIPartActionColorPicker != null && uIPartActionColorPicker.id.GetHashCode().Equals(pickerID))
				{
					break;
				}
			}
		}
		return uIPartActionColorPicker;
	}

	public IEnumerator waitAndRefreshColors()
	{
		yield return wfs;
		matchColorPickers();
	}

	public string pickerID2FieldName(int pickerID)
	{
		string result = "";
		UIPartActionColorPicker uIPartActionColorPicker = fetchColorPickerFromID(pickerID);
		if (uIPartActionColorPicker != null)
		{
			result = uIPartActionColorPicker.Field.name;
		}
		return result;
	}

	public void matchColorPickers()
	{
		int hashCode = base.Fields["primaryBurstColorChanger"].name.GetHashCode();
		int hashCode2 = base.Fields["secondaryBurstColorChanger"].name.GetHashCode();
		int hashCode3 = base.Fields["tertiaryBurstColorChanger"].name.GetHashCode();
		int hashCode4 = base.Fields["primaryTrailColorChanger"].name.GetHashCode();
		int hashCode5 = base.Fields["secondaryTrailColorChanger"].name.GetHashCode();
		burstColorPicker1 = fetchColorPickerFromID(hashCode);
		burstColorPicker2 = fetchColorPickerFromID(hashCode2);
		burstColorPicker3 = fetchColorPickerFromID(hashCode3);
		trailColorPicker1 = fetchColorPickerFromID(hashCode4);
		trailColorPicker2 = fetchColorPickerFromID(hashCode5);
	}

	public void setAllColors(Color colB1, Color colB2, Color colB3, Color colT1, Color colT2)
	{
		if (burstColorPicker1 != null)
		{
			burstColorPicker1.colorPicker.SetColor(colB1);
			burstColor1R = colB1.r;
			burstColor1G = colB1.g;
			burstColor1B = colB1.b;
		}
		if (burstColorPicker2 != null)
		{
			burstColorPicker2.colorPicker.SetColor(colB2);
			burstColor2R = colB2.r;
			burstColor2G = colB2.g;
			burstColor2B = colB2.b;
		}
		if (burstColorPicker3 != null)
		{
			burstColorPicker3.colorPicker.SetColor(colB3);
			burstColor3R = colB3.r;
			burstColor3G = colB3.g;
			burstColor3B = colB3.b;
		}
		if (trailColorPicker1 != null)
		{
			trailColorPicker1.colorPicker.SetColor(colT1);
			trailColor1R = colT1.r;
			trailColor1G = colT1.g;
			trailColor1B = colT1.b;
		}
		if (trailColorPicker2 != null)
		{
			trailColorPicker2.colorPicker.SetColor(colT2);
			trailColor2R = colT2.r;
			trailColor2G = colT2.g;
			trailColor2B = colT2.b;
		}
	}

	public override Color GetCurrentColor(string fieldName)
	{
		return fieldName switch
		{
			"secondaryTrailColorChanger" => fireworkColors[4], 
			"primaryTrailColorChanger" => fireworkColors[3], 
			"tertiaryBurstColorChanger" => fireworkColors[2], 
			"secondaryBurstColorChanger" => fireworkColors[1], 
			"primaryBurstColorChanger" => fireworkColors[0], 
			_ => base.GetCurrentColor(fieldName), 
		};
	}

	public float GetModuleMass(float defaultMass, ModifierStagingSituation sit)
	{
		return defaultMass + fireworkShots * shellMass * 0.001f;
	}

	public ModifierChangeWhen GetModuleMassChangeWhen()
	{
		return ModifierChangeWhen.CONSTANTLY;
	}
}
