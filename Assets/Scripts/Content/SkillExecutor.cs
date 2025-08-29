using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SkillExecutor : MonoBehaviour
{
	// 스킬별 다음 사용 가능 시각
	// SkillData, SkillTime
	private readonly Dictionary<SkillData, float> nextUsable = new();

	public bool TryUse(SkillData skillData, SkillContext ctx)
	{
		if (!CanUse(skillData))
		{
			Debug.Log("Fail");
			return false;
		}

		nextUsable[skillData] = Time.time;

		StartCoroutine(RunEffects(skillData, ctx));

		return true;
	}

	public bool CanUse(SkillData skillData)
	{
		if (skillData == null) 
		{ 
			return false;
		}

		if (!nextUsable.TryGetValue(skillData, out float beforeUserTime))
		{
			return true;
		}

		float elapsedTime = Time.time - beforeUserTime;

		return elapsedTime >= skillData.cooldown;
	}

	private IEnumerator RunEffects(SkillData data, SkillContext ctx)
	{
		foreach (var eff in data.effects)
		{
			if (eff != null) 
				yield return eff.Apply(ctx); // 읽기 전용으로 전달
		}
	}
}
