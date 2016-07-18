﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class UIcontroller : MonoBehaviour {

    public GameObject minimapPosition;
    public GameObject feetPosition;
    GameObject model;
    GameObject modelPosition;
    GameObject building; //requires a building in the scene tagged as "Building"
    Vector3 modelScale;
    List<Collider> collisions = new List<Collider>();
    bool isMoved = false;
    bool mapVisible = false;
    GameObject minimap = null;
    GameObject minimapBuilding = null;

	// Use this for initialization
	void Start () {
        modelPosition = new GameObject();
        modelPosition.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y - .5f, this.gameObject.transform.position.z);
        modelPosition.transform.Translate(this.gameObject.transform.forward*1.5f, this.gameObject.transform);
        modelPosition.transform.parent = this.gameObject.transform;
    }
	
	// Update is called once per frame
	void Update () {
        collisions.Clear();
        Collider[] temp = Physics.OverlapSphere(feetPosition.transform.position, 0.25f);
        foreach(Collider co in temp)
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
        if(mapVisible)
        {
            if(!touchingFloor)
            {
                UnloadMinimap(ref minimap, ref minimapBuilding);
                mapVisible = false;
            }
            else
            {
                //recalculate current position
            }
        }
        else
        {
            if(touchingFloor)
            {
                LoadMinimap(ref minimap, ref minimapBuilding);
                mapVisible = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Ray downRay = new Ray(this.gameObject.transform.position, Vector3.down);
            RaycastHit hit;
            Physics.Raycast(downRay, out hit);
            while(hit.collider.gameObject.tag == "Player")
            {
                Physics.Raycast(downRay, out hit);
            }
            if(hit.collider.gameObject.transform.GetChild(0).tag == "teleportModel" | hit.collider.gameObject.transform.GetChild(0).name.Contains("Model"))
            {
                model = hit.collider.gameObject.transform.GetChild(0).gameObject;
            }
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

    void LoadMinimap(ref GameObject minimap, ref GameObject minimapBuilding)
    {
        foreach(Collider c in collisions)
        {
            if (c.gameObject.name.Contains("Floor"))
            {
                foreach(Transform t in c.gameObject.transform.GetComponentsInChildren<Transform>())
                {
                    if(t.gameObject.name.Contains("Model") | t.gameObject.tag.Contains("Model"))
                    {
                        minimap = Instantiate(t.gameObject);
                        break;
                    }
                }
                GameObject temp = c.gameObject;
                while (temp.transform.parent != null)
                {
                    temp = temp.transform.parent.gameObject;
                }
                minimapBuilding = temp;
                Debug.Log("minimap assigned");
            }
        }
        ChildMeshToggle(ref minimap);
        minimap.transform.parent = minimapPosition.transform;
        minimap.transform.position = minimapPosition.transform.position;
        minimap.transform.localScale = new Vector3(1, .4f, 1);
        foreach(Transform t in minimap.transform.GetComponentsInChildren<Transform>())
        {
            t.gameObject.layer = 8;
        }
    }

    void UnloadMinimap(ref GameObject minimap, ref GameObject minimapBuilding)
    {
        Destroy(minimap);
        minimapBuilding = null;
        Debug.Log("Minimap unassigned");
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
            while (hit.collider.gameObject.tag == "Player")
            {
                Physics.Raycast(ray, out hit);
            }
            for (int i=0;i<hit.collider.gameObject.transform.childCount;i++)
            {
                if(hit.collider.gameObject.transform.GetChild(i).name.Contains("Model") | hit.collider.gameObject.transform.GetChild(i).tag == "teleportModel")
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
        while(hit.collider.gameObject.tag == "teleportModel")
        {
            floorRay.origin = hit.point;
            Physics.Raycast(floorRay, out hit);
        }
        GameObject floor = hit.collider.gameObject;
        GameObject temp = hit.collider.gameObject;
        while(temp.transform.parent != null)
        {
            temp = temp.transform.parent.gameObject;
        }
        building = temp;
        RaycastHit[] hitArray;
        hitArray = Physics.RaycastAll(ray);
        foreach(RaycastHit h in hitArray)
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
}
