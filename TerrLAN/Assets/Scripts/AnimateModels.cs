using UnityEngine;
using System.Collections;

public class AnimateModels : MonoBehaviour {

    //Selection to determine what kind of animation to apply to the object
    public enum AnimType { Rotate, Expand, Shake }
    public AnimType currType = AnimType.Rotate;

    //How fast to rotate the object. Negative values will rotate to the right
    public float rotSpeed = .5f;

    //Int to determine which direction the object will be expanding/contracting
    int expandDirection = 1;
    //Ratio to determine how much to expand at maximum/minimum scaling
    public float expandRatio = .5f;
    //The current change in scaling
    float expandAmt = 0;
    //The original scale of the object
    Vector3 originalScale;

    //Int to determine which direction the object will be expanding/contracting
    int shakeDirection = 1;
    //Is the object moving in the x direction?
    bool isXDirection = true;
    //Is the object ready to switch direction?
    bool isDirReady = false;
    //Ratio to determine how much to expand at maximum/minimum scaling
    public float shakeRatio = .5f;
    //The current change in scaling
    float shakeAmt = 0;

    // Use this for initialization
    void Start () {
        originalScale = transform.localScale;
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
        //Based on the AnimType Enum...
        switch(currType){
            //If it is Rotate...
            case AnimType.Rotate:
                //Rotate around the y axis at |rotSpeed| quickness
                transform.Rotate(new Vector3(0, rotSpeed, 0));
                break;

            //If it is Expand...
            case AnimType.Expand:
                //Add an value scaled by deltaTime, the set ratio and the direction of the scaling
                expandAmt += Time.deltaTime * expandDirection;
                //If that value is over 1 times the ratio...
                if (expandAmt >= 1){
                    //Set the the Direction to contract
                    expandDirection = -1;
                    expandAmt = 1;
                }
                //If that value is under -1 times the ratio...
                if (expandAmt <= -1){
                    //Set the Direction to expand
                    expandDirection = 1;
                    expandAmt = -1;
                }
                Debug.Log(expandAmt * expandRatio);
                //Apply the set value to the original scale and then set that as the current scaling
                transform.localScale = originalScale + new Vector3(expandAmt * expandRatio, expandAmt * expandRatio, expandAmt * expandRatio);
                break;

            case AnimType.Shake:
                //Add an value scaled by deltaTime, the set ratio and the direction of the shaking
                shakeAmt += Time.deltaTime * shakeDirection;

                //If the direction is moving positive, the object is ready to change direction, and the object is moving past the halfway point...
                if (shakeDirection == 1 && isDirReady && shakeAmt >= 0){
                    //Reverse the current direction the object is moving in
                    isXDirection = !isXDirection;
                    //Reset the movement to the center of the object
                    shakeAmt = 0;
                    //Reset the object being ready to move the current direction
                    isDirReady = false;
                }

                //If the value is higher than 1 times the ratio...
                if (shakeAmt >= 1 && shakeDirection == 1)
                {
                    //Set the the Direction to negative
                    shakeDirection = -1;
                    shakeAmt = 1;
                }
                //If that value is under -1 times the ratio...
                if (shakeAmt <= -1 && shakeDirection == -1)
                {
                    //Set the Direction to positive
                    shakeDirection = 1;
                    shakeAmt = -1;
                    //Set the bool so the next time it moves in this direction, it changes direction midway
                    isDirReady = true;
                }

                Debug.Log(shakeAmt * shakeRatio);
                //If isXDir true, move the object along the X axis
                if (isXDirection){
                    transform.position += new Vector3(shakeAmt * shakeRatio, 0, 0);
                }
                //Else, move the object along the Z axis
                else {
                    transform.position += new Vector3(0, 0, shakeAmt * shakeRatio);
                }
                break;
        }
	
	}
}
