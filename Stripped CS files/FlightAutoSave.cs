using System.Collections;
using ns9;
using UnityEngine;

public class FlightAutoSave : MonoBehaviour
{
	public static FlightAutoSave fetch;

	public bool bypassAutoSave;

	public bool lastSaveSuccessful;

	public void Awake()
	{
		if ((bool)fetch)
		{
			Object.Destroy(this);
		}
		else
		{
			fetch = this;
		}
	}

	public void OnDestroy()
	{
		if (fetch != null && fetch == this)
		{
			fetch = null;
		}
	}

	[ContextMenu("Start")]
	public void Start()
	{
		if (!bypassAutoSave && HighLogic.CurrentGame.Parameters.Flight.CanAutoSave)
		{
			StartCoroutine(PersistentSave());
		}
	}

	public IEnumerator PersistentSave()
	{
		lastSaveSuccessful = true;
		while (base.gameObject.activeInHierarchy && HighLogic.CurrentGame.Parameters.Flight.CanAutoSave)
		{
			yield return new WaitForSeconds(lastSaveSuccessful ? GameSettings.AUTOSAVE_INTERVAL : GameSettings.AUTOSAVE_SHORT_INTERVAL);
			lastSaveSuccessful = false;
			switch (FlightGlobals.ClearToSave())
			{
			case ClearToSaveStatus.CLEAR:
				if (FlightGlobals.ActiveVessel.state == Vessel.State.DEAD)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_133393"), 2f);
					break;
				}
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_133397"), 2f);
				GamePersistence.SaveGame("persistent", HighLogic.SaveFolder, SaveMode.BACKUP);
				lastSaveSuccessful = true;
				MonoBehaviour.print("[AutoSave]: " + ((GameSettings.SAVE_BACKUPS > 0) ? "Game Backed Up and Saved" : "Game Saved"));
				break;
			}
		}
	}
}
