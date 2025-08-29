using TMPro;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float startTime = 99f;
    public float curTime;
	public TMP_Text timeText;

	bool playing = false;

	private void Awake()
	{
	}

    void GameStart()
    {
        playing = true;
    }

    void GameEnd(bool clear)
    {
        playing = false;
    }

	void Start()
    {
        curTime = startTime;

		GameManager.Instance.OnGameStart.AddListener(GameStart);
		GameManager.Instance.OnGameEnd.AddListener(GameEnd);

        GameStart();
	}

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            curTime -= Time.deltaTime;
            timeText.text = ((int)curTime).ToString();
        }
    }
}
