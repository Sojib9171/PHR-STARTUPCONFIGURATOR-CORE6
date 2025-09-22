<template>
    <div>
        <div>
            <h2>Success Count</h2>
        </div>
        <div>
            <Doughnut :data="this.successChartData" :options="this.successChartOptions" />
        </div>
    </div>
</template>
  
<script>
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from 'chart.js'
import { Doughnut } from 'vue-chartjs'
import ChartDataLabels from 'chartjs-plugin-datalabels';

ChartJS.register(ArcElement, Tooltip, Legend, ChartDataLabels)

export default {
    data() {
        return {
            successChartOptions: {
                cutoutPercentage: 30,
                legend: {
                    display: false
                },
                tooltips: {
                    enabled: false
                },
                animation: {
                    duration: 2000,
                    easing: 'easeOutQuart'
                },
                responsive: true,
                maintainAspectRatio: false,
                hoverOffset: 4,
                title: {
                    display: true,
                    text: 'Success Count'
                },
            },
            
            successChartData: {
                datasets: [
                    {
                        data: [this.successCount, 100 - this.successCount],
                        backgroundColor: [
                            'green',
                            'transparent',
                        ],
                        borderWidth: 0,
                    },
                ],
            }
        }
    },
    components: {
        Doughnut
    },
    props: {
        successCount: {
            type: Number,
            required: true,
        },
    },
    // mounted() {
    //     const chartData = {
    //         datasets: [
    //             {
    //                 data: [this.successCount, 100 - this.successCount],
    //                 backgroundColor: [
    //                     'green',
    //                     'transparent',
    //                 ],
    //                 borderWidth: 0,
    //             },
    //         ],
    //     };

    //     const chartOptions = {
    //         cutoutPercentage: 80,
    //         tooltips: {
    //             enabled: false,
    //         },
    //         hover: {
    //             mode: null,
    //         },
    //     };

    //     new Chart(this.$refs.chart, {
    //         type: 'doughnut',
    //         data: chartData,
    //         options: chartOptions,
    //     });
    // },
};
</script>
  
<style>
.chart-container {
    position: relative;
    width: 100px;
    height: 100px;
}

.chart-label {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    font-size: 20px;
    font-weight: bold;
}
</style>
  