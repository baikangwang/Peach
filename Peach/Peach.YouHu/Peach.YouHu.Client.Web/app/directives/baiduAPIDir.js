app.directive("yhlocation", [function () {
    return {
        restrict: 'E',
        template:
            '<div style="display:none"></div>',

        link: function ($scope, element, attrs) {
            var j = element;

            var target= j.attr("target");

            var map = new BMap.Map(j.find("div")[0]);

            var point = new BMap.Point(116.331398, 39.897445); // 北京
            map.centerAndZoom(point, 12);                   // 初始化地图,设置城市和地图级别。

            var ac = new BMap.Autocomplete(    //建立一个自动完成的对象
            {
                "input": target,
                "location": map
            });

            ac.addEventListener("onconfirm", function (e) {    //鼠标点击下拉列表后的事件
                var value = e.item.value;
                var address = value.province + value.city + value.district + value.street + value.business;
                var geo = new BMap.Geocoder();
                geo.getPoint(address, function (point) {
                    if (point) {
                        var location = address + "|" +value.city + "|" + point.lng + "," + point.lat;
                        if (target === "source")
                            $scope.model.Source = location;
                        else if (target == "destination")
                            $scope.model.Destination = location;
                        else
                            $scope.model.Location = location;
                    }
                    else {
                        alert("找不到这个地方");
                        this.preventDefault();
                    }
                });
            });
            
        }
    };
}]);

