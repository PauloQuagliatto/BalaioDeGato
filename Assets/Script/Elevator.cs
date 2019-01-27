using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
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
            if(transform.position.y>= 31.4f)
            {
                status = false;
            }
        }
        else
        {
            transform.Translate(Vector3.down*Time.deltaTime);
            if (transform.position.y <= 18.4f)
            {
                status = true;
            }
        }
    }
}
