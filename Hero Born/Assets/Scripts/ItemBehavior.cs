using UnityEngine;

public class ItemBehavior : MonoBehaviour
{

    public GameBehavior GameManager;

    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameBehavior>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Destroy(this.transform.gameObject);

            Debug.Log("Item collected!");
            GameManager.Items += 1;
        }
    }


}
