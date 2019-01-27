using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator1 : MonoBehaviour
{
    public bool status=true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (status)
        {
            transform.Translate(Vector3.up*Time.deltaTime);
            if(transform.position.y>= 12.7f)
            {
                status = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down*Time.deltaTime);
            if (transform.position.y <= -2.291f)
            {
                status = true;
            }
        }
    }
}
