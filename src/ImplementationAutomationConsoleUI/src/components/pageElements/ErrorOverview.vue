<template>
    <div>
        <div>
            <h2>Error Count</h2>
        </div>
        <div>
            <Doughnut :data="this.errorChartData" :options="this.errorChartOptions" />
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
            errorChartOptions: {
                cutoutPercentage: 80,
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
                    text: 'Error Count'
                },
            },
            
            errorChartData: {
                datasets: [
                    {
                        data: [this.errorCount, 100 - this.errorCount],
                        backgroundColor: [
                            'red',
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
        errorCount: {
            type: Number,
            required: true,
        },
    },
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
  