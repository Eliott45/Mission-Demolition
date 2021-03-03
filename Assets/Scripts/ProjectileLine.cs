using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Одиночка

    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake() {
        S = this; // Установить ссылку на объект одиночку
        line = GetComponent<LineRenderer>(); // Получить ссылку на LineRenderer
        line.enabled = false;
        // Иницилизировать список точек
        points = new List<Vector3>();
    }

    public GameObject poi {
        get {
            return (_poi);
        }
        set {
            _poi = value;
            if (_poi != null) {
                // Если поле содержит действительную ссылку, сбросить все остальные параметры в исходное состояния 
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    
    // Метод для стерания линии (непосредственно)
    public void Clear() {
        _poi = null;
        line.enabled=false;
        points = new List<Vector3>();
    }

    public void AddPoint() {
        // Вызывается для добавления точки в линии
        Vector3 pt = _poi.transform.position;
        if ( points.Count > 0 && (pt - lastPoint).magnitude < minDist) {
            // Если точка недостаточно далека от предыдущей, просто выйти
            return;
        }
        if (points.Count == 0) { // Если это точка запуска
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения добавить доп фрагмент линии, что бы помочь лучше прицелится
            points.Add(pt+launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Установить первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Включить линию
            line.enabled = true;
        } else {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count-1, lastPoint);
            line.enabled = true;
        }
    }

    // Возращает местоположение последней добавленной точки
    public Vector3 lastPoint {
        get {
            if (points == null) {
                // Если точек нет, вернуть 0
                return (Vector3.zero);
            }
            return (points[points.Count-1]);
        }
    }

    void FixedUpdate() {
        if (poi == null) {
            // Если свойство содержит пустое значение, найти ИБ
            if (FollowCam.POI != null) {
                if (FollowCam.POI.tag == "Projectile") {
                    poi = FollowCam.POI;
                } else {
                    return; // Выйти если ИБ не найден
                }
            } else {
                return;
            }
        }
        // Если объект найден, попытаться добавить точку с его координатами в каждом кадре
        AddPoint();
        if(FollowCam.POI == null) {
            poi = null;
        }
    }

}
