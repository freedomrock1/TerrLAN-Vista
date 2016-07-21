using UnityEngine;
using System.Collections;

public class teleportControl : MonoBehaviour {

    GameObject model;
    GameObject modelPosition;
    GameObject building; //requires a building in the scene tagged as "Building"
    bool isMoved = false;

    // Use this for initialization
    void Start () {
        modelPosition = new GameObject();
        modelPosition.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - .5f, this.gameObject.transform.position.z);
        modelPosition.transform.Translate(this.gameObject.transform.forward * 1.5f, this.gameObject.transform);
        modelPosition.transform.parent = this.gameObject.transform;
        modelPosition.name = "modelPosition";//all this code sets up a modelPosition for you, so you don't have to pack another prefab around with this script.
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.T))//when you push T, code retrieves the teleport model from the floor
        {
            if (isMoved)
            {
                Materialize(ref model);
            }
            else
            {
                Ray downRay = new Ray(this.gameObject.transform.position, Vector3.down);
                RaycastHit hit;
                Physics.Raycast(downRay, out hit);
                while (hit.collider.gameObject.tag == "Player")
                {
                    Physics.Raycast(downRay, out hit);
                }
                if (hit.collider.gameObject.transform.GetChild(0).tag == "teleportModel" | hit.collider.gameObject.transform.GetChild(0).name.Contains("Model"))
                {
                    model = hit.collider.gameObject.transform.GetChild(0).gameObject;
                }
                Materialize(ref model);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))//if you click
        {
            if (isMoved)//and there's a model in front of you
            {
                Teleport(this.gameObject);//we teleport boys
            }
        }
        if (Input.GetKeyDown(KeyCode.R))//some simple code to change floors. Modify input as you please.
        {
            ChangeFloor(this.gameObject, true);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeFloor(this.gameObject, false);
        }

    }

    public bool IsMoved
    {//A property of the ismoved bool. The minimap likes to see it every once in a while.
        get { return isMoved; }
    }

    void ChangeFloor(GameObject player, bool direction = true) //player is the gameobject to move, direction is up or down. true is up, down is false.
    {
        float[] scaleList = { player.transform.localScale.x, player.transform.localScale.y, player.transform.localScale.z };
        float scale = Mathf.Max(scaleList); //Finds largest scale value to allow for differently sized gameobjects with varying symmetry.
        scale = scale / 2;
        if (direction)
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

    void Materialize(ref GameObject model)//a parent function to call positiontoggle and childmeshtoggle in the right order depending on if we're getting the model or putting it away
    {//positiontoggle sets model to null when you're putting it away, y'see?
        if (!isMoved)
        {
            PositionToggle(ref model);
            ChildMeshToggle(ref model);
        }
        else
        {
            ChildMeshToggle(ref model);
            PositionToggle(ref model);
        }
    }

    void PositionToggle(ref GameObject model)
    {//retrieves or removes the model
        if (!isMoved)
        {
            isMoved = true;
            Ray ray = new Ray(this.gameObject.transform.position, Vector3.down);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            while (hit.collider.gameObject.tag == "Player")
            {
                Physics.Raycast(ray, out hit);
            }
            for (int i = 0; i < hit.collider.gameObject.transform.childCount; i++)
            {
                if (hit.collider.gameObject.transform.GetChild(i).name.Contains("Model") | hit.collider.gameObject.transform.GetChild(i).tag == "teleportModel")
                {
                    model = hit.collider.gameObject.transform.GetChild(i).gameObject;
                    break;
                }
            }
            model.transform.parent = null;
            model.transform.position = modelPosition.transform.position;
            model.transform.rotation = modelPosition.transform.rotation;
        }
        else
        {
            isMoved = false;
            RaycastHit hit;
            Physics.Raycast(new Ray(model.transform.position, Vector3.down), out hit);
            model.transform.parent = hit.collider.gameObject.transform;
            model.transform.position = hit.point;
            model.transform.rotation = Quaternion.identity;
            model = null;
        }
    }

    void Teleport(GameObject player)
    {//teleports the player to the desired position
        Camera camera = player.GetComponentInChildren<Camera>();
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Ray floorRay = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(floorRay, out hit);
        while (hit.collider.gameObject.tag == "teleportModel")
        {
            floorRay.origin = hit.point;
            Physics.Raycast(floorRay, out hit);
        }
        GameObject floor = hit.collider.gameObject;
        GameObject temp = hit.collider.gameObject;
        while (temp.transform.parent != null)
        {
            temp = temp.transform.parent.gameObject;
        }
        building = temp;
        RaycastHit[] hitArray;
        hitArray = Physics.RaycastAll(ray);
        foreach (RaycastHit h in hitArray)
        {
            if (h.collider.gameObject.name.Contains("Model") | h.collider.gameObject.tag.Contains("Model"))
            {
                GameObject pointer = new GameObject();
                pointer.transform.position = h.point;
                pointer.transform.parent = model.transform;
                model.transform.rotation = Quaternion.identity;
                Vector3 output = new Vector3(pointer.transform.position.x - h.collider.gameObject.transform.position.x, pointer.transform.position.y - h.collider.gameObject.transform.position.y, pointer.transform.position.z - h.collider.gameObject.transform.position.z);
                output.x = output.x * building.transform.localScale.x;
                output.y = output.y * building.transform.localScale.y;
                output.z = output.z * building.transform.localScale.z;
                player.transform.position = new Vector3(floor.transform.position.x + output.x, player.transform.position.y, floor.transform.position.z + output.z);
                Destroy(pointer);
            }
        }
        Materialize(ref model);
    }

    public void ChildMeshToggle(ref GameObject model)
    {
        model.GetComponent<BoxCollider>().enabled = !model.GetComponent<BoxCollider>().enabled;
        foreach (MeshRenderer m in model.GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = !m.enabled;
        }
    }
}
