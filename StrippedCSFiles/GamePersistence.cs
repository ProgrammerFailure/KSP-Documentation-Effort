using System.IO;
using System.Runtime.CompilerServices;

public class GamePersistence
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public GamePersistence()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static string SaveGame(string saveFileName, string saveFolder, SaveMode saveMode, GameScenes startScene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveGame(string saveFileName, string saveFolder, SaveMode saveMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveGame(Game game, string saveFileName, string saveFolder, SaveMode saveMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int saveCompareToDate(FileInfo a, FileInfo b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string SaveGame(GameBackup game, string saveFileName, string saveFolder, SaveMode saveMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ConfigNode LoadSFSFile(string filename, string saveFolder)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Game LoadGameCfg(ConfigNode node, string saveName, bool nullIfIncompatible, bool suppressIncompatibleMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Game LoadGame(string filename, string saveFolder, bool nullIfIncompatible, bool suppressIncompatibleMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool UpdateScenarioModules(Game game)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Game CreateNewGame(string name, Game.Modes mode, GameParameters parameters, string flagURL, GameScenes startScene, EditorFacility editorFacility)
	{
		throw null;
	}
}
