a = 100.1
str = "hahaha"
isDie = false

person = {
name = "xumeixi";
age1 = 26,108,18,"haha",true,2.34;

add = function(self,a,b)
print(a+b)
end

--[[
--这是注释掉的两种方法实现方式

function person:add(a,b)--默认带一个self的参数，代表当前table
print(a+b)
end

function person.add(self,a,b)
print(a+b)
end

--这是注释掉的两种方法实现方式
--]]

}

function add()
    print("调用全局Function:add")
end

function cut(a,b)
    print("相减获得："..(a-b))
	return a - b
end

function add_cut_mul_div(a,b)
    return a+b,a-b,a*b,a/b
end
