using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightWall : MonoBehaviour
{
   public float input { get; set; }

   protected GameObject[] _lightWals;

   public MeshRenderer[] _meshRenderer;

    [SerializeField]
    float noize_x;

    [SerializeField]
    float noize_y;

    // Start is called before the first frame update
    void Start()
    {
        _lightWals = new GameObject[transform.childCount];
        _meshRenderer = new MeshRenderer[transform.childCount];

        for (int i = 0; i < _lightWals.Length; i++)
        {        

            _lightWals[i] = transform.GetChild(i).gameObject;
            _meshRenderer[i] = _lightWals[i].GetComponent<MeshRenderer>();
        }

    }
    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < _lightWals.Length; i++)
        {
            _meshRenderer[i].material.SetFloat("_noize_x", noize_x + (input * 10));
            _meshRenderer[i].material.SetFloat("_noize_y", noize_y + (input * 10));
        }

    }
}
