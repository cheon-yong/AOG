using UnityEngine;

public class Flame : SpawnedObject
{
	public int damage = 10;
	float damageTime = 0f;				// 마지막으로 데미지를 준 시간
	public float damageInterval = 0.2f; // 데미지 간격

	public void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.Character)
		{
			if (collision.gameObject == owner)
				return;

			// damageInterval 
			if(damageTime != 0f && Time.time - damageTime < damageInterval)
			{
				return;
			}

			damageTime = Time.time;
			PlayerController Controller = collision.gameObject.GetComponent<PlayerController>();
			if (Controller != null)
			{
				Controller.TakeDamage(gameObject, damage);
				return;
			}
		}
	}
}
