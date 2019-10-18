using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movinglight : MonoBehaviour
{

    public Transform target;
    public Transform PartTorotate;
    [SerializeField]
    protected float turnspeed;

    // Start is called before the first frame update
    void Start()
    {
        turnspeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(PartTorotate.rotation, lookRotation, Time.deltaTime * turnspeed).eulerAngles;
        PartTorotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f);

    }
}