using UnityEngine;

public class Arrow : SpawnedObject
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public int damage = 0;

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerMask.Platform)
		{
			Destroy(gameObject);
			return;
		}

		if (collision.gameObject.layer == LayerMask.Character)
		{
			if (collision.gameObject == owner)
				return;

			PlayerController Controller = collision.gameObject.GetComponent<PlayerController>();
			if (Controller != null)
			{
				Controller.TakeDamage(gameObject, damage);
				Destroy(gameObject);
				return;
			}
		}
	}
}
