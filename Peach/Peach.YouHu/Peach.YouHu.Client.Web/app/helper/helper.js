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
                    return "未接单";//"ready";
                case 1:
                    return "待接单";//"dealing";
                case 2:
                    return "被拒绝";//"rejected";
                case 3:
                    return "已接单";//"dealt";
                case 4:
                    return "待付款";//"paying";
                case 5:
                    return "已付款";//"paid";
                case 6:
                    return "运输中";//"inprogress";
                case 7:
                    return "已送达";//"arrived";
                case 8:
                    return "已收货";//"consigned";
                default:
                    return "待命中";//"ready";
            }

        } else
            return "ready";
    },

    toValue:function (state) {
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
    },

    toCSS: function (state) {

        //Ready = 0,
        //Dealing = 1,
        //Rejected = 2,
        //Dealt = 3,
        //Paying = 4,
        //Paid = 5,
        //InProgress = 6,
        //Arrived = 7,
        //Consigned = 8

        switch (state) {
            case 0://"ready":
            case 2://"rejected":
                return "warning";
            case 1://"dealing":
                return "info";
            case 3://"dealt":
            case 4://"paying":
            case 5://"paid":
                return "danger";
            case 6://"inprogress":
                return "info";
            case 7://"arrived":
                return "success";
            case 8://"consigned":
                return "default";
            default:
                return "warning";
        }
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
                    return "牵引车";//"lorry";
                case 1:
                    return "面包车";//"miniVan";
                case 2:
                    return "罐车";//"tankLorry";
                case 3:
                    return "卡车";//"truck";
                case 4:
                    return "皮卡车";//"van";
                default:
                    return "卡车";//"truck";
            }

        } else
            return "卡车";//"truck";
    }
};

YouHuHelper.freightUnitStateHelper = {
    toLabel: function (state) {
        if (state) {
            switch (state) {
                case 0:
                    return "未发布";//"none";
                case 1:
                    return "待接单";//"ready";
                case 2:
                    return "运输中";//"busy";
                default:
                    return "未发布";//"none";
            }

        } else
            return "未发布";//"none";
    },

    toCSS: function (state) {
        switch (state) {
            case "none":
                return "default";
            case "busy":
                return "danger";
            case "ready":
                return "success";
            default:
                return "default";
        }
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
            msg = '发生错误';
        //alert(msg);
        return msg;
    }
}