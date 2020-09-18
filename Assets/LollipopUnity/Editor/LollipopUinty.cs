using UnityEngine;
using UnityEditor;

//因为这个脚本不需要作为组件存在所以不需要继承自MonoBehaviour
public class NewButton
{
    //MenuItem(string itemName,bool isValidateFunction,int priority);
    [MenuItem("LollipopUinty/Test1", false, 1)]
    static void Test1()
    {
        Debug.Log("Test1");
    }
}