print('启动了Lua')

function init()
	print("执行了Lua的全局方法init")--无参数的方法
	CreatUnityObj()
end

function CreatUnityObj()
    
	local FatherClass=CS.FatherClass--获取一个类的映射
	local fatherClass=CS.FatherClass()--实例化一个类
	
	print('------fatherClass普通方法成员方法、字段属性-------')
	print('fatherClass.BaseField:',fatherClass.BaseField)--获取BaseClass的成员变量
	print('BaseProperty:',fatherClass.BaseProperty)--获取变量属性
	--baseClass:BaseClass()--构造方法，直接传入参数调用
	fatherClass:BaseFunc()--用冒号调用(:)成员方法
	
	print('------FatherClass静态成员方法、字段属性-------')
	print('FatherClass.BaseStaticField:',FatherClass.BaseStaticField)--获取静态属性（要用类名直接调用，否则无法调用到的）
	FatherClass.BaseStaticFunc()--调用静态方法
	
	print('---调用子类的方法')
	----子类SonClass--继承自父类FatherClass
	local SonClass=CS.SonClass
	local sonClass=CS.SonClass()
	---普通方法调用--
	print("sonClass.SonField:",sonClass.SonField)--获取子类的普通成员变量
	sonClass:SonPrint()--调用子类的普通成员方法
	
	---通过子类调用父类的方法
	print('---通过子类调用父类的成员方法')
	print('sonClass.BaseField:',sonClass.BaseField)--获取BaseClass的成员变量
	print('sonClass.BaseProperty:',sonClass.BaseProperty)--获取变量属性
	
	print('---通过子类调用父类的静态方法')
	print('SonClass.SonStaticField:',SonClass.SonStaticField)--获取BaseClass的成员变量
	SonClass.OnStaticPrint()		
end