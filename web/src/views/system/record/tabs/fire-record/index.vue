<template>
  <div class="wrapper">
    <div class="container-header flex-justify-main">
      <div class="search-wrapper">
        <sn-form :formList="filterMenu" @submitForm="handleSearch"></sn-form>
      </div>
      <div class="search-wrapper">
        <sn-form
          :formList="addBtnConfig"
          :formStyle="{}"
          :itemStyle="{
          'margin-bottom': '6px',
          'margin-top': '20px'
        }"
          @submitForm="editInfo"
          class="add-field-btn"
        ></sn-form>
      </div>
    </div>
    <div class="container-table">
      <div
        :infinite-scroll-disabled="tablePage.finish"
        :infinite-scroll-distance="50"
        class="table-wrapper"
        element-loading-background="rgba(0, 0, 0, 0.5)"
        element-loading-spinner="el-icon-loading"
        element-loading-text="拼命加载中"
        v-infinite-scroll="getTableList"
        v-loading.body.lock="isLoading"
      >
        <el-table
          :data="tableList"
          :header-cell-style="setHeaderCellStyle"
          @sort-change="detailTableSort"
          stripe
        >
          <el-table-column
            :key="column.prop||column.label"
            :label="column.label"
            :width="column.width||''"
            v-for="column of tableProps"
          >
            <template slot-scope="scope">
              <template v-if="column.label==='性别'">{{scope.row.gender == 0?'男':'女'}}</template>
              <template v-else-if="column.label==='附件详情'">
                <span>
                  <a :href="scope.row.file?scope.row.file.url:''" target="_blank" class="watch-accessory color-secondary text-underline">
                    查看附件
                  </a>
                </span>
              </template>
              <template v-else-if="column.label==='操作记录'">
                <span class="cursor-pointer color-secondary text-underline " @click="showHistory(scope.row)">管理情况</span>
              </template>
              <template v-else-if="column.label==='操作'">
                <span
                  @click="editInfo(scope.row)"
                  class="color-secondary cursor-pointer inline-flex-center operation-btn"
                >
                  <sn-icon name="edit"></sn-icon>编辑
                </span>
                <span
                  @click="deleteInfo(scope.row)"
                  class="color-danger cursor-pointer inline-flex-center operation-btn"
                >
                  <sn-icon name="delete"></sn-icon>删除
                </span>
              </template>
              <template v-else>{{scope.row[column.prop]?scope.row[column.prop]:'-'}}</template>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>
    <div class="pagination-wrapper flex-center">
      <el-pagination
        :current-page.sync="tablePage.page"
        :page-size="tablePage.perPage"
        :total="tablePage.total"
        layout="total, prev, pager, next, jumper"
      ></el-pagination>
    </div>
    <div class="dialog-wrapper">
      <editItem
        :itemInfo="itemInfoActive"
        :visible.sync="editDialogVisible"
        @update-list="getTableList"
        v-if="editDialogVisible"
      ></editItem>
      <recordList
        :itemInfo="itemInfoActive"
        :visible.sync="historyDialogVisible"
        v-if="historyDialogVisible"
      ></recordList>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { filterMenu } from './config/config'
import editItem from './components/editItem/index'
import recordList from './components/record-list'

export default {
  components: {
    editItem,
    recordList
  },
  data () {
    return {
      isLoading: false,
      editDialogVisible: false,
      historyDialogVisible: false,
      filterMenu: filterMenu,
      itemInfoActive: {},
      addBtnConfig: [],
      tableProps: [
        {
          label: '档案名称',
          prop: 'file_name',
          width: 150
        },
        {
          label: '创建时间',
          prop: 'create_time'
        },
        {
          label: '责任人',
          prop: 'charge_person'
        },
        {
          label: '附件详情',
          prop: 'accessory'
        },
        {
          label: '操作记录',
          prop: 'records'
        },
        {
          label: '操作',
          prop: 'showHistory',
          width: 315
        }
      ],
      tableData: [],
      tablePage: {
        page: 1,
        perPage: 20,
        total: 0,
        // 不开启
        finish: true
      },
      searchParams: {}
    }
  },
  mounted () {
    this.getTableList()
  },
  computed: {
    ...mapState('sinuo/fireControl', {
      tableList: state => {
        return state.fireRecordList
      }
    })
  },
  methods: {
    handleSearch (res) {
      // 重置状态
      this.tablePage = {
        page: 1,
        perPage: 20,
        total: 0,
        finish: true
      }
      this.searchParams = res
      this.getTableList()
    },
    getTableList () {
      const { page, perPage } = this.tablePage
      const params = {
        page,
        perPage,
        ...this.searchParams
      }
      this.$store.dispatch('sinuo/fireControl/getFireRecordList', params).then(res => {
        this.tablePage.total = res.total
      })
    },
    formSubmit (res) {
      console.log(res)
    },
    editInfo (info = {}) {
      this.itemInfoActive = info
      this.editDialogVisible = true
    },
    showHistory (info = {}) {
      this.itemInfoActive = info
      this.historyDialogVisible = true
    },
    deleteInfo (info) {
      this.$confirm('此操作将对其进行永远删除，是否继续？', '提示', {
        type: 'warning'
      }).then(() => {
        const params = {
          id: info.id
        }
        this.$store
          .dispatch('sinuo/fireControl/deleteFireRecord', params)
          .then(res => {
            this.getTableList()
          })
      })
    },
    detailTableSort ({ column, prop, order }) {
      if (order === 'descending') {
        switch (prop) {
          case 'time': {
            this.$set(this.deviceActive, 'time', 'desc')
            this.getDeviceDetail()
            break
          }
          default:
            break
        }
      } else if (order === 'ascending') {
        switch (prop) {
          case 'time': {
            this.$set(this.deviceActive, 'time', 'asc')
            this.getDeviceDetail()
            break
          }
          default:
            break
        }
      }
    },
    setHeaderCellStyle ({ row, column, rowIndex, columnIndex }) {
      let style = ''
      const nameIndex = this.tableProps.findIndex(item => {
        return item.label === '器材'
      })
      switch (columnIndex) {
        case nameIndex: {
          style = 'text-align:center'
          break
        }
        default: {
          break
        }
      }
      return style
    }
  }
}
</script>

<style lang="scss" scoped>
  @import '~@/assets/style/theme/register.scss';
  .wrapper {
    &::v-deep .table-wrapper {
      @include el-table-reset-primary();
    }
    .table-wrapper {
      @include styled-scrollbar;
      position: relative;
      max-height: 700px;
      overflow-y: auto;
      .operation-btn {
        margin-right: 20px;
      }
    }
    .handle-btn {
      margin-right: 30px;
    }
  }
  .pagination-wrapper::v-deep {
    margin: 20px 0 0;
    @include el-pagination-reset();
  }
</style>
