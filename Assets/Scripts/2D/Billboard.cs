using UnityEngine;
using NaughtyAttributes;

public class Billboard : MonoBehaviour
{
    //Variables

    [Header("Billboard settings")]
    [SerializeField] private bool freezeXZ = false;

    [Header("Private variables")]
    private Transform playerCam = null;

    //Functions
    private void Start()
    {
        playerCam = GameObject.FindGameObjectWithTag(Tags.CAMERA_TAG).GetComponent<Transform>();
    }

    private void Update()
    {
        if (!playerCam) return;

        if (freezeXZ) transform.rotation = Quaternion.Euler(0, playerCam.rotation.eulerAngles.y - 90, 0);
        else transform.rotation = Quaternion.Euler(playerCam.rotation.eulerAngles.x, playerCam.rotation.eulerAngles.y, 0);

    }
}
