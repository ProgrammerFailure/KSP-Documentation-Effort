using System;
using System.ComponentModel;
using Expansions;
using Expansions.Missions;
using Expansions.Serenity;
using Experience;
using Experience.Effects;
using ns9;
using UnityEngine;

[Serializable]
public class ProtoCrewMember
{
	public enum RosterStatus
	{
		[Description("#autoLOC_901072")]
		Available,
		[Description("#autoLOC_901073")]
		Assigned,
		[Description("#autoLOC_901074")]
		Dead,
		[Description("#autoLOC_901075")]
		Missing
	}

	public enum KerbalType
	{
		[Description("#autoLoc_6002189")]
		Crew,
		[Description("#autoLOC_900290")]
		Applicant,
		[Description("#autoLOC_901070")]
		Unowned,
		[Description("#autoLOC_476080")]
		Tourist
	}

	public enum Gender
	{
		Male,
		Female
	}

	public enum KerbalSuit
	{
		Default,
		Vintage,
		Future,
		Slim
	}

	public delegate void voidDelegate_PCM_Part(ProtoCrewMember pcm, Part p);

	public delegate void voidDelegate_PCM(ProtoCrewMember pcm);

	public delegate double doubleDelegatePCM(ProtoCrewMember pcm);

	public delegate void voidDelegate_PCM_UT(ProtoCrewMember pcm, double double_0);

	public delegate Kerbal SpawnKerbalDelegate(ProtoCrewMember pcm);

	[SerializeField]
	public string _name;

	public float stupidity;

	public float courage;

	public bool isBadass;

	public bool hasHelmetOn;

	public bool hasNeckRingOn;

	public bool hasVisorDown;

	public float lightR = 1f;

	public float lightG = 0.5176f;

	public float lightB;

	public bool completedFirstEVA;

	public bool _inactive;

	public double inactiveTimeEnd;

	public bool veteran;

	public bool hasToured;

	public double UTaR;

	public Kerbal KerbalRef;

	public RosterStatus _rosterStatus;

	public KerbalType _type;

	public Gender _gender;

	public KerbalSuit _suit;

	public string suitTexturePath;

	public string normalTexturePath;

	public string spritePath;

	public bool useStockTexture;

	public string comboId;

	public uint persistentID;

	public ConfigNode chuteNode;

	public ModuleInventoryPart cachedPrefabModule;

	public ModuleInventoryPart instanceKerbalModule;

	public ConfigNode inventoryNode;

	public Part cachedPart;

	public int seatIdx = -1;

	public InternalSeat seat;

	public static voidDelegate_PCM_Part CallbackActiveFixedUpdate;

	public static voidDelegate_PCM_Part CallbackActiveUpdate;

	public static voidDelegate_PCM_Part CallbackOnPartPack;

	public static voidDelegate_PCM_Part CallbackOnPartUnpack;

	public static voidDelegate_PCM_UT CallbackUpdate;

	public static voidDelegate_PCM_Part ApplyGCallback;

	public double geeForce;

	public double gExperienced;

	public double gIncrement;

	public bool outDueToG;

	public static doubleDelegatePCM GToleranceMult = _GToleranceMult;

	public static doubleDelegatePCM MaxSustainedG = _MaxSustainedG;

	public static bool doStockGCalcs = true;

	public static SpawnKerbalDelegate Spawn = _Spawn;

	public FlightLog careerLog = new FlightLog();

	public FlightLog flightLog = new FlightLog();

	public float experience;

	public float extraExperience;

	public int experienceLevel;

	public ExperienceTrait experienceTrait;

	public string trait;

	public string name => _name;

	public string displayName => _name switch
	{
		"Valentina Kerman" => Localizer.Format("#autoLOC_1100005"), 
		"Bob Kerman" => Localizer.Format("#autoLOC_1100004"), 
		"Bill Kerman" => Localizer.Format("#autoLOC_1100003"), 
		"Jebediah Kerman" => Localizer.Format("#autoLOC_1100002"), 
		_ => Localizer.Format(_name), 
	};

	public string nameWithGender => _name.LocalizeName(_gender);

	public bool inactive
	{
		get
		{
			return _inactive;
		}
		set
		{
			if (_inactive != value)
			{
				GameEvents.onKerbalInactiveChange.Fire(this, _inactive, value);
				_inactive = value;
			}
		}
	}

	public bool isHero { get; set; }

	public RosterStatus rosterStatus
	{
		get
		{
			return _rosterStatus;
		}
		set
		{
			GameEvents.onKerbalStatusChange.Fire(this, _rosterStatus, value);
			RosterStatus data = _rosterStatus;
			_rosterStatus = value;
			GameEvents.onKerbalStatusChanged.Fire(this, data, value);
		}
	}

