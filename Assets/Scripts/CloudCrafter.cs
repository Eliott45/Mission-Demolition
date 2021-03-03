using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in inspecitr")]
    public int numClouds = 40; // Кол-во облоков
    public GameObject cloudPrefab; 
    public Vector3 cloudPosMin = new Vector3(-50,-5,10);
    public Vector3 cloudPosMax = new Vector3(150,100,10);
    public float cloudScaleMin = 1; // Мин. масштаб облаков
    public float cloudScaleMax = 3; // Максимальный масштаб обалков
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    void Awake() {
        cloudInstances = new GameObject[numClouds]; // Все экхемляры облаков
        GameObject anchor = GameObject.Find("CloudAnchor");
        // Создать в цикле облака
        GameObject cloud; 
        for (int i=0; i<numClouds; i++) {
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Выбрать местоположение для облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Масштабировать облако 
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudPosMin.y, cloudScaleMax, scaleU); 
            // Меньшие облака (с меньшим значением scaleU) должны быть ближе к земле
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            // Меньшие облака должны быть дальше 
            cPos.z = 100 - 90*scaleU;
            // Применить полученные значения координат и масштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Сделать облако дочерним по отношения к anchor
            cloud.transform.SetParent(anchor.transform);
            // Добавить облако в массив 
            cloudInstances[i] = cloud;
        }
    }

    void Update() {
        foreach (GameObject cloud in cloudInstances) {
            // Получиь масштаб и координаты облака 
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Увеличить скорость для ближних облаков 
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            if (cPos.x <= cloudPosMin.x) {
                cPos.x = cloudPosMax.x;
            }
            // Применить новые координаты к облаку
            cloud.transform.position = cPos;
        }
    }
}
