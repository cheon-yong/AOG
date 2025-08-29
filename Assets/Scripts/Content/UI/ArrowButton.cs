using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ArrowButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{

	[SerializeField] Button button;
	[SerializeField] PlayerController player;
	[SerializeField] float dir;

	bool held = false;

	public void Start()
	{
		button = GetComponent<Button>();

		//button.onClick.AddListener(OnClick);
	}

	private void Update()
	{
		if (held)
		{
			player.MoveByButton(dir);
		}
		else
		{
			player.MoveByButton(0);
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		held = true;
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		held = false;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		held = false;
	}
}
