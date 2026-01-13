<template>
  <div class="dash-card-wrapper">
    <dashCard title="隐患单位">
      <div class="content flex-justify-main flex-center-cross">
        <el-carousel height="100px" width="100%" direction="vertical" indicator-position="none">
          <el-carousel-item v-for="item of hiddenDangerUnitList" :key="item.unitID" >
            <div class="unit-content flex-column flex-center">
              <p class="unit-name"> 隐患单位：{{item.name}}</p>
              <p class="unit-cause"> 隐患原因：{{item.cause}}</p>
            </div>
          </el-carousel-item>
        </el-carousel>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
import VeRing from 'v-charts/lib/ring.common'

export default {
  name: 'unit-legend',
  components: {
    dashCard,
  },
  mounted () {
    this.getData()
  },
  data () {
    return {
    };
  },
  computed: {
    getUnitName () {
      return this.$store.getters['sinuo/user/companyName']
    },
    hiddenDangerUnitList () {
      const hiddenDangerList = this.$store.state.sinuo.dashboard.hiddenDangerList
      const unitIDList = this.$store.state.sinuo.dashboard.hiddenDangerList.map(item => {
        return item.unitID
      })
      const unitNameList = unitIDList.map((unitID, index) => {
        return { name: this.getUnitName(unitID), unitID, cause: hiddenDangerList[index].cause }
      })
      return unitNameList
    },
  },
  methods: {
    getData () {
      this.$store.dispatch('sinuo/dashboard/getHiddenDangerList')
    }
  },
}
</script>

<style scoped lang="scss">
.dash-card-wrapper{
  height: 160px;
  .content{
    height: 100px;
    &::v-deep{
      .el-carousel{
        width: 100%;
        .unit-content{
          text-align: center;
          text-align-last: center;
          height: 100px;
          color: #fff;
          font-size: 20px;
          .unit-name{
            margin-bottom: 10px;
          }
        }
      }
    }
  }
}
</style>
