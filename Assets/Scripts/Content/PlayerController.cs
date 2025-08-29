using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static Define;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
	[Header("Stat")]
	public int MaxHp = 100;
	public int Hp = 100;
	public int Damage = 10;
	public float moveSpeed = 5;

	[Header("State")]
	public bool usingSkill = false;

	public UnityEvent<DamageContext> OnDamage;
	public UnityEvent<DamageContext> OnDeath;
	public UnityEvent<SkillData> OnSkill;

	protected Rigidbody2D rb;
	
	protected float inputValue;
	protected Animator anim;
	protected SpriteRenderer spriter;
	protected SkillExecutor skillExecutor;

	[SerializeField] GameObject target;

	[Header("Skill")]
	[SerializeField] protected SkillData baseSkillData;

	[SerializeField] protected SkillData skillData1;
	[SerializeField] protected SkillData skillData2;
	[SerializeField] protected SkillData skillData3;

	private PlayerInput pi;

	protected virtual void Start()
	{
		pi = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriter = GetComponent<SpriteRenderer>();
		skillExecutor = GetComponent<SkillExecutor>();

		OnDamage.AddListener(DecreaseHp);

		if (pi)
			pi.SwitchCurrentActionMap("Player");

		GameManager.Instance.OnGameEnd.AddListener(GameEnd);
	}

	protected void FixedUpdate()
	{
		rb.linearVelocityX = inputValue * moveSpeed;
	}

	protected virtual void LateUpdate()
	{
		anim.SetFloat("Speed", Mathf.Abs(rb.linearVelocityX));

		if (inputValue != 0)
		{
			spriter.flipX = rb.linearVelocityX < 0;
			anim.SetTrigger("Run");
		}
		else
		{
			anim.SetTrigger("Shot");
		}
	}

	public virtual void GameEnd(bool clear)
	{
		if (pi)
			pi.SwitchCurrentActionMap("UI");

		if (Hp > 0f)
		{
			SetIdle(false);
		}
	}

	public void OnMove(InputValue value)
	{
		if (usingSkill)
		{
			inputValue = 0f;
			return;
		}

		inputValue = value.Get<Vector2>().x;
	}

	public void MoveByButton(float x)
	{
		if (usingSkill)
		{
			inputValue = 0f;
			return;
		}

		inputValue = x;
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerNumber.Platform)
		{
			SetIdle(true);
		}
	}

	public void TakeDamage(GameObject attacker, int damage)
	{
		if (Hp <= 0f)
			return;

		DamageContext ctx = BuildDamageContext(attacker, gameObject, damage);
		DecreaseHp(ctx);
		OnDamage.Invoke(ctx);
	}

	public void DecreaseHp(DamageContext context)
	{
		Hp = (int)Mathf.Clamp(Hp - context.Damage, 0f, MaxHp);
		if (Hp == 0f)
		{
			anim.SetTrigger("Dead");
			anim.SetBool("CanTrigger", false);
			OnDeath.Invoke(context);
		}
	}

	public virtual void OnShot()
	{
		skillExecutor.TryUse(baseSkillData, BuildContext());
	}
	public virtual void OnChageShotEnd()
	{
		SetIdle(true);
	}

	protected void SetIdle(bool CanTrigger)
	{
		usingSkill = false;
		anim.SetBool("CanTrigger", CanTrigger);
		anim.SetTrigger("Idle");
	}

	public void OnAttack1(InputValue value)
	{
		skillExecutor.TryUse(skillData1, BuildContext());
		OnSkill.Invoke(skillData1);
	}

	public void OnAttack2(InputValue value)
	{
		inputValue = 0f;
		skillExecutor.TryUse(skillData2, BuildContext());
		OnSkill.Invoke(skillData2);
	}

	public void OnAttack3(InputValue value)
	{
		inputValue = 0f;
		skillExecutor.TryUse(skillData3, BuildContext());
		OnSkill.Invoke(skillData3);
	}

	public void AttackByButton(int skillId)
	{
		// 하드코딩. 수정해야함
		switch (skillId)
		{
			case 1:
				OnAttack1(null);
				break;
			case 2:
				OnAttack2(null);
				break;
			case 3:
				OnAttack3(null);
				break;
			default:
				return;
		}
	}

	public GameObject GetTarget()
	{
		MonsterController monster = FindFirstObjectByType<MonsterController>();
		target = monster.gameObject;
		return target;
	}

	protected SkillContext BuildContext()
	{
		return new SkillContext
		{
			Caster = gameObject,
			CastPos = transform.position,
			AimDir = spriter.flipX ? new Vector2(-1f, 0f).normalized : new Vector2(1f, 0f).normalized,
			Animator = anim,
			Now = Time.time,
			Target = target ? target : GetTarget(),
		};
	}

	protected DamageContext BuildDamageContext(GameObject attacker,
											GameObject victim,
											int damage)
	{
		return new DamageContext
		{
			Attacker = attacker,
			Victim = victim,
			Damage = damage,
			Now = Time.time
		};
	}
}
