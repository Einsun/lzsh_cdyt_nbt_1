<template>
  <div class="dash-card-wrapper">
    <dashCard title="全部设备">
      <div class="container-table">
        <div class="flex-column">
          <el-tree
            class="filter-tree"
            :data="tableData"
            :props="defaultProps"
            default-expand-all
            ref="tree">
          </el-tree>
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
    return {
      defaultProps: {
        children: 'datas',
        label: 'name'
      },
    };
  },
  computed: {
    tableData () {
      return this.$store.state.sinuo.dashboard.alarmDevice.devlist
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

.dash-card-wrapper {
  height: 300px;

  .content {
    padding-top: 20px;

    .value-item-wrapper {
      width: 100%;
      max-height: 160px;
      flex-wrap: wrap;
      align-content: space-between;
    }
  }
}

.container-table {
  @include corner-borders(19px, $color-secondary);
  // 防止四角border被同颜色覆盖
  padding: 10px;

  &::v-deep {
    @include el-table-reset-primary();
  }
  .filter-tree {
    background-color: rgba(255,255,255,0);
    color:  $color-secondary;
  }
}
</style>
