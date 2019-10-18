using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mu_Sp : MonoBehaviour
{
    public float input { get; set; }


    public GameObject[] Pa;

    ParticleSystem.MainModule mainEmo_1;
    ParticleSystem.MainModule mainEmo_2;
    ParticleSystem.MainModule mainEmo_3;
    ParticleSystem.MainModule mainEmo_4;
   

    [SerializeField]
    protected float Emo_1_Size;
    [SerializeField]
    protected float Emo_2_Size;
    [SerializeField]
    protected float Emo_3_Size;
    [SerializeField]
    protected float Emo_4_Size;   
    [SerializeField]
    protected float normalInputSize;
    [SerializeField]
    protected float highInputSize;

    // Start is called before the first frame update
    void Start()
    {
        Emo_1_Size = 1.5f;
        Emo_2_Size = 2.5f;
        Emo_3_Size = 3.5f;
        Emo_4_Size = 0.78f;

        Pa = new GameObject[transform.childCount];

        for (int i = 0; i < Pa.Length; i++)
        {
            Pa[0] = transform.GetChild(0).gameObject;
            Pa[1] = transform.GetChild(1).gameObject;
            Pa[2] = transform.GetChild(2).gameObject;
            Pa[3] = transform.GetChild(3).gameObject;


            var _emo1 = Pa[0].GetComponent(typeof(ParticleSystem)) as ParticleSystem;
            mainEmo_1 = _emo1.main;

            var _emo2 = Pa[1].GetComponent(typeof(ParticleSystem)) as ParticleSystem;
            mainEmo_2 = _emo2.main;

            var _emo3 = Pa[2].GetComponent(typeof(ParticleSystem)) as ParticleSystem;
            mainEmo_3 = _emo3.main;

            var _emo4 = Pa[3].GetComponent(typeof(ParticleSystem)) as ParticleSystem;
            mainEmo_4 = _emo4.main;

          
        }

    }

    // Update is called once per frame
    void Update()
    {
        mainEmo_1.startSize = (input * normalInputSize) + Emo_1_Size;
        mainEmo_2.startSize = (input * normalInputSize) + Emo_2_Size;
        mainEmo_3.startSize = (input * normalInputSize) + Emo_3_Size;
        mainEmo_4.startSize = (input * highInputSize) + Emo_4_Size;

    }
}
