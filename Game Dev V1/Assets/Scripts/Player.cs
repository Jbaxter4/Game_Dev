using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    CharacterController cc;
    [SerializeField]
    float movmentSpeed;
    [SerializeField]
    Transform modelTransform;
    [SerializeField]
    bool transformDirection;
    private void Start()
    {
        movmentSpeed = movmentSpeed;
    }
    private void LateUpdate()
    {
        Movement();
        MouseRotation();
    }
    void MouseRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200f, 1 << 10))
        {
            Vector3 rotationVector = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            modelTransform.LookAt(rotationVector, Vector3.up);
        }
    }
    void Movement()
    {


        Vector3 movmentVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (transformDirection)
        {
            movmentVector = modelTransform.TransformDirection(movmentVector);
        }
        movmentVector.Normalize();




        cc.Move(movmentVector * movmentSpeed);
    }


}