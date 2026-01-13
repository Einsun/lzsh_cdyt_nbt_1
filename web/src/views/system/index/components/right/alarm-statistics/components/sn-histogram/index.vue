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
        grid: {
          top: 15,
        },
        xAxis: {
          type: 'value',
          // data: columns,
          splitLine: {
            show: false
          },
          axisLine: {
            show: true,
            lineStyle: {
              color: labelColor
            }
          },
          axisTick: {
            show: false
          }
        },
        yAxis: {
          type: 'category',
          data: columns,
          splitLine: {
            show: false
          },
          axisLine: {
            show: false,
          },
          axisLabel: {
            color: labelColor,
            fontSize: 10,
          }
        },
        series: [{
          name: 'bar',
          type: 'bar',
          data: rows,
          barWidth: '10',
          barMinHeight: '5',
          animationDelay: function (idx) {
            return idx * 10;
          },
          label: {
            show: true,
            position: 'right',
            formatter: '({c})'
          },
          itemStyle: {
            color (params) {
              const colorList = ['#C53F36', '#389CC7', '#E27C49', '#219E82']
              return colorList[params.dataIndex % 4]
            },
          }
        }]
      }
      chartsInstance.setOption(options)
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
        return '180px'
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
