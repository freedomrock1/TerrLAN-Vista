using UnityEngine;
using System.Collections;

public class info : MonoBehaviour {

    public string city;
    public float latitude;
    public float longitude;
    public int buildingNum;
    public string nextScene;
    public string strFloorPlan;
    public string strNetwork;

    // Use this for initialization
    void Start () {
        Application.LoadLevel(nextScene);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
