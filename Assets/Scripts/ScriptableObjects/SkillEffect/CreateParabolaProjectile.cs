using System.Collections;
using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "CreateParabolaProjectile", menuName = "Scriptable Objects/SkillEffect/CreateParabolaProjectile")]
public class CreateParabolaProjectile : SkillEffect
{
	public GameObject prefab;
	public float lifetime = 3.0f;
	public Vector2 offset;
	public float speed = 1.0f;

	public override IEnumerator Apply(SkillContext ctx)
	{
		if (!prefab)
			yield break;

		var go = Instantiate(prefab, ctx.CastPos + offset, Quaternion.identity);

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
