using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode { // Метод перечисления 
    idle, 
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // объект одиночка

    [Header("Set in Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton; 
    public Vector3 castlePos; // Местоположение замка
    public GameObject[] castles; // Массив замков

    [Header("Set Dynamically")]
    public int level;
    public int levelMax; // Кол-во уровней
    public int shotsTaken;
    public GameObject castle; // Текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // Режим FollowCam

    void Start() {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel() {
        // Уничтожить прежний замок, если он существует
        if (castle != null) {
            Destroy(castle);
        }

        // Уничтожить прежние снаряды, если они есть
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos) {
            Destroy(pTemp);
        }

        // Создать новый замок 
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Переустановить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        // Сбросить цель
        Goal.goalMet = false;

        UpdateGUI();
        
        mode = GameMode.playing;
    }
    
    void UpdateGUI() {
        // Показать данные в UI 
        uitLevel.text = "Level: " +(level+1)+ " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    void Update() {
        UpdateGUI();
        // Проверить завершение уровня
        if ((mode == GameMode.playing) && Goal.goalMet) {
            // Изменить режим, чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            // Уменьшить масштаб
            SwitchView("Show Both");
            // Начать новый уровень, через 2 сек
            Invoke("NextLevel",2f);
        }

        if (Input.GetKey("r"))  
        {
            StartLevel();
        }
    }

    void NextLevel() {
        level++;
        if(level == levelMax) {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "" ) {
        if (eView == ""){
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing) {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    // Увелечение выстрелов

    public static void ShotFired() {
        S.shotsTaken++;
    }
}

