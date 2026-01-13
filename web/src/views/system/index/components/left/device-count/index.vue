<template>
  <div class="dash-card-wrapper">
    <dashCard title="设备汇总栏">
      <div class="content flex-center">
        <div class="data-item flex-column flex-center" v-for="item of dataDisplay" :key="item.label" flex-box="1" >
          <p class="number"><d2-count-up :end="item.number" /></p>
          <div class="flex-center">
            <p class="label">{{item.label}}</p>
          </div>
        </div>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
export default {
  name: 'device-count',
  components: {
    dashCard
  },
  mounted () {
    this.getData()
  },
  data () {
    return {
      dataDisplay: [
        {
          label: '总检测线',
          key: 'productLine',
          number: 0,
        },
        {
          label: '设备总数',
          number: 0,
          key: 'device',
        },
        {
          label: '在线设备',
          number: 0,
          key: 'online',
        },
      ]
    };
  },
  methods: {
    getData () {
      this.$store.dispatch('sinuo/dashboard/getDeviceCount')
    },
  },
  computed: {
    deviceCount () {
      return this.$store.state.sinuo.dashboard.deviceCount
    }
  },
  watch: {
    deviceCount: {
      deep: true,
      immediate: true,
      handler (newVal) {
        if (Object.keys(newVal).length > 0) {
          const list = this.dataDisplay.map(item => {
            const key = item.key
            item.number = newVal[key]

            return { ...item }
          })
          this.dataDisplay = list
        }
      }
    }
  }
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 160px;
  .content{
    height: 100%;
    .data-item{
      height: 100%;
      cursor: pointer;
      .number{
        margin-bottom: 16px;
        font-size:44px;
        font-weight:500;
        color: rgba(46, 192, 243, 1);
      }
      .label{
        font-size:14px;
        font-family:PingFangSC-Medium,PingFangSC;
        font-weight:500;
        color:rgba(255,255,255,1);
      }
    }
  }
}
</style>
