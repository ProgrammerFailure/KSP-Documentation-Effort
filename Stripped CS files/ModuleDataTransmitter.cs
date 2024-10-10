using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CommNet;
using Expansions.Missions.Adjusters;
using ns9;
using UnityEngine;

public class ModuleDataTransmitter : PartModule, IScienceDataTransmitter, IResourceConsumer, IContractObjectiveModule, ICommAntenna, IModuleInfo
{
	[KSPField]
	public AntennaType antennaType = AntennaType.DIRECT;

	[KSPField]
	public float packetInterval = 0.5f;

	[KSPField]
	public float packetSize = 1f;

	[KSPField(isPersistant = true)]
	public bool xmitIncomplete;

	[KSPField]
	public double packetResourceCost = 10.0;

	[KSPField]
	public int animationModuleIndex = -1;

	[KSPField(guiActive = true, guiName = "#autoLOC_6001428")]
	public string statusText = Localizer.Format("#autoLOC_6001415");

	[KSPField(guiActive = true, guiActiveEditor = true, guiName = "#autoLOC_6001429")]
	public string powerText = string.Empty;

	[KSPField]
	public double antennaPower = 500000.0;

	[KSPField]
	public DoubleCurve rangeCurve;

	[KSPField]
	public DoubleCurve scienceCurve;

	[KSPField]
	public bool antennaCombinable;

	[KSPField]
	public double antennaCombinableExponent = 0.75;

	public bool busy;

	public bool xmitAborted;

	public bool xmitOnHold;

	public float deployFxModuleStartPosition;

	public double capacitorCharge;

	public List<ScienceData> transmissionQueue;

	public int[] DeployFxModuleIndices;

	public List<IScalarModule> deployFxModules;

	public int[] ProgressFxModuleIndices;

	public List<IScalarModule> progressFxModules;

	public ScreenMessage statusMessage;

	public ScreenMessage progressMessage;

	public ScreenMessage errorMessage;

	public RnDCommsStream commStream;

	public List<PartResourceDefinition> consumedResources;

	public string errorStr = string.Empty;

	public List<string> tempAppliedUpgrades = new List<string>();

	public List<AdjusterDataTransmitterBase> adjusterCache = new List<AdjusterDataTransmitterBase>();

	public virtual float DataRate => packetSize * (1f / packetInterval);

	public virtual double DataResourceCost => packetResourceCost / (double)packetSize;

	public bool CommCombinable => antennaCombinable;

	public double CommCombinableExponent => antennaCombinableExponent;

	public AntennaType CommType => antennaType;

	public DoubleCurve CommRangeCurve => rangeCurve;

	public DoubleCurve CommScienceCurve => scienceCurve;

	public double CommPower => ApplyPowerAdjustments(antennaPower, adjusterCache);

	public List<PartResourceDefinition> GetConsumedResources()
	{
		return consumedResources;
	}

