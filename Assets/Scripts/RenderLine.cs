using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]

public class RenderLine : BoxChain
{

    LineRenderer _lineRenderer;
    Vector3[] _lerpPosition;


    public Material _material;
    //public Color _color;
    private Material _matInstence;

    public float _emissionMultiply;


    private float[] _lerpAudio;

    public float input { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _lerpAudio = new float[_initiatorPointAmount];

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = true;
        _lineRenderer.useWorldSpace = false;
        _lineRenderer.loop = true;
        _lineRenderer.positionCount = _position.Length;
        _lineRenderer.SetPositions(_position);

        _lerpPosition = new Vector3[_position.Length];

        _matInstence = new Material(_material);
        _lineRenderer.material = _matInstence;
    }

    // Update is called once per frame
    void Update()
    {
        //_matInstence.SetColor("_EmissionColor", _color * (input * 2.5f) * _emissionMultiply);
        //_matInstence.EnableKeyword("_EMISSION");

        _matInstence.SetFloat("_emissionColor", (input * 2.5f) * _emissionMultiply);

        //レンダー描画

        if (_generationCount != 0)
        {
            int count = 0;

            for (int i = 0; i < _initiatorPointAmount; i++)
            {
                //_initiatorPointAmount分のfor
                for (int j = 0; j < (_position.Length / _initiatorPointAmount); j++)
                {
                    _lerpPosition[count] = Vector3.Lerp(_position[count], _targetPosition[count], _lerpAudio[i] + (input * .5f));
                    count++;
                    //j分count++
                }
            }
            _lerpPosition[count] = Vector3.Lerp(_position[count], _targetPosition[count], _lerpAudio[_initiatorPointAmount - 1] + (input * .5f));
            

            _lineRenderer.positionCount = _lerpPosition.Length;
            _lineRenderer.SetPositions(_lerpPosition);

        }

    }
}

