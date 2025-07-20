using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cameraMusicMovement : MonoBehaviour
{

    [Header("configuration")]
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    [SerializeField] private float fovAmount;
    public Transform cameraPosition;
    private bool RightLastRotation = false;
    private bool stopRotating = false;
    [SerializeField] private bool fovMovement;
    private bool canFovMovement;
    private bool returnFovMovement;
    [SerializeField] private int fovTimeAmount;
    [SerializeField] private float fovTimeBack = 0.2f;
    private float fovCurrentTimeAmount;
    [SerializeField][Range(0, 10)] private float fovSpeed = 1;
    [SerializeField][Range(1, 10)] private float _pulseAmount = 1;
    [SerializeField][Range(1, 20)] private float _pulseSpeed = 1;
    [SerializeField] private GameObject[] _menuObjects;
    private Dictionary<GameObject, Vector3> _startSizes;

    private float startFov;
    Quaternion startRotation;
    private Camera gameCamera;

    private void Start()
    {
        startRotation = cameraPosition.rotation;
        gameCamera = cameraPosition.GetComponent<Camera>();
        startFov = gameCamera.fieldOfView;
        fovCurrentTimeAmount = 0;
        SetStartScaleMenuObjects();
    }

    private void Update()
    {

        CameraShake();
    }


    /// <summary>
    /// Metodo encargado de llamar al movimiento de la camara
    /// </summary>
    public void CameraShake()
    {

        // Devuelve la posicion principal a los objetos del Menu
        foreach (var item in _menuObjects)
        {
            item.GetComponent<RectTransform>().localScale = Vector3.Lerp(item.GetComponent<RectTransform>().localScale, _startSizes[item], Time.deltaTime * _pulseSpeed);
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            if (fovMovement)
            {
                fovCurrentTimeAmount++;
                if (fovCurrentTimeAmount >= fovTimeAmount)
                {
                    canFovMovement = true;
                    fovCurrentTimeAmount = 0;
                }
                else
                {
                    canFovMovement = false;
                }
            }
            StopAllCoroutines();
            StartCoroutine(Rotate());
            StartCoroutine(Fov(canFovMovement));
        }
    }

    /// <summary>
    /// Establece la posicion inicial a los objetos del menu
    /// </summary>
    public void SetStartScaleMenuObjects()
    {
        _startSizes = new Dictionary<GameObject, Vector3>();
        foreach (var item in _menuObjects)
        {
            _startSizes.Add(item, item.GetComponent<RectTransform>().localScale);

        }
    }

    /// <summary>
    /// Metodo encargado de hacer la pulsacion de los objetos del menu
    /// </summary>
    public void MenuObjectsShake()
    {
        foreach (var item in _menuObjects)
        {
            item.GetComponent<RectTransform>().localScale = _startSizes[item] * _pulseAmount;
        }
    }

    /// <summary>
    /// Llamada al metodo para girar la camara en cada intervalo
    /// </summary>
    public void CameraShakeRotation() => StartCoroutine(Rotate());

    /// <summary>
    /// LLamada al metodo para hacer la pulsacion del FOV de la camara en cada intervalo
    /// </summary>
    public void CameraShakeFov() => StartCoroutine(Fov(fovMovement));

    /// <summary>
    /// Controla la rotacion de la camara
    /// </summary>
    /// <returns></returns>
    private IEnumerator Rotate()
    {
        RightLastRotation = !RightLastRotation;
        stopRotating = false;
        bool firstReturningToStart = false;
        bool stop = false;
        while (!stop)
        {
            if (CheckDistance(!RightLastRotation) > 0.1f && !stopRotating)
            {

                if (!RightLastRotation)
                    cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, NewRotation(true), speed * Time.deltaTime);
                else
                    cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, NewRotation(false), speed * Time.deltaTime);

                if (CheckDistance(!RightLastRotation) <= 0.12f)
                {
                    stopRotating = true;
                    firstReturningToStart = true;
                }
            }

            if (stopRotating)
            {
                if (cameraPosition.rotation == startRotation) stop = true;
                if (firstReturningToStart)
                {
                    yield return new WaitForSeconds(0.2f);
                    firstReturningToStart = false;
                }
                cameraPosition.rotation = Quaternion.RotateTowards(cameraPosition.rotation, startRotation, (speed * Time.deltaTime) / 2);

            }
            yield return null;

        }

    }


    /// <summary>
    /// Sirve para controlar el fov
    /// </summary>
    /// <returns></returns>
    private IEnumerator Fov(bool _canFovMovement)
    {
        bool stop = false;
        bool returnFovFirst = false;
        returnFovMovement = false;
        while (!stop && fovMovement)
        {
            if (_canFovMovement && !returnFovMovement)
            {
                gameCamera.fieldOfView = IntLerp((int)startFov, (int)(gameCamera.fieldOfView - fovAmount), (fovSpeed * 500) * Time.deltaTime);

                if (gameCamera.fieldOfView >= (gameCamera.fieldOfView - fovAmount))
                {
                    returnFovMovement = true;
                    returnFovFirst = true;
                }
            }
            else if (_canFovMovement && returnFovMovement && gameCamera.fieldOfView < startFov)
            {
                if (returnFovFirst)
                {
                    yield return new WaitForSeconds(fovTimeBack);
                    returnFovFirst = false;
                }
                gameCamera.fieldOfView = IntLerp((int)(gameCamera.fieldOfView - fovAmount), (int)startFov, (fovSpeed * 500) * Time.deltaTime);
            }
            else stop = true;

            yield return null;
        }


    }

    /// <summary>
    /// De forma suave interpola de un entero a otro
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    int IntLerp(int a, int b, float t)
    {
        if (t > 0.9999f)
        {
            return b;
        }

        return a + (int)(((float)b - (float)a) * t);
    }

    /// <summary>
    /// Comprueba la distancia entre las dos rotaciones
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private float CheckDistance(bool rotation)
    {
        float _distance;

        if (rotation)
            _distance = distance - WrapAngle(cameraPosition.rotation.eulerAngles.z);
        else _distance = distance + WrapAngle(cameraPosition.rotation.eulerAngles.z);

        return _distance;

    }

    /// <summary>
    /// Sirve para pasar el angulo correcto
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    /// <summary>
    /// Calcula la nueva posicion en base a la derecha o izquieda
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private Quaternion NewRotation(bool rotation)
    {
        Quaternion newRot;
        if (rotation) newRot = Quaternion.Euler(new Vector3(0, 0, distance));
        else newRot = Quaternion.Euler(new Vector3(0, 0, -distance));

        return newRot;
    }


}
