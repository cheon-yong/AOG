using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Jump", menuName = "Scriptable Objects/SkillEffect/Jump")]

public class Jump : SkillEffect
{

	public float height = 1f;

	public override IEnumerator Apply(Define.SkillContext ctx)
	{
		if (ctx.Caster == null)
		{
			yield break;
		}

		Rigidbody2D rb = ctx.Caster.GetComponent<Rigidbody2D>();
		if (rb == null)
			yield break;

		rb.linearVelocity = new Vector2(rb.linearVelocity.x, height); // Jump

	}
}
