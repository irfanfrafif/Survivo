using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour {

    public static UIManager Instance;

    public GameObject SecondaryScreen;
    public GameObject PlayerContainer;
    public GameObject NpcContainer;
    public RunningText PlayerText;
    public RunningText NpcText;
    public TMP_Text NpcName;


    private void Awake() {
        Instance = this;
    }
}
