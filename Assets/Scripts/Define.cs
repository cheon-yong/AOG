using UnityEngine;

public class Define
{
	public struct SkillContext
	{
		public GameObject Caster;     // ������
		public Vector2 CastPos;       // ��� ���� ��ġ
		internal Animator Animator;   // �ִϸ�����
		public Vector2 AimDir;        // ��/�� �� ���� ����
		public GameObject Target;	  // Ÿ��
		public float Now;             // ��ų ��� �ð� ������
		
	}
}
