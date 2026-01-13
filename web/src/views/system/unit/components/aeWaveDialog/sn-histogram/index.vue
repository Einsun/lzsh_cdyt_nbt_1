<!--当前只考虑了一条数据一种颜色，如需更多需进一步抽象-->

<template>
  <div class="charts-histogram-wrapper">
    <div class="histogram-render" ref="histogramRender" :style="styles">

    </div>
  </div>
</template>

<script>
import Echarts from 'echarts'
export default {
  name: 'sn-histogram',
  data () {
    return {

    }
  },
  computed: {
    styles () {
      return {
        width: this.width,
        height: this.height
      }
    }
  },
  mounted () {
    this.$nextTick().then(() => {
      this.__initCharts()
    })
  },
  watch: {
    chartSettings () {
      this.__initCharts()
    },
    chartData: {
      deep: true,
      handler (newVal) {
        this.__initCharts()
      }
    },
  },
  methods: {
    __initCharts () {
      // 使用methods、computed、还是watch
      // 虽然computed更简单也有缓存，但个人（huangjinbo）不愿意使用computed，因为computed触发更新不可控
      // 必须使用watch来触发变更
      const targetNode = this.$refs.histogramRender
      const chartsInstance = Echarts.init(targetNode)
      // 维度暂时默认不可设     debugger
      console.log(this.chartData);
      const { rows = [], columns = [] } = this.chartData

      var initialDataLength = 1000;
      var initialData = columns.slice(0, initialDataLength);
      var currentIndex = initialDataLength;

      const labelColor = 'rgba(46, 192, 243, 1)'

      const axisStyle = {
        nameTextStyle: {
          color: labelColor
        },
        axisLabel: {
          color: labelColor
        }
      }
      let options = {
        color: ['#3398DB'],
        tooltip: {
          trigger: 'axis'
        },
        grid: {
          top: 15,
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          data: initialData.map(function (_, index) {
            return index + 1;
          }),
          splitLine: {
            show: false
          },
          axisLabel: {
            show: false,
          },
          axisLine: {
            show: true,
            lineStyle: {
              color: labelColor
            }
          },
          axisTick: {
            show: true
          }
        },
        yAxis: {
          type: 'value',
          splitLine: {
            show: false
          },
          axisLine: {
            show: false,
          },
          axisLabel: {
            color: labelColor,
            fontSize: 10,
          },
        },
        series: {
          type: 'line',
          data: rows,
          markLine: {
            silent: true,
            lineStyle: {
              color: '#333'
            },
          },
          label: {
            show: false,
            position: 'right',
            formatter: '({c})'
          },
          itemStyle: {
            color (params) {
              return '#389CC7'
            },
          }
        },
        dataZoom: [{
          type: 'slider',
          start: 0,
          end: 100,
        }],
      }
      chartsInstance.setOption(options)
      // 每秒更新一次数据
      var updateInterval = 1000;
      setInterval(function () {
        if (currentIndex < columns.length) {
          // 添加新的数据点
          chartsInstance.setOption({
            xAxis: {
              data: columns.slice(0, currentIndex + 100).map(function (_, index) {
                return index + 1;
              }),
            },
            series: [{
              data: rows.slice(0, currentIndex + 100),
            }],
          });

          currentIndex += 1000;
        }
      }, updateInterval);
    },
  },
  props: {
    width: {
      type: String,
      default () {
        return '100%'
      }
    },
    height: {
      type: String,
      default () {
        return '600px'
      }
    },
    chartData: {
      type: Object,
      default () {
        return {
          columns: [],
          rows: []
        }
      }
    }
  }
}
</script>

<style scoped lang="scss">
</style>
