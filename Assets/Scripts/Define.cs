using UnityEngine;

public class Define
{
	public struct SkillContext
	{
		public GameObject Caster;		// ������
		public Vector2 CastPos;			// ��� ���� ��ġ
		public Animator Animator;		// �ִϸ�����
		public Vector2 AimDir;			// ��/�� �� ���� ����
		public GameObject Target;		// Ÿ��
		public float Now;				// ��ų ��� �ð� ������	
	}

	public struct DamageContext
	{
		public GameObject Attacker;		// ������
		public GameObject Victim;      // ��� ���� ��ġ
		public int Damage;			// ������ ��ġ
		public float Now;				// ������ �߻� �ð�
	}
}
