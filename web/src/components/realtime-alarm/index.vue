<template>
  <div class="wrapper">
    <!--    visible总是true, 因为当false的时候就需要被销毁，所以不需要false-->
    <handleAlarmDialog
      v-for="alarm of alarmDialogList"
      :key="alarm.id"
      :visible="true"
      :itemInfo="alarm"
      @closeDialog="changeAlarmDialogStatus"
    ></handleAlarmDialog>
  </div>
</template>

<script>
import handleAlarmDialog from './components/handle-alarm-dialog'
const audioNode = new Audio(`/audio/alarm.mp3`)
const PAGESIZE = 15
export default {
  name: 'realtime-alarm',
  components: {
    handleAlarmDialog
  },
  mounted () {
    // this.watchAlarm()
  },
  data () {
    return {
      alarmDialogList: []
    }
  },
  methods: {
    updateData () {
      if (this.activeTabType === 'fireAlarm') {
        this.$store.dispatch('sinuo/dashboard/getAlarmList', { deviceTypeID: 1 });
      }
      // this.$store.dispatch('sinuo/dashboard/getTodayStatistics')
      // this.$store.dispatch('sinuo/user/getCompanyList')
    },

  },
  computed: {
  }
}
</script>
