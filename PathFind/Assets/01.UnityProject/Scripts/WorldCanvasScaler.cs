using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasScaler : MonoBehaviour
{
    private Canvas worldCanvas = default;
    private Vector2 cameraSize = default;

    [SerializeField]
    private Vector2 canvasAspect = default;

    // Start is called before the first frame update
    void Start()
    {
        worldCanvas = gameObject.GetComponentMust<Canvas>();
        cameraSize = GFunc.GetCameraSize();
        Vector2 canvasSize = worldCanvas.gameObject.GetRectSizeDelta();

        // ī�޶� ������� ĵ���� ������ ������ ũ�� �� ���Ѵ�.
        // width�� height �� �� �ϳ��� ������ ������ �����Ѵ�.
        canvasAspect.x = cameraSize.x / canvasSize.x;
        canvasAspect.y = canvasAspect.x;

        // ���� ĵ������ ���� �������� ������ ������ ������ �����Ѵ�.
        worldCanvas.transform.localScale = canvasAspect;
    }       // Start()

    // Update is called once per frame
    void Update()
    {
        
    }
}
