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
              <template v-else-if="column.label==='单位名称'" >
                {{getUnitName(scope.row.unit_id)}}
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
    <div class="plan-record-dialog-wrapper">
      <control-room-dialog
        :userInfo="controlRoomInfoActive"
        :visible.sync="controlRoomDialogVisible"
        @update-list="getTableList"
        v-if="controlRoomDialogVisible"
      ></control-room-dialog>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { filterMenu } from './config/config'
import controlRoomDialog from './components/controlRoom/index'

export default {
  components: {
    controlRoomDialog
  },
  data () {
    return {
      isLoading: false,
      controlRoomDialogVisible: false,
      filterMenu: filterMenu,
      controlRoomInfoActive: {},
      addBtnConfig: [],
      tableProps: [
        {
          label: '单位名称',
          prop: 'unit_name',
          width: 200
        },
        {
          label: '姓名',
          prop: 'name',
          width: 150
        },
        {
          label: '联系电话',
          prop: 'phone'
        },
        {
          label: '性别',
          prop: 'gender'
        },
        {
          label: '岗位',
          prop: 'job'
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
        return state.controlRoomList
      }
    })
  },
  watch: {
    'tablePage.page' (newVal, oldVal) {
      this.getTableList();
    },
  },
  methods: {
    getUnitName (unitID) {
      return this.$store.getters['sinuo/user/companyName'](unitID)
    },
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
      this.$store.dispatch('sinuo/fireControl/getControlRoomList', params).then(res => {
        this.tablePage.total = res.total
      })
    },
    formSubmit (res) {
      console.log(res)
    },
    editInfo (info = {}) {
      this.controlRoomInfoActive = info
      this.controlRoomDialogVisible = true
    },
    deleteInfo (info) {
      this.$confirm('此操作将对其进行永远删除，是否继续？', '提示', {
        type: 'warning'
      }).then(() => {
        const params = {
          id: info.id
        }
        this.$store
          .dispatch('sinuo/fireControl/deleteControlRoom', params)
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
