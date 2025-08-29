using TMPro;
using UnityEngine;

public class NumberText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TMP_Text tmp;
    public float speed = 0f;

    void Start()
    {
		tmp = GetComponent<TMP_Text>();
    }

	private void Update()
	{
        var curPos = transform.position;
        transform.position = curPos + new Vector3(0, speed * Time.deltaTime, 0);
	}

	public void SetText(string text)
    {
        if (tmp == null)
        {
            tmp = GetComponent<TMP_Text>();
        }
        tmp.text = text;
    }
}
