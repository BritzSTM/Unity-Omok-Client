using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Om
{
    public class DragRotator : MonoBehaviour
    {
        public GameObject target;
        public Camera _camera;
        public float DragRotBais = 100.0f;

        private bool _isDrag;
        private Vector3 _prevDragPos;
        private Vector3 _currDragPos;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Start OnDrag");
                _isDrag = true;
                _prevDragPos = _currDragPos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                //Debug.Log("OnDrag");
                _prevDragPos = _currDragPos;
                _currDragPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("End OnDrag");
                _isDrag = false;
            }

            if (_isDrag)
                UpdateTargetRotation();
        }

        private void UpdateTargetRotation()
        {
            Debug.Assert(_isDrag);

            var rotTr = _currDragPos - _prevDragPos;
            if (rotTr.magnitude == 0)
                return;

            //Debug.Log($"{_currDragPos}, {_prevDragPos}");
            var rotAxis = Vector3.Cross(rotTr.normalized, _camera.transform.forward);
            Debug.DrawLine(transform.position, transform.position + rotAxis);

            //Debug.Log(rotTr.magnitude);
            var rotAngle = rotTr.magnitude * DragRotBais * Time.deltaTime;
            //Debug.Log(rotAngle);
            target.transform.Rotate(rotAxis, rotAngle, Space.World);
        }
    }
}