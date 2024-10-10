using System.Collections;
using UnityEngine;

namespace TMPro.Examples;

public class TeleType : MonoBehaviour
{
	public string label01 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=1>";

	public string label02 = "Example <sprite=2> of using <sprite=7> <#ffa000>Graphics Inline</color> <sprite=5> with Text in <font=\"Bangers SDF\" material=\"Bangers SDF - Drop Shadow\">TextMesh<#40a0ff>Pro</color></font><sprite=0> and Unity<sprite=2>";

	public TMP_Text m_textMeshPro;

	public void Awake()
	{
		m_textMeshPro = GetComponent<TMP_Text>();
		m_textMeshPro.text = label01;
		m_textMeshPro.enableWordWrapping = true;
		m_textMeshPro.alignment = TextAlignmentOptions.Top;
	}

	public IEnumerator Start()
	{
		m_textMeshPro.ForceMeshUpdate();
		int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount;
		int counter = 0;
		while (true)
		{
			int num = counter % (totalVisibleCharacters + 1);
			m_textMeshPro.maxVisibleCharacters = num;
			if (num >= totalVisibleCharacters)
			{
				yield return new WaitForSeconds(1f);
				m_textMeshPro.text = label02;
				yield return new WaitForSeconds(1f);
				m_textMeshPro.text = label01;
				yield return new WaitForSeconds(1f);
			}
			counter++;
			yield return new WaitForSeconds(0.05f);
		}
	}
}
