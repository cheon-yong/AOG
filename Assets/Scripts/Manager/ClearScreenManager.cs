using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearScreenManager : MonoBehaviour
{
    public Image blackScreen;
	public TMP_Text clearText;
	public float duration = 1f;
	public float time = 0f;

	private void Start()
	{
		blackScreen.color = new Color(0, 0, 0, 255);

		GameManager.Instance.OnGameStart.AddListener(GameStart);
		GameManager.Instance.OnGameEnd.AddListener(GameEnd);
	}

	public void GameStart()
	{
		blackScreen.color = new Color(0, 0, 0, 0);
		blackScreen.gameObject.SetActive(false);
		time = 0f;
	}

	public void GameEnd(bool clear)
	{
		clearText.text = clear ? "WIN" : "LOSE";
		StartCoroutine(MakeBlack());
	}

	public IEnumerator MakeBlack()
	{
		blackScreen.gameObject.SetActive(true);

		Color c1 = Color.black;
		Color c2 = Color.white;
		while (time < duration)
		{
			time += Time.deltaTime;
			float k = Mathf.Clamp01(time / duration);
			c1.a = Mathf.Lerp(0, 1, k);
			c2.a = Mathf.Lerp(0, 1, k);

			blackScreen.color = c1;
			clearText.color = c2;
			yield return null;
		}

		yield break;
	}
}
