using NUnit.Framework.Constraints;
using UnityEngine;
using static Define;
using static UnityEngine.UI.Image;

public class MonsterController : PlayerController
{
	[Header("AI")]
	[SerializeField] float moveTime = 0f;
	[SerializeField] float idleTime = 0f;
	[SerializeField] float skillTime = 0f;
	[SerializeField] GroundProbe groundProbe;
	[SerializeField] Vector2 originScale;

	float elapsedTime = 0f;
	float stateChangeInteval = 0f;

	MonsterState currentState;
	MonsterState beforeState;

	protected override void Start()
	{
		base.Start();

		groundProbe = GetComponentInChildren<GroundProbe>();
		groundProbe.OnCliff.AddListener(OnCliff);
		originScale = transform.localScale;
		OnDeath.AddListener(MakeDead);
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();

		spriter.flipX = false;
	}

	public override void GameEnd(bool clear)
	{
		base.GameEnd(clear);

		if (Hp > 0f)
		{
			currentState = MonsterState.Win;
			inputValue = 0f;
			rb.linearVelocityX = 0;
		}
	}

	void Flip(Vector2 dir)
	{
		transform.localScale = new Vector2(originScale.x * dir.x, originScale.y);
	}

	public void MakeDead(DamageContext ctx)
	{
		currentState = MonsterState.Dead;
		inputValue = 0f;
		rb.linearVelocityX = 0;
	}

	private void Update()
	{

		elapsedTime += Time.deltaTime;

		switch (currentState)
		{
			case MonsterState.Move:
				MoveTick();
				break;
			case MonsterState.Idle:
				break;
			case MonsterState.Skill:
				break;
			case MonsterState.Dead:
				return;
			case MonsterState.Win:
				return;
		}

		if (elapsedTime >= stateChangeInteval)
		{
			EnterNextState(GetRandomNextState());
		}
	}

	MonsterState GetRandomNextState()
	{
		return (MonsterState)Random.Range((int)MonsterState.Move, (int)MonsterState.Dead);
	}

	void EnterNextState(MonsterState newState)
	{
		beforeState = currentState;
		currentState = newState;
		elapsedTime = 0;

		switch (currentState)
		{
			case MonsterState.Move:
				// 같은 상태가 연속이면 시간을 감소
				stateChangeInteval = beforeState == currentState ? moveTime * 0.7f : moveTime;

				// 시작 방향 랜덤
				inputValue = Random.value < 0.5f ? -1 : +1;
				Flip(new Vector2(inputValue, 0));				
				break;

			case MonsterState.Idle:
				stateChangeInteval = beforeState == currentState ? idleTime * 0.5f : idleTime;
				inputValue = -1;
				Flip(new Vector2(inputValue, 0));
				break;

			case MonsterState.Skill:
				stateChangeInteval = skillTime;
				inputValue = -1;
				Flip(new Vector2(inputValue, 0));
				UseSkill(); // 즉시 발동
				break;
		}
	}

	void MoveTick()
	{
		if (!rb) 
			return;

		rb.linearVelocityX = inputValue * moveSpeed;
	}

	void OnCliff()
	{
		inputValue *= -1;
		Flip(new Vector2(inputValue, 0));
	}

	// 아무것도 안함

	public void UseSkill()
	{
		stateChangeInteval = skillTime;
		// 하드 코딩. 수정 필요
		int skillIndex = Random.Range(0, 3);
		switch (skillIndex)
		{
			case 0:
				skillExecutor.TryUse(skillData1, BuildContext());
				break;

			case 1:
				skillExecutor.TryUse(skillData2, BuildContext());
				break;

			case 2:
				skillExecutor.TryUse(skillData3, BuildContext());
				break;
		}
	}

}
