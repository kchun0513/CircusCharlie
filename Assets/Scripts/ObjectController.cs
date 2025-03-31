using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public GameObject ObjGenerator;
    public int minSpeed;
    public int maxSpeed;
    private int speed; 
    // Start is called before the first frame update
    void Start()
    {
        minSpeed = -minSpeed;
        maxSpeed = -maxSpeed;
        speed = Random.Range(maxSpeed, minSpeed+1);
        transform.position = ObjGenerator.transform.position;
        print(speed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
