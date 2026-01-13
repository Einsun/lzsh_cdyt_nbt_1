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
              transform:scale(${1 / zoom});
              transform-origin:0 0`"
    ></div>
  </div>
</template>

<script>
import Echarts from "echarts";
export default {
  name: "sn-histogram",
  data() {
    return {
      zoom: 1,
      chart: null,
    };
  },
  computed: {
    styles() {
      return {
        width: this.width,
        height: this.height,
      };
    },
  },
  mounted() {
    this.$nextTick().then(() => {
      this.zoom = 1 / document.body.style.zoom;
      this.__initCharts();
    });
  },
  watch: {
    chartSettings() {
      this.__initCharts();
    },
    xAxisData: {
      deep: true,
      handler(newVal) {
        console.log("出发而xAxisData");
        this.__initCharts();
      },
    },
    chartData: {
      deep: true,
      handler(newVal) {
        console.log("出发而chartData");
        this.__initCharts();
      },
    },
  },
  methods: {
    __initCharts() {
      // 使用methods、computed、还是watch
      // 虽然computed更简单也有缓存，但个人（huangjinbo）不愿意使用computed，因为computed触发更新不可控
      // 必须使用watch来触发变更
      const targetNode = this.$refs.histogramRender;
      this.chart = []
      this.chart = Echarts.init(targetNode);
      // 维度暂时默认不可设     debugger

      const labelColor = "rgba(46, 192, 243, 1)";

      let sr_data = this.chartData.map((v) => {
        return { name: v.name };
      });
      const axisStyle = {
        nameTextStyle: {
          color: labelColor,
        },
        axisLabel: {
          color: labelColor,
        },
      };
      let options = {
        tooltip: {
          trigger: "axis",
        },
        legend: {
          right: "2%",
          top: "2%",
          data: sr_data,
          textStyle: {
            color: "#ffffff",
            fontSize: 16,
          },
        },
        grid: {
          left: "3%",
          right: "4%",
          bottom: "3%",
          containLabel: true,
        },
        xAxis: {
          type: "category",
          boundaryGap: false,
          data: this.xAxisData,
          axisLine: {
            show: true,
            lineStyle: {
              color: labelColor,
            },
          },
        },
        yAxis: {
          type: "value",
          splitLine: {
            show: false,
          },
          axisLine: {
            show: false,
          },
          axisLabel: {
            color: labelColor,
            fontSize: 10,
          },
        },
        series: this.chartData,
      };
      let that = this;
      this.chart.on('click', function (params) {
        console.log(params)
        if (that.dataType === 'day') {
          that.dateChangeByTime(params.name)
        }
      })
      this.chart.setOption(options);
    },
  },
  props: {
    width: {
      type: String,
      default() {
        return "100%";
      },
    },
    height: {
      type: String,
      default() {
        return "280px";
      },
    },
    dateChangeByTime: {
      type: Function,
      default: null
    },
    dataType: {
      type: String,
    },
    chartData: {
      type: Array,
    },
    xAxisData: {
      type: Array,
    },
  },
};
</script>

<style scoped lang="scss">
</style>
