<!--当前只考虑了一条数据一种颜色，如需更多需进一步抽象-->

<template>
  <div class="charts-line-wrapper">
    <div class="line-render" ref="lineRender" :style="styles">

    </div>
  </div>
</template>

<script>
// 查看文档及源码后发现v-charts line不能改变样式，所以更换为echarts
// v-charts 源码可参考getLineXAxis，参数已被写死，传入也是无效的
// 参考v-charts传入的数据以和其他组件保持一致，将其转换为echarts options
import Echarts from 'echarts'
import VeRing from 'v-charts/lib/line.common'
export default {
  name: 'sn-line',
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
      const targetNode = this.$refs.lineRender
      const chartsInstance = Echarts.init(targetNode)
      // 维度暂时默认不可设     debugger

      const { rows = [], columns = [] } = this.chartData

      const labelColor = 'rgba(255,255,255,.8)'
      const axisLineColor = 'rgba(101,226,245,.5)'
      const splitLineColor = 'rgba(101,226,245,.1)'
      const axisPointerLineColor = 'rgba(101,226,245,.2)'

      const axisStyle = {
        nameTextStyle: {
          color: labelColor
        },
        axisLine: {
          lineStyle: {
            // notice 无法与设计稿保持一致
            color: axisLineColor
          }
        },
        axisLabel: {
          color: labelColor
        }
      }

      const seriesStyle = {
        itemStyle: {
          color: '#65E2F5',
          borderColor: '#65E2F5',
          borderWidth: 2,

        },
        lineStyle: {
          color: '#65E2F5',
          width: 2
        },
        areaStyle: {
          color: {
            type: 'linear',
            x: 0,
            y: 0,
            x2: 0,
            y2: 1,
            colorStops: [{
              offset: 0.5, color: 'rgba(101, 226, 245, 0.5)' // 50% 处的颜色
            }, {
              offset: 1, color: 'rgba(101, 226, 245, 0)' // 100% 处的颜色
            }],
            global: false // 缺省为 false
          }
        }
      }
      const { yAxisName } = this.chartSettings
      let options = {
        // grid: {
        //   show: true,
        //   backgroundColor: 'rgba(101,226,245,0.1)',
        //   borderColor: 'rgba(101,226,245,0.1)',
        //   borderWidth: 10,
        //   z: 100
        // },
        tooltip: {
          trigger: 'axis',
          formatter: (params) => {
            return `
                <div class="toolTip-content-wrapper">
                  <span class="color-secondary">${params[0].name}</span><br/>
                  <span class="gape">${yAxisName}:${params[0].value}</span>
                  <span class="status">状态:正常</span>
                </div>
            `
          },
          axisPointer: {
            show: true,
            lineStyle: {
              color: axisPointerLineColor
            }
          },
        },
        xAxis: {
          type: this.chartSettings.xAxisType || 'value',
          boundaryGap: false,
          minInterval: 5,
          data: rows.map(item => {
            return item[columns[0]]
          }),
          name: this.chartSettings.xAxisName || '',
          ...axisStyle
        },
        yAxis: {
          type: 'value',
          name: this.chartSettings.yAxisName || '',
          splitLine: {
            show: true,
            lineStyle: {
              color: splitLineColor
            }
          },
          ...axisStyle
        },
        series: columns.map((key, index) => {
          if (index === 0) {
            return undefined
          } else {
            return {
              data: rows.map(item => {
                return item[key]
              }),
              type: 'line',
              ...seriesStyle
            }
          }
        }).filter(item => item !== undefined),
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
        return '550px'
      }
    },
    chartSettings: {
      type: Object,
      default () {
        return {
          xAxisType: 'category',
          xAxisName: '(日期)',
          yAxisName: '(水压)'
        }
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
  @import "~@/assets/style/theme/register.scss";
  .charts-line-wrapper::v-deep{
    .line-render{
      min-height: 500px;
      /*而且这样所有的toolTip 颜色都被固定了，也需注意*/
      &>div:last-child{
        border:none !important;
        background: transparent !important;
      }
      .toolTip-content-wrapper{
        @include corner-borders(9px, $color-secondary);
        box-sizing: border-box;
        width:200px !important;
        height:68px !important;
        background:rgba(255,255,255,0.05) !important;
        padding: 15px 20px !important;
        color: rgba(101, 226, 245, 1) !important;
        font-size: 14px !important;
        .gape,.status{
          color: #fff;
        }
      }
    }
  }
</style>
