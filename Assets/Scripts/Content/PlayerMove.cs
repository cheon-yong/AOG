using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
	public float speed;
	private Rigidbody2D rb;
	private Vector2 moveVelocity;
	Animator anim;
	SpriteRenderer spriter;

	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		spriter = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		moveVelocity = moveInput.normalized * speed;
	}

	private void FixedUpdate()
	{
		rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
	}

	private void LateUpdate()
	{
		anim.SetFloat("Speed", Mathf.Abs(moveVelocity.x));

		if (moveVelocity.x != 0)
		{
			spriter.flipX = moveVelocity.x < 0;
			anim.SetTrigger("Run");
		}
		else
		{
			anim.SetTrigger("Idle");
		}
	}

	//[SerializeField]
	//float speed;
	//float inputValue;

	//Rigidbody2D body;
	//Animator anim;
	//SpriteRenderer spriter;

	//private void Awake()
	//{
	//	body = GetComponent<Rigidbody2D>();
	//	anim = GetComponent<Animator>();
	//	spriter = GetComponent<SpriteRenderer>();
	//}

	//private void FixedUpdate()
	//{
	//	body.linearVelocityX = inputValue * speed;
	//}



	//private void OnMove(InputValue value)
	//{
	//	Debug.Log("Move");
	//	inputValue = value.Get<Vector2>().x;
	//}
}
