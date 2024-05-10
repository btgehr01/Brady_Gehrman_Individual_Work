using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button RightArrowButton;
    [SerializeField] private Button LeftArrowButton;
    [SerializeField] private GameObject[] carPrefabs;

    private GameObject currentCarPrefab;
    private int carPlatform = 0;
    private int numberOfCarPlatforms = 4;

    private void Awake()
    {
        currentCarPrefab = carPrefabs[carPlatform];
        startButton.onClick.AddListener(OnStartButtonClick);
        exitButton.onClick.AddListener(OnEditButtonClick);
        RightArrowButton.onClick.AddListener(OnRightButtonClick);
        LeftArrowButton.onClick.AddListener(OnLeftButtonClick);
    }

    void Update()
    {
        if (Input.GetAxis("XboxDpadHorizontal") > 0)
        {
            moveRight();
        }
        else if (Input.GetAxis("XboxDpadHorizontal") < 0)
        {
            moveLeft();
        }

        if(Input.GetButtonDown("Submit"))
        {
            OnStartButtonClick();
        }

        if(Input.GetButtonDown("Cancel"))
        {
            OnEditButtonClick();
        }

    }

    private void OnStartButtonClick()
    {
        print("Starting game with car: " + currentCarPrefab.name);
        PlayerPrefs.SetString("SelectedCar", currentCarPrefab.name);
        //Load Racing Game Scene
        SceneManager.LoadScene("Assets/Scenes/NYC.unity");
    }

    private void OnEditButtonClick()
    {
        #if UNITY_EDITOR
                    // If we're in the Unity Editor, stop playing the scene
                    UnityEditor.EditorApplication.isPlaying = false;
        #else
                // If we're running an .exe file, quit the application
                Application.Quit();
        #endif
    }

    private void OnRightButtonClick()
    {
        moveRight();
    }

    private void OnLeftButtonClick()
    {
        moveLeft();

    }

    private void moveRight()
    {
        if (carPlatform < numberOfCarPlatforms - 1)
        {
            //increment carPlatform
            carPlatform++;
            //set next prefab in the carPrefabs array as the currentCarPrefab
            currentCarPrefab = carPrefabs[carPlatform];

            // Move the camera to the right
            Vector3 cameraTargetPosition = Camera.main.transform.position + new Vector3(2000f, 0f, 0f);
            StartCoroutine(MoveCamera(cameraTargetPosition, 0.5f));
        }
    }

    private void moveLeft()
    {
        if (carPlatform > 0)
        {
            //decrement carPlatform
            carPlatform--;
            //set previous prefab in the carPrefabs array as the currentCarPrefab
            currentCarPrefab = carPrefabs[carPlatform];

            // Move the camera to the left
            Vector3 cameraTargetPosition = Camera.main.transform.position - new Vector3(2000f, 0f, 0f);
            StartCoroutine(MoveCamera(cameraTargetPosition, 0.5f));
        }
    }


    private IEnumerator MoveCamera(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = Camera.main.transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            Camera.main.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        Camera.main.transform.position = targetPosition;
    }

}
