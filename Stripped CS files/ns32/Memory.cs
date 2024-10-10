using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Profiling;

namespace ns32;

public class Memory : MonoBehaviour
{
	public float updateRate = 0.5f;

	public TextMeshProUGUI heapUsed;

	public TextMeshProUGUI allocated;

	public TextMeshProUGUI reserved;

	public TextMeshProUGUI reservedFree;

	public TextMeshProUGUI monoHeap;

	public TextMeshProUGUI monoHeapUsed;

	public Coroutine coroutine;

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

	public void UpdateDisplay()
	{
		string memoryText = GetMemoryText(Profiler.usedHeapSizeLong);
		if (heapUsed.text != memoryText)
		{
			heapUsed.text = memoryText;
		}
		memoryText = GetMemoryText(Profiler.GetTotalAllocatedMemoryLong());
		if (allocated.text != memoryText)
		{
			allocated.text = memoryText;
		}
		memoryText = GetMemoryText(Profiler.GetTotalReservedMemoryLong());
		if (reserved.text != memoryText)
		{
			reserved.text = memoryText;
		}
		memoryText = GetMemoryText(Profiler.GetTotalUnusedReservedMemoryLong());
		if (reservedFree.text != memoryText)
		{
			reservedFree.text = memoryText;
		}
		memoryText = GetMemoryText(Profiler.GetMonoHeapSizeLong());
		if (monoHeap.text != memoryText)
		{
			monoHeap.text = memoryText;
		}
		memoryText = GetMemoryText(Profiler.GetMonoUsedSizeLong());
		if (monoHeapUsed.text != memoryText)
		{
			monoHeapUsed.text = memoryText;
		}
	}

	public string GetMemoryText(long size)
	{
		return ((float)size / 1048576f).ToString("F2") + "mb";
	}
}
