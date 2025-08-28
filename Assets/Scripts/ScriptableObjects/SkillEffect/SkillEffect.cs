using System.Collections;
using UnityEngine;
using static Define;


public abstract class SkillEffect : ScriptableObject
{
	public abstract IEnumerator Apply(SkillContext ctx);
}
