using UnityEngine;

public class MenuRandomKerbalAnims : MonoBehaviour
{
	public string idle;

	public string[] anims;

	public float elapsedTime;

	public float randomTime;

	public Animation _animation;

	public void Start()
	{
		randomTime = Random.Range(3, 10);
		this.GetComponentCached(ref _animation).Play(idle);
	}

	public void Update()
	{
		elapsedTime += Time.deltaTime;
		if (elapsedTime > randomTime)
		{
			randomTime = Random.Range(15, 30);
			this.GetComponentCached(ref _animation).Blend(anims[Random.Range(0, anims.Length)]);
			elapsedTime = 0f;
		}
		else if (!_animation.isPlaying)
		{
			this.GetComponentCached(ref _animation).Blend(idle);
		}
	}
}
