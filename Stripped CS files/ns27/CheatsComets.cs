using SentinelMission;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns27;

public class CheatsComets : MonoBehaviour
{
	public Button KillButton;

	public Button genRandomButton;

	public Button genShortButton;

	public Button genMidButton;

	public Button genLongButton;

	public Button genInterstellarButton;

	public Button sentinelBackButton;

	public TextMeshProUGUI sentinelNameText;

	public Button sentinelForwardButton;

	public Button genSentinelButton;

	public Button genSentinelShortButton;

	public Button genSentinelMidButton;

	public Button genSentinelLongButton;

	public Button genSentinelInterButton;

	public Toggle collidersEnabled;

	public Toggle geysersEnabled;

	public Toggle dustsEnabled;

	public Toggle comaEnabled;

	public Toggle ionTailEnabled;

	public Toggle dustTailEnabled;

	public Slider geyserEmitterMultiplier;

	public TextMeshProUGUI geyserMultiplierText;

	public Slider snowEmitterMultiplier;

	public TextMeshProUGUI snowMultiplierText;

	public Slider ionEmitterMultiplier;

	public TextMeshProUGUI ionMultiplierText;

	public Slider dustEmitterMultiplier;

	public TextMeshProUGUI dustMultiplierText;

	public Slider dustSizeMultiplier;

	public TextMeshProUGUI dustSizeMultiplierText;

	public Button rePrimeParticles;

	public int selectedSentinel;

	public SentinelScenario sentinelScenario;

	public float gametime;

	public Vessel SelectedSentinel
	{
		get
		{
			if (sentinelScenario != null && sentinelScenario.deployedSentinels.Count > selectedSentinel)
			{
				return sentinelScenario.deployedSentinels[selectedSentinel];
			}
			return null;
		}
	}

	public void Awake()
	{
		KillButton.onClick.AddListener(OnKillClick);
		genRandomButton.onClick.AddListener(OnRandomClick);
		genShortButton.onClick.AddListener(OnShortClick);
		genMidButton.onClick.AddListener(OnMidClick);
		genLongButton.onClick.AddListener(OnLongClick);
		genInterstellarButton.onClick.AddListener(OnInterstellarClick);
		sentinelBackButton.onClick.AddListener(OnVesselBackClick);
		sentinelForwardButton.onClick.AddListener(OnVesselForwardClick);
		genSentinelButton.onClick.AddListener(OnSentinelClick);
		genSentinelShortButton.onClick.AddListener(OnSentinelShortClick);
		genSentinelMidButton.onClick.AddListener(OnSentinelMidClick);
		genSentinelLongButton.onClick.AddListener(OnSentinelLongClick);
		genSentinelInterButton.onClick.AddListener(OnSentinelInterstellarClick);
		collidersEnabled.onValueChanged.AddListener(OnCollidersClick);
		geysersEnabled.onValueChanged.AddListener(OnGeysersClick);
		dustsEnabled.onValueChanged.AddListener(OnDustClick);
		comaEnabled.onValueChanged.AddListener(OnComaClick);
		ionTailEnabled.onValueChanged.AddListener(OnIonTailClick);
		dustTailEnabled.onValueChanged.AddListener(OnDustTailClick);
		geyserEmitterMultiplier.onValueChanged.AddListener(OnGeyserMultiplierChange);
		snowEmitterMultiplier.onValueChanged.AddListener(OnSnowMultiplierChange);
		ionEmitterMultiplier.onValueChanged.AddListener(OnIonMultiplierChange);
		dustEmitterMultiplier.onValueChanged.AddListener(OnDustMultiplierChange);
		dustSizeMultiplier.onValueChanged.AddListener(OnDustSizeMultiplierChange);
		rePrimeParticles.onClick.AddListener(OnReprimeParticles);
	}

	public void Start()
	{
		sentinelScenario = SentinelScenario.Instance;
		if (sentinelScenario != null)
		{
			selectedSentinel = 0;
		}
		UpdateSentinelOptions();
		gametime = Time.realtimeSinceStartup;
	}

	public void Update()
	{
		if (sentinelScenario != null)
		{
			gametime += Time.unscaledDeltaTime;
			if (gametime > 3f)
			{
				gametime = 3f;
				UpdateSentinelOptions();
			}
		}
	}

	public void UpdateSentinelOptions()
	{
		sentinelBackButton.gameObject.SetActive(sentinelScenario != null);
		sentinelNameText.gameObject.SetActive(sentinelScenario != null);
		sentinelForwardButton.gameObject.SetActive(sentinelScenario != null);
		genSentinelButton.gameObject.SetActive(sentinelScenario != null);
		SetSelectedSentinelText();
	}

	public void OnKillClick()
	{
		if (FlightGlobals.fetch == null)
		{
			return;
		}
		int count = FlightGlobals.Vessels.Count;
		while (count-- > 0)
		{
			if (FlightGlobals.Vessels[count].Comet != null)
			{
				FlightGlobals.Vessels[count].Die();
			}
		}
	}

