



$(document).ready(function () {

    $("div.container-fluid").LoadingOverlay("show");

    fetch("/DashBoard/GetResume").then(response => {
        $("div.container-fluid").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    }).then(res => {
        if (res.state) {
            let d = res.object;

            // Show data in cards 
            $("#totalSale").text(d.totalSales)
            $("#totalIncome").text(d.totalIncome)
            $("#totalProducts").text(d.totalProducts)
            $("#totalCategories").text(d.totalCategories)

            // Get text and values for bar chart
            let barchart_labels;
            let barchart_data;

            if (d.lastWeekSales.length > 0) {
                barchart_labels = d.lastWeekSales.map((item) => { return item.date })
                barchart_data = d.lastWeekSales.map((item) => { return item.total })
            } else {
                barchart_labels = ["Sin resultados"];
                barchart_labels = ["0"];
            }

            // Get text and values for pie chart
            let piechart_labels;
            let piechart_data;

            if (d.lastWeekTopProducts.length > 0) {
                piechart_labels = d.lastWeekTopProducts.map((item) => { return item.product })
                piechart_data = d.lastWeekTopProducts.map((item) => { return item.quantity })
            } else {
                piechart_labels = ["Sin resultados"];
                piechart_data = ["0"];
            }

            // Bar Chart Example
            let controlVenta = document.getElementById("salesChart");
            let myBarChart = new Chart(controlVenta, {
                type: 'bar',
                data: {
                    labels: barchart_labels,
                    datasets: [{
                        label: "Cantidad",
                        backgroundColor: "#4e73df",
                        hoverBackgroundColor: "#2e59d9",
                        borderColor: "#4e73df",
                        data: barchart_data,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    legend: {
                        display: false
                    },
                    scales: {
                        xAxes: [{
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            maxBarThickness: 50,
                        }],
                        yAxes: [{
                            ticks: {
                                min: 0,
                                maxTicksLimit: 5
                            }
                        }],
                    },
                }
            });

            // Pie Chart Example
            let controlProducto = document.getElementById("productsChart");
            let myPieChart = new Chart(controlProducto, {
                type: 'doughnut',
                data: {
                    labels: piechart_labels,
                    datasets: [{
                        data: piechart_data,
                        backgroundColor: ['#4e73df', '#1cc88a', '#36b9cc', "#FF785B"],
                        hoverBackgroundColor: ['#2e59d9', '#17a673', '#2c9faf', "#FF5733"],
                        hoverBorderColor: "rgba(234, 236, 244, 1)",
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        caretPadding: 10,
                    },
                    legend: {
                        display: true
                    },
                    cutoutPercentage: 80,
                },
            });

        }


    })


})