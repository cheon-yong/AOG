using UnityEngine;

public class Define
{
	public struct SkillContext
	{
		public GameObject Caster;		// 시전자
		public Vector2 CastPos;			// 사용 순간 위치
		public Animator Animator;		// 애니메이터
		public Vector2 AimDir;			// 좌/우 등 조준 방향
		public GameObject Target;		// 타겟
		public float Now;				// 스킬 사용 시간 스냅샷	
	}

	public struct DamageContext
	{
		public GameObject Attacker;		// 시전자
		public GameObject Victim;      // 사용 순간 위치
		public int Damage;			// 데미지 수치
		public float Now;				// 데미지 발생 시간
	}
}
