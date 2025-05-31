using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR.Interaction.Toolkit.UI;
using UnityEngine.SceneManagement;
public class Game_manager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float Remainingtime = 60.0f;
    public float remainingtimer;

    [SerializeField] Button StartButton;
    [SerializeField] TrackedDeviceGraphicRaycaster raycastInteraction;
    [SerializeField] bool timerEnabled = false;
    [SerializeField] FruitSpawner fruitSpawner;

    public sliceObject sliceObject;
    void Awake()
    {
        remainingtimer = Remainingtime;
    }

    void Update()
    {
        if (timerEnabled)
        {
            remainingtimer -= Time.deltaTime;
            timerText.text = remainingtimer.ToString("0");
        }
    }
    public void StartSpawningFruits()
    {
        if (!timerEnabled)
        {
            sliceObject.score = 0;
            sliceObject.scoreText.text = "0";
            StartCoroutine(startSpawnerTimer());
            StartButton.interactable = false;
            raycastInteraction.enabled = false;
        }
    }

    IEnumerator startSpawnerTimer()
    {
        yield return new WaitForSeconds(3.0f);
        fruitSpawner.StartCoroutine(fruitSpawner.Spawner());
        timerEnabled = true;
        yield return new WaitForSeconds(Remainingtime);
        remainingtimer = Remainingtime;
        StartButton.interactable = true; //re-enable startButton
        raycastInteraction.enabled = true; //enable UI interaction
        timerText.text = remainingtimer.ToString("0"); // Reset timertext
        timerEnabled = false; // disable the timer
        fruitSpawner.StopAllCoroutines();

    }
    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
