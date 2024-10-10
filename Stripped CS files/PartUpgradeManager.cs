using UnityEngine;

[KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[]
{
	GameScenes.EDITOR,
	GameScenes.SPACECENTER,
	GameScenes.FLIGHT
})]
public class PartUpgradeManager : ScenarioModule
{
	public static PartUpgradeManager instance;

	public static PartUpgradeHandler handler;

	public static PartUpgradeManager Instance => instance;

	public static PartUpgradeHandler Handler
	{
		get
		{
			if (handler == null)
			{
				handler = new PartUpgradeHandler();
			}
			return handler;
		}
		set
		{
			handler = value;
		}
	}

	public override void OnAwake()
	{
		if (instance != null && instance != this)
		{
			Object.Destroy(instance);
		}
		instance = this;
	}

	public virtual void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		Handler.OnLoad(node.GetNode("UPGRADES"));
	}

	public override void OnSave(ConfigNode node)
	{
		handler.OnSave(node.AddNode("UPGRADES"));
	}
}
