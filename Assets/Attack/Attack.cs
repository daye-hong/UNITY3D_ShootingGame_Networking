using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float speed = 10.0f;
    private Transform tr;
    
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    void Update()
    {
        tr.Translate(Vector3.left * speed * Time.deltaTime);
        Destroy(this.gameObject,3f);
    }
}
