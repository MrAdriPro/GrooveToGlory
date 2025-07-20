using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class M_MainMenu : MonoBehaviour
{
    //Variables
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _mainMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _settingsMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _displaySettings;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _audioSettings;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject _inputSettings;
    [BoxGroup("Menus")]
    public Dictionary<int, GameObject> _menus = new Dictionary<int, GameObject>();
    

    [BoxGroup("Button Selector Config")]
    [SerializeField] private GameObject _fileSelectorMenu;
    [BoxGroup("Button Selector Config")]
    [SerializeField] private Vector2 selectorOffset = new Vector2(-30f, 0f);
    [BoxGroup("Button Selector Config")]
    [SerializeField][Range(0, 20)] private float selectorVelocity = 0.01f;
    [BoxGroup("Button Selector Config")]
    [SerializeField] private Volume efectos;
    [BoxGroup("Button Selector Config")]
    [SerializeField] private AnimationCurve curve;

    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingScreen;
    [BoxGroup("Loading configs")]
    [SerializeField] private GameObject LoadingIcon;
    [BoxGroup("Loading configs")]
    [SerializeField] private Vector3 RotateAmount;
    [SerializeField] private RectTransform _selector;


    [Header("Private variables")]
    private bool isSelectorMoving;
    private Vector3 targetPos;
    private Vector3 currentPos;
    private Vector3 startPos;
    private Canvas canvas;
    private float elapsedTime = 0f;


    public enum Menus
    {
        Main,
        Configuration,
        FileSelector,
        DisplaySettings,
        AudioSettings,
        InputSettings,
        Off
    }

    //Functions
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        _selector.SetParent(this.transform);
        MoveSelectorToButton(GetFirstMenuButton(Menus.Main));
        SetMenus();
        SetActive((int)Menus.Main);
        SetDOF(true);
    }


    /// <summary>
    /// Moves the selector icon to the next button
    /// </summary>
    /// <param name="boton"></param>
    public void MoveSelectorToButton(Button boton)
    {
        RectTransform butonRect = boton.GetComponent<RectTransform>();

        // Paso 1: Obtener posición de pantalla del botón
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(
               canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
               butonRect.position
           );
        // Paso 2: Convertir pantalla a posición local del parent del selector
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _selector.parent as RectTransform,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPoint
        );

        localPoint = new Vector2((localPoint.x - (butonRect.sizeDelta.x / 2)) + selectorOffset.x, localPoint.y + selectorOffset.y);

        SetTargetPost(localPoint);
    }

    public void SetTargetPost(Vector2 _targetPost) 
    {
        elapsedTime = 0f;
        targetPos = _targetPost;
        currentPos = _selector.GetComponent<RectTransform>().anchoredPosition;
        isSelectorMoving = true;
    }

    private void Update()
    {
        if (isSelectorMoving)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / selectorVelocity;

            Vector2 nuevaPos = Vector2.Lerp(currentPos, targetPos, curve.Evaluate(percentageComplete));
            _selector.GetComponent<RectTransform>().anchoredPosition = nuevaPos;

            // Si está muy cerca del destino, terminar
            if (Vector2.Distance(nuevaPos, targetPos) < 1f)
            {
                _selector.GetComponent<RectTransform>().anchoredPosition = targetPos;
                isSelectorMoving = false;
            }
        }
    }


    /*
     * Metodo que se encarga de Activar un menu
     * */
    private Configuration.Exceptions SetActive(int menu)
    {
        try
        {
            foreach (var item in _menus)
            {
                if (item.Key.Equals(menu))
                {
                    item.Value.gameObject.SetActive(true);
                }
                else
                {
                    item.Value.gameObject.SetActive(false);
                }
            }

            int aux = 0;
            foreach (var item in _menus)
            {
                aux += 1;

                if (item.Value.gameObject.activeSelf) break;

                if (_menus.Count == aux) return Configuration.Exceptions.MenuException;
            }
        }
        catch (Exception) { return Configuration.Exceptions.None; }
        return Configuration.Exceptions.None;
    }


    /*
     * Metodo ejecutado tras pulsar el boton de New Game
     * */
    public void OnMouseButton_NewGame()
    {
        StartCoroutine(LoadSceneAsync(1));
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Continuar en el menu de pausa
     * */
    public void OnMouseButton_ContinuePauseMenu()
    {
        try
        {
            var pauseMenu = Hud_Controller.Instance.GetPauseMenu;
            if (pauseMenu) pauseMenu.SetActive(false);
            int value = (int)SetActive((int)Menus.Main);
            ExceptionController(value);
        }
        catch (Exception) { ExceptionController((int)Menus.Main); }
    }
    /*
     * Metodo ejecutado tras pulsar el boton de Configuracion
     * */
    public void OnMouseButton_Configuration()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.Configuration));
        int value = (int)SetActive((int)Menus.Configuration);
        ExceptionController(value);
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Cargar
     * */
    public void OnMouseButton_FileSelector()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.FileSelector));
        int value = (int)SetActive((int)Menus.FileSelector);
        ExceptionController(value);
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Cargar
     * */
    public void OnMouseButton_MainMenu()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.Main));
        int value = (int)SetActive((int)Menus.Main);
        ExceptionController(value);
    }

    /*
 * Metodo ejecutado tras pulsar el boton de Display Settings
 * */
    public void OnMouseButton_DisplaySettings()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.DisplaySettings));
        int value = (int)SetActive((int)Menus.DisplaySettings);
        ExceptionController(value);
    }
    /*
    * Metodo ejecutado tras pulsar el boton de Audio Settings
    * */
    public void OnMouseButton_AudioSettings()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.AudioSettings));
        int value = (int)SetActive((int)Menus.AudioSettings);
        ExceptionController(value);
    }
    /*
    * Metodo ejecutado tras pulsar el boton de Input Settings
    * */
    public void OnMouseButton_InputSettings()
    {
        MoveSelectorToButton(GetFirstMenuButton(Menus.InputSettings));
        int value = (int)SetActive((int)Menus.InputSettings);
        ExceptionController(value);
    }
    /*
     * Metodo ejecutado tras pulsar el boton de Salir
     * */
    public void OnMouseButton_Exit()
    {
        Application.Quit();
    }

    /*
     * Metodo ejecutado tras pulsar el boton de Salir al menu
     * */
    public void OnMouseButton_ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    /*
     * Metodo qe se ejecuta si ha ocurrido una excepcion
     */
    private void ExceptionController(int value)
    {
        if (Configuration.Exceptions.MenuException.Equals(value)) 
        {
            print(Configuration.ToString((int)Configuration.Exceptions.MenuException));
        }
    }

    public void SetMenus() 
    {
        _menus.Add(0,_mainMenu);
        _menus.Add(1, _settingsMenu);
        _menus.Add(2, _fileSelectorMenu);
        _menus.Add(3, _displaySettings);
        _menus.Add(4, _audioSettings);
        _menus.Add(5, _inputSettings);

    }

    private void SetDOF(bool state)
    {
        DepthOfField tempDof;
        if(efectos){
            if (efectos.profile.TryGet<DepthOfField>(out tempDof))
            {
                tempDof.active = state;
            }
        }
    }


    /// <summary>
    /// Gets the first menu button
    /// </summary>
    /// <returns></returns>
    public Button GetFirstMenuButton(Menus menu) 
    {
        Button[] childButtons = new Button[1];
        switch (menu)
        {
            case Menus.Main:
                childButtons = _mainMenu.GetComponentsInChildren<Button>();
                break;
            case Menus.Configuration:
                childButtons = _settingsMenu.GetComponentsInChildren<Button>();
                break;
            case Menus.FileSelector:
                childButtons = _fileSelectorMenu.GetComponentsInChildren<Button>();
                break;
            case Menus.DisplaySettings:
                childButtons = _displaySettings.GetComponentsInChildren<Button>();
                break;
            case Menus.AudioSettings:
                childButtons = _audioSettings.GetComponentsInChildren<Button>();
                break;
            case Menus.InputSettings:
                childButtons = _inputSettings.GetComponentsInChildren<Button>();
                break;
            case Menus.Off:
                break;
            default:
                break;
        }

        return childButtons[0];
    }

    IEnumerator LoadSceneAsync(int sceneId) 
    {
        LoadingScreen.SetActive(true);
        int i = 0;
        while(i < 400)
        {
            i++;
            yield return new WaitForSeconds(0.001f);
            LoadingIcon.transform.Rotate(RotateAmount * Time.deltaTime);

        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);


        while (!operation.isDone)
        {
            LoadingIcon.transform.Rotate(RotateAmount * Time.deltaTime);

            yield return null;
        }
        
    }
}
