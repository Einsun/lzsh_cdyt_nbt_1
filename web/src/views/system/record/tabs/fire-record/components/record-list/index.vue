<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" title="管理情况">
      <div class="dialog-content-wrapper flex-column flex-center">
        <el-table
          v-loading.body.lock="recordIsLoading && dialogVisible"
          element-loading-text="拼命加载中"
          element-loading-spinner="el-icon-loading"
          element-loading-background="rgba(0, 0, 0, 0.5)"
          :data="recordData"
          :border="true"
          @sort-change="tableSort"
        >
          <el-table-column
            :key="column.prop||column.label"
            v-bind="column"
            v-for="column of tableProps"
          >
            <template slot-scope="scope">
              <template>{{scope.row[column.prop]?scope.row[column.prop]:'-'}}</template>
            </template>
          </el-table-column>
        </el-table>
        <el-pagination
          :page-size="recordPage.perPage"
          :current-page.sync="recordPage.page"
          layout="total, prev, pager, next, jumper"
          :total="recordPage.total">
        </el-pagination>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
export default {
  name: 'record-list',
  props: {
    itemInfo: {
      type: Object
    },
    visible: {
      type: Boolean
    }
  },
  mounted () {
    this.getList()
  },
  data () {
    return {
      recordData: [],
      recordPage: {
        page: 1,
        perPage: 10,
        total: 0
      },
      tableProps: [
        {
          label: '序号',
          prop: 'sequenceNumber'
        },
        {
          label: '操作人',
          prop: 'manage_person'
        },
        {
          label: '时间',
          prop: 'created_at'
        },
        {
          label: '操作记录',
          prop: 'manage_log'
        },
      ],
      params: {},
      dialogVisible: false,
      recordIsLoading: false
    }
  },
  methods: {
    getList () {
      const params = {
        ...this.params,
        id: this.itemInfo.id
      }
      const { page, perPage } = this.recordPage
      this.recordIsLoading = true
      this.$api.fireManage.getFireRecordHistory({ params }).then(res => {
        this.recordData = res.data.map((item, index) => {
          return {
            ...item,
            sequenceNumber: (index + 1) + ((page - 1) * perPage)
          }
        })
        this.recordPage.total = res.total
      }).finally(() => {
        this.recordIsLoading = false
      })
    },
    tableSort ({ column, prop, order }) {
      if (order === 'descending') {
        switch (prop) {
          case 'time': {
            this.$set(this.params, 'time', 'desc')
            this.getList()
            break;
          }
          default:
            break
        }
      } else if (order === 'ascending') {
        switch (prop) {
          case 'time': {
            this.$set(this.params, 'time', 'asc')
            this.getList()
            break
          }
          default:
            break
        }
      }
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler (val) {
        this.dialogVisible = val
      }
    },
    dialogVisible (newVal) {
      this.$emit('update:visible', newVal)
    },
    'recordPage.page' () {
      this.getList()
    }
  }
}
</script>

<style scoped lang="scss">
  @import "~@/assets/style/theme/register.scss";

  .dialog-wrapper::v-deep{
    .dialog-content-wrapper{
      justify-content: space-around;
      height: 100%;
    }
    @include el-table-reset();
    @include el-pagination-reset();
    .el-pagination{
      margin-top: 20px;
    }
  }
</style>
