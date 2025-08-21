using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
//자동으로 컴포넌트 추가
public class Player : LivingEntity
{//LivingEntity는 Monobehavior, IDamagable 상속중
    public float moveSpeed = 5;
    Camera viewCamera;
    PlayerController controller;
    GunController gunController;
    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();  
        viewCamera = Camera.main;
    }

    void Update()
    {
        //Movement input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;    
        controller.Move(moveVelocity);
        
        //Look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        //Ray로 마우스가 클릭한 위치 알 수 있음
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }
        //weapon input

        if(Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }
}
