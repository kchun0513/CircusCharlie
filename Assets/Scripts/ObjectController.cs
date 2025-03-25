using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject ObjGenerator;
    public int speed;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = ObjGenerator.transform.position;
        print(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }


}