	public override void OnAwake()
	{
		transmissionQueue = new List<ScienceData>();
		rangeCurve = new DoubleCurve();
		rangeCurve.Add(0.0, 0.0, 0.0, 0.0);
		rangeCurve.Add(1.0, 1.0, 0.0, 0.0);
		scienceCurve = new DoubleCurve();
		scienceCurve.Add(0.0, 0.0, 0.0, 0.0);
		scienceCurve.Add(0.44999998807907104, 0.07999999821186066, 0.4951019, 0.4951019);
		scienceCurve.Add(0.8100000023841858, 0.3499999940395355, 0.8020515, 0.8020515);
		scienceCurve.Add(1.0, 0.4000000059604645, 0.0, 0.0);
		if (consumedResources == null)
		{
			consumedResources = new List<PartResourceDefinition>();
		}
		else
		{
			consumedResources.Clear();
		}
		int i = 0;
		for (int count = resHandler.inputResources.Count; i < count; i++)
		{
			consumedResources.Add(PartResourceLibrary.Instance.GetDefinition(resHandler.inputResources[i].name));
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("DeployFxModules"))
		{
			DeployFxModuleIndices = KSPUtil.ParseArray(node.GetValue("DeployFxModules"), int.Parse);
		}
		if (node.HasValue("ProgressFxModules"))
		{
			ProgressFxModuleIndices = KSPUtil.ParseArray(node.GetValue("ProgressFxModules"), int.Parse);
		}
		ConfigNode[] nodes = node.GetNodes("CommsData");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ConfigNode node2 = nodes[i];
			transmissionQueue.Add(new ScienceData(node2));
		}
		base.Events["TransmitIncompleteToggle"].guiName = (xmitIncomplete ? "#autoLOC_7001224" : "#autoLOC_7001225");
		UpdatePowerText();
		if (resHandler.inputResources.Count == 0 && base.part.partInfo == null)
		{
			string value = "ElectricCharge";
			node.TryGetValue("requiredResource", ref value);
			ModuleResource moduleResource = new ModuleResource();
			moduleResource.name = value;
			moduleResource.title = KSPUtil.PrintModuleName(value);
			moduleResource.id = value.GetHashCode();
			moduleResource.rate = 1.0;
			resHandler.inputResources.Add(moduleResource);
		}
	}

	public void UpdatePowerText()
	{
		powerText = KSPUtil.PrintSI(CommPower, string.Empty) + (antennaCombinable ? (" " + Localizer.Format("#autoLOC_236248")) : string.Empty);
	}

	public override void OnSave(ConfigNode node)
	{
		int i = 0;
		for (int count = transmissionQueue.Count; i < count; i++)
		{
			transmissionQueue[i].Save(node.AddNode("CommsData"));
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			node.AddValue("canComm", CanComm());
		}
	}

	public override void OnStart(StartState state)
	{
		deployFxModules = findFxModules(DeployFxModuleIndices, showUI: true);
		progressFxModules = findFxModules(ProgressFxModuleIndices, showUI: false);
		statusMessage = new ScreenMessage("", 4f, ScreenMessageStyle.UPPER_LEFT);
		progressMessage = new ScreenMessage("", 4f, ScreenMessageStyle.UPPER_LEFT);
		errorMessage = new ScreenMessage("", 4f, ScreenMessageStyle.UPPER_LEFT);
		if (transmissionQueue.Count == 0)
		{
			base.Actions["StartTransmissionAction"].active = moduleIsEnabled && antennaType != AntennaType.INTERNAL;
			base.Events["StartTransmission"].active = moduleIsEnabled && antennaType != AntennaType.INTERNAL;
			base.Events["StopTransmission"].active = false;
		}
		else
		{
			StartCoroutine(transmitQueuedData(packetInterval, packetSize));
		}
		base.Fields["statusText"].guiActive = CommNetScenario.CommNetEnabled && moduleIsEnabled;
		BaseField baseField = base.Fields["powerText"];
		bool guiActive = (base.Fields["powerText"].guiActiveEditor = CommNetScenario.CommNetEnabled && moduleIsEnabled);
		baseField.guiActive = guiActive;
		base.Events["TransmitIncompleteToggle"].active = moduleIsEnabled && antennaType != AntennaType.INTERNAL;
	}

	public virtual float GetVesselSignalStrength()
	{
		if (!CommNetScenario.CommNetEnabled)
		{
			return 1f;
		}
		if (!(base.vessel != null))
		{
			return 1f;
		}
		return (float)base.vessel.connection.ControlPath.signalStrength;
	}

	public virtual double EvaluateScienceMultiplier(double signalStrength)
	{
		if (scienceCurve == null)
		{
			return 1.0;
		}
		return scienceCurve.Evaluate(signalStrength);
	}

	public virtual List<IScalarModule> findFxModules(int[] indices, bool showUI)
	{
		if (indices != null)
		{
			List<IScalarModule> list = new List<IScalarModule>();
			int i = 0;
			for (int num = indices.Length; i < num; i++)
			{
				int num2 = indices[i];
				if (base.part.Modules[num2] is IScalarModule)
				{
					IScalarModule scalarModule = (IScalarModule)base.part.Modules[num2];
					scalarModule.SetUIWrite(showUI);
					scalarModule.SetUIRead(showUI);
					list.Add(scalarModule);
				}
				else
				{
					Debug.LogError("[TransmitterModule]: Part Module " + num2 + " doesn't implement IScalarModule", base.gameObject);
				}
			}
			return list;
		}
		return null;
	}

	[KSPAction("#autoLOC_6001430")]
	public virtual void StartTransmissionAction(KSPActionParam param)
	{
		StartTransmission();
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_6001430")]
	public virtual void StartTransmission()
	{
		if (!CanSetFXModules(deployFxModules))
		{
			errorMessage.message = Localizer.Format("#autoLOC_236367", base.part.partInfo.title);
			ScreenMessages.PostScreenMessage(errorMessage);
			return;
		}
		if (!CanTransmit())
		{
			errorMessage.message = Localizer.Format("#autoLOC_236374", base.part.partInfo.title);
			ScreenMessages.PostScreenMessage(errorMessage);
			return;
		}
		if (CommNetScenario.CommNetEnabled && base.vessel != null && base.vessel.connection != null)
		{
			double cost = base.vessel.connection.ControlPath.First.start[base.vessel.connection.ControlPath.First.end].cost;
			cost *= cost;
			double power = base.vessel.connection.ControlPath.First.end.antennaRelay.power;
			bool combined = ((base.vessel.connection.Comm.antennaRelay.power > base.vessel.connection.Comm.antennaTransmit.power) ? base.vessel.connection.Comm.antennaRelay.combined : base.vessel.connection.Comm.antennaTransmit.combined);
			if (!CanScienceTo(combined, power, cost))
			{
				errorMessage.message = Localizer.Format("#autoLOC_236387", base.part.partInfo.title);
				ScreenMessages.PostScreenMessage(errorMessage);
				return;
			}
		}
		PartItemTransfer.DismissActive();
		List<IScienceDataContainer> list = base.vessel.FindPartModulesImplementing<IScienceDataContainer>();
		int num = 0;
		List<IScienceDataContainer> list2 = new List<IScienceDataContainer>();
		int count = list.Count;
		while (count-- > 0)
		{
			IScienceDataContainer scienceDataContainer = list[count];
			if (!scienceDataContainer.IsRerunnable())
			{
				if (scienceDataContainer.GetScienceCount() > 0)
				{
					num++;
				}
			}
			else
			{
				list2.Add(scienceDataContainer);
			}
		}
		if (num > 0)
		{
			if (list2.Count > 0)
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog(Localizer.Format("#autoLOC_5050029", base.part.partInfo.title), Localizer.Format("#autoLOC_7001302", num.ToString()), Localizer.Format("#autoLOC_5050029", base.part.partInfo.title), HighLogic.UISkin, new DialogGUIButton<List<IScienceDataContainer>>(Localizer.Format("#autoLOC_7001303"), onStartTransmission, list), new DialogGUIButton<List<IScienceDataContainer>>(Localizer.Format("#autoLOC_7001304"), onStartTransmission, list2), new DialogGUIButton(Localizer.Format("#autoLOC_190323"), delegate
				{
				})), persistAcrossScenes: false, HighLogic.UISkin);
			}
			else
			{
				PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog(Localizer.Format("#autoLOC_5050029", base.part.partInfo.title), Localizer.Format("#autoLOC_7001302", num.ToString()), Localizer.Format("#autoLOC_5050029", base.part.partInfo.title), HighLogic.UISkin, new DialogGUIButton<List<IScienceDataContainer>>(Localizer.Format("#autoLOC_7001305"), onStartTransmission, list), new DialogGUIButton(Localizer.Format("#autoLOC_190323"), delegate
				{
				})), persistAcrossScenes: false, HighLogic.UISkin);
			}
		}
		else
		{
			onStartTransmission(list);
		}
	}

	public virtual void onStartTransmission(List<IScienceDataContainer> experiments)
	{
		transmissionQueue = queueVesselData(experiments);
		StartCoroutine(transmitQueuedData(packetInterval, packetSize));
	}

	public virtual List<ScienceData> queueVesselData(List<IScienceDataContainer> experiments)
	{
		List<ScienceData> list = new List<ScienceData>();
		int i = 0;
		for (int count = experiments.Count; i < count; i++)
		{
			IScienceDataContainer scienceDataContainer = experiments[i];
			ScienceData[] data = scienceDataContainer.GetData();
			int j = 0;
			for (int num = data.Length; j < num; j++)
			{
				ScienceData scienceData = data[j];
				if (scienceData != null)
				{
					scienceDataContainer.DumpData(scienceData);
					list.Add(scienceData);
				}
			}
		}
		return list;
	}

	public virtual IEnumerator transmitQueuedData(float transmitInterval, float dataPacketSize, Callback callback = null, bool sendData = true)
	{
		if (!CanSetFXModules(deployFxModules))
		{
			errorMessage.message = Localizer.Format("#autoLOC_236475", base.part.partInfo.title);
			ScreenMessages.PostScreenMessage(errorMessage);
			yield break;
		}
		base.Actions["StartTransmissionAction"].active = false;
		base.Events["StartTransmission"].active = false;
		base.Events["StopTransmission"].active = true;
		xmitOnHold = false;
		busy = true;
		statusMessage.message = "[" + base.part.partInfo.title + "]: " + Localizer.Format("#autoLOC_7003222");
		ScreenMessages.PostScreenMessage(statusMessage);
		SetFXModulesUI(deployFxModules, readState: true, writeState: false);
		deployFxModuleStartPosition = GetFxModuleScalar(deployFxModules);
		yield return StartCoroutine(SetFXModules(deployFxModules, 1f));
		SetFXModulesUI(deployFxModules, readState: false, writeState: false);
		while (transmissionQueue.Count > 0)
		{
			ScienceData data = transmissionQueue[0];
			transmissionQueue.RemoveAt(0);
			float totalData = data.dataAmount;
			int packetsToSend = Mathf.CeilToInt(totalData / dataPacketSize);
			int totalPackets = packetsToSend;
			if (ResearchAndDevelopment.Instance != null && sendData && !data.triggered)
			{
				ScienceSubject subjectByID = ResearchAndDevelopment.GetSubjectByID(data.subjectID);
				commStream = new RnDCommsStream(subjectByID, totalData, transmitInterval + 0.5f, data.baseTransmitValue * data.transmitBonus, data.scienceValueRatio, xmitIncomplete, ResearchAndDevelopment.Instance);
			}
			StartCoroutine(SetFXModules(progressFxModules, 0f));
			capacitorCharge = 0.0;
			while (packetsToSend > 0)
			{
				double num = packetResourceCost - capacitorCharge;
				capacitorCharge += num * resHandler.UpdateModuleResourceInputs(ref errorStr, num / (double)TimeWarp.fixedDeltaTime, 0.999, returnOnFirstLack: false, average: false, stringOps: true);
				if (capacitorCharge >= packetResourceCost * 0.95)
				{
					capacitorCharge = 0.0;
					float dataAmount = Mathf.Min(packetSize, totalData);
					totalData -= packetSize;
					xmitOnHold = false;
					packetsToSend--;
					statusText = Localizer.Format("#autoLOC_236534");
					StartCoroutine(SetFXModules(progressFxModules, (data.dataAmount - totalData) / data.dataAmount));
					if (packetsToSend % (int)TimeWarp.CurrentRate == 0)
					{
						progressMessage.message = Localizer.Format("#autoLOC_236540", base.part.partInfo.title) + " " + ((1f - (float)packetsToSend / (float)totalPackets) * 100f).ToString("0.00") + "%";
						ScreenMessages.PostScreenMessage(progressMessage);
					}
					if (commStream != null)
					{
						commStream.StreamData(dataAmount, base.vessel.protoVessel);
					}
				}
				else
				{
					if (!xmitOnHold)
					{
						xmitOnHold = true;
					}
					errorMessage.message = Localizer.Format("#autoLOC_236559", base.part.partInfo.title, errorStr);
					ScreenMessages.PostScreenMessage(errorMessage);
					statusText = Localizer.Format("#autoLOC_236562", capacitorCharge.ToString("0.0"), packetResourceCost.ToString("0.0"));
					if (!xmitIncomplete)
					{
						AbortTransmission(Localizer.Format("#autoLOC_236564"));
					}
				}
				yield return StartCoroutine(WaitForFixedSeconds(transmitInterval));
				if (xmitAborted)
				{
					ReturnDataToContainer(data);
					break;
				}
			}
			if (data.triggered)
			{
				GameEvents.OnTriggeredDataTransmission.Fire(data, base.vessel, xmitAborted);
			}
			if (transmissionQueue.Count > 0)
			{
				yield return new WaitForSeconds(transmitInterval * 2f);
			}
			if (xmitAborted)
			{
				break;
			}
		}
		if (!xmitAborted)
		{
			statusText = Localizer.Format("#autoLOC_236592");
			statusMessage.message = Localizer.Format("#autoLOC_236594", base.part.partInfo.title);
			ScreenMessages.PostScreenMessage(statusMessage);
			yield return new WaitForSeconds(3f);
		}
		commStream = null;
		SetFXModulesUI(deployFxModules, readState: true, writeState: false);
		StartCoroutine(SetFXModules(progressFxModules, 0f));
		yield return StartCoroutine(SetFXModules(deployFxModules, deployFxModuleStartPosition));
		SetFXModulesUI(deployFxModules, readState: true, writeState: true);
		busy = false;
		statusText = Localizer.Format("#autoLOC_236147");
		xmitAborted = false;
		base.Actions["StartTransmissionAction"].active = true;
		base.Events["StartTransmission"].active = true;
		base.Events["StopTransmission"].active = false;
		if (transmissionQueue.Count > 0)
		{
			StartCoroutine(transmitQueuedData(packetInterval, packetSize));
		}
	}

	public IEnumerator WaitForFixedSeconds(float seconds)
	{
		for (float curTime = 0f; curTime < seconds; curTime += TimeWarp.fixedDeltaTime)
		{
			yield return new WaitForFixedUpdate();
		}
	}

	public virtual bool CanSetFXModules(List<IScalarModule> modules)
	{
		int count = modules.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (modules[count].CanMove);
		return false;
	}

	public virtual float GetFxModuleScalar(List<IScalarModule> modules)
	{
		if (modules != null && modules.Count > 0)
		{
			if (modules[0].GetScalar < 0.5f)
			{
				return 0f;
			}
			return 1f;
		}
		return 0f;
	}

	public virtual float GetModulesScalarMax(List<IScalarModule> modules)
	{
		float num = 0f;
		int count = modules.Count;
		while (count-- > 0)
		{
			float getScalar = modules[count].GetScalar;
			if (getScalar > num)
			{
				num = getScalar;
			}
		}
		return num;
	}

	public virtual float GetModulesScalarMin(List<IScalarModule> modules)
	{
		float num = 1f;
		int count = modules.Count;
		while (count-- > 0)
		{
			float getScalar = modules[count].GetScalar;
			if (getScalar < num)
			{
				num = getScalar;
			}
		}
		return num;
	}

	public virtual IEnumerator SetFXModules(List<IScalarModule> modules, float tgtValue)
	{
		if (modules == null)
		{
			yield break;
		}
		bool allFXDone = false;
		while (!allFXDone)
		{
			allFXDone = true;
			int i = 0;
			for (int count = modules.Count; i < count; i++)
			{
				if (modules[i].CanMove && !(Mathf.Abs(modules[i].GetScalar - tgtValue) < 0.01f))
				{
					allFXDone = false;
					modules[i].SetScalar(tgtValue);
				}
			}
			if (!allFXDone)
			{
				yield return null;
			}
		}
	}

	public virtual void SetFXModulesUI(List<IScalarModule> fxModules, bool readState, bool writeState)
	{
		int i = 0;
		for (int count = fxModules.Count; i < count; i++)
		{
			fxModules[i].SetUIRead(readState);
			fxModules[i].SetUIWrite(writeState);
		}
	}

	[KSPEvent(guiActive = true, guiName = "#autoLOC_6001860")]
	public virtual void TransmitIncompleteToggle()
	{
		if (!xmitIncomplete)
		{
			errorMessage.message = Localizer.Format("#autoLOC_236732", base.part.partInfo.title);
			ScreenMessages.PostScreenMessage(errorMessage);
			xmitIncomplete = true;
			base.Events["TransmitIncompleteToggle"].guiName = "#autoLOC_7001224";
		}
		else
		{
			xmitIncomplete = false;
			base.Events["TransmitIncompleteToggle"].guiName = "#autoLOC_7001225";
		}
		if (commStream != null)
		{
			commStream.xmitIncomplete = xmitIncomplete;
		}
	}

	[KSPEvent(active = true, guiActive = true, guiName = "#autoLOC_7001226")]
	public virtual void StopTransmission()
	{
		AbortTransmission(Localizer.Format("#autoLOC_236748"));
	}

	public virtual void AbortTransmission(string message)
	{
		statusMessage.message = "[" + base.part.partInfo.title + "]: " + message;
		ScreenMessages.PostScreenMessage(statusMessage);
		statusText = Localizer.Format("#autoLOC_236755");
		xmitAborted = true;
		base.Events["StopTransmission"].active = false;
	}

	public virtual void ReturnDataToContainer(ScienceData data)
	{
		if (data == null)
		{
			return;
		}
		List<IScienceDataContainer> list = base.vessel.FindPartModulesImplementing<IScienceDataContainer>();
		int count = list.Count;
		PartModule partModule;
		do
		{
			if (count-- > 0)
			{
				partModule = list[count] as PartModule;
				continue;
			}
			return;
		}
		while (partModule == null || partModule.part == null || partModule.part.flightID != data.container);
		list[count].ReturnData(data);
	}

	public virtual bool CanTransmit()
	{
		if (moduleIsEnabled && antennaType != 0 && (!CommNetScenario.CommNetEnabled || base.vessel == null || base.vessel.connection == null || (base.vessel.connection.SignalStrength > 0.0 && base.vessel.connection.ControlPath.IsLastHopHome())))
		{
			if (deployFxModules != null && deployFxModules.Count != 0 && GetModulesScalarMin(deployFxModules) != 1f)
			{
				return CanSetFXModules(deployFxModules);
			}
			return true;
		}
		return false;
	}

	public virtual bool IsBusy()
	{
		return busy;
	}

	public virtual void TransmitData(List<ScienceData> dataQueue)
	{
		transmissionQueue.AddRange(dataQueue);
		if (!busy)
		{
			StartCoroutine(transmitQueuedData(packetInterval, packetSize));
		}
	}

	public virtual void TransmitData(List<ScienceData> dataQueue, Callback callback)
	{
		transmissionQueue.AddRange(dataQueue);
		if (!busy)
		{
			StartCoroutine(transmitQueuedData(packetInterval, packetSize, callback, sendData: false));
		}
	}

	public override string GetInfo()
	{
		string text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(antennaType.displayDescription());
		string text2 = "";
		string unitName = Localizer.Format("#autoLOC_7001411");
		text2 += Localizer.Format("#autoLOC_7001005", text);
		text2 += Localizer.Format("#autoLOC_7001006", powerText);
		text2 = text2 + Localizer.Format("#autoLOC_236834") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(0f)), unitName);
		text2 = text2 + Localizer.Format("#autoLOC_236835") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(0.5f)), unitName);
		text2 = text2 + Localizer.Format("#autoLOC_236836") + " " + KSPUtil.PrintSI(CommNetScenario.RangeModel.GetMaximumRange(antennaPower, GameVariables.Instance.GetDSNRange(1f)), unitName);
		text2 += "\n";
		if (antennaType != 0)
		{
			text2 += Localizer.Format("#autoLOC_236840", packetSize.ToString("0.0"));
			text2 += Localizer.Format("#autoLOC_236841", (packetSize / packetInterval).ToString("0.0###"));
			text2 += Localizer.Format("#autoLOC_236842");
			text2 += resHandler.PrintModuleResources(packetResourceCost / (double)packetInterval);
		}
		else
		{
			text2 += Localizer.Format("#autoLOC_236846");
		}
		if (!moduleIsEnabled)
		{
			text2 += Localizer.Format("#autoLOC_236849");
		}
		return text2;
	}

	public virtual string GetContractObjectiveType()
	{
		return "Antenna";
	}

	public virtual bool CheckContractObjectiveValidity()
	{
		return antennaType != AntennaType.INTERNAL;
	}

	public bool CanScienceTo(bool combined, double bPower, double sqrDistance)
	{
		if (!(antennaCombinable && combined))
		{
			return CommNetScenario.RangeModel.InRange(CommPower, bPower, sqrDistance);
		}
		return true;
	}

	public virtual bool CanComm()
	{
		if ((!moduleIsEnabled || deployFxModules != null) && deployFxModules.Count != 0 && GetModulesScalarMin(deployFxModules) != 1f)
		{
			return false;
		}
		return !IsDataTransmitterBroken(adjusterCache);
	}

	public virtual bool CanCommUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		if (mSnap == null)
		{
			return true;
		}
		if (mSnap != null && IsDataTransmitterBroken(mSnap.GetListOfActiveAdjusters<AdjusterDataTransmitterBase>()))
		{
			return false;
		}
		int count = mSnap.moduleValues.values.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (!(mSnap.moduleValues.values[count].name == "canComm"));
		return mSnap.moduleValues.values[count].value != "False";
	}

	public double CommPowerUnloaded(ProtoPartModuleSnapshot mSnap)
	{
		double value = antennaPower;
		ConfigNode node;
		if (mSnap != null && (node = mSnap.moduleValues.GetNode("UPGRADESAPPLIED")) != null)
		{
			LoadUpgradesApplied(tempAppliedUpgrades, node);
			ConfigNode configNode = new ConfigNode();
			ApplyUpgradeNode(tempAppliedUpgrades, configNode, doLoad: false);
			configNode.TryGetValue("antennaPower", ref value);
		}
		if (mSnap != null)
		{
			value = ApplyPowerAdjustments(value, mSnap.GetListOfActiveAdjusters<AdjusterDataTransmitterBase>());
		}
		return value;
	}

	public override void OnModuleAdjusterAdded(AdjusterPartModuleBase adjuster)
	{
		if (adjuster is AdjusterDataTransmitterBase item)
		{
			adjusterCache.Add(item);
		}
		base.OnModuleAdjusterAdded(adjuster);
	}

	public override void OnModuleAdjusterRemoved(AdjusterPartModuleBase adjuster)
	{
		AdjusterDataTransmitterBase item = adjuster as AdjusterDataTransmitterBase;
		adjusterCache.Remove(item);
		base.OnModuleAdjusterRemoved(adjuster);
	}

	public double ApplyPowerAdjustments(double power, List<AdjusterDataTransmitterBase> adjusterList)
	{
		for (int i = 0; i < adjusterList.Count; i++)
		{
			power = adjusterList[i].ApplyPowerAdjustment(power);
		}
		return power;
	}

	public bool IsDataTransmitterBroken(List<AdjusterDataTransmitterBase> adjusterList)
	{
		int num = 0;
		while (true)
		{
			if (num < adjusterList.Count)
			{
				if (adjusterList[num].IsDataTransmitterBroken())
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public string GetModuleTitle()
	{
		return "Data Transmitter";
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return null;
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLoc_6003034");
	}
}
