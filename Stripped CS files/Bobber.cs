using UnityEngine;

public class Bobber : MonoBehaviour
{
	public float val1;

	public float val2;

	public float val3;

	public float ofs1;

	public float ofs2;

	public float ofs3;

	public Vector3 initialPos;

	public float seed;

	public void Start()
	{
		initialPos = base.transform.position;
	}

	public void Update()
	{
		val1 = Mathf.Sin(Time.time + seed) / ofs1;
		val2 = Mathf.Sin(Time.time + 0.5f + seed) / ofs2;
		val3 = Mathf.Sin(Time.time + 0.2f + seed) / ofs3;
		base.transform.position = initialPos + new Vector3(val1, val2, val3);
	}
}
