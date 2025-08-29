using UnityEngine;
using UnityEngine.Events;

public class GroundProbe : MonoBehaviour
{
	public UnityEvent OnCliff;
    Collider2D probe;

	private void Awake()
	{
		probe = GetComponent<Collider2D>();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.layer == LayerNumber.Platform)
		{
			Debug.Log("Exit");
			OnCliff.Invoke();
		}
	}
}
