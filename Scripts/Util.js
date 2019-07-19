function showLoading(message, element) {
    if (element) {
        $(element).block({
            message: loadingImage.outerHTML + '</br><h3 id="text_load">' + message + '</h3>',
            css: { backgroundColor: 'initial', border: 'none', color: '#fff' }
        });
    }
    else {
        $.blockUI({
            message: loadingImage.outerHTML + '</br><h3>' + message + '</h3>',
            css: { backgroundColor: 'initial', border: 'none', color: '#fff' }
        });
    }
}
function hideLoading(element) {
    if (element) {
        $(element).unblock();
    }
    else {
        $.unblockUI();
    }
}