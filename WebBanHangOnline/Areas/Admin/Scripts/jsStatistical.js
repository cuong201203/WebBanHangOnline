$(function () {
    $('#fromDate').change(function () {
        var fromDate = new Date($('#fromDate').val());
        var toDate = new Date($('#toDate').val());

        if (fromDate > toDate) {
            $('#toDate').val($('#fromDate').val());
        }
    });

    $('#toDate').change(function () {
        var fromDate = new Date($('#fromDate').val());
        var toDate = new Date($('#toDate').val());

        if (toDate < fromDate) {
            $('#fromDate').val($('#toDate').val());
        }
    });

    $('#btnSearch').click(function () {
        var fromDate = $('#fromDate').val();
        var toDate = $('#toDate').val();
        var viewMode = $('#viewMode').val();
        loadStatisticalData(fromDate, toDate, viewMode);
    });

    // Load default data for last 15 days
    var defaultFromDate = moment().subtract(14, 'days').format('YYYY-MM-DD');
    var defaultToDate = moment().format('YYYY-MM-DD');
    $('#fromDate').val(defaultFromDate);
    $('#toDate').val(defaultToDate);
    $('#viewMode').val('day');
    loadStatisticalData(defaultFromDate, defaultToDate, 'day');
});

var barChart;
function loadStatisticalData(fromDate, toDate, viewMode) {
    var arrRevenue = [];
    var arrProfit = [];
    var arrDate = [];

    $.ajax({
        url: '/Admin/Statistical/GetStatistical',
        type: 'GET',
        data: { fromDate: fromDate, toDate: toDate, viewMode: viewMode },
        success: function (result) {
            var isMonthView = viewMode === 'month';
            var isYearView = viewMode === 'year';

            $.each(result.Data, function (i, item) {
                var strDate = isMonthView ? moment(item.Date).format('MM/YYYY')
                            : isYearView ? moment(item.Date).format('YYYY')
                            : moment(item.Date).format('DD/MM/YYYY');
                arrDate.push(strDate);
                arrRevenue.push(item.Revenue);
                arrProfit.push(item.Profit);
            });

            var areaChartData = {
                labels: arrDate,
                datasets: [
                    {
                        label: '', // Lợi nhuận
                        //backgroundColor: 'rgba(60,141,188,0.9)',
                        backgroundColor: '#fff',
                        borderColor: 'rgba(60,141,188,0.8)',
                        pointRadius: false,
                        pointColor: '#3b8bba',
                        pointStrokeColor: 'rgba(60,141,188,1)',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(60,141,188,1)',
                        data: arrProfit,
                        hidden: true // Ẩn cột lợi nhuận
                    },
                    {
                        label: '', // Doanh thu
                        backgroundColor: 'rgba(210, 214, 222, 1)',
                        //backgroundColor: '#fff',
                        borderColor: 'rgba(210, 214, 222, 1)',
                        pointRadius: false,
                        pointColor: 'rgba(210, 214, 222, 1)',
                        pointStrokeColor: '#c1c7d1',
                        pointHighlightFill: '#fff',
                        pointHighlightStroke: 'rgba(220,220,220,1)',
                        data: arrRevenue
                    },
                ]
            };

            if (barChart) {
                barChart.destroy();
            }

            var barChartCanvas = $('#barChart').get(0).getContext('2d');
            var barChartData = $.extend(true, {}, areaChartData);
            var temp0 = areaChartData.datasets[0];
            var temp1 = areaChartData.datasets[1];
            barChartData.datasets[0] = temp1;
            barChartData.datasets[1] = temp0;

            var barChartOptions = {
                responsive: true,
                maintainAspectRatio: false,
                datasetFill: false,
                tooltips: {
                    callbacks: {
                        label: function (tooltipItem, data) {
                            var dataset = data.datasets[tooltipItem.datasetIndex];
                            var value = dataset.data[tooltipItem.index];
                            var formattedValue = value.toLocaleString('vi-VN') + 'đ';
                            //return dataset.label + ': ' + formattedValue;
                            return formattedValue;
                        }
                    }
                },
                legend: {
                    display: false // Ẩn nhãn của cả hai cột
                }
            };

            barChart = new Chart(barChartCanvas, {
                type: 'bar',
                data: barChartData,
                options: barChartOptions
            });

            loadRevenue(result.Data, viewMode);
        }
    });
}

