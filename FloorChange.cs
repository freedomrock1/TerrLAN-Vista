using UnityEngine;
using System.Collections;

public class FloorChange : MonoBehaviour {

    float speed;

	// Use this for initialization
	void Start () {
        speed = 10f;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKey(KeyCode.W))
        {//Z+
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, speed));
        }
        if (Input.GetKey(KeyCode.S))
        {//Z-
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -speed));
        }
        if (Input.GetKey(KeyCode.A))
        {//X-
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(-speed, 0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {//X+
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(speed, 0, 0));
        }
        */
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeFloor(this.gameObject, true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeFloor(this.gameObject, false);
        }
        
    }

    void ChangeFloor(GameObject player, bool direction=true) //player is the gameobject to move, direction is up or down. true is up, down is false.
    {
        float[] scaleList = { player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z };
        float scale = Mathf.Max(scaleList); //Finds largest scale value to allow for differently sized gameobjects with varying symmetry.
        scale = scale / 2;
        if(direction)
        {
            Ray ray = new Ray(player.transform.position, Vector3.up);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                //player.transform.position = new Vector3(player.transform.position.x, hit.point.y - player.transform.localScale.y, player.transform.position.z);
                ray.origin = new Vector3(hit.point.x, hit.point.y + scale + .2f, hit.point.z);
                ray.direction = Vector3.down;
            }
            if (Physics.Raycast(ray, out hit))
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                player.transform.position = new Vector3(player.transform.position.x, hit.point.y + scale, player.transform.position.z);
            }
        }
        else
        {
            Ray ray = new Ray(player.transform.position, Vector3.down);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                //player.transform.position = new Vector3(player.transform.position.x, hit.point.y - player.transform.localScale.y, player.transform.position.z);
                ray.origin = new Vector3(hit.point.x, hit.point.y - scale, hit.point.z);
            }
            if (Physics.Raycast(ray, out hit))
            {
                player.transform.rotation = Quaternion.Euler(0, 0, 0);
                player.transform.position = new Vector3(player.transform.position.x, hit.point.y + scale, player.transform.position.z);
            }
        }
    }
}
