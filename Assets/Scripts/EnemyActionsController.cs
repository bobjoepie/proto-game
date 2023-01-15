using UnityEngine;

public class EnemyActionsController : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var closestPoint = col.contacts[0].point;
            col.gameObject.GetComponent<PlayerControllerOld>()?.TakeDamage(damage, closestPoint);
        }
    }
}
