using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
	[Header("기본 정보")]
	public string skillId;
	public string displayName;
	public float cooldown = 2f;

	[Header("애니메이션/연출")]
	public string animatorTrigger; 
	public AudioClip sfx;

	[Header("효과 목록")]
	public SkillEffect[] effects; 
}