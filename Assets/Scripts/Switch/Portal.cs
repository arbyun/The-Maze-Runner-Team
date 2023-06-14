using UnityEngine;

public class Portal : MonoBehaviour 
{
    public GameObject portalToTP;
    private GameObject player;
    
    void Start()
    {
        player = GlobalController.Instance.player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.transform.position = new Vector2(portal.transform.position.x, portal.transform.position.y);
        }
    }
}