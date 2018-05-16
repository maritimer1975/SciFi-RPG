using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;

        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        CameraRaycaster cameraRaycaster;

        // Use this for initialization
        void Start()
        {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; // registering as an observer and the method name
        }

        // Update is called once per frame
        // the observer notification method
        void OnLayerChanged(int newLayer)
        {
            //Debug.Log("Cursor over new layer." + newLayer);
            switch(newLayer)
            {
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;

                case enemyLayerNumber:
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    break;
            }
        }

        // TODO consider de-registering OnLayerChanged on leaving all game scenes
    }
}