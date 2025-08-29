using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] int skillID = 0;
	[SerializeField] Button button;
	[SerializeField] TMP_Text countDownText;
	int countDown = 0;

	[SerializeField] PlayerController player;

	public void Start()
	{
		button = GetComponent<Button>();
		player.OnSkill.AddListener(OnUseSkill);
		countDownText.text = "";

		button.onClick.AddListener(OnClick);
	}

	public void OnClick()
	{
		player.AttackByButton(skillID);
	}

	public void OnUseSkill(SkillData skillData)
    {
		if (skillData.skillId == skillID)
		{
			button.enabled = false;
			StartCoroutine(Cooldown(skillData.cooldown));
			StartCoroutine(Countdown(skillData.cooldown));
		}
    }

	private IEnumerator Cooldown(float cooldown)
	{
		yield return new WaitForSeconds(cooldown);
		button.enabled = true;
	}

	private IEnumerator Countdown(float cooldown) 
	{
		countDown = (int)cooldown;
		countDownText.text = countDown.ToString();
		while (countDown >= 0)
		{
			yield return new WaitForSeconds(1f);
			countDown--;
			countDownText.text = countDown.ToString();
		}

		countDownText.text = "";
	}
}
