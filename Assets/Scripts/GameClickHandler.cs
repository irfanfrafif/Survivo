using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClickHandler : MonoBehaviour
{
    IsometricGridMovement movement;

    private void Start()
    {
        movement = GetComponent<IsometricGridMovement>();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //if (hit != false && hit.collider.CompareTag("Machine"))
            //{
            //    if (!ServiceLocator.Instance.uiManager.isOpeningMenu)
            //    {
            //        Debug.Log("Clicked" + hit.collider.gameObject.name);

            //        var machine = hit.collider.GetComponent<Machinery>();
            //        movement.GoHere((Vector3Int)machine.GetFrontGrid(), true);
            //        ServiceLocator.Instance.uiManager.currentSelectedIndex = 1; //PAIN
            //        ServiceLocator.Instance.uiManager.machineUI.SetMachineType((int)(machine.machineType));
            //        ServiceLocator.Instance.uiManager.currentMachinerySelected = machine;
            //    }             
            //}
            //else if (hit != false && hit.collider.CompareTag("Spawner"))
            //{
            //    if (!ServiceLocator.Instance.uiManager.isOpeningMenu && ServiceLocator.Instance.dayManager.isTimeRunning)
            //    {
            //        Debug.Log("Clicked" + hit.collider.gameObject.name);

            //        var machine = hit.collider.GetComponent<MaterialSpawner>();
            //        ServiceLocator.Instance.uiManager.currentMaterialSpawnerSelected = machine;
            //        ServiceLocator.Instance.uiManager.currentSelectedIndex = 2; //PAIN
            //        movement.GoHere((Vector3Int)machine.GetFrontGrid(), true);
            //    }

            //}
            //else if (hit != false && hit.collider.CompareTag("ATM"))
            //{
            //    if (!ServiceLocator.Instance.uiManager.isOpeningMenu)
            //    {
            //        Debug.Log("Clicked" + hit.collider.gameObject.name);

            //        var machine = hit.collider.GetComponent<ATM>();
            //        ServiceLocator.Instance.uiManager.currentSelectedIndex = 3; //PAIN
            //        ServiceLocator.Instance.uiManager.SetIsATM(true);
            //        movement.GoHere((Vector3Int)machine.GetFrontGrid(), true);
            //    }

            //}
            //else if(ServiceLocator.Instance.uiManager.isOpeningMenu)
            //{

            //}
            //else
            //{
            //    Debug.Log("Clicked groundtile");
            //    movement.ClickGoHere();
            //}

            movement.ClickGoHere();
        }
    }
}
