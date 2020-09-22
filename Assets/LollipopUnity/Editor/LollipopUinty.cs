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

        
    /// <summary>
    /// _w 单一的快捷键 W
    /// #w shift + w
    /// %w ctrl + w
    /// &w alt + w
    /// </summary>

    [MenuItem("LollipopUinty/Test _w")]
    public static void Test1()
    {
        Debug.Log("Test_-----");
    }

    [MenuItem("LollipopUinty/Test# #w")]
    public static void Test2 ()
    {
        Debug.Log("Test#-----");
    }
 
    [MenuItem("LollipopUinty/Test% %w")]
    public static void Test3 ()
    {
        Debug.Log("Test%-----");
    }

    [MenuItem("LollipopUinty/Test& &w")]
    public static void Test4 ()
    {
        Debug.Log("Test&-----");
    }
    }
}