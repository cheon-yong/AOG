using UnityEngine;

public class FlameArrow : Arrow
{
    public GameObject flame;
	public float flameLifeTime = 3f;

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerNumber.Platform)
		{
			var go = Instantiate(flame, transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(go, flameLifeTime);
			return;
		}

		if (collision.gameObject.layer == LayerNumber.Character)
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
