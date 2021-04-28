using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 20.0f;
    private Transform tr;
    
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(this.gameObject, 3f);
    }
}
