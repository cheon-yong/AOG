using System.Collections;
using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "CreateParabolaProjectile", menuName = "Scriptable Objects/SkillEffect/CreateParabolaProjectile")]
public class CreateParabolaProjectile : SkillEffect
{
	public GameObject prefab;
	public float delay = 0f;
	public float lifetime = 3.0f;
	public Vector2 offset;
	public float speed = 1.0f;
	public float damageRatio = 1.0f;

	public override IEnumerator Apply(SkillContext ctx)
	{
		if (!prefab)
			yield break;

		if (delay > 0f) 
			yield return new WaitForSeconds(delay);

		var go = Instantiate(prefab, ctx.CastPos + offset, Quaternion.identity);

		var so = go.GetComponent<Arrow>();
		if (so != null)
		{
			so.owner = ctx.Caster;
			so.damage = (int)(ctx.Caster.GetComponent<PlayerController>().Damage * damageRatio);
		}
			

		var rb = go.GetComponent<Rigidbody2D>();
		rb.linearVelocityX = Mathf.Abs(ctx.AimDir.x) * speed;

		//var sp = go.GetComponent<SpriteRenderer>();
		//sp.flipX = ctx.AimDir.x < 0 ? true : false;

		ctx.Animator.SetTrigger("Shot");

		Vector2 targetPos = ctx.Target.transform.position;
		var parabola = go.GetComponent<ParabolaArrow>();
		float distance = Vector2.Distance(ctx.CastPos, targetPos);
		float height = distance / 3;

		Vector2 middle = new Vector2((ctx.CastPos.x + targetPos.x) / 2, (ctx.CastPos.y + targetPos.y) / 2 + height);
		parabola.Launch(ctx.CastPos, middle, targetPos, speed);

		Destroy(go, lifetime);

		yield return null;
	}
}
