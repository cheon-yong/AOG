using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor : MonoBehaviour
{
	// 스킬별 다음 사용 가능 시각
	// SkillData, SkillTime
	private readonly Dictionary<SkillData, float> nextUsable = new();

	public bool TryUse(SkillData skillData)
	{
		if (!CanUse(skillData))
		{
			Debug.Log("Fail");
			return false;
		}

		nextUsable[skillData] = Time.time;
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
}
