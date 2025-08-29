using UnityEngine;
using static Define;

public class NumberTextManager : MonoBehaviour
{
    public GameObject numberPrefab;
    public float lifeTime = 0.3f;
    public Vector2 offset = Vector2.zero;   
    private PlayerController[] characters;

	private void Start()
	{
        characters = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var character in characters)
        {
            character.OnDamage.AddListener(CreateNumberText);
        }
	}

	public void CreateNumberText(DamageContext ctx)
    {
        Vector2 spawnPos = ctx.Victim.transform.position;
		var go = Instantiate(numberPrefab, spawnPos + offset, Quaternion.identity);
        var tmp = go.GetComponent<NumberText>();
		if (tmp != null )
        {
            tmp.SetText(ctx.Damage.ToString());
        }

        Destroy(go, lifeTime);
	}
}
