namespace ns25;

public class ScreenDatabaseTextures : ScreenDatabaseList
{
	public override void LoadDatabaseListItems()
	{
		int i = 0;
		for (int count = GameDatabase.Instance.databaseTexture.Count; i < count; i++)
		{
			textMeshQueue.AddLine(GameDatabase.Instance.databaseTexture[i].name);
		}
	}
}
