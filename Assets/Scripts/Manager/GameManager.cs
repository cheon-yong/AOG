using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using static Define;

public class GameManager : MonoBehaviour
{	
    private PlayerController[] characters;

	public UnityEvent OnGameStart;
	public UnityEvent<bool> OnGameEnd;

	private static GameManager instance = null;

	void Awake()
	{
		if (null == instance)
		{
			instance = this;

		}
		else
		{
			Destroy(this.gameObject);
		}

		characters = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
		foreach (var character in characters)
		{
			character.OnDeath.AddListener(CharacterDeath);
		}
	}

	public static GameManager Instance
	{
		get
		{
			if (null == instance)
			{
				return null;
			}

			return instance;
		}
	}

	public void CharacterDeath(DamageContext ctx)
	{
		// 사망자가 몬스터면
		if (ctx.Victim.GetComponent<MonsterController>() != null)
		{
			OnGameEnd.Invoke(true);
		}
		else // 플레이어라면
		{
			OnGameEnd.Invoke(true);
		}
		
	}

	public void Start()
	{
		OnGameStart.Invoke();
	}
}
