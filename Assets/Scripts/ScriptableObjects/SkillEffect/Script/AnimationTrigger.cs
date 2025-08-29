using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationTrigger", menuName = "Scriptable Objects/SkillEffect/AnimationTrigger")]

public class AnimationTrigger : SkillEffect
{
	public string animationTrigger;
	public bool canTrigger = false;
	public override IEnumerator Apply(Define.SkillContext ctx)
	{
		if (ctx.Caster == null || ctx.Animator == null) 
		{ 
			yield break;
		}

		ctx.Animator.SetTrigger(animationTrigger);
		ctx.Animator.SetBool("CanTrigger", canTrigger);


		yield return null;
	}
}
