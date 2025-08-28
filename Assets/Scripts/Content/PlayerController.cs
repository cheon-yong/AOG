using UnityEngine;
using UnityEngine.InputSystem;
using static Define;
using UnityEngine.UIElements.Experimental;

public class PlayerController : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rb;
	private Vector2 moveVelocity;
	float inputValue;
	Animator anim;
	SpriteRenderer spriter;
	SkillExecutor skillExecutor;

	
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
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveVelocity = moveInput.normalized * speed;
	}

	private void FixedUpdate()
	{
		rb.linearVelocityX = inputValue * speed;
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
			anim.SetTrigger("Idle");
		}
	}

	public void OnMove(InputValue value)
	{
		inputValue = value.Get<Vector2>().x;
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

	private SkillContext BuildContext()
	{
		return new SkillContext
		{
			Caster = gameObject,
			CastPos = transform.position,
			AimDir = spriter.flipX ? new Vector2(-1f, 0f).normalized : new Vector2(1f, 0f).normalized,
		};
	}
}
