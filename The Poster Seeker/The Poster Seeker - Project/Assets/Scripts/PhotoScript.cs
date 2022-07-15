using UnityEngine.UI;
using UnityEngine;

public class PhotoScript : MonoBehaviour
{
    public GameObject player;
    public Image img;

    // Start is called before the first frame update
    void Start()
    {
        img.enabled = false;
        img.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Play("CollectSound");
            player.GetComponent<PlayerMovement>().counter++;
            if(player.GetComponent<PlayerMovement>().counter == 10)
                FindObjectOfType<AudioManager>().Play("VictorySound");
            img.enabled = true;
        }  
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            img.enabled = false;
            Destroy(this.gameObject);
        }
    }
}