using UnityEngine;
using System.Collections;
using static Define;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "CreateMultiProjectile", menuName = "Scriptable Objects/SkillEffect/CreateMultiProjectile")]
public class CreateMultiProjectile : SkillEffect
{
	public GameObject prefab;
	public float delay = 0;
	public float interval = 0;
	public int count = 1;
	public float lifetime = 1.5f;
	public Vector2 offset;
	public float speed = 1.0f;
	public float damageRatio = 1.0f;

	public override IEnumerator Apply(SkillContext ctx)
	{
		if (!prefab)
			yield break;

		if (delay > 0)
			yield return new WaitForSeconds(delay);

		for (int i = 0; i < count; i++)
		{
			Vector2 curPos = ctx.Caster.transform.position;
			Vector2 targetPos = ctx.Target.transform.position;
			Vector2 dir = (targetPos - curPos).normalized;
			var go = Instantiate(prefab, curPos + offset, Quaternion.identity);

			var so = go.GetComponent<Arrow>();
			if (so != null)
			{
				so.owner = ctx.Caster;
				so.damage = (int)(ctx.Caster.GetComponent<PlayerController>().Damage * damageRatio);
			}

			var rb = go.GetComponent<Rigidbody2D>();


			//rb.linearVelocityX = Mathf.Abs(ctx.AimDir.x) * speed;
			rb.linearVelocity = new Vector2(dir.x, dir.y) * speed;


			// Z축만 회전
			float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			go.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); 

			var sp = go.GetComponent<SpriteRenderer>();

			sp.flipX = ctx.AimDir.x < 0 ? true : false;

			var parabola = go.GetComponent<ParabolaArrow>();
			parabola.enabled = false;

			Object.Destroy(go, lifetime);

			yield return new WaitForSeconds(interval);
		}

		yield return null;
	}
}
