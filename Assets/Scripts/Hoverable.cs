using UnityEngine;

public class Hoverable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //throw new NotImplementedException();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //this.gameObject.GetComponentInChildren<SpriteRenderer>()?.sprite.border;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //throw new NotImplementedException();
    }
}
