using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
	[Header("�⺻ ����")]
	public string skillId;
	public string displayName;
	public float cooldown = 2f;

	public bool cancelable = true; 
	public AudioClip sfx;

	[Header("ȿ�� ���")]
	public SkillEffect[] effects; 
}