	public void OnRandomClick()
	{
		if (!(ScenarioDiscoverableObjects.Instance == null))
		{
			ScenarioDiscoverableObjects.Instance.SpawnComet();
		}
	}

	public void OnShortClick()
	{
		if (!(ScenarioDiscoverableObjects.Instance == null))
		{
			ScenarioDiscoverableObjects.Instance.SpawnComet("short");
		}
	}

	public void OnMidClick()
	{
		if (!(ScenarioDiscoverableObjects.Instance == null))
		{
			ScenarioDiscoverableObjects.Instance.SpawnComet("intermediate");
		}
	}

	public void OnLongClick()
	{
		if (!(ScenarioDiscoverableObjects.Instance == null))
		{
			ScenarioDiscoverableObjects.Instance.SpawnComet("long");
		}
	}

	public void OnInterstellarClick()
	{
		if (!(ScenarioDiscoverableObjects.Instance == null))
		{
			ScenarioDiscoverableObjects.Instance.SpawnComet("interstellar");
		}
	}

	public void OnSentinelClick()
	{
		if (!(sentinelScenario == null))
		{
			sentinelScenario.SpawnComet(SelectedSentinel);
		}
	}

	public void OnSentinelShortClick()
	{
		if (!(sentinelScenario == null))
		{
			sentinelScenario.SpawnComet(SelectedSentinel, "short");
		}
	}

	public void OnSentinelMidClick()
	{
		if (!(sentinelScenario == null))
		{
			sentinelScenario.SpawnComet(SelectedSentinel, "intermediate");
		}
	}

	public void OnSentinelLongClick()
	{
		if (!(sentinelScenario == null))
		{
			sentinelScenario.SpawnComet(SelectedSentinel, "long");
		}
	}

	public void OnSentinelInterstellarClick()
	{
		if (!(sentinelScenario == null))
		{
			sentinelScenario.SpawnComet(SelectedSentinel, "interstellar");
		}
	}

	public void OnVesselBackClick()
	{
		if (!(sentinelScenario == null))
		{
			selectedSentinel--;
			if (selectedSentinel < 0)
			{
				selectedSentinel = sentinelScenario.deployedSentinels.Count - 1;
			}
			SetSelectedSentinelText();
		}
	}

	public void OnVesselForwardClick()
	{
		if (!(sentinelScenario == null))
		{
			selectedSentinel++;
			if (selectedSentinel >= sentinelScenario.deployedSentinels.Count)
			{
				selectedSentinel = 0;
			}
			SetSelectedSentinelText();
		}
	}

	public void SetSelectedSentinelText()
	{
		Vessel vessel = SelectedSentinel;
		sentinelNameText.text = ((vessel != null) ? vessel.GetDisplayName() : string.Empty);
	}

	public void OnCollidersClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetCollider(value);
		}
	}

	public void OnGeysersClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetGeyser(value, geyserEmitterMultiplier.value / 100f);
		}
	}

	public void OnDustClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetDust(value, snowEmitterMultiplier.value / 100f);
		}
	}

	public void OnComaClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetComa(value);
		}
	}

	public void OnIonTailClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetIonTail(value, ionEmitterMultiplier.value / 100f);
		}
	}

	public void OnDustTailClick(bool value)
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.SetDustTail(value, dustEmitterMultiplier.value / 100f, dustSizeMultiplier.value / 100f);
		}
	}

	public void OnGeyserMultiplierChange(float value)
	{
		if (CometManager.Instance != null)
		{
			geyserMultiplierText.text = $"{value:0}%";
			CometManager.Instance.SetGeyser(geysersEnabled.isOn, value / 100f);
		}
	}

	public void OnSnowMultiplierChange(float value)
	{
		if (CometManager.Instance != null)
		{
			snowMultiplierText.text = $"{value:0}%";
			CometManager.Instance.SetDust(dustsEnabled.isOn, value / 100f);
		}
	}

	public void OnDustMultiplierChange(float value)
	{
		if (CometManager.Instance != null)
		{
			dustMultiplierText.text = $"{value:0}%";
			CometManager.Instance.SetDustTail(dustTailEnabled.isOn, value / 100f, dustSizeMultiplier.value / 100f);
		}
	}

	public void OnIonMultiplierChange(float value)
	{
		if (CometManager.Instance != null)
		{
			ionMultiplierText.text = $"{value:0}%";
			CometManager.Instance.SetIonTail(ionTailEnabled.isOn, value / 100f);
		}
	}

	public void OnDustSizeMultiplierChange(float value)
	{
		if (CometManager.Instance != null)
		{
			dustSizeMultiplierText.text = $"{value:0}%";
			CometManager.Instance.SetDustTail(dustTailEnabled.isOn, dustEmitterMultiplier.value / 100f, value / 100f);
		}
	}

	public void OnReprimeParticles()
	{
		if (CometManager.Instance != null)
		{
			CometManager.Instance.ReprimeParticles();
		}
	}
}
