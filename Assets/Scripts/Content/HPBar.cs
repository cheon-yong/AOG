using UnityEngine;
using UnityEngine.UI;
using static Define;

public class HPBar : MonoBehaviour
{
    public PlayerController targetController;
	public Slider slider;

	private void Awake()
	{
		targetController.OnDamage.AddListener(UpdateHpBar);
		slider = GetComponent<Slider>();
	}
	private void Start()
	{
		UpdateHpBar(new DamageContext { });
	}

	private void UpdateHpBar(DamageContext ctx)
	{
		if (targetController != null) 
		{
			slider.value = (float)targetController.Hp / targetController.MaxHp;
		}
	}
}
