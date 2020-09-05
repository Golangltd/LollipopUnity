UIManager={}
UIManager.__index = UIManager
UIManager.name="UIManager"
local uiLayer = {}
UIManager.Layer = {
    Buttom = 1,
    Normal = 2,
    Pop = 3,
    Message = 4,
}


function UIManager.New( resPath,layer )
    local M = {}
    setmetatable(M,UIManager)
    M.resPath = resPath
    M.layer = layer
    M.visible = false
    return M
end

function UIManager:SetVisible( state )
    if self.visible == state then return end
    self.visible = state
    if state then
        self.obj = ResourceManager.LoadUI(self.resPath)
        self.uicomponent = self.obj:GetComponent("UIComponent")
        Util.SetParent(self.obj,uiLayer[self.layer])
        Util.SetUIAnchor(self.obj)
        self:OnShow() 
    else
        self:OnHide() 
        Util.DestroyObj(self.obj)
    end
end

function UIManager.InitRoot()
    local gameObject = GameObject.Find("UIRoot") 
    local lanuch = GameObject.Find("lanuch") 
    if not gameObject then
        return
    end
    local tra = gameObject.transform
    uiLayer[UIManager.Layer.Buttom] = tra:GetChild(0).gameObject 
    uiLayer[UIManager.Layer.Normal] = tra:GetChild(1).gameObject 
    uiLayer[UIManager.Layer.Pop] = tra:GetChild(2).gameObject 
    uiLayer[UIManager.Layer.Message] = tra:GetChild(3).gameObject 
end

UIManager.InitRoot()
