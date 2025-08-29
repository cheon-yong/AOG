using UnityEngine;
using static Define;
using static UnityEngine.UI.Image;

public class MonsterController : PlayerController
{
	[Header("AI")]
	[SerializeField] float moveTime = 0f;
	[SerializeField] float idleTime = 0f;
	[SerializeField] float skillTime = 0f;
	[SerializeField] float cliffCheckDist = 0f;
	[SerializeField] GroundProbe groundProbe;
	[SerializeField] Vector2 originProbePos;
	bool dead = false;

	float elapsedTime = 0f;
	float stateChangeInteval = 0f;
	float additionalInteval = 0f;

	MonsterState currentState;
	MonsterState beforeState;

	protected override void Start()
	{
		base.Start();

		groundProbe = GetComponentInChildren<GroundProbe>();
		groundProbe.OnCliff.AddListener(OnCliff);
		originProbePos = groundProbe.transform.localPosition;
		OnDeath.AddListener(MakeDead);
	}

	public void MakeDead(DamageContext ctx)
	{
		dead = true;
	}

	private void Update()
	{
		if (dead)
		{
			return;
		}

		elapsedTime += Time.deltaTime;

		switch (currentState)
		{
			case MonsterState.Move:
				MoveTick();
				break;
			case MonsterState.Idle:
				StayIdle();
				break;
			case MonsterState.Skill:
				break;
			case MonsterState.Max:
				break;
		}

		if (elapsedTime >= stateChangeInteval)
		{
			EnterNextState(GetRandomNextState());
		}
	}

	MonsterState GetRandomNextState()
	{
		return (MonsterState)Random.Range((int)MonsterState.Move, (int)MonsterState.Idle);
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
				spriter.flipX = inputValue < 0f;
				float probePosX = originProbePos.x * inputValue;
				groundProbe.transform.localPosition = new Vector2(probePosX, originProbePos.y);
				
				break;

			case MonsterState.Idle:
				stateChangeInteval = beforeState == currentState ? idleTime * 0.5f : idleTime;
				break;

			case MonsterState.Skill:
				stateChangeInteval = skillTime;
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
		groundProbe.transform.localPosition = new Vector2(-groundProbe.transform.localPosition.x, originProbePos.y);
		inputValue *= -1;

		rb.linearVelocityX = inputValue * moveSpeed;
		spriter.flipX = inputValue < 0f;
	}

	// 아무것도 안함
	void StayIdle()
	{
		return;
	}

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
