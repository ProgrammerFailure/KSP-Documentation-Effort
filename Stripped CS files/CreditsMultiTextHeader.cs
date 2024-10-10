using System;
using System.Collections;
using UnityEngine;

public class CreditsMultiTextHeader : MonoBehaviour
{
	[Serializable]
	public class TextHeader
	{
		public Transform headerRef;

		public Transform currentAnchor;
	}

	public TextHeader[] textTrfs;

	public int currentText;

	public Transform startAnchor;

	public Transform centerAnchor;

	public Transform endAnchor;

	public float transitionDuration = 0.2f;

	public void Start()
	{
		for (int i = 0; i < textTrfs.Length; i++)
		{
			textTrfs[i].headerRef.localPosition = textTrfs[i].currentAnchor.localPosition;
			textTrfs[i].headerRef.localScale = textTrfs[i].currentAnchor.localScale;
			textTrfs[i].headerRef.localRotation = textTrfs[i].currentAnchor.localRotation;
		}
	}

	public void SetText(int idx, float duration)
	{
		if (idx != currentText)
		{
			if (idx > currentText)
			{
				StartCoroutine(shiftText(textTrfs[currentText], endAnchor, duration));
				StartCoroutine(shiftText(textTrfs[idx], centerAnchor, duration));
			}
			else
			{
				StartCoroutine(shiftText(textTrfs[currentText], startAnchor, duration));
				StartCoroutine(shiftText(textTrfs[idx], centerAnchor, duration));
			}
			currentText = idx;
		}
	}

	public void NextText()
	{
		SetText(currentText + 1, transitionDuration);
	}

	public IEnumerator shiftText(TextHeader textTrf, Transform tgtAnchor, float duration)
	{
		float t = 0f;
		do
		{
			t += Time.deltaTime;
			float num = ((duration != 0f) ? (t / duration) : 1f);
			textTrf.headerRef.localPosition = Vector3.Lerp(textTrf.currentAnchor.localPosition, tgtAnchor.localPosition, num);
			textTrf.headerRef.localScale = Vector3.Lerp(textTrf.currentAnchor.localScale, tgtAnchor.localScale, num);
			textTrf.headerRef.localRotation = Quaternion.Lerp(textTrf.currentAnchor.localRotation, tgtAnchor.localRotation, num);
			if (!(num >= 1f))
			{
				yield return null;
			}
		}
		while (t <= duration);
		textTrf.currentAnchor = tgtAnchor;
	}
}
