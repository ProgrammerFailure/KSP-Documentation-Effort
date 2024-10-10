namespace ns25;

public class ScreenDatabaseAudio : ScreenDatabaseList
{
	public override void LoadDatabaseListItems()
	{
		int i = 0;
		for (int count = GameDatabase.Instance.databaseAudio.Count; i < count; i++)
		{
			textMeshQueue.AddLine(GameDatabase.Instance.databaseAudio[i].name);
		}
	}
}
