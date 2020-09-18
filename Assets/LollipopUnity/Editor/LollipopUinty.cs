using UnityEngine;
using UnityEditor;

namespace BinGe
{
    // 因为这个脚本不需要作为组件存在所以不需要继承自MonoBehaviour
    public class LollipopUinty
    {
        // MenuItem(string itemName,bool isValidateFunction,int priority);
        [MenuItem("LollipopUinty/Author", false, 1)]
        static void Author()
        {
            Debug.Log("彬哥");
        }
    }
}