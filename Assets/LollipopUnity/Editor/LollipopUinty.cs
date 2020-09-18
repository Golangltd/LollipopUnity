using UnityEngine;
using UnityEditor;

namespace BinGeTools
{
    public class LollipopUinty: MonoBehaviour
    {
        // MenuItem(string itemName,bool isValidateFunction,int priority);
        [MenuItem("LollipopUinty/Author", false, 1)]
        static void Author()
        {
            Debug.Log("彬哥");
            GameObject Part3D = (GameObject)Resources.Load("SubModules/Tools/Prefabs/tools"); //这一步是加载，注意文件夹资源要在Resources目录下
            GameObject mUICanvas = GameObject.Find("Canvas");
            Part3D = Instantiate(Part3D);
            Part3D.transform.parent = mUICanvas.transform;
        }
    }
}