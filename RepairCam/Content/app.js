function webDialogsManagerCallback(calbackPanel) {
    if (calbackPanel.cpDialogs) {
        eval(calbackPanel.cpDialogs + ".Show();");
    }
}

function createDialog(name, mode, isFirstPanel) {
    var json = { dialog: name, mode: mode };
    json = JSON.stringify(json);
    if (isFirstPanel || isFirstPanel == undefined)
        $DMCP.PerformCallback(json);
    else 
        $DMCP2.PerformCallback(json);
}

function renderScripts(panel) {
    if (panel.cpResulrScript) {
        eval(panel.cpResulrScript);
    }
}

function refreshState() {
    var enabled = _selectCustomerRadio.GetChecked();
    _customerCombo.SetEnabled(enabled);
    _nameTxt.SetEnabled(!enabled);
    _emailTxt.SetEnabled(!enabled);
    _phoneTxt.SetEnabled(!enabled);
}
