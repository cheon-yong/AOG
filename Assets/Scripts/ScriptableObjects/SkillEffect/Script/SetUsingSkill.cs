using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "CreateProjectile", menuName = "Scriptable Objects/SkillEffect/SetUsingSkill")]

public class SetUsingSkill : SkillEffect
{
	public bool usingSkill = true;
	public override IEnumerator Apply(Define.SkillContext ctx)
	{

		ctx.Caster.GetComponent<PlayerController>().usingSkill = usingSkill;

		yield return null;
	}
}
