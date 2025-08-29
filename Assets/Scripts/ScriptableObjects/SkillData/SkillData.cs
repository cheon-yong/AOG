using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
	[Header("기본 정보")]
	public string skillId;
	public string displayName;
	public float cooldown = 2f;

	public bool cancelable = true; 
	public AudioClip sfx;

	[Header("효과 목록")]
	public SkillEffect[] effects; 
}