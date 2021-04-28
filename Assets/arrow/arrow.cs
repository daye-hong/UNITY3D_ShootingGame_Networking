using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow : MonoBehaviour
{
    public float speed = 10f;
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
        //tr.rotation = Quaternion.Euler(0, -90, 90);
    }

    void Update()
    {
        tr.Translate(Vector3.forward * speed * Time.deltaTime);
        //Destroy(this.gameObject, 3f);
    }
}
