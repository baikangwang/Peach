var YouHuHelper = YouHuHelper || {};

YouHuHelper.orderStateHelper = {

    toLabel: function (state) {
        //Ready = 0,
        //Dealing = 1,
        //Rejected = 2,
        //Dealt = 3,
        //Paying = 4,
        //Paid = 5,
        //InProgress = 6,
        //Arrived = 7,
        //Consigned = 8

        if (state) {
            switch (state) {
                case 0:
                    return "ready";
                case 1:
                    return "dealing";
                case 2:
                    return "rejected";
                case 3:
                    return "dealt";
                case 4:
                    return "paying";
                case 5:
                    return "paid";
                case 6:
                    return "inprogress";
                case 7:
                    return "arrived";
                case 8:
                    return "consigned";
                default:
                    return "ready";
            }

        } else
            return "ready";
    }
};

YouHuHelper.freightUnitTypeHelper = {
    toLabel: function (state) {

        // Ready = 0,
        // Dealing = 1,
        // Rejected = 2,
        // Dealt = 3,
        // Paying = 4,
        // Paid = 5,
        // InProgress = 6,
        // Arrived = 7,
        // Consigned = 8

        if (state) {
            switch (state) {
                case 0:
                    return "lorry";
                case 1:
                    return "miniVan";
                case 2:
                    return "tankLorry";
                case 3:
                    return "truck";
                case 4:
                    return "van";
                default:
                    return "truck";
            }

        } else
            return "truck";
    }
};

YouHuHelper.freightUnitStateHelper= {
    toLabel:function (state) {
        if (state) {
            switch (state) {
                case 0:
                    return "none";
                case 1:
                    return "ready";
                case 2:
                    return "busy";
                default:
                    return "none";
            }

        } else
            return "none";
    }
}

YouHuHelper.errorHelper= {
    getErrorMsg:function (error) {
        var msg;
        if (error.ExceptionMessage)
            msg = error.ExceptionMessage;
        else if (error.Message)
            msg = error.Message;
        else
            msg = 'Error occured';
        //alert(msg);
        return msg;
    }
}