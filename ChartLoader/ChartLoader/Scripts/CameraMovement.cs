using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    private float _speed;

    /// <summary>
    /// The Camera Speed.
    /// </summary>
    public float Speed
    {
        get 
        { 
            return _speed; 
        }
        set 
        { 
            _speed = value; 
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(Speed * Vector3.forward * Time.deltaTime, Space.World);
	}
}
