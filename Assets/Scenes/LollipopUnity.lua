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
--����ע�͵������ַ���ʵ�ַ�ʽ

function person:add(a,b)--Ĭ�ϴ�һ��self�Ĳ���������ǰtable
print(a+b)
end

function person.add(self,a,b)
print(a+b)
end

--����ע�͵������ַ���ʵ�ַ�ʽ
--]]

}

function add()
    print("����ȫ��Function:add")
end

function cut(a,b)
    print("�����ã�"..(a-b))
	return a - b
end

function add_cut_mul_div(a,b)
    return a+b,a-b,a*b,a/b
end
