using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

[CreateAssetMenu(fileName = "CreateProjectile", menuName = "Scriptable Objects/SkillEffect/CreateProjectile")]
public class CreateProjectile : SkillEffect
{
	public GameObject prefab;
	public float lifetime = 1.5f;
	public Vector2 offset;
	public float speed = 1.0f;

	public override IEnumerator Apply(SkillContext ctx)
	{
		if (!prefab) 
			yield break;

		var go = Instantiate(prefab, ctx.CastPos + offset, Quaternion.identity);

		var rb = go.GetComponent<Rigidbody2D>();
		rb.linearVelocityX = Mathf.Abs(ctx.AimDir.x) * speed;

		var sp = go.GetComponent<SpriteRenderer>();
		sp.flipX = ctx.AimDir.x < 0 ? true : false;
		

		Object.Destroy(go, lifetime);

		yield return null;
	}
}
