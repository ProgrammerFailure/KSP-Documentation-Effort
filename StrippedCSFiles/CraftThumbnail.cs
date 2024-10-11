using System.Runtime.CompilerServices;
using UnityEngine;

public static class CraftThumbnail
{
	private static float camFov;

	private static float camDist;

	private static Camera snapshotCamera;

	private static RenderTexture renderTexture;

	private static Texture2D thumbTexture;

	public static EventData<ShipConstruct, string, byte[]> OnSnapshotCapture;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static CraftThumbnail()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void CreateRenderTextures(int resolution)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void TakeStockSnaphot(ShipConstruct ship, int resolution, string facilityName, bool expansion, string craftPath, float elevation = 45f, float azimuth = 45f, float pitch = 45f, float hdg = 45f, float fovFactor = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void TakeSnaphot(ShipConstruct ship, int resolution, string folderPath, string craftName, float elevation = 45f, float azimuth = 45f, float pitch = 45f, float hdg = 45f, float fovFactor = 1f)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D TakePartSnaphot(string partName, StoredPart sPart, int resolution, string folderPath, out string fullFileName, float elevation = 15f, float azimuth = 25f, float pitch = 15f, float hdg = 25f, float fovFactor = 1f, int variantIndex = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D TakePartSnapshot(string partName, StoredPart sPart, Part part, int resolution, string folderPath, out string fullFileName, float elevation = 15f, float azimuth = 25f, float pitch = 15f, float hdg = 25f, float fovFactor = 1f, int variantIndex = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetPartIconTexturePath(Part part, out int variantIdx)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IThumbnailSetup GetThumbNailSetupIface(AvailablePart availablePart)
	{
		throw null;
	}
}
