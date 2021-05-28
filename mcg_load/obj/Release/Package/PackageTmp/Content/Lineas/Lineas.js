(function() {
    var selectedIds;
    function onGridViewInit(s, e) {
        AddAdjustmentDelegate(adjustGridView);
        updateToolbarButtonsState();
    }
    function onGridViewSelectionChanged(s, e) {
        updateToolbarButtonsState();
    }
    function adjustGridView() {
        gridView.AdjustControl();
    }
    function updateToolbarButtonsState() {
        var enabled = gridView.GetSelectedRowCount() > 0;
        pageToolbar.GetItemByName("Delete").SetEnabled(enabled);
        pageToolbar.GetItemByName("Export").SetEnabled(enabled);
        pageToolbar.GetItemByName("Edit").SetEnabled(enabled);
        pageToolbar.GetItemByName("Horas").SetEnabled(enabled);
        pageToolbar.GetItemByName("OEE").SetEnabled(enabled);
        pageToolbar.GetItemByName("OEEDesglose").SetEnabled(enabled);

        //pageToolbar.GetItemByName("Edit").SetEnabled(gridView.GetFocusedRowIndex() !== -1);
    }
    function onPageToolbarItemClick(s, e) {
        switch(e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                gridView.AddNewRow();
                break;
            case "Edit":
                gridView.StartEditRow(gridView.GetFocusedRowIndex());
                break;
            case "Horas":
                AccionesSelectedRecords('Horas');
                break;
            case "OEE":
                AccionesSelectedRecords('OEE');
                break;
            case "OEEDesglose":
                AccionesSelectedRecords('OEEDesglose');
                break;
            case "Delete":
                deleteSelectedRecords();
                break;
            case "Export":
                gridView.ExportTo(ASPxClientGridViewExportFormat.Xlsx);
                break;
        }
    }
    function AccionesSelectedRecords(acc) {
        accion = acc;
        gridView.GetSelectedFieldValues("Linea", getSelectedEditFieldValuesCallback);
    }

    function deleteSelectedRecords() {
        if(confirm('Confirm Delete?')) {
            gridView.GetSelectedFieldValues("Linea", getSelectedFieldValuesCallback);
        }
    }
    function onFiltersNavBarItemClick(s, e) {
        var filters = {
            All: "",
            disponible: "[eliminado] = false",
            no_disponible: "[eliminado] = true",
            Active: "[activo] = true",
            Inactive: "[activo] = false",
            escenario_escenario: "[id_tipo_escenario] = 2",
            escenario_base: "[id_tipo_escenario] = 1"
        };
        gridView.ApplyFilter(filters[e.item.name]);
        HideLeftPanelIfRequired();
    }

    function toggleFilterPanel() {
        filterPanel.Toggle();
    }

    function onFilterPanelExpanded(s, e) {
        adjustPageControls();
        searchButtonEdit.SetFocus();
    }

    function onGridViewBeginCallback(s, e) {
        e.customArgs['SelectedRows'] = selectedIds;
    }
    function getSelectedFieldValuesCallback(values) {
        selectedIds = values.join(',');
        gridView.PerformCallback({ customAction: 'delete' });
    }

    function getSelectedEditFieldValuesCallback(values) {
        selectedIds = values.join(',');
        gridView.PerformCallback({ customAction: accion });
    }

    window.onGridViewBeginCallback = onGridViewBeginCallback;
    window.onGridViewInit = onGridViewInit;
    window.onGridViewSelectionChanged = onGridViewSelectionChanged;
    window.onPageToolbarItemClick = onPageToolbarItemClick;
    window.onFilterPanelExpanded = onFilterPanelExpanded;
    window.onFiltersNavBarItemClick = onFiltersNavBarItemClick;
})();