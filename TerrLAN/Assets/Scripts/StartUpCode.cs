using UnityEngine;
using System.Collections;
using Terr01;

public class StartUpCode : MonoBehaviour {

	public Model newModel;

	public GameObject WindowsWorkstation, LinixWorkstation, Printer, Router, Switch, Firewall;

    string floorPlan, networkPlan;

	// Use this for initialization
	void Start () {

        GameObject info = GameObject.FindWithTag("Information");

        floorPlan = info.GetComponent<info>().strFloorPlan;
        networkPlan = info.GetComponent<info>().strNetwork;

        newModel = new Model();
        if (networkPlan != "")
        {
            newModel.loadNetFile(networkPlan + ".csv");
        }
        else
        {
            newModel.loadNetFile("");
        }

        SpawnObjects();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void SpawnObjects (){
        if (floorPlan != "")
        {
            Instantiate(Resources.Load(floorPlan) as GameObject, new Vector3(0, 0, 0), this.transform.rotation);
        }
        else
        {
            Instantiate(Resources.Load("FloorPlan1") as GameObject, new Vector3(0, 0, 0), this.transform.rotation);
        }
		foreach (Terr01.Device d in newModel.network) {
			GameObject hold;
			switch (d.type) {
			case Terr01.DeviceType.WorkStation:
				hold = (GameObject)Instantiate (WindowsWorkstation, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                hold.GetComponent<WorkstationWindows>();
				break;
			case Terr01.DeviceType.Printer:
				hold = (GameObject)Instantiate (Printer, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                    hold.GetComponent<Printer>();
                    break;
			case Terr01.DeviceType.Router:
				hold = (GameObject)Instantiate (Router, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                    hold.GetComponent<Router>();
                    break;
			case Terr01.DeviceType.Switch:
				hold = (GameObject)Instantiate (Switch, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                    hold.GetComponent<Switch>();
                    break;
			case Terr01.DeviceType.Firewall:
				hold = (GameObject)Instantiate (Firewall, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                    hold.GetComponent<Firewall>();
                    break;
			default:
				hold = (GameObject)Instantiate (WindowsWorkstation, new Vector3 (d.loc.x, d.loc.z, d.loc.y), this.transform.rotation);
				hold.name = d.name;
                    hold.GetComponent<WindowsWorkstation>();
                    break;
			}
		}
	}
}
