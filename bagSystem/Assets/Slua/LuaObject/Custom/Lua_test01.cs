using System;
using SLua;
using System.Collections.Generic;
[UnityEngine.Scripting.Preserve]
public class Lua_test01 : LuaObject {
	[UnityEngine.Scripting.Preserve]
	static public void reg(IntPtr l) {
		getTypeTable(l,"test01");
		createTypeMetatable(l,null, typeof(test01),typeof(UnityEngine.MonoBehaviour));
	}
}
