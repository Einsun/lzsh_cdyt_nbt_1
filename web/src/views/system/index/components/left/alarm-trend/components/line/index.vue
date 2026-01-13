<template>
  <div class="charts-line-wrapper">
    <div class="line-render" ref="lineRender"></div>
  </div>
</template>

<script>
import Echarts from 'echarts';
export default {
  name: 'index',
  data () {
    return {};
  },
  computed: {},
  mounted () {
    this.$nextTick().then(() => {
      this._initCharts();
    });
  },
  methods: {
    _initCharts () {
      const targetNode = this.$refs.lineRender;
      const chartsInstance = Echarts.init(targetNode);
      const { columns = [], alarms = [], fault = [] } = this.chartData;
      // 指定图表的配置项和数据
      const option = {
        title: {
          // text: '30天内报警故障趋势'
        },
        tooltip: {
          trigger: 'axis',
          axisPointer: {
            animation: true
          }
        },
        legend: {
          data: ['报警', '故障'],
          left: '276',
          icon: 'circle',
          textStyle: {
            color: 'rgba(255,255,255,1)',
            fontWeight: 500,
            fontSize: 12
          }
        },
        markLine: {
          lineStyle: {
            color: 'rgba(46, 192, 243, 1)'
          }
        },
        textStyle: {
          color: 'rgba(46, 192, 243, 1)'
        },

        xAxis: {
          type: 'category',
          boundaryGap: false,
          splitLine: {
            show: false
          },
          axisLine: {
            lineStyle: {
              color: 'rgba(46, 192, 243, 1)'
            }
          },
          data: columns
        },
        yAxis: {
          type: 'value',
          splitLine: {
            show: false
          },
          axisLine: {
            show: false,
            width: 0
          },
          axisTick: {
            show: false
          }
        },
        series: [
          {
            name: '报警',
            type: 'line',
            // areaStyle: { color: "#E27C49" },
            data: alarms,
            color: '#C53F36',
            smooth: true,
            showSymbol: false

          },
          {
            name: '故障',
            type: 'line',
            // areaStyle: { color: "#C53F36" },
            data: fault,
            color: '#E27C49',
            smooth: true,
            showSymbol: false

          }
        ]
      };
      // 使用刚指定的配置项和数据显示图表。
      chartsInstance.setOption(option);
    }
  },
  props: {
    chartData: {
      type: Object,
      default () {
        return {
          columns: [], // x轴的内容
          alarms: [], // 报警
          fault: [] // 故障
        };
      }
    }
  },
  watch: {
    chartData: {
      deep: true,
      handler (newVal) {
        this._initCharts();
      }
    }
  }
};
</script>

<style scoped lang="scss">
.line-render {
  width: 419px;
  height: 200px;
}
</style>
