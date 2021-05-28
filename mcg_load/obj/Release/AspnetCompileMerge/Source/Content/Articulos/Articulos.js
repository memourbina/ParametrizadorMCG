(function () {
    var selectedIds;
    var accion = "edit";
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
        pageToolbar.GetItemByName("Demanda").SetEnabled(enabled);
        pageToolbar.GetItemByName("Velocidad").SetEnabled(enabled);
        pageToolbar.GetItemByName("Cavidades").SetEnabled(enabled);
        pageToolbar.GetItemByName("Rechazos").SetEnabled(enabled);
        pageToolbar.GetItemByName("Copy").SetEnabled(enabled);
        pageToolbar.GetItemByName("demandamoving").SetEnabled(enabled);

        pageToolbar.GetItemByName("Edit").SetEnabled(gridView.GetFocusedRowIndex() !== -1);
    }

    function onPageToolbarItemClick(s, e) {
        switch (e.item.name) {
            case "ToggleFilterPanel":
                toggleFilterPanel();
                break;
            case "New":
                gridView.AddNewRow();
                break;
            case "Edit":
                EditSelectedRecords('edit');
                break;
            case "Delete":
                deleteSelectedRecords();
                break;
            case "Export":
                gridView.ExportTo(ASPxClientGridViewExportFormat.Xlsx);
                break;            
            case "Demanda":
                AccionesSelectedRecords('demanda');
                break;
            case "Velocidad":
                AccionesSelectedRecords('velocidad');
                break;
            case "Cavidades":
                AccionesSelectedRecords('cavidades');
                break;
            case "Rechazos":
                AccionesSelectedRecords('rechazos');
                break;
            case "Parametros":
                AccionesSelectedRecords('parametros');
                break;
            case "Horas":
                AccionesSelectedRecords('Horas');
                break;
            case "HorasCorazon":
                AccionesSelectedRecords('HorasCorazon');
                break;
            case "OEE":
                AccionesSelectedRecords('OEE');
                break;
            case "OEECorazon":
                AccionesSelectedRecords('OEECorazon');
                break;
            case "OEEDesglose":
                AccionesSelectedRecords('OEEDesglose');
                break;
            case "Copy":
                AccionesSelectedRecords('Copy');
                break;
            case "demandamoving":
                AccionesSelectedRecords('Moving');
                //MovingSelectedRecords('Moving');
                //window.location.href = 'Articulo/ExternalEditFormEdit?id_UpLoad=-1';                
                break;
        }
    }  
    function deleteSelectedRecords() {
        if (confirm('Confirm Delete?')) {
            gridView.GetSelectedFieldValues("id_articulo", getSelectedFieldValuesCallback);
        }
    }
    function EditSelectedRecords() {
        accion = 'edit';
        gridView.GetSelectedFieldValues("id_articulo", getSelectedEditFieldValuesCallback);
        gridView.StartEditRow(gridView.GetFocusedRowIndex());
    }
    function AccionesSelectedRecords(acc) {
        accion = acc;
        gridView.GetSelectedFieldValues("id_articulo", getSelectedEditFieldValuesCallback);
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