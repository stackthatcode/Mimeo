
var Monster = Monster || {};

Monster.StandardErrorMessage = "Something went wrong. Please refresh your browser and try again.";
Monster.Http403ErrorMessage = "You are not authorized to view that resource.";
Monster.Http401ErrorMessage = "You appear to be currently logged out of EWC.";

// Test it out!
//
Monster.BaseUrl = "";


Monster.ErrorPopup = function (message) {
    alert(message || Monster.StandardErrorMessage);
}

Monster.AjaxSettings = function (modal) {
    return {
        BaseUrl: Monster.BaseUrl,
        Timeout: 120000,    // Two minutes for development
        UseSpinner: true,
        Modal: modal || false,
    };
};

Monster.ShowLoading = function () {
    $("#spinner-layer").show();
};

Monster.HideLoading = function () {
    $("#spinner-layer").hide();
};

Monster.AjaxGet = function (url) {
    flow.exec(
        function () {
            var ajax = new Monster.Ajax();
            ajax.HttpGet(url, this);
        },
        function (data) {
            return data;
        });
};

Monster.Ajax = function (settings) {
    var self = this;

    self.Settings = settings || new Monster.AjaxSettings();

    self.SuppressErrorPopup = false;

    self.DisablePopupsAndSpinners = function () {
        self.SuppressErrorPopup = false;
        self.Settings.UseSpinner = false;
    };


    self.ErrorCallback = function (jqXHR, textStatus, errorThrown) {
        self.HideLoading();

        if (jqXHR.status != 0 || textStatus == "timeout") {
            var message = Monster.StandardErrorMessage;

            if (jqXHR.status == 401) {
                message = Monster.Http401ErrorMessage;
            }

            if (jqXHR.status == 403) {
                message = Monster.Http403ErrorMessage;
            }

            if (self.SuppressErrorPopup) {
                console.log(jqXHR, textStatus, message);
                return;
            }

            if (self.Settings.Modal) {
                alert(message);
            } else {
                Monster.ErrorPopup(message);
            }
        }
    };

    self.AjaxUniqueUrl = function (url) {
        var dateStampParam = "_=" + new Date().getTime();
        return url.indexOf("?") == -1 ? url + "?" + dateStampParam : url + "&" + dateStampParam;
    };

    self.HttpGet = function (url, successFunc) {
        flow.exec(
            function () {
                self.ShowLoading();
                $.ajax({
                    type: 'GET',
                    dataType: 'json',
                    url: self.Settings.BaseUrl + self.AjaxUniqueUrl(url),
                    timeout: self.Settings.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function (data, textStatus, jqXHR) {
                self.HideLoading();

                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    };

    self.HttpPost = function (url, data, successFunc) {
        var token = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();

        flow.exec(
            function () {
                self.ShowLoading();

                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    beforeSend: function (request) {
                        request.setRequestHeader("__RequestVerificationToken", token);
                    },
                    contentType: 'application/json; charset=utf-8',
                    url: self.Settings.BaseUrl + self.AjaxUniqueUrl(url),
                    data: JSON.stringify(data),
                    timeout: self.Settings.Timeout,
                    error: self.ErrorCallback,
                    success: this
                });
            },
            function (data, textStatus, jqXHR) {
                self.HideLoading();
                if (successFunc) {
                    successFunc(data, textStatus, jqXHR);
                }
            }
        );
    };

    self.ShowLoading = function () {
        if (self.Settings.UseSpinner) {
            Monster.ShowLoading();
        }
    };

    self.HideLoading = function () {
        if (self.Settings.UseSpinner) {
            Monster.HideLoading();
        }
    };

    return self;
};

