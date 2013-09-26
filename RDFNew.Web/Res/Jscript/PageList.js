Array.prototype.index = function(el) {
    var i = 0;
    for (var i = 0, len = this.length; i < len; i++) {
        if (el == this[i]) {
            return i;
        }
    }
    return -1;
}

var PageList = {
    "grid1ClientID": "",
    "window1ClientID": "",
    "SelectedRowIndex": -1,

    "grid_SelectRow": function(keyVal) {
        var grid = Ext.getCmp(this.grid1ClientID);
        var rowIndex = grid.x_state.X_Rows.DataKeys.index(keyValue);
        grid.getSelectionModel().selectRow(rowIndex);
        grid.getView().focusRow(rowIndex);
    },

    "grid_SelectRowFocus": function() {
        var grid = Ext.getCmp(this.grid1ClientID);
        var rows = grid.getSelectionModel().getSelections();
        if (rows.length > 0) {
            var keyvalue = grid.x_state.X_Rows.DataKeys[0];
            this.grid_SelectRow(keyvalue);
        }
    },

    "setWindowTitle": function(str) {
        var win = Ext.getCmp(this.window1ClientID);
        win.setTitle(str);
    },

    "showItemImage": function(sender) {
        var grid = Ext.getCmp(this.grid1ClientID);
        var rows = grid.getSelectionModel().getSelections();
        if (rows.length > 0) {
            var keyvalue = grid.x_state.X_Rows.DataKeys[0];
            var win = top.Ext.getCmp(this.window1ClientID);
            win.box_show(sender.src.replace("_S.", "_L."), "图片-[" + keyvalue + "]");
        }
    },

    "setOnReady": function() {
        var grid = Ext.getCmp(this.grid1ClientID);
        grid.on('rowclick', function(r, rowIndex) {
            PageList.SelectedRowIndex = rowIndex;
        });
        grid.addListener('viewready', function() {
            grid.getSelectionModel().selectRow(PageList.SelectedRowIndex);
            grid.getView().focusRow(PageList.SelectedRowIndex);
        });
    },

    "setOnAjaxReady": function() {
        var grid = Ext.getCmp(this.grid1ClientID);
        grid.getSelectionModel().selectRow(this.SelectedRowIndex);
        grid.getView().focusRow(this.SelectedRowIndex);
    }
}
