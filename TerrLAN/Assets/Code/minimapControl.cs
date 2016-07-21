using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class minimapControl : MonoBehaviour {

    public GameObject minimapPosition; //A gameobject positioned in view of a camera for minimap screen rendering
    public GameObject feetPosition; //A gameobject positioned as close to the bottom of the character model as possible for in-house collision detection
    List<Collider> collisions = new List<Collider>(); //A list of colliders hitting the feetPosition. Unity's built in collision detection annoyed me, hence this.
    bool mapVisible = false;
    GameObject minimap = null;
    GameObject minimapBuilding = null; //minimap and building are values assigned when a minimap is found, null otherwise.
    public GameObject placementSphere; //What you want to indicate the player on the minimap
    GameObject placementMarker; //properly instantiated version of the above
    public int minimapLayer;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if(!this.gameObject.GetComponent<teleportControl>().IsMoved) //don't bother updating the minimap if the teleport script stole the floor's teleport model
        {
            collisions.Clear();
            Collider[] temp = Physics.OverlapSphere(feetPosition.transform.position, 0.25f);
            foreach (Collider co in temp)//update collider list for this frame
            {
                collisions.Add(co);
            }

            bool touchingFloor = false;
            foreach (Collider c in collisions)
            {
                if (c.gameObject.name.Contains("Floor"))
                {
                    touchingFloor = true;
                }
            }
            if (mapVisible)
            {
                if (!touchingFloor)//this code removes the minimap if you're not standing on a floor with a model anymore
                {
                    UnloadMinimap(ref minimap, ref minimapBuilding);
                    mapVisible = false;
                }
                else
                {
                    Ray ray = new Ray(this.gameObject.GetComponentInChildren<Camera>().transform.position, Vector3.down);
                    RaycastHit[] hitList = Physics.RaycastAll(ray);
                    foreach (RaycastHit hit in hitList)
                    {
                        if (hit.collider.gameObject.name.Contains("Floor") && Vector3.Distance(this.gameObject.transform.position, hit.point) < 10)
                        {//these are the calculations to find the relative player position for marker placement
                            Vector3 output = new Vector3(hit.point.x - hit.collider.gameObject.transform.position.x, hit.point.y - hit.collider.gameObject.transform.position.y, hit.point.z - hit.collider.gameObject.transform.position.z);
                            output.x = output.x / minimapBuilding.transform.localScale.x;
                            output.y = output.y / minimapBuilding.transform.localScale.y;
                            output.z = output.z / minimapBuilding.transform.localScale.z;
                            placementMarker.transform.position = new Vector3(minimap.transform.position.x + output.x, minimap.transform.position.y, minimap.transform.position.z + output.z);
                        }
                    }
                }
            }
            else
            {
                if (touchingFloor)//this code loads the minimap if you touch a floor with a model
                {
                    LoadMinimap(ref minimap, ref minimapBuilding);
                    mapVisible = true;
                }
            }
        }
    }

    void LoadMinimap(ref GameObject minimap, ref GameObject minimapBuilding) //retrieves and instantiates a copy of the teleport model attached to the floor the player is standing on
    {
        foreach(Collider c in collisions)
        {
            if (c.gameObject.name.Contains("Floor"))
            {
                foreach(Transform t in c.gameObject.transform.GetComponentsInChildren<Transform>())
                {
                    if(t.gameObject.name.Contains("Model") | t.gameObject.tag.Contains("Model"))//About the comments below: at the time of making this, the only building I had access to had some issuse with the teleport models scaling going wonky after it was instantiated. This is expected to be fixed in future with more reliable floor models, at which time this code will have to be changed
                    {
                        minimap = Instantiate(t.gameObject);
                        //minimap.transform.localScale = t.gameObject.transform.localScale; enable this line when the input models are their expected scale
                        minimap.transform.localScale = new Vector3(0.1932911f, 0.04328918f, 0.1932911f); //at that time, disable this line.
                        break;
                    }
                }
                GameObject temp = c.gameObject;
                while (temp.transform.parent != null)
                {
                    temp = temp.transform.parent.gameObject;
                }
                minimapBuilding = temp;//assigns the models building. This is used for scaling in the position calculations
            }
        }
        this.gameObject.GetComponent<teleportControl>().ChildMeshToggle(ref minimap); //calls a handy method from the teleport class to toggle the model's visibility
        minimap.transform.parent = minimapPosition.transform;
        minimap.transform.position = minimapPosition.transform.position;
        foreach(Transform t in minimap.transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = minimapLayer; //sets the model and its children to the minimap layer, for rendering in the camera.
        }
        placementMarker = Instantiate(placementSphere);
        placementMarker.layer = minimapLayer;
        placementMarker.transform.parent = minimap.transform;
    }

    void UnloadMinimap(ref GameObject minimap, ref GameObject minimapBuilding)
    {
        Destroy(minimap);
        minimapBuilding = null;
        Destroy(placementMarker);
    }
}
