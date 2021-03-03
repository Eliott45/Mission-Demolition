using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{   
    static private Slingshot S; // Объект одиночка для линии стрельбы

    [Header("Set in Inspector")]
    public GameObject prefabProjectile; // Префаб для шара
    public float velocityMult = 8f; // Скорость шара

    [Header("Set Dynamically")]
    public GameObject launchPoint; // Подсветка
    public Vector3 launchPos; // Координаты подсветки
    public GameObject projectile; // Снаряд
    public bool aimingMode; // Когда нажатие на мышку, включается прицеливание

    static public Vector3 LAUNCH_POS { // Метод для линии стрельбы
        get {
            if (S == null) return Vector3.zero;
                return S.launchPos;
        }
    }

    private Rigidbody projectileRigidbody; // Получение к доступу модуля RigidBody

    void Awake() {
        S = this; 
        Transform launchPointTrans = transform.Find("LaunchPoint"); // Находит объект
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // Выключает свет
        launchPos = launchPointTrans.position; // Установка координат 
    }

    void OnMouseEnter() { // Если ли курсор попадает на объект
        // print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true); // Включает подсветку
    }

    void OnMouseExit() { // Курсор не попадает на объект
        // print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false); // Выключает подсветку
    }

    void OnMouseDown() { // При нажатие ЛКМ
        // Игрок нажал кнопку мыши, когда указатель находился над рогаткой
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate(prefabProjectile) as GameObject;
        // Поместить в точку launhPoint
        projectile.transform.position = launchPos;
        // Сделать его кинематическим
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        // projectileRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        projectileRigidbody.isKinematic = true;
    }

    void Update() {
        if(!aimingMode) return; // Если рагатка не в режиме прицеливания, не выполнять этот код

        // Получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);
        // Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos; 
        // Ограничить mouseDelta радиусом коллайдера обхекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Передвинуть снаряд в новую позицию 
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if ( Input.GetMouseButtonUp(0)) {
            // Кнопка мыши отпущена
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile; // Передать шар, на отслеживание
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
