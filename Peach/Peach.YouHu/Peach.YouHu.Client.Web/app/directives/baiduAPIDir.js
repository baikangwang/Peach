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

app.directive("yhfinding", [function () {
    return {
        restrict: 'E',
        replace:false,
        template:
            '<div style="width: 100%; height: 100%;"></div>',

        link: function ($scope, element, attrs) {

            $scope.load = function () {

                var j = element;

                var order = $scope.Order;
                var fus = $scope.freightUnits;

                var source = order.Source;

                // m : map
                // p: origin point
                // od: order
                function addOrgMarker(m, p, od) {
                    var icon = new BMap.Icon("../Content/img/box.png", new BMap.Size(32, 32));

                    var marker = new BMap.Marker(p, { icon: icon });
                    m.addOverlay(marker);

                    var label = new BMap.Label(od.Description, { offset: new BMap.Size(30, -10) });
                    label.setStyle({
                        color: "blue",
                        fontSize: "18px",
                        height: "20px",
                        lineHeight: "20px",
                        fontFamily: "微软雅黑",
                        width: "40px",
                        background: "none",
                        border: "none",
                        fontWeight: "bold"
                    });

                    marker.setLabel(label);
                }

                // m : map
                // p: origin point
                // f: freightUnit
                function addFreightUnitMaker(m, p, f) {
                    var icon = new BMap.Icon("../Content/img/lorry.png", new BMap.Size(32, 32));

                    var marker = new BMap.Marker(p, { icon: icon });

                    m.addOverlay(marker);
                    // marker.setAnimation(BMAP_ANIMATION_BOUNCE);

                    var label = new BMap.Label("&yen;" + f.Cost, { offset: new BMap.Size(30, -10) });
                    label.setStyle({
                        color: "green",
                        fontSize: "18px",
                        height: "20px",
                        lineHeight: "20px",
                        fontFamily: "微软雅黑",
                        width: "40px",
                        background: "none",
                        border: "none",
                        fontWeight: "bold"
                    });

                    marker.setLabel(label);
                }

                // m : map
                // op: origin point
                // od: order
                // fs: freightUnits
                function addMakers(m, op, od, fs) {

                    m.centerAndZoom(op, 12);                 // 货源位置做地图中心。

                    addOrgMarker(m, op, od);
                    // set freight units
                    for (var i = 0; i < fs.length; i++) {
                        var fu = fs[i];

                        var futemp = fu.Location.split("|")[2].split(",");
                        var fulng = new Number(futemp[0]); var fulat = new Number(futemp[1]);

                        var fuPoint = new BMap.Point(fulng, fulat);
                        addFreightUnitMaker(m, fuPoint, fu);
                    }
                }

                // set order source point
                var otemp = source.split("|")[2].split(",");
                var olng = new Number(otemp[0]);
                var olat = new Number(otemp[1]);
                // olng = 116.404; olat = 39.915;
                var oPoint = new BMap.Point(olng, olat);
                $scope.Map = new BMap.Map(j.find("div")[0]);
                $scope.Map.centerAndZoom(oPoint, 12);                // 货源位置做地图中心。
                $scope.Map.setMapStyle({ style: 'googlelite' });
                var map = $scope.Map;

                var loadCount = 1;
                map.addEventListener("tilesloaded", function () {
                    if (loadCount === 1) {
                        addMakers(map, oPoint, order, fus);
                    }
                    loadCount = loadCount + 1;
                });
            }
        }
    };
}]);

