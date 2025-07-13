using UnityEngine;
using NaughtyAttributes;
using static UnityEngine.GraphicsBuffer;

public class Player_Camera : MonoBehaviour
{


    //Variables
    [Header("Configuration")]
    [SerializeField] private float mouseSens = 100f;
    [SerializeField] private float smoothRot = 0;
    [SerializeField] private Vector3 camOffset = Vector3.zero;

    [Header("Transforms")]
    [SerializeField] private Transform playerCamPivot = null;
    [SerializeField] private Transform playerCam = null;
    [SerializeField] private Transform playerBody = null;


    [Header("Private variables")]
    private float mouseX;
    private float mouseY;
    private float minY = -30f;
    private float maxY = 60f;
    private Vector3 currentRotation;
    private Vector3 rotationSmoothVelocity;
    //Functions


    private void Start()
    {
        SetStartConfiguration();
    }


    private void LateUpdate()
    {
        MoveCamera();
    }




    /// <summary>
    /// Set the starting configuration
    /// </summary>
    private void SetStartConfiguration()
    {
        mouseSens = Player_Inputs.Mouse_Sens;
        smoothRot = Player_Inputs.smoothRot;
        camOffset = Player_Inputs.camOffset;


        playerCam = GameObject.FindGameObjectWithTag(Tags.CAMERA_TAG).GetComponent<Transform>();
        playerCamPivot = GameObject.FindGameObjectWithTag(Tags.CAMERA_PIVOT_TAG).GetComponent<Transform>();
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Transform>();
    }

    private void MoveCamera()
    {
        if (!playerCam || !playerBody) return;


        mouseX += Input.GetAxis("Mouse X") * mouseSens;
        mouseY -= Input.GetAxis("Mouse Y") * mouseSens;

        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        // Objetivo de rotaci�n
        Vector3 targetRotation = new Vector3(mouseY, mouseX, 0);

        // Interpolacion suave
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, smoothRot);

        //Aplicar rotacion
        playerCamPivot.transform.position = playerBody.position;
        playerCamPivot.transform.rotation = Quaternion.Euler(currentRotation);

        // Posicionar la camara en el offset
        playerCam.transform.position = playerCamPivot.transform.position + playerCamPivot.transform.rotation * camOffset;
        playerCam.transform.LookAt(playerCamPivot.transform.position + Vector3.up * 1.5f);


    }


}
