using System.Collections;
using Unity.Mathematics.Geometry;
using UnityEngine;


[CreateAssetMenu(fileName = "CreateArrowRain", menuName = "Scriptable Objects/SkillEffect/CreateArrowRain")]
public class CreateArrowRain : SkillEffect
{

	public GameObject prefab;
	public float delay = 0;
	public float minInterval = 0;
	public float maxInterval = 0;

	public int count = 1;
	public float lifetime = 1.5f;
	public Vector2 offset;
	public float minoffsetX = 0f;
	public float maxoffsetX = 0f;
	public float speed = 5.0f;
	public float damageRatio = 1f;

	public override IEnumerator Apply(Define.SkillContext ctx)
	{
		if (!prefab)
			yield break;

		if (delay > 0)
			yield return new WaitForSeconds(delay);

		Vector2 standardPos = (Vector2)ctx.Target.transform.position + offset;

		for (int i = 0; i < count; i++)
		{
			var dir = (Vector2)ctx.Target.transform.position - standardPos;
			var go = Instantiate(prefab, standardPos + new Vector2(Random.Range(minoffsetX, maxoffsetX), 0), Quaternion.identity);

			var so = go.GetComponent<Arrow>();
			if (so != null)
			{
				so.owner = ctx.Caster;
				so.damage = (int)(ctx.Caster.GetComponent<PlayerController>().Damage * damageRatio);
			}
				

			var rb = go.GetComponent<Rigidbody2D>();


			//rb.linearVelocityX = Mathf.Abs(ctx.AimDir.x) * speed;
			rb.linearVelocity = new Vector2(0, -1 * speed);


			// Z축만 회전
			float angle = -90f;
			go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			var parabola = go.GetComponent<ParabolaArrow>();
			parabola.enabled = false;

			Object.Destroy(go, lifetime);

			float interval = Random.Range(minInterval, maxInterval);
			yield return new WaitForSeconds(interval);
		}

		yield return null;
	}
}
