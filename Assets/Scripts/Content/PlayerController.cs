using UnityEngine;
using UnityEngine.InputSystem;
using static Define;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed;
	private Rigidbody2D rb;
	private Vector2 moveVelocity;
	float inputValue;
	Animator anim;
	SpriteRenderer spriter;
	SkillExecutor skillExecutor;

	public bool usingSkill = false;


	[SerializeField] GameObject target;

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
		// 하드 코딩. 고쳐야함
		if (collision.gameObject.layer == LayerMask.Platform)
		{
			SetIdle();
		}
	}

	public virtual void OnShot()
	{
		skillExecutor.TryUse(baseSkillData, BuildContext());
	}
	public virtual void OnChageShotEnd()
	{
		SetIdle();
	}

	private void SetIdle()
	{
		usingSkill = false;
		anim.SetBool("CanTrigger", true);
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
		MonsterController monster = FindObjectOfType<MonsterController>();
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
}
