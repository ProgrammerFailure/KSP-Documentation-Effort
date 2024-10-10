using UnityEngine;

namespace CommNet;

[KSPScenario((ScenarioCreationOptions)3198, new GameScenes[]
{
	GameScenes.FLIGHT,
	GameScenes.TRACKSTATION,
	GameScenes.SPACECENTER,
	GameScenes.EDITOR
})]
public class CommNetScenario : ScenarioModule
{
	public static IRangeModel _rangeModel;

	public CommNetNetwork network;

	public CommNetUI ui;

	public static CommNetScenario Instance { get; set; }

	public static IRangeModel RangeModel
	{
		get
		{
			if (_rangeModel == null)
			{
				_rangeModel = new CommRangeModel();
			}
			return _rangeModel;
		}
		set
		{
			_rangeModel = value;
		}
	}

	public static bool CommNetEnabled
	{
		get
		{
			if (HighLogic.CurrentGame == null)
			{
				return false;
			}
			return HighLogic.CurrentGame.Parameters.Difficulty.EnableCommNet;
		}
	}

	public override void OnAwake()
	{
		if (!CommNetEnabled)
		{
			Object.Destroy(this);
			return;
		}
		if (Instance != null && Instance != this)
		{
			Debug.LogError("[CommNetScenario]: Instance already exists!", Instance.gameObject);
			Object.Destroy(Instance);
		}
		Instance = this;
	}

	public virtual void Start()
	{
		ui = base.gameObject.AddComponent<CommNetUI>();
		network = base.gameObject.AddComponent<CommNetNetwork>();
	}

	public void OnDestroy()
	{
		if (network != null)
		{
			Object.Destroy(network);
		}
		if (ui != null)
		{
			Object.Destroy(ui);
		}
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public override void OnLoad(ConfigNode gameNode)
	{
		if (!(Instance != this))
		{
			base.OnLoad(gameNode);
		}
	}

	public override void OnSave(ConfigNode gameNode)
	{
		if (!(Instance != this))
		{
			base.OnSave(gameNode);
		}
	}
}
