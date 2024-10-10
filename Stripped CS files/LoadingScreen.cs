using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[Serializable]
	public class LoadingScreenState
	{
		public UnityEngine.Object[] screens;

		public float fadeInTime = 2f;

		public float displayTime = 3f;

		public float fadeOutTime = 3f;

		public string[] tips;

		public float tipTime = 15f;

		[HideInInspector]
		public Texture2D activeScreen;
	}

	public static GameScenes loadScene = GameScenes.PSYSTEM;

	public static float minFrameTime = 0f;

	public List<LoadingSystem> loaders = new List<LoadingSystem>();

	[SerializeField]
	public List<LoadingScreenState> screens = new List<LoadingScreenState>();

	public AspectRatioFitter aspectFitter;

	public GameObject screenInstance;

	[SerializeField]
	[Tooltip("The index of the list inside screens that contains the randomized loading screens. This list is the one that will have user screens added to it")]
	public int loadingScreensListIndex = 3;

	public string userScreensPath = "";

	public bool addedCustomScreens;

	public string[] customScreenExtensions = new string[2] { ".png", ".jpg" };

	[SerializeField]
	[Tooltip("How many frames to wait for isDone from the uwr if the request is finished, but the content is not done yet")]
	public int loadingScreenIsDoneRetryLimit = 30;

	public string[] filePaths;

	public FileInfo fileInfo;

	[SerializeField]
	public ProgressBar scrollBar;

	[SerializeField]
	public TextMeshProUGUI scrollBarText;

	[SerializeField]
	public TextMeshProUGUI scrollBarTextMasked;

	[SerializeField]
	public RawImage screenImage;

	[SerializeField]
	public CanvasGroup screenMask;

	[SerializeField]
	public TextMeshProUGUI tipsText;

	public static LoadingScreen Instance { get; set; }

	public List<LoadingScreenState> Screens
	{
		get
		{
			return screens;
		}
		set
		{
			screens = value;
		}
	}

	public void Awake()
	{
		Instance = this;
		tipsText.text = "";
		scrollBar.Value = 0f;
		scrollBarText.text = "";
		scrollBarTextMasked.text = "";
		userScreensPath = KSPUtil.ApplicationRootPath + "/UserLoadingScreens";
		if (!Directory.Exists(userScreensPath))
		{
			Directory.CreateDirectory(userScreensPath);
		}
	}

	public void Start()
	{
		if (loaders.Count == 0)
		{
			Debug.LogError("Loading Screen: No loaders defined");
		}
		StartCoroutine(LoadSystems());
	}

	public static void StartLoadingScreens()
	{
		if (!GameDatabase.Instance.Recompile)
		{
			Instance.StartCoroutine(Instance.UpdateLoadingScreen());
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public IEnumerator LoadSystems()
	{
		int currentIndex = 0;
		float startTime = Time.realtimeSinceStartup;
		QualitySettings.vSyncCount = 0;
		float loadStep = 0f;
		for (; currentIndex < loaders.Count; currentIndex++)
		{
			LoadingSystem current = loaders[currentIndex];
			float totalWeight = 0f;
			int i = 0;
			for (int count = loaders.Count; i < count; i++)
			{
				totalWeight += loaders[i].LoadWeight();
			}
			float loadDelta = current.LoadWeight() / totalWeight;
			current.StartLoad();
			while (!current.IsReady())
			{
				UpdateProgressBar(current.ProgressTitle(), loadStep / totalWeight + current.ProgressFraction() * loadDelta);
				yield return null;
			}
			loadStep += current.LoadWeight();
		}
		QualitySettings.vSyncCount = GameSettings.SYNC_VBL;
		Debug.Log("Loading Systems: Elapsed time is " + (Time.realtimeSinceStartup - startTime) + "s");
		AnalyticsUtil.LogGameStart(Time.realtimeSinceStartup - startTime);
		SceneManager.LoadScene((int)loadScene);
	}

	public void UpdateProgressBar(string title, float fraction)
	{
		scrollBarText.text = title;
		scrollBarTextMasked.text = title;
		scrollBar.Value = fraction;
	}

	public IEnumerator UpdateLoadingScreen()
	{
		if (!addedCustomScreens)
		{
			int iSuccess = 0;
			filePaths = Directory.GetFiles(userScreensPath, "*.*", SearchOption.AllDirectories);
			if (filePaths != null && screens.Count > loadingScreensListIndex)
			{
				for (int i = 0; i < filePaths.Length; i++)
				{
					fileInfo = new FileInfo(filePaths[i]);
					if (customScreenExtensions.IndexOf(fileInfo.Extension) < 0)
					{
						continue;
					}
					using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(KSPUtil.ApplicationFileProtocol + fileInfo.FullName);
					int isDoneRepeat = 0;
					yield return uwr.SendWebRequest();
					while (!uwr.isDone && isDoneRepeat < loadingScreenIsDoneRetryLimit)
					{
						isDoneRepeat++;
						yield return null;
					}
					if (!uwr.isDone)
					{
						Debug.LogWarningFormat("[LoadingScreen] Unable to load screen {0} : Cannot grab the file", fileInfo.FullName);
					}
					else if (!uwr.isNetworkError && !uwr.isHttpError)
					{
						try
						{
							Texture2D content = DownloadHandlerTexture.GetContent(uwr);
							content.name = fileInfo.Name;
							if (content.width < 1280 || content.height < 720)
							{
								if (content.width == 8 && content.height == 8)
								{
									throw new Exception("Image unable to be loaded, skipping the file");
								}
								throw new Exception("Image file dimensions below minimum size (1280x720), skipping the file");
							}
							Array.Resize(ref screens[loadingScreensListIndex].screens, screens[loadingScreensListIndex].screens.Length + 1);
							screens[loadingScreensListIndex].screens[screens[loadingScreensListIndex].screens.Length - 1] = content;
							iSuccess++;
						}
						catch (Exception ex)
						{
							Debug.LogWarningFormat("[LoadingScreen] Unable to load screen {0} : {1}", fileInfo.FullName, ex.Message);
						}
					}
					else
					{
						Debug.LogWarningFormat("[LoadingScreen] Unable to load screen {0} : {1}", fileInfo.FullName, uwr.error);
					}
				}
			}
			addedCustomScreens = true;
		}
		int currentIndex = 0;
		aspectFitter = screenImage.gameObject.GetComponent<AspectRatioFitter>();
		while (currentIndex < screens.Count)
		{
			if (screenInstance != null)
			{
				UnityEngine.Object.Destroy(screenInstance);
			}
			LoadingScreenState current = screens[currentIndex];
			UnityEngine.Object @object = current.screens[UnityEngine.Random.Range(0, current.screens.Length)];
			if (!@object.GetType().IsSubclassOf(typeof(Texture)) && !@object.GetType().Equals(typeof(Texture)))
			{
				if (@object.GetType().IsSubclassOf(typeof(GameObject)) || @object.GetType().Equals(typeof(GameObject)))
				{
					screenImage.gameObject.SetActive(value: false);
					screenInstance = UnityEngine.Object.Instantiate(@object) as GameObject;
					screenInstance.transform.SetParent(screenMask.transform, worldPositionStays: false);
				}
			}
			else
			{
				screenImage.gameObject.SetActive(value: true);
				Texture2D texture2D = @object as Texture2D;
				aspectFitter.aspectRatio = (float)texture2D.width / (float)texture2D.height;
				current.activeScreen = texture2D;
				screenImage.texture = current.activeScreen;
			}
			float tipTime = Time.realtimeSinceStartup + current.tipTime;
			SetTip(current);
			minFrameTime = 0f;
			yield return StartCoroutine(FadeIn(current));
			float endTime = Time.realtimeSinceStartup + current.displayTime;
			minFrameTime = 0.5f;
			while (Time.realtimeSinceStartup < endTime)
			{
				if (Time.realtimeSinceStartup >= tipTime)
				{
					tipTime = Time.realtimeSinceStartup + current.tipTime;
					SetTip(current);
				}
				yield return null;
			}
			minFrameTime = 0f;
			yield return StartCoroutine(FadeOut(current));
			minFrameTime = 0.5f;
			yield return new WaitForSeconds(1f);
			if (currentIndex < screens.Count - 1)
			{
				currentIndex++;
			}
		}
	}

	public void SetTip(LoadingScreenState current)
	{
		string text = Localizer.Format("#autoLOC_165067");
		if (current.tips.Length != 0)
		{
			text = Localizer.Format(current.tips[UnityEngine.Random.Range(0, current.tips.Length)]);
		}
		tipsText.text = text;
	}

	public IEnumerator FadeOut(LoadingScreenState current)
	{
		float alpha2 = 1f;
		float dAlpha = 1f / current.fadeInTime;
		float endTime = Time.realtimeSinceStartup + current.fadeInTime;
		screenMask.alpha = alpha2;
		while (Time.realtimeSinceStartup < endTime)
		{
			alpha2 -= dAlpha * Time.deltaTime;
			alpha2 = Mathf.Clamp01(alpha2);
			screenMask.alpha = alpha2;
			yield return null;
		}
		screenMask.alpha = 0f;
	}

	public IEnumerator FadeIn(LoadingScreenState current)
	{
		float alpha2 = 0f;
		float dAlpha = 1f / current.fadeOutTime;
		float endTime = Time.realtimeSinceStartup + current.fadeInTime;
		screenMask.alpha = alpha2;
		while (Time.realtimeSinceStartup < endTime)
		{
			alpha2 += dAlpha * Time.deltaTime;
			alpha2 = Mathf.Clamp01(alpha2);
			screenMask.alpha = alpha2;
			yield return null;
		}
		screenMask.alpha = 1f;
	}
}
