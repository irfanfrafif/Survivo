using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class ServiceLocator : MonoBehaviour
{
    private static ServiceLocator _instance;

    public static ServiceLocator Instance { get { return _instance; } }

    public GridManager gridManager { get; private set; }
    //public MachineManager machineManager { get; private set; }
    //public UIManager uiManager { get; private set; }
    //public InventoryManager inventoryManager { get; private set; }
    //public CustomerManager customerManager { get; private set; }
    //public DayManager dayManager { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ConnectManagers();
    }

    private void ConnectManagers()
    {
        gridManager = GetComponentInChildren<GridManager>();
        //machineManager = GetComponentInChildren<MachineManager>();
        //uiManager = GetComponentInChildren<UIManager>();
        //inventoryManager = GetComponentInChildren<InventoryManager>();
        //customerManager = GetComponentInChildren<CustomerManager>();
        //dayManager = GetComponentInChildren<DayManager>();
    }
}