	public KerbalType type
	{
		get
		{
			return _type;
		}
		set
		{
			GameEvents.onKerbalTypeChange.Fire(this, _type, value);
			KerbalType data = _type;
			_type = value;
			GameEvents.onKerbalTypeChanged.Fire(this, data, value);
		}
	}

	public Gender gender
	{
		get
		{
			return _gender;
		}
		set
		{
			_gender = value;
		}
	}

	public KerbalSuit suit
	{
		get
		{
			if (!ExpansionsLoader.IsExpansionKerbalSuitInstalled(_suit))
			{
				_suit = KerbalSuit.Default;
			}
			return _suit;
		}
		set
		{
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(value))
			{
				_suit = value;
			}
		}
	}

	public string SuitTexturePath
	{
		get
		{
			return suitTexturePath;
		}
		set
		{
			suitTexturePath = value;
		}
	}

	public string NormalTexturePath
	{
		get
		{
			return normalTexturePath;
		}
		set
		{
			normalTexturePath = value;
		}
	}

	public string SpritePath
	{
		get
		{
			return spritePath;
		}
		set
		{
			spritePath = value;
		}
	}

	public bool UseStockTexture
	{
		get
		{
			return useStockTexture;
		}
		set
		{
			useStockTexture = value;
		}
	}

	public string ComboId
	{
		get
		{
			return comboId;
		}
		set
		{
			comboId = value;
		}
	}

	public ConfigNode ChuteNode
	{
		get
		{
			return chuteNode;
		}
		set
		{
			chuteNode = value;
		}
	}

	public ModuleInventoryPart kerbalModule
	{
		get
		{
			if (instanceKerbalModule == null)
			{
				if (cachedPrefabModule == null)
				{
					cachedPrefabModule = PartLoader.getPartInfoByName(GetKerbalEVAPartName()).partPrefab.gameObject.GetComponent<ModuleInventoryPart>();
				}
				instanceKerbalModule = UnityEngine.Object.Instantiate(cachedPrefabModule).GetComponent<ModuleInventoryPart>();
				instanceKerbalModule.kerbalReference = this;
				if (KerbalInventoryScenario.Instance != null && HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO || HighLogic.CurrentGame.Mode == Game.Modes.SCENARIO_NON_RESUMABLE))
				{
					KerbalInventoryScenario.Instance.AddKerbalInventoryInstance(name, instanceKerbalModule);
				}
				instanceKerbalModule.Awake();
				instanceKerbalModule.Load(InventoryNode);
				instanceKerbalModule.OnStart(PartModule.StartState.Editor);
			}
			return instanceKerbalModule;
		}
	}

	public ModuleInventoryPart KerbalInventoryModule => kerbalModule;

	public ConfigNode InventoryNode
	{
		get
		{
			if (inventoryNode == null)
			{
				SetDefaultInventory();
			}
			return inventoryNode;
		}
		set
		{
			inventoryNode = value;
		}
	}

	public double GExperiencedNormalized => gExperienced / (PhysicsGlobals.KerbalGThresholdLOC * GToleranceMult(this) * (double)HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalGToleranceMult);

	public float ExtraExperience
	{
		get
		{
			return extraExperience;
		}
		set
		{
			extraExperience = value;
			UpdateExperience();
		}
	}

	public float ExperienceLevelDelta
	{
		get
		{
			if (experienceLevel == KerbalRoster.GetExperienceMaxLevel())
			{
				return 1f;
			}
			if (experience == 0f)
			{
				return 0f;
			}
			float experienceLevelRequirement = KerbalRoster.GetExperienceLevelRequirement(experienceLevel);
			float experienceLevelRequirement2 = KerbalRoster.GetExperienceLevelRequirement(experienceLevel - 1);
			return (experience - experienceLevelRequirement2) / (experienceLevelRequirement - experienceLevelRequirement2);
		}
	}

	public ProtoCrewMember(KerbalType type)
	{
		_type = type;
		completedFirstEVA = false;
		SetDefaultInventory();
	}

	public ProtoCrewMember(KerbalType type, string newName)
	{
		_type = type;
		_name = newName;
		completedFirstEVA = false;
		SetDefaultInventory();
	}

	public ProtoCrewMember(ProtoCrewMember copyOf)
	{
		_name = copyOf._name;
		gender = copyOf.gender;
		courage = copyOf.courage;
		stupidity = copyOf.stupidity;
		isBadass = copyOf.isBadass;
		veteran = copyOf.veteran;
		hasToured = copyOf.hasToured;
		_rosterStatus = copyOf.rosterStatus;
		_inactive = copyOf._inactive;
		inactiveTimeEnd = copyOf.inactiveTimeEnd;
		gExperienced = copyOf.gExperienced;
		outDueToG = copyOf.outDueToG;
		_type = copyOf._type;
		UTaR = copyOf.UTaR;
		flightLog = copyOf.flightLog.CreateCopy();
		careerLog = copyOf.careerLog.CreateCopy();
		if (ExpansionsLoader.IsExpansionAnyKerbalSuitInstalled())
		{
			suit = copyOf.suit;
		}
		isHero = copyOf.isHero;
		trait = copyOf.trait;
		KerbalRoster.SetExperienceTrait(this);
		extraExperience = copyOf.extraExperience;
		experience = CalculateExperiencePoints(HighLogic.CurrentGame);
		experienceLevel = KerbalRoster.CalculateExperienceLevel(experience);
		KerbalRef = copyOf.KerbalRef;
		hasHelmetOn = copyOf.hasHelmetOn;
		hasNeckRingOn = copyOf.hasNeckRingOn;
		hasVisorDown = copyOf.hasVisorDown;
		lightR = copyOf.lightR;
		lightG = copyOf.lightG;
		lightB = copyOf.lightB;
		completedFirstEVA = copyOf.completedFirstEVA;
		suitTexturePath = copyOf.suitTexturePath;
		normalTexturePath = copyOf.normalTexturePath;
		spritePath = copyOf.spritePath;
		comboId = copyOf.comboId;
		inventoryNode = copyOf.inventoryNode;
		GameEvents.onProtoCrewMemberLoad.Fire(new GameEvents.FromToAction<ProtoCrewMember, ConfigNode>(this, null));
	}

	public ProtoCrewMember(Game.Modes mode, ConfigNode node, KerbalType crewType = KerbalType.Crew)
	{
		GameEvents.onProtoCrewMemberLoad.Fire(new GameEvents.FromToAction<ProtoCrewMember, ConfigNode>(this, node));
		_type = crewType;
		bool flag = false;
		for (int i = 0; i < node.values.Count; i++)
		{
			ConfigNode.Value value = node.values[i];
			switch (value.name)
			{
			case "lightB":
				lightB = float.Parse(value.value);
				break;
			case "lightG":
				lightG = float.Parse(value.value);
				break;
			case "hero":
				isHero = bool.Parse(value.value);
				break;
			case "suit":
				suit = (KerbalSuit)Enum.Parse(typeof(KerbalSuit), value.value);
				if (!ExpansionsLoader.IsExpansionKerbalSuitInstalled(suit))
				{
					suit = KerbalSuit.Default;
				}
				break;
			case "hasHelmetOn":
				hasHelmetOn = bool.Parse(value.value);
				break;
			case "inactiveTimeEnd":
				inactiveTimeEnd = double.Parse(value.value);
				break;
			case "completedFirstEVA":
				completedFirstEVA = bool.Parse(value.value);
				break;
			case "lightR":
				lightR = float.Parse(value.value);
				break;
			case "tour":
				hasToured = bool.Parse(value.value);
				break;
			case "spritePath":
				spritePath = value.value;
				break;
			case "outDueToG":
				outDueToG = bool.Parse(value.value);
				break;
			case "hasNeckRingOn":
				hasNeckRingOn = bool.Parse(value.value);
				break;
			case "type":
				_type = (KerbalType)Enum.Parse(typeof(KerbalType), value.value);
				break;
			case "badS":
				isBadass = bool.Parse(value.value);
				break;
			case "gender":
				gender = (Gender)Enum.Parse(typeof(Gender), value.value);
				break;
			case "state":
			{
				int result = -1;
				if (int.TryParse(value.value, out result))
				{
					_rosterStatus = (RosterStatus)result;
				}
				else
				{
					_rosterStatus = (RosterStatus)Enum.Parse(typeof(RosterStatus), value.value);
				}
				break;
			}
			case "hasVisorDown":
				hasVisorDown = bool.Parse(value.value);
				break;
			case "trait":
				trait = node.GetValue("trait");
				break;
			case "idx":
				seatIdx = int.Parse(value.value);
				break;
			case "comboId":
				comboId = value.value;
				break;
			case "gExperienced":
				gExperienced = double.Parse(value.value);
				break;
			case "name":
				_name = value.value;
				break;
			case "brave":
				courage = float.Parse(value.value);
				break;
			case "extraXP":
				extraExperience = float.Parse(value.value);
				break;
			case "normalTexturePath":
				normalTexturePath = value.value;
				break;
			case "veteran":
				veteran = bool.Parse(value.value);
				flag = true;
				break;
			case "suitTexturePath":
				suitTexturePath = value.value;
				break;
			case "dumb":
				stupidity = float.Parse(value.value);
				break;
			case "inactive":
				_inactive = bool.Parse(value.value);
				break;
			case "ToD":
				UTaR = double.Parse(value.value);
				break;
			}
		}
		if (!flag)
		{
			if (!(name == "Jebediah Kerman") && !(name == "Bill Kerman") && !(name == "Bob Kerman") && !(name == "Valentina Kerman"))
			{
				veteran = false;
			}
			else
			{
				veteran = true;
				isHero = true;
			}
		}
		node.TryGetNode("EVACHUTE", ref chuteNode);
		node.TryGetNode("INVENTORY", ref inventoryNode);
		careerLog.Load(node.GetNode("CAREER_LOG"));
		flightLog.Load(node.GetNode("FLIGHT_LOG"));
		KerbalRoster.SetExperienceTrait(this);
		if (HighLogic.CurrentGame != null)
		{
			experience = CalculateExperiencePoints(HighLogic.CurrentGame);
			experienceLevel = KerbalRoster.CalculateExperienceLevel(experience);
		}
		else
		{
			GameEvents.onGameStateCreated.Add(OnGameStateCreated);
		}
	}

	public bool SetInactive(double time, bool additive = true)
	{
		bool result = _inactive;
		if (additive && _inactive)
		{
			inactiveTimeEnd += time;
		}
		else
		{
			inactiveTimeEnd = Planetarium.GetUniversalTime() + time;
		}
		inactive = true;
		return result;
	}

	public bool CheckActive(double ut)
	{
		if (_inactive && ut >= inactiveTimeEnd)
		{
			inactive = false;
			return true;
		}
		return false;
	}

	public bool ChangeName(string newName)
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster != null)
		{
			_name = name;
			if (!HighLogic.CurrentGame.CrewRoster.ChangeNameCalledFromPCM(this, _name, newName))
			{
				return false;
			}
		}
		else
		{
			Debug.LogFormat("[ProtoCrewMember]: Cannot rename kerbal. No loaded Game or CrewRoster.");
		}
		GameEvents.onKerbalNameChange.Fire(this, _name, newName);
		string data = _name;
		_name = newName;
		GameEvents.onKerbalNameChanged.Fire(this, data, newName);
		return true;
	}

	public string GetLocalizedType()
	{
		return _type.Description();
	}

	public string GetLocalizedStatus()
	{
		return _rosterStatus.Description();
	}

	public string GetLocalizedTrait()
	{
		return KerbalRoster.GetLocalizedExperienceTraitName(trait);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", name);
		node.AddValue("gender", gender.ToString());
		node.AddValue("type", type);
		node.AddValue("trait", trait);
		node.AddValue("brave", courage);
		node.AddValue("dumb", stupidity);
		node.AddValue("badS", isBadass);
		node.AddValue("veteran", veteran);
		node.AddValue("tour", hasToured);
		node.AddValue("state", rosterStatus);
		node.AddValue("inactive", inactive);
		node.AddValue("inactiveTimeEnd", inactiveTimeEnd.ToString("G17"));
		node.AddValue("gExperienced", gExperienced.ToString("G17"));
		node.AddValue("outDueToG", outDueToG);
		node.AddValue("ToD", UTaR);
		node.AddValue("idx", seatIdx);
		node.AddValue("extraXP", extraExperience);
		node.AddValue("hasHelmetOn", hasHelmetOn);
		node.AddValue("hasNeckRingOn", hasNeckRingOn);
		node.AddValue("hasVisorDown", hasVisorDown);
		node.AddValue("lightR", lightR);
		node.AddValue("lightG", lightG);
		node.AddValue("lightB", lightB);
		node.AddValue("completedFirstEVA", completedFirstEVA);
		if (chuteNode != null)
		{
			node.AddNode(chuteNode);
		}
		if (inventoryNode != null)
		{
			node.AddNode(inventoryNode);
		}
		node.AddValue("suit", suit);
		node.AddValue("hero", isHero);
		if (suitTexturePath != null)
		{
			node.AddValue("suitTexturePath", suitTexturePath);
		}
		if (normalTexturePath != null)
		{
			node.AddValue("normalTexturePath", normalTexturePath);
		}
		if (spritePath != null)
		{
			node.AddValue("spritePath", spritePath);
		}
		if (comboId != null)
		{
			node.AddValue("comboId", comboId);
		}
		careerLog.Save(node.AddNode("CAREER_LOG"));
		flightLog.Save(node.AddNode("FLIGHT_LOG"));
		GameEvents.onProtoCrewMemberSave.Fire(new GameEvents.FromToAction<ProtoCrewMember, ConfigNode>(this, node));
	}

	public void OnGameStateCreated(Game game)
	{
		experience = CalculateExperiencePoints(game);
		experienceLevel = KerbalRoster.CalculateExperienceLevel(experience);
		GameEvents.onGameStateCreated.Remove(OnGameStateCreated);
	}

	public void SaveEVAChute(ModuleEvaChute chute)
	{
		if (chute != null)
		{
			chuteNode = new ConfigNode("EVACHUTE");
			chute.Save(chuteNode);
		}
	}

	public float InventoryCosts()
	{
		float result = 0f;
		if (type == KerbalType.Tourist)
		{
			return result;
		}
		if (kerbalModule != null)
		{
			result = kerbalModule.GetModuleCost(0f, ModifierStagingSituation.CURRENT);
		}
		return result;
	}

	public float InventoryMass()
	{
		float result = 0f;
		if (type == KerbalType.Tourist)
		{
			return result;
		}
		if (kerbalModule != null)
		{
			result = kerbalModule.GetModuleMass(0f, ModifierStagingSituation.CURRENT);
		}
		return result;
	}

	public float ResourceMass()
	{
		float num = 0f;
		if (type == KerbalType.Tourist)
		{
			return num;
		}
		if (cachedPart == null)
		{
			cachedPart = PartLoader.getPartInfoByName(GetKerbalEVAPartName()).partPrefab.gameObject.GetComponent<Part>();
		}
		if (cachedPart != null)
		{
			for (int i = 0; i < cachedPart.Resources.Count; i++)
			{
				num += (float)cachedPart.Resources[i].amount * cachedPart.Resources[i].info.density;
			}
		}
		return num;
	}

	public void SetDefaultInventory()
	{
		inventoryNode = new ConfigNode("INVENTORY");
		if (type == KerbalType.Tourist)
		{
			return;
		}
		if (cachedPrefabModule == null)
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(GetKerbalEVAPartName());
			if (partInfoByName != null)
			{
				cachedPrefabModule = partInfoByName.partPrefab.gameObject.GetComponent<ModuleInventoryPart>();
			}
		}
		if (cachedPrefabModule != null)
		{
			cachedPrefabModule.SaveDefault(inventoryNode);
		}
	}

	public void SaveInventory(ModuleInventoryPart inventory)
	{
		inventoryNode = new ConfigNode("INVENTORY");
		if (type != KerbalType.Tourist && inventory != null)
		{
			inventory.Save(inventoryNode);
		}
	}

	public void OnPartUnpack(Part p)
	{
		if (CallbackOnPartUnpack != null)
		{
			CallbackOnPartUnpack(this, p);
		}
	}

	public void OnPartPack(Part p)
	{
		if (CallbackOnPartPack != null)
		{
			CallbackOnPartPack(this, p);
		}
	}

	public static double _GToleranceMult(ProtoCrewMember pcm)
	{
		double num = 1.0 + (PhysicsGlobals.KerbalGBraveMult - 1.0) * (double)pcm.courage;
		GeeForceTolerance effect = pcm.GetEffect<GeeForceTolerance>();
		if (effect != null)
		{
			num *= (double)effect.GeeTolerance();
		}
		if (pcm.isBadass)
		{
			num *= PhysicsGlobals.KerbalGBadMult;
		}
		return num;
	}

	public static double _MaxSustainedG(ProtoCrewMember pcm)
	{
		return Math.Pow(PhysicsGlobals.KerbalGOffset * GToleranceMult(pcm), 1.0 / PhysicsGlobals.KerbalGPower);
	}

	public void ActiveFixedUpdate(Part p)
	{
		geeForce = p.vessel.geeForce;
		if (HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().GKerbalLimits)
		{
			double num = TimeWarp.fixedDeltaTime;
			double num2 = GToleranceMult(this) * (double)HighLogic.CurrentGame.Parameters.CustomParams<GameParameters.AdvancedParams>().KerbalGToleranceMult;
			if (doStockGCalcs)
			{
				gIncrement = (Math.Pow(Math.Min(geeForce, PhysicsGlobals.KerbalGClamp), PhysicsGlobals.KerbalGPower) - PhysicsGlobals.KerbalGOffset * num2) * num;
				if (gIncrement < 0.0)
				{
					gIncrement = Math.Pow(0.0 - gIncrement, PhysicsGlobals.KerbalGDecayPower);
					gIncrement = 0.0 - gIncrement;
				}
				gIncrement *= 1.0 + ((double)UnityEngine.Random.Range(0f, 1f) * 0.1 - 0.05);
				gExperienced += gIncrement;
			}
			else if (ApplyGCallback != null)
			{
				ApplyGCallback(this, p);
			}
			if (gExperienced < 0.0)
			{
				gExperienced = 0.0;
			}
			else if (gExperienced > PhysicsGlobals.KerbalGThresholdWarn * num2)
			{
				if (gExperienced > PhysicsGlobals.KerbalGThresholdLOC * num2)
				{
					if (_inactive)
					{
						if (PhysicsGlobals.KerbalGClampGExperienced)
						{
							SetInactive(UtilMath.Clamp(gIncrement, num, PhysicsGlobals.KerbalGLOCMaxTimeIncrement * num));
						}
						else
						{
							SetInactive(num);
						}
					}
					else
					{
						if (FlightGlobals.ActiveVessel == p.vessel)
						{
							ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_143389", name), 5f, ScreenMessageStyle.UPPER_CENTER);
						}
						outDueToG = true;
						SetInactive(PhysicsGlobals.KerbalGLOCBaseTime, additive: false);
						GameEvents.onKerbalPassedOutFromGeeForce.Fire(this);
					}
					if (PhysicsGlobals.KerbalGClampGExperienced)
					{
						gExperienced = PhysicsGlobals.KerbalGThresholdLOC * num2;
					}
				}
				else if (outDueToG)
				{
					SetInactive(num);
				}
				else if (FlightGlobals.ActiveVessel == p.vessel)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_143403", name), 3f, ScreenMessageStyle.UPPER_CENTER);
				}
			}
		}
		else
		{
			gExperienced = 0.0;
		}
		if (outDueToG && !_inactive)
		{
			outDueToG = false;
			if (FlightGlobals.ActiveVessel == p.vessel)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_143415", name), 3f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		if (CallbackActiveFixedUpdate != null)
		{
			CallbackActiveFixedUpdate(this, p);
		}
	}

	public void ActiveUpdate(Part p)
	{
		if (CallbackActiveUpdate != null)
		{
			CallbackActiveUpdate(this, p);
		}
	}

	public void Update(double double_0)
	{
		if (_inactive)
		{
			CheckActive(double_0);
		}
		if (CallbackUpdate != null)
		{
			CallbackUpdate(this, double_0);
		}
	}

	public static Kerbal _Spawn(ProtoCrewMember pcm)
	{
		if (pcm.KerbalRef != null)
		{
			return pcm.KerbalRef;
		}
		GameObject gameObject = null;
		string kerbalIVAName = pcm.GetKerbalIVAName();
		gameObject = (kerbalIVAName.Contains("Vintage") ? MissionsUtils.MEPrefab("Kerbals/IVA/" + kerbalIVAName + ".prefab") : (kerbalIVAName.Contains("Future") ? SerenityUtils.SerenityPrefab("Kerbals/IVA/" + kerbalIVAName + ".prefab") : ((!kerbalIVAName.Contains("slim")) ? AssetBase.GetPrefab(kerbalIVAName) : AssetBase.GetPrefab(kerbalIVAName))));
		if (gameObject == null)
		{
			pcm.suit = KerbalSuit.Default;
			gameObject = AssetBase.GetPrefab(pcm.GetKerbalIVAName());
		}
		Kerbal component = UnityEngine.Object.Instantiate(gameObject).GetComponent<Kerbal>();
		component.name = pcm.name;
		component.crewMemberName = pcm.name;
		component.stupidity = pcm.stupidity;
		component.courage = pcm.courage;
		component.isBadass = pcm.isBadass;
		component.veteran = pcm.veteran;
		component.protoCrewMember = pcm;
		component.rosterStatus = pcm.rosterStatus;
		component.showHelmet = pcm.hasHelmetOn;
		pcm.KerbalRef = component;
		return component;
	}

	public string GetKerbalEVAPartName()
	{
		Gender gender = this.gender;
		if (gender != 0 && gender == Gender.Female)
		{
			switch (suit)
			{
			case KerbalSuit.Vintage:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Vintage))
				{
					return "kerbalEVAfemaleVintage";
				}
				goto default;
			case KerbalSuit.Future:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Future))
				{
					return "kerbalEVAfemaleFuture";
				}
				goto default;
			default:
				return "kerbalEVAfemale";
			case KerbalSuit.Slim:
				return "kerbalEVASlimSuitFemale";
			}
		}
		switch (suit)
		{
		case KerbalSuit.Vintage:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Vintage))
			{
				return "kerbalEVAVintage";
			}
			goto default;
		case KerbalSuit.Future:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Future))
			{
				return "kerbalEVAFuture";
			}
			goto default;
		default:
			return "kerbalEVA";
		case KerbalSuit.Slim:
			return "kerbalEVASlimSuit";
		}
	}

	public string GetKerbalIVAName()
	{
		Gender gender = this.gender;
		if (gender != 0 && gender == Gender.Female)
		{
			switch (suit)
			{
			case KerbalSuit.Vintage:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Vintage))
				{
					return "kerbalFemaleVintage";
				}
				goto default;
			case KerbalSuit.Future:
				if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Future))
				{
					return "kerbalFemaleFuture";
				}
				goto default;
			default:
				return "kerbalFemale";
			case KerbalSuit.Slim:
				return "slimSuitIVAFemale";
			}
		}
		switch (suit)
		{
		case KerbalSuit.Vintage:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Vintage))
			{
				return "kerbalMaleVintage";
			}
			goto default;
		case KerbalSuit.Future:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Future))
			{
				return "kerbalMaleFuture";
			}
			goto default;
		default:
			return "kerbalMale";
		case KerbalSuit.Slim:
			return "slimSuitIVAMale";
		}
	}

	public string GetKerbalIconSuitSuffix()
	{
		switch (suit)
		{
		case KerbalSuit.Vintage:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Vintage))
			{
				return "_vintage";
			}
			goto default;
		case KerbalSuit.Future:
			if (ExpansionsLoader.IsExpansionKerbalSuitInstalled(KerbalSuit.Future))
			{
				return "_future";
			}
			goto default;
		default:
			return "";
		case KerbalSuit.Slim:
			return "_slim";
		}
	}

	public string GetKerbalIconSuitPath()
	{
		string text = "";
		if (SpritePath == null)
		{
			string kerbalIconSuitSuffix = GetKerbalIconSuitSuffix();
			Gender gender = this.gender;
			if (gender != 0 && gender == Gender.Female)
			{
				if (veteran)
				{
					return kerbalIconSuitSuffix switch
					{
						"_vintage" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_orange_female" + kerbalIconSuitSuffix, 
						"_future" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_orange_female" + kerbalIconSuitSuffix, 
						"_slim" => "kerbalicon_suit_orange_female" + kerbalIconSuitSuffix, 
						_ => "kerbalicon_suit_orange_female", 
					};
				}
				switch (type)
				{
				default:
					return kerbalIconSuitSuffix switch
					{
						"_vintage" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_female" + kerbalIconSuitSuffix, 
						"_future" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_female" + kerbalIconSuitSuffix, 
						"_slim" => "kerbalicon_suit_female" + kerbalIconSuitSuffix, 
						_ => "kerbalicon_suit_female", 
					};
				case KerbalType.Applicant:
					return "kerbalicon_recruit_female";
				case KerbalType.Unowned:
				case KerbalType.Tourist:
					return "kerbalicon_tourist_female";
				}
			}
			if (veteran)
			{
				return kerbalIconSuitSuffix switch
				{
					"_vintage" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_orange" + kerbalIconSuitSuffix, 
					"_future" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit_orange" + kerbalIconSuitSuffix, 
					"_slim" => "kerbalicon_suit_orange" + kerbalIconSuitSuffix, 
					_ => "kerbalicon_suit_orange", 
				};
			}
			switch (type)
			{
			default:
				return kerbalIconSuitSuffix switch
				{
					"_vintage" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit" + kerbalIconSuitSuffix, 
					"_future" => "Kerbals/Textures/kerbalIcons/kerbalicon_suit" + kerbalIconSuitSuffix, 
					"_slim" => "kerbalicon_suit" + kerbalIconSuitSuffix, 
					_ => "kerbalicon_suit", 
				};
			case KerbalType.Applicant:
				return "kerbalicon_recruit";
			case KerbalType.Unowned:
			case KerbalType.Tourist:
				return "kerbalicon_tourist";
			}
		}
		return spritePath;
	}

	public void Die()
	{
		flightLog.AddEntryUnique(FlightLog.EntryType.Die);
		ArchiveFlightLog();
		rosterStatus = RosterStatus.Dead;
		GameEvents.onCrewKilled.Fire(new EventReport(FlightEvents.CREW_KILLED, null, name));
		if ((bool)KerbalRef)
		{
			KerbalRef.die();
		}
		if (type == KerbalType.Tourist)
		{
			HighLogic.CurrentGame.CrewRoster.Remove(name);
		}
	}

	public void CheckRespawnTimer(double double_0, GameParameters gameParameters)
	{
		if (rosterStatus == RosterStatus.Missing && double_0 >= UTaR)
		{
			if (gameParameters.Difficulty.MissingCrewsRespawn)
			{
				Debug.Log("Crewmember " + name + " has respawned!");
				rosterStatus = RosterStatus.Available;
				flightLog.AddEntryUnique(FlightLog.EntryType.Spawn);
			}
			else
			{
				Debug.Log("Crewmember " + name + " has been missing for too long! Must be dead by now.");
				Die();
			}
		}
	}

	public void StartRespawnPeriod(double timeToRespawn = -1.0)
	{
		if (timeToRespawn < 0.0)
		{
			timeToRespawn = HighLogic.CurrentGame.Parameters.Difficulty.RespawnTimer;
		}
		SetTimeForRespawn(Planetarium.GetUniversalTime() + timeToRespawn);
	}

	public void SetTimeForRespawn(double UTforRespawn)
	{
		rosterStatus = RosterStatus.Missing;
		UTaR = UTforRespawn;
	}

	public void ArchiveFlightLog()
	{
		int i = 0;
		for (int count = flightLog.Count; i < count; i++)
		{
			careerLog.AddEntry(flightLog[i]);
		}
		careerLog.AddFlight();
		UpdateExperience();
		flightLog.Entries.Clear();
		flightLog.AddFlight();
	}

	public void UpdateExperience()
	{
		int num = experienceLevel;
		experience = CalculateExperiencePoints(HighLogic.CurrentGame);
		experienceLevel = KerbalRoster.CalculateExperienceLevel(experience);
		if (num != experienceLevel)
		{
			GameEvents.onKerbalLevelUp.Fire(this);
		}
	}

	public float CalculateExperiencePoints(Game game)
	{
		return CalculateExperiencePoints(game.Parameters, game.Mode);
	}

	public float CalculateExperiencePoints(GameParameters parameters, Game.Modes mode)
	{
		if (parameters.CustomParams<GameParameters.AdvancedParams>().KerbalExperienceEnabled(mode))
		{
			if (parameters.CustomParams<GameParameters.AdvancedParams>().ImmediateLevelUp)
			{
				return KerbalRoster.CalculateExperience(careerLog, flightLog);
			}
			return KerbalRoster.CalculateExperience(careerLog);
		}
		return 99999f;
	}

	public void RegisterExperienceTraits(Part part)
	{
		if (experienceTrait != null)
		{
			experienceTrait.Register(part);
		}
	}

	public void UnregisterExperienceTraits(Part part)
	{
		if (experienceTrait != null)
		{
			experienceTrait.Unregister(part);
		}
	}

	public bool HasEffect<T>() where T : ExperienceEffect
	{
		return GetEffect<T>() != null;
	}

	public bool HasEffect(string effect)
	{
		return GetEffect(effect) != null;
	}

	public T GetEffect<T>() where T : ExperienceEffect
	{
		int count = experienceTrait.Effects.Count;
		T val;
		do
		{
			if (count-- > 0)
			{
				val = experienceTrait.Effects[count] as T;
				continue;
			}
			return null;
		}
		while (val == null);
		return val;
	}

	public ExperienceEffect GetEffect(string effect)
	{
		int count = experienceTrait.Effects.Count;
		do
		{
			if (count-- <= 0)
			{
				return null;
			}
		}
		while (!(experienceTrait.Effects[count].Name == effect));
		return experienceTrait.Effects[count];
	}

	public override string ToString()
	{
		return _name;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ProtoCrewMember protoCrewMember))
		{
			return false;
		}
		if (_name.Equals(protoCrewMember._name) && stupidity.Equals(protoCrewMember.stupidity) && courage.Equals(protoCrewMember.courage) && isBadass.Equals(protoCrewMember.isBadass) && veteran.Equals(protoCrewMember.veteran) && hasToured.Equals(hasToured))
		{
			return UTaR.Equals(protoCrewMember.UTaR);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _name.GetHashCode_Net35() ^ stupidity.GetHashCode() ^ courage.GetHashCode() ^ isBadass.GetHashCode() ^ veteran.GetHashCode() ^ hasToured.GetHashCode() ^ UTaR.GetHashCode();
	}
}
