using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Define;

public class PlayerController : MonoBehaviour
{
	[Header("Stat")]
	public int MaxHp { get; private set; } = 200;
	public int Hp { get; private set; } = 200;

	public int Damage { get; private set; } = 10;

	public float moveSpeed { get; private set; } = 5;

	[Header("State")]
	public bool usingSkill = false;

	public UnityEvent<DamageContext> OnDamage;
	public UnityEvent<DamageContext> OnDeath;

	private Rigidbody2D rb;
	
	float inputValue;
	Animator anim;
	SpriteRenderer spriter;
	SkillExecutor skillExecutor;

	private Vector2 moveVelocity;

	[SerializeField] GameObject target;

	[Header("Skill")]
	[SerializeField] SkillData baseSkillData;

	[SerializeField] SkillData skillData1;
	[SerializeField] SkillData skillData2;
	[SerializeField] SkillData skillData3;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriter = GetComponent<SpriteRenderer>();
		skillExecutor = GetComponent<SkillExecutor>();

		OnDamage.AddListener(DecreaseHp);

		GameManager.Instance.OnGameEnd.AddListener(GameEnd);
	}

	private void Update()
	{
		// Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		// moveVelocity = moveInput.normalized * moveSpeed;
	}

	private void FixedUpdate()
	{
		rb.linearVelocityX = inputValue * moveSpeed;
		//rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}

	private void LateUpdate()
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

	public void GameEnd(bool clear)
	{
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

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == LayerMask.Platform)
		{
			SetIdle(true);
		}
	}

	public void TakeDamage(GameObject attacker, int damage)
	{
		if (Hp <= 0f)
			return;

		OnDamage.Invoke(BuildDamageContext(attacker, gameObject, damage));
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

	private void SetIdle(bool CanTrigger)
	{
		usingSkill = false;
		anim.SetBool("CanTrigger", CanTrigger);
		anim.SetTrigger("Idle");
	}

	public void OnAttack1(InputValue value)
	{
		Debug.Log("Attack1");
		skillExecutor.TryUse(skillData1, BuildContext());
	}

	public void OnAttack2(InputValue value)
	{
		Debug.Log("Attack2");
		skillExecutor.TryUse(skillData2, BuildContext());
	}

	public void OnAttack3(InputValue value)
	{
		Debug.Log("Attack3");
		skillExecutor.TryUse(skillData3, BuildContext());
	}

	public GameObject GetTarget()
	{
		MonsterController monster = FindFirstObjectByType<MonsterController>();
		target = monster.gameObject;
		return target;
	}

	private SkillContext BuildContext()
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

	private DamageContext BuildDamageContext(GameObject attacker,
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
