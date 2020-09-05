using UnityEngine;
using System.Collections;

namespace LuaFramework {

    /// <summary>
    /// </summary>
    public class Main : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad(gameObject);  //防止销毁自己
        }

        void Start() {
            AppFacade.Instance.StartUp();   //启动游戏
        }
    }
}
