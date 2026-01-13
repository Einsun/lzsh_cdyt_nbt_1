<template>
  <div class="dash-card-wrapper">
    <dashCard title="30天内报警故障趋势" :decorationNumber="18">
      <div class="content">
        <chart-index :chartData="chartData"></chart-index>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
import chartIndex from './components/line/index'
export default {
  name: 'alarm-trend',
  components: {
    dashCard,
    chartIndex
  },
  mounted () {
    this.getData()
  },
  data () {
    return {
    };
  },
  methods: {
    getData () {
      this.$store.dispatch('sinuo/dashboard/getAlarmAndFaultData')
    }
  },
  computed: {
    trendData () {
      return this.$store.state.sinuo.dashboard.alarmAndFaultData
    },
    chartData () {
      const { trendData } = this

      const columns = trendData.map(item => {
        return item.name
      })
      const alarms = trendData.map(item => {
        return item.fireAlarmCount
      })
      const fault = trendData.map(item => {
        return item.faultAlarmCount
      })
      return {
        columns,
        alarms,
        fault
      }
    }
  }
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 246px;
}
</style>
