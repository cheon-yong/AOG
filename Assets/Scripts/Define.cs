using UnityEngine;

public class Define
{
	public struct SkillContext
	{
		public GameObject Caster;     // ������
		public Vector2 CastPos;    // ��� ���� ��ġ(������)
		public Vector2 AimDir;     // ��/�� �� ���� ����(����ȭ ����)
		public Transform FirePoint;  // ������: ����ü ���� ��ġ
		public float Now;        // Time.time ������

	}
}
