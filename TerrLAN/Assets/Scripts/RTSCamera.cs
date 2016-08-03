using UnityEngine;
using System.Collections;

public class RTSCamera : MonoBehaviour {

	public Vector2 rotationSens = new Vector2 (0.5f, -0.5f);
	public Vector2 translationSens = new Vector2 (1, 1);
	public float zoomSens = 1;

	public Vector2 maxTranslation;
	public Vector2 minTranslation;
	public float maxHeight = 100;

	bool isRotating = false;
	Vector3 mouseStartPosition;
	Vector3 startRotation;

	// Update is called once per frame
	void Update () {
		//Listen for RMB
		if (Input.GetKey (KeyCode.Mouse1)) {
			if (!isRotating) {
				isRotating = true;
				mouseStartPosition = Input.mousePosition;
				startRotation = transform.rotation.eulerAngles;
			} else {
				Vector3 curMousePosition = Input.mousePosition;

				transform.rotation = Quaternion.Euler (new Vector3 (startRotation.x + ((curMousePosition.y - mouseStartPosition.y) *  rotationSens.y), startRotation.y  + ((curMousePosition.x - mouseStartPosition.x) * rotationSens.x), 0));
			}
		} 
		else 
		{
			isRotating = false;
		}

		transform.Translate (Input.GetAxis ("Horizontal") * translationSens.x, Input.GetAxis ("Vertical") * Mathf.Sin(transform.rotation.eulerAngles.x * (3.14f / 180)) * translationSens.y, Input.GetAxis ("Vertical") * Mathf.Cos(transform.rotation.eulerAngles.x * (3.14f / 180)) + (Input.GetAxis("Mouse ScrollWheel") * zoomSens) * translationSens.y, Space.Self);

		/*if (transform.position.x > maxTranslation.x)
			transform.position = new Vector3 (maxTranslation.x, transform.position.y, transform.position.z);
	
		if (transform.position.x < minTranslation.x)
			transform.position = new Vector3 (minTranslation.x, transform.position.y, transform.position.z);


		if (transform.position.y > maxTranslation.y)
			transform.position = new Vector3 (transform.position.x, transform.position.y, maxTranslation.y);

		if (transform.position.x < minTranslation.y)
			transform.position = new Vector3 (transform.position.x, transform.position.y, minTranslation.y);
*/
	}
}
