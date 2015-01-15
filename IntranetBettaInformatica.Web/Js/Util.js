function confirm(title, question, fnOk, fnCancel) {
    Ext.Msg.confirm(title, question, function (btn) {
        if (btn == 'yes') {
            if (fnOk)
                fnOk();
            return true;
        } else {
            if (fnCancel)
                fnCancel();
            return false;
        }
    });
}

function alert(title, msg) {
    Ext.Msg.alert(title, msg);
}
