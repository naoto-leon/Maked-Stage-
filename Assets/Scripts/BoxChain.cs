using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxChain : MonoBehaviour
{
    protected enum _axis
    {
        XAxis,
        YAxis,
        ZAxis

    };

    protected enum _initiator
    {
        Triangle,
        Square,
        Pentagon,
        Hexagon,
        Heptagon,
        Octagon

    };


    [SerializeField]
    protected _axis axis = new _axis();

    [SerializeField]
    protected _initiator initiator = new _initiator();



    protected int _initiatorPointAmount;
    private Vector3[] _initiatorPoint;
    private Vector3 _rotateVector;
    private Vector3 _rotateAxis;

    private float _initialrotation;

    [SerializeField]
    protected float _initiatorSize;


    //to render 
    protected Vector3[] _position;
    protected Vector3[] _targetPosition;
    protected int _generationCount;


    [System.Serializable]
    public struct StartGen
    {
        public bool outwards;
        public float scale;
    }

    public struct LineSegment
    {
        public Vector3 StartPosition { get; set; }
        public Vector3 EndPosition { get; set; }
        public Vector3 Direction { get; set; }
        public float Length { get; set; }

    }


    public StartGen[] _startGen;

    private List<LineSegment> _lineSegment;


    [SerializeField]
    protected AnimationCurve _generator;
    protected Keyframe[] _keys;



    private void Awake()
    {

        GetInisiatorPoints();

        _keys = _generator.keys;
        _lineSegment = new List<LineSegment>();

        _position = new Vector3[_initiatorPointAmount + 1];
        _targetPosition = new Vector3[_initiatorPointAmount + 1];
        //三角なら4点で出来てる



        _rotateVector = Quaternion.AngleAxis(_initialrotation, _rotateAxis) * _rotateVector;

        for (int i = 0; i < _initiatorPointAmount; i++)
        {
            _position[i] = _rotateVector * _initiatorSize;
            //z方向かける_initiateSize

            _rotateVector = Quaternion.AngleAxis(360 / _initiatorPointAmount, _rotateAxis) * _rotateVector;

        }

        _position[_initiatorPointAmount] = _position[0];
        //初期値の0を最後に繋げる　三角なら　配列の0を3に繋げてる
        //初期値の値_position[0]の値
        //通常は0,0,0になってしまう

        _targetPosition = _position;
        //_targetpositionの移動　


        for (int i = 0; i < _startGen.Length; i++)
        {
            KochGenerator(_targetPosition, _startGen[i].outwards, _startGen[i].scale);
            //(Vector3[] position, bool outwards, float generatorMaltiply)
        }

    }


    protected void KochGenerator(Vector3[] position, bool outwards, float generatorMaltiply)
    {
        _lineSegment.Clear();

        for (int i = 0; i < position.Length - 1; i++)
        {
            LineSegment line = new LineSegment();
            line.StartPosition = position[i];
            //頂点座標がstartposition

            if (i == position.Length - 1)
            {
                line.EndPosition = position[0];
                //最後なら塞ぐのでendpositionは[0]
            }
            else
            {
                line.EndPosition = position[i + 1];
                //それ以外はendpositionは頂点の次(startpositionの次)
            }

            line.Direction = (line.EndPosition - line.StartPosition).normalized;
            line.Length = Vector3.Distance(line.EndPosition, line.StartPosition);
            _lineSegment.Add(line);

        }

        //ポイントとポイント

        List<Vector3> newpos = new List<Vector3>();
        List<Vector3> targetPos = new List<Vector3>();


        for (int i = 0; i < _lineSegment.Count; i++)
        {
            newpos.Add(_lineSegment[i].StartPosition);
            targetPos.Add(_lineSegment[i].StartPosition);

            //keyフレーム

            for (int j = 1; j < _keys.Length - 1; j++)
            {
                //keyframeの点は５つなら使うのは3つ(真ん中)

                float moveAmount = _lineSegment[i].Length * _keys[j].time;
                //length = 二点間のdistance time = keyframeの横軸

                float heightAmount = (_lineSegment[i].Length * _keys[j].value) * generatorMaltiply;
                //generatorMaltiply = 調整

                Vector3 movePos = _lineSegment[i].StartPosition + (_lineSegment[i].Direction * moveAmount);
                //スタート位置 + (ベクトル×横軸)


                Vector3 Dir;

                if (outwards)
                {
                    Dir = Quaternion.AngleAxis(-90, _rotateAxis) * _lineSegment[i].Direction;
                }
                else
                {
                    Dir = Quaternion.AngleAxis(90, _rotateAxis) * _lineSegment[i].Direction;
                }

                newpos.Add(movePos);
                targetPos.Add(movePos + (Dir * heightAmount));
                //movepos=横軸　　
            }
        }

        newpos.Add(_lineSegment[0].StartPosition);
        targetPos.Add(_lineSegment[0].StartPosition);

        _position = new Vector3[newpos.Count];
        //_lineRenderer.PositionsCountの更新
        _targetPosition = new Vector3[targetPos.Count];
        //_lineRenderer.SetPositionsの更新


        _position = newpos.ToArray();
        //_positionにnewpos適用　newposはlistなのでarray変換　
        _targetPosition = targetPos.ToArray();

        _generationCount++;
        //renderline起動ジェネレーター　
    }



    private void GetInisiatorPoints()
    {
        switch (initiator)
        {
            case _initiator.Triangle:
                _initiatorPointAmount = 3;
                _initialrotation = 0;
                break;

            case _initiator.Square:
                _initiatorPointAmount = 4;
                _initialrotation = 45;
                break;

            case _initiator.Pentagon:
                _initiatorPointAmount = 5;
                _initialrotation = 36;
                break;

            case _initiator.Hexagon:
                _initiatorPointAmount = 6;
                _initialrotation = 30;
                break;

            case _initiator.Heptagon:
                _initiatorPointAmount = 7;
                _initialrotation = 25.71428f;
                break;

            case _initiator.Octagon:
                _initiatorPointAmount = 8;
                _initialrotation = 22.5f;
                break;

            default:
                _initiatorPointAmount = 3;
                _initialrotation = 0;
                break;

        }

        switch (axis)
        {
            case _axis.XAxis:
                _rotateVector = new Vector3(1, 0, 0);
                _rotateAxis = new Vector3(0, 0, 1);
                break;

            case _axis.YAxis:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;

            case _axis.ZAxis:
                _rotateVector = new Vector3(0, 0, 1);
                _rotateAxis = new Vector3(0, 1, 0);
                break;

            default:
                _rotateVector = new Vector3(0, 1, 0);
                _rotateAxis = new Vector3(1, 0, 0);
                break;

        }

    }
}