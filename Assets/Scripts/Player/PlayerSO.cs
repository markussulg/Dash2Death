using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PlayerSO : ScriptableObject {

    public GameObject playerSwordPrefab;
    public GameObject playerVisualPrefab;
    public GameObject playerCameraPrefab;
    public string playerName;

    public GameObject playerCanvasPrefab;
    public GameObject playerHealthPrefab;
    public GameObject playerHealthFillPrefab;

}
