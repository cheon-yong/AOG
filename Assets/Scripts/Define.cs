using UnityEngine;

public class Define
{
	public struct SkillContext
	{
		public GameObject Caster;     // 시전자
		public Vector2 CastPos;    // 사용 순간 위치(스냅샷)
		public Vector2 AimDir;     // 좌/우 등 조준 방향(정규화 권장)
		public Transform FirePoint;  // 선택적: 투사체 스폰 위치
		public float Now;        // Time.time 스냅샷

	}
}
