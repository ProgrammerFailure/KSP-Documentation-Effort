using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

public class ShipConstruction
{
	[CompilerGenerated]
	private sealed class _003CLoadThumbnail_003Ed__55 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string url;

		public Texture2D tex;

		private UnityWebRequest _003CtexLoad_003E5__2;

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
		public _003CLoadThumbnail_003Ed__55(int _003C_003E1__state)
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
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public static List<ConfigNode> backups;

	public static ConfigNode ShipConfig;

	public static VesselCrewManifest ShipManifest;

	public static EditorFacility ShipType;

	private const string saveFolder = "saves";

	private const string folderSeparator = "/";

	private const string shipsFolder = "Ships";

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ShipConstruction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ShipConstruction()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ClearBackups()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateBackup(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void ShiftAndCreateBackup(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ShipConstruct RestoreBackup(int index)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void DebugBackup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ShipConstruct LoadShip(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ShipConstruct LoadShip()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveShip(ShipConstruct ship, string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveShip(string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveShipToPath(string shipName, string path)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveShipToPath(string gameFolder, EditorFacility editorFacility, string localPath, string shipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPlayerCraftThumbnailName(string fullpath, string craftFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPlayerCraftThumbnailName(string gameName, string subDirectoryPath, string shipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetSavePath(string shipName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetCurrentGameShipsPathFor(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetShipsPathFor(string gameName, EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetShipsPathFor(string gameName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetRootCraftSavePath()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static EditorFacility CheckCraftFileType(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetShipsSubfolderFor(EditorFacility facility)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void AssembleForLaunch(ShipConstruct ship, string landedAt, string displaylandedAt, string flagURL, Game sceneState, VesselCrewManifest crewManifest)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Vessel AssembleForLaunch(ShipConstruct ship, string landedAt, string displaylandedAt, string flagURL, Game sceneState, VesselCrewManifest crewManifest, bool fromShipAssembly, bool setActiveVessel, bool isLanded, bool preCreate, Orbit orbit, bool orbiting, bool isSplashed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static uint GetUniqueFlightID(FlightState flightState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part findFirstCrewablePart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part findFirstPod_Placeholder(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part findFirstControlSource(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void PutShipToGround(ShipConstruct ship, Transform spawnPoint)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 CalculateCraftSize(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 CalculateCraftSize(List<Part> parts, Part rootPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 CalculateCraftSize(ShipTemplate ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 FindCraftCenter(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 FindCraftCenter(ShipConstruct ship, bool excludeClamps)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 FindCraftMOI(Part rootPart, Vector3 CoM)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Vector3 FindCraftMOI(Part part, Vector3 CoM, Vector3 MOI)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use FindVesselsAtLaunchSite and RecoverVesselFromFlight instead")]
	public static bool CheckLaunchSiteClear(FlightState flightState, string launchSiteName, bool doCleanup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use FindVesselsAtLaunchSite and RecoverVesselFromFlight instead")]
	public static bool CheckLaunchSiteClear(FlightState flightState, string launchSiteName, bool doCleanup, out int firstVesselOnPadIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<ProtoVessel> FindVesselsLandedAt(FlightState flightState, string landedAt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void FindVesselsLandedAt(FlightState flightState, string landedAt, out int count, out string name, out int idx, out VesselType vType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[Obsolete("Use FindVesselsLandedAt instead.")]
	public static List<ProtoVessel> FindVesselsAtLaunchSite(FlightState flightState, string launchSiteName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RecoverVesselFromFlight(ProtoVessel vessel, FlightState fromState)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RecoverVesselFromFlight(ProtoVessel vessel, FlightState fromState, bool quick)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void AutoGenerateThumbnail(ShipConstruct newShip, string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool ThumbnailIsOutdated(string filepath, string thumbPath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CaptureStockThumbnail(ShipConstruct ship, string craftPath, bool expansion)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CaptureThumbnail(ShipConstruct ship, string baseFolder, string thumbURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D GetThumbnail(string thumbURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Texture2D GetThumbnail(string thumbURL, bool fullPath, bool addFileExt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Texture2D GetThumbnail(string thumbURL, bool fullPath, bool addFileExt, FileInfo fInfo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadThumbnail_003Ed__55))]
	internal static IEnumerator LoadThumbnail(Texture2D tex, string url)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ShipTemplate LoadTemplate(string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveSubassembly(ShipConstruct ship, string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool SubassemblyExists(string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ShipConstruct LoadSubassembly(string shipFilename)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D CreateSubassemblyIcon(ShipConstruct sa, int size)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Texture2D RenderCamera(Camera cam, int width, int height, int depth, RenderTextureReadWrite rtReadWrite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void StripPartComponents(GameObject newPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Bounds GetSABounds(GameObject sc)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void SetLayerRecursively(GameObject root, int layer)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CreateConstructFromTemplate(ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnPipelineFailed(KSPUpgradePipeline.UpgradeFailOption opt, ConfigNode n, ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void OnPipelineFinished(ConfigNode node, ShipTemplate template, Callback<ShipConstruct> onComplete)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SanitizeCraftIDs(List<Part> parts, bool preserveIDsOnGivenParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetPartCosts(ProtoPartSnapshot protoPart, AvailablePart aP, out float dryCost, out float fuelCost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetPartCosts(ProtoPartSnapshot protoPart, bool includeModuleCosts, AvailablePart aP, out float dryCost, out float fuelCost)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static float GetPartCostsAndMass(ConfigNode partNode, AvailablePart aP, out float dryCost, out float fuelCost, out float dryMass, out float fuelMass)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SanitizePartCosts(AvailablePart aP, ConfigNode partNode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Part FindPartWithCraftID(uint craftID)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool AllPartsFound(ConfigNode root, ref string error)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsCompatible(ConfigNode root, ref string error)
	{
		throw null;
	}
}
