
function initMenu() {
    var mainMenu = Ext.getCmp(IDS.mainMenu);
    var mainTabStrip = Ext.getCmp(IDS.mainTabStrip);
    var windowSourceCode = Ext.getCmp(IDS.windowSourceCode);

    function getExpandedPanel() {
        var panel = null;
        mainMenu.items.each(function(item) {
            if (!item.collapsed) {
                panel = item;
            }
        });
        return panel;
    }

    // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
    // 1. treeMenu, 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
    // 2. mainTabStrip, 主框架中的选项卡控件实例
    // 3. tbarCallback, 在每个选项卡上创建工具栏的回调函数，如果不需要选项卡工具栏，可以设置此值为null
    // 4. updateLocationHash, 切换选项卡时是否在top.location.hash记录当前页面的地址
    X.util.initTreeTabStrip(mainMenu, mainTabStrip, null, true);

    // 公开添加示例标签页的方法
    window.addExampleTab = function(id, url, text, icon) {
        X.util.addMainTab(mainTabStrip, id, url, text, icon);
    };

    window.removeActiveTab = function() {
        var activeTab = mainTabStrip.getActiveTab();
        mainTabStrip.removeTab(activeTab.id);
    };
}

Ext.onReady(initMenu);