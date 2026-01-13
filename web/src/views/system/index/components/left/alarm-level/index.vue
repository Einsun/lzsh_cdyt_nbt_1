<template>
  <div class="dash-card-wrapper">
    <dashCard title="报警等级统计">
      <div class="dashboard-today-alarm-proportion">
        <ve-pie :legend-visible="false" height="180px" :data="chartData" :settings="chartSettings"></ve-pie>
        <div class="alert-total">
          <span ref="alarmTotal">{{alarmTotal}}</span>
          <label>设备总数</label>
        </div>
      </div>
    </dashCard>
  </div>
</template>

<style lang="scss" scoped>
.dashboard-today-alarm-proportion{
  position: relative;
  .alert-total{
    color: #04C0C8;
    position: absolute;
    top: 80px;
    left: 178px;
    display: flex;
    flex-direction: column;
    text-align: center;
    span{
      font-size: 30px;
      line-height: 40px;
      font-weight: bold;
      min-width: 70px;
    }
    label{
      font-size: 12px;
    }
  }
}
</style>

<script>
// import { mapState } from 'vuex'
// VCharts
import VePie from 'v-charts/lib/pie.common'
// 调节字体大小函数
import { fitFontSize } from '@/util/styles';
import dashCard from '../../card/index'

const DAYS = 90
export default {
  name: 'dashboard-today-alarm-proportion',
  mounted () {
    this.getAlarmScale()
  },
  components: {
    dashCard,
    VePie
  },
  props: {
    head: String
  },
  data () {
    return {
      chartSettings: {
        offsetY: 110,
        radius: 70,
        // level: [
        // [],
        // ['1', '11', '等级3', '等级4']
        // ],
        itemStyle: {
          color (params) {
            return ['#f89728', '#1BAE77', '#ff435c', '#00a0e6'][params.dataIndex]
          }
        },
        label: {
          formatter: ['{b}:{c}起，', '{second|占比{d}%}'].join('\n'),
          align: 'right',
          fontSize: '12px',
          rich: {
            second: {
              align: 'right'
            }
          }
        },
      },
      alarmTotal: 0,
      chartData: []
    }
  },
  computed: {
  },
  methods: {
    getAlarmScale () {
      const params = {
      }
      this.$api.getAlarmData({ params }).then(res => {
        console.log('----', res)
        const list = res.data
        let data = {
          columns: ['类型', '数量'],
          rows: []
        }
        data.rows = list.map((item) => {
          return {
            '类型': item.type,
            '数量': item.total
          }
        })
        let total = list.map(item => {
          return item.total
        }).reduce((accumulator, currentValue) => accumulator + currentValue)
        this.alarmTotal = total
        this.chartData = data
        console.log(this.chartData)
      })
    }
  },
  watch: {
    alarmTotal (newVal, oldVal) {
      this.$nextTick().then(() => {
        fitFontSize(70, this.$refs.alarmTotal)
      })
    }
  }
}
</script>
