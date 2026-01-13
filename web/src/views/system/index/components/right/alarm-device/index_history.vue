<template>
  <div class="dash-card-wrapper">
    <dashCard title="报警/故障设备">
      <div class="container-table">
        <div class="table-wrapper flex-column">
          <el-table
            :data="tableData"
            stripe
            size="mini"
            style="width: 100%;">
            <el-table-column
              prop="part_number"
              label="产线">
            </el-table-column>
            <el-table-column
              prop="status"
              label="通知类型">
            </el-table-column>
            <el-table-column
              prop="name"
              label="设备">
            </el-table-column>
            <el-table-column
              prop="alarm_rules"
              label="故障原因">
            </el-table-column>
            <el-table-column
              prop="start_time"
              width="100"
              label="上传时间">
            </el-table-column>
            <el-table-column
              prop="gather_rules"
              label="处理状态">
            </el-table-column>
          </el-table>
        </div>
      </div>
    </dashCard>
  </div>
</template>

<script>
import dashCard from '../../card/index'
export default {
  name: 'alarm-history',
  components: {
    dashCard,
  },
  mounted () {
    this.getData()
  },
  data () {
    return {};
  },
  computed: {
    tableData () {
      return this.$store.state.sinuo.dashboard.alarmDevice
    }
  },
  methods: {
    getData () {
      this.$store.dispatch('sinuo/dashboard/getAlarmDevice')
    }
  }
}
</script>

<style scoped lang="scss">
@import "~@/assets/style/theme/register.scss";
.dash-card-wrapper{
  height: 400px;
  .content{
    padding-top: 20px;
    .value-item-wrapper{
      width: 100%;
      max-height: 160px;
      flex-wrap: wrap;
      align-content: space-between;
    }
  }
}
.container-table {
  @include corner-borders(19px,$color-secondary);
  // 防止四角border被同颜色覆盖
  padding: 8px;
  &::v-deep{
    @include el-table-reset-primary();
  }
  .table-wrapper {
    @include styled-scrollbar;
    position: relative;
    max-height: 65.5vh;
    overflow-y: auto;

    table{
      th,td{
        text-align: center;
      }
    }
  }
}
</style>
