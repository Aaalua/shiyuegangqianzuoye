using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLua;

[CustomLuaClass]
public class test01 : MonoBehaviour
{

    [DoNotToLua]
    LuaSvr svr;
    [DoNotToLua]
    void Start()
    {
        svr = new LuaSvr();
        svr.init(null, complete);
    }
    [DoNotToLua]
    private void complete()
    {
        Debug.Log("--------------------------start test01 main------------------------------");
        svr.start("test01");
        Debug.Log("--------------------------end test01 main------------------------------");
    }

}
