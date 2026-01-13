<!--当前只考虑了一条数据一种颜色，如需更多需进一步抽象-->

<template>
  <div class="charts-histogram-wrapper">
    <div
      class="histogram-render"
      ref="histogramRender"
      style=""
      :style="`
              width:${this.width};height:${this.height};
              zoom:${zoom};
              transform:scale(${1/zoom});
              transform-origin:0 0`"
    >

    </div>
  </div>
</template>

<script>
import Echarts from 'echarts'
export default {
  name: 'sn-histogram',
  data() {
    return {
      zoom: 1
    }
  },
  computed: {
    styles() {
      return {
        width: this.width,
        height: this.height
      }
    }
  },
  mounted() {
    this.$nextTick().then(() => {
      this.zoom = 1 / document.body.style.zoom
      this.__initCharts()
    })
  },
  watch: {
    chartSettings() {
      this.__initCharts()
    },
    chartData: {
      deep: true,
      handler(newVal) {
        console.log('出发而', newVal);
        this.__initCharts()
      }
    },
  },
  methods: {
    __initCharts() {
      // 使用methods、computed、还是watch
      // 虽然computed更简单也有缓存，但个人（huangjinbo）不愿意使用computed，因为computed触发更新不可控
      // 必须使用watch来触发变更
      const targetNode = this.$refs.histogramRender
      let chartsInstance = Echarts.init(targetNode)
      // 维度暂时默认不可设     debugger

      const labelColor = 'rgba(46, 192, 243, 1)'

      // 生成底部时间数组
      let dateTime = ['1ms', '300', '500', '1000', '2000', '3000']
      console.log(dateTime);
      const axisStyle = {
        nameTextStyle: {
          color: labelColor
        },
        axisLabel: {
          color: labelColor
        }
      }
      let options = {
        tooltip: {
          trigger: 'axis'
        },
        grid: {
          top: '5%',
          left: '3%',
          right: '4%',
          bottom: '10%',
          containLabel: true
        },
        xAxis: {
          type: 'category',
          boundaryGap: false,
          // data: dateTime,
          data: this.chartData.data.map((v, index) => index),
          axisLine: {
            show: true,
            lineStyle: {
              color: labelColor
            }
          },
        },
        yAxis: {
          axisLabel: {
            color: labelColor,
            fontSize: 10,
          },
        },
        dataZoom: [
          {
            startValue: '0'
          },
          {
            type: 'inside'
          }
        ],
        series: this.chartData
      }
      chartsInstance.setOption(options)
    },
  },
  props: {
    width: {
      type: String,
      default() {
        return '100%'
      }
    },
    height: {
      type: String,
      default() {
        return '280px'
      }
    },
    chartData: {
      type: Object
    }
  }
}
</script>

<style scoped lang="scss">
</style>