function loadRevenue(data, viewMode) {
    var strHtml = '';
    $('#headerDate').text(viewMode === 'month' ? 'Tháng' : viewMode === 'year' ? 'Năm' : 'Ngày');
    $.each(data, function (i, item) {
        var strDate = viewMode === 'month' ? moment(item.Date).format('MM/YYYY')
                    : viewMode === 'year' ? moment(item.Date).format('YYYY')
                    : moment(item.Date).format('DD/MM/YYYY');
        var formattedRevenue = item.Revenue.toLocaleString('vi-VN') + "<u>đ</u>";
        //var formattedProfit = item.Profit.toLocaleString('vi-VN') + "<u>đ</u>";
        strHtml += "<tr>";
        strHtml += "<td>" + (i + 1) + "</td>";
        strHtml += "<td>" + strDate + "</td>";
        strHtml += "<td>" + formattedRevenue + "</td>";
        //strHtml += "<td>" + formattedProfit + "</td>";
        strHtml += "</tr>";
    });
    $('#loadRevenue').html(strHtml);
}

$(document).ready(function () {
    loadProductStatistics();

    // Xử lý sắp xếp
    $('.sortable').click(function () {
        var sortField = $(this).data('sort-field');
        var currentSortOrder = $(this).data('sort-order') || 'desc';
        var newSortOrder = currentSortOrder === 'desc' ? 'asc' : 'desc';
        $(this).data('sort-order', newSortOrder);

        // Đặt lại icon về mặc định
        $('.sortable i').removeClass('fa-caret-up fa-caret-down').addClass('fa-sort');

        // Cập nhật icon
        if (newSortOrder === 'asc') {
            $(this).find('i').removeClass('fa-sort fa-caret-down').addClass('fas fa-caret-up');
        } else {
            $(this).find('i').removeClass('fa-sort fa-caret-up').addClass('fas fa-caret-down');
        }

        loadProductStatistics(sortField, newSortOrder);
    });
});

function loadProductStatistics(sortField = 'soldQuantity', sortOrder = 'desc') {
    $.ajax({
        url: '/Admin/Statistical/GetProductStatistics',
        type: 'GET',
        data: { sortField: sortField, sortOrder: sortOrder },
        success: function (result) {
            var strHtml = '';
            $.each(result.Data, function (index, item) {
                // Chuyển đổi ExpiredDate từ /Date(<timestamp>)/ sang đối tượng Date
                var expiredDate = new Date(parseInt(item.ExpiredDate.replace("/Date(", "").replace(")/", "")));

                // Định dạng lại ExpiredDate thành "dd//MM/yyyy"
                var formattedDate = expiredDate.getDate().toString().padStart(2, '0') + '/' +
                    (expiredDate.getMonth() + 1).toString().padStart(2, '0') + '/' +
                    expiredDate.getFullYear();

                strHtml += '<tr>';
                strHtml += '<td>' + (index + 1) + '</td>';
                strHtml += '<td><img src="' + item.ProductImage + '" style="width: 50px; height: 50px;" /></td>';
                strHtml += '<td>' + item.ProductName + '</td>';
                strHtml += '<td>' + item.SoldQuantity + '</td>';
                strHtml += '<td>' + item.RemainingQuantity + '</td>';
                strHtml += '<td>' + formattedDate + '</td>';
                strHtml += '</tr>';
            });
            $('#loadProductStatistic').html(strHtml);
        }
    });
}