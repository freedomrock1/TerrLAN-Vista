using UnityEngine;
using System.Collections;

public class UIcontroller : MonoBehaviour {

    GameObject model;
    GameObject modelPosition;
    GameObject building; //requires a building in the scene tagged as "Building"
    Vector3 modelScale;
    bool isMoved = false;

	// Use this for initialization
	void Start () {
        building = GameObject.FindGameObjectWithTag("Building");
        modelPosition = new GameObject();
        modelPosition.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - .5f, this.gameObject.transform.position.z);
        modelPosition.transform.Translate(this.gameObject.transform.forward*1.5f, this.gameObject.transform);
        modelPosition.transform.parent = this.gameObject.transform;
        /*
        foreach(Transform t in building.GetComponentsInChildren<Transform>())
        {
            if(t.tag == "teleportModel")
            {
                model = t.gameObject;
                break;
            }
        }
        model.transform.position = modelPosition.transform.position;
        model.transform.parent = modelPosition.transform;
        modelScale = model.transform.localScale;*/
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.T))
        {
            Materialize(ref model);
        }

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(isMoved)
            {
                Teleport(this.gameObject);
            }
        }
	
	}

    void Materialize(ref GameObject model)
    {
        if(!isMoved)
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
    {
        if(!isMoved)
        {
            isMoved = true;
            Ray ray = new Ray(this.gameObject.transform.position, Vector3.down);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            for(int i=0;i<hit.collider.gameObject.transform.childCount;i++)
            {
                if(hit.collider.gameObject.transform.GetChild(i).tag == "teleportModel")
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

    void ChildMeshToggle(ref GameObject model)
    {
        model.GetComponent<BoxCollider>().enabled = !model.GetComponent<BoxCollider>().enabled;
        foreach (MeshRenderer m in model.GetComponentsInChildren<MeshRenderer>())
        {
            m.enabled = !m.enabled;
        }
    }

    void Teleport(GameObject player)
    {
        Camera camera = player.GetComponentInChildren<Camera>();
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Ray floorRay = new Ray(player.transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(floorRay, out hit);
        if(hit.collider.gameObject.tag == "teleportModel")
        {
            floorRay.origin = hit.point;
            Physics.Raycast(floorRay, out hit);
        }
        GameObject floor = hit.collider.gameObject;
        Physics.Raycast(ray, out hit);
        if(hit.collider.gameObject.tag == "teleportModel")
        {
            GameObject pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointer.transform.position = hit.point;
            pointer.transform.parent = model.transform;
            model.transform.rotation = Quaternion.identity;
            Vector3 output = new Vector3(pointer.transform.position.x - hit.collider.gameObject.transform.position.x, pointer.transform.position.y - hit.collider.gameObject.transform.position.y, pointer.transform.position.z - hit.collider.gameObject.transform.position.z);
            output.x = output.x * building.transform.localScale.x;
            output.y = output.y * building.transform.localScale.y;
            output.z = output.z * building.transform.localScale.z;
            player.transform.position = new Vector3(floor.transform.position.x + output.x, player.transform.position.y, floor.transform.position.z + output.z);
            Destroy(pointer);
        }
        ChildMeshToggle(ref model);
        PositionToggle(ref model);
    }
}
