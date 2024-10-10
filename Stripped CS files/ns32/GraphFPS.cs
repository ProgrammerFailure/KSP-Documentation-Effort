using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Vectrosity;

namespace ns32;

public class GraphFPS : MonoBehaviour
{
	public float updateRate = 0.5f;

	public RectTransform graphTransform;

	public Color graphColor;

	public float graphWidth = 1f;

	public TextMeshProUGUI fpsText;

	public TextMeshProUGUI fpsMinText;

	public TextMeshProUGUI fpsMaxText;

	public VectorObject2D horizLine;

	public VectorObject2D vertLine;

	public List<Vector2> fpsPoints;

	public VectorLine fpsLine;

	public Coroutine coroutine;

	public void Awake()
	{
	}

	public void Start()
	{
		fpsPoints = new List<Vector2>(new Vector2[Performance.Instance.FrameTimeCount]);
		Performance.Instance.GetFramePerSecondPoints(fpsPoints, graphTransform.sizeDelta.x, graphTransform.sizeDelta.y);
		fpsLine = new VectorLine("FPSLine", fpsPoints, graphWidth);
		fpsLine.lineType = LineType.Continuous;
		fpsLine.color = graphColor;
		UpdateDisplay();
	}

	public void UpdateDisplay()
	{
		if (horizLine.vectorLine.points2[1].x != graphTransform.sizeDelta.x)
		{
			horizLine.vectorLine.points2[1] = new Vector2(graphTransform.sizeDelta.x, 0f);
			horizLine.vectorLine.Draw();
		}
		if (vertLine.vectorLine.points2[1].y != graphTransform.sizeDelta.y)
		{
			vertLine.vectorLine.points2[1] = new Vector2(0f, graphTransform.sizeDelta.y);
			vertLine.vectorLine.Draw();
		}
		UpdateText();
		Performance.Instance.GetFramePerSecondPoints(fpsPoints, graphTransform.sizeDelta.x, graphTransform.sizeDelta.y);
		fpsLine.Draw();
		fpsLine.rectTransform.SetParent(graphTransform, worldPositionStays: false);
	}

	public void UpdateText()
	{
		fpsText.text = Performance.Instance.FramesPerSecond.ToString("F1");
		fpsMaxText.text = (1f / Performance.Instance.FrameTimeMin).ToString("F1");
		fpsMinText.text = (1f / Performance.Instance.FrameTimeMax).ToString("F1");
	}

	public void OnEnable()
	{
		if (coroutine == null)
		{
			coroutine = StartCoroutine(UpdateCoroutine());
		}
	}

	public void OnDisable()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	public IEnumerator UpdateCoroutine()
	{
		while (true)
		{
			yield return new WaitForSecondsRealtime(updateRate);
			UpdateDisplay();
		}
	}
}
