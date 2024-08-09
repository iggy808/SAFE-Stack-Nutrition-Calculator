import Chart from "chart.js/auto";

// userWeightHistory : (DateOnly * float) = (Date, Weight)
export function initChart(userWeightHistory) {
    
    const data = [
        { year: 2010, count: 10 },
        { year: 2011, count: 20 },
        { year: 2012, count: 15 },
        { year: 2013, count: 25 },
        { year: 2014, count: 22 },
        { year: 2015, count: 30 },
        { year: 2016, count: 28 },
    ];

    console.log(userWeightHistory);

    const weightHistory = [
        { Date: userWeightHistory.head.Date, Weight: userWeightHistory.head.Weight }
    ];
    console.log(weightHistory);

    new Chart(
        document.getElementById('weight-chart-container'),
        {
            type: 'line',
            data: {
                labels: weightHistory.map(row => row.Date),
                datasets: [
                    {
                        label: 'Weight by day',
                        data: weightHistory.map(row => row.Weight)
                    }
                ]
            }
        }
    );
}