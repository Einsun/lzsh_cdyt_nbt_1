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
          :cell-style="setCellStyle"
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
              <template v-if="column.label==='检查记录'">
                <el-image
                  style="height: 30px"
                  fit="contain"
                  :src="scope.row.fm_facility_check.url"
                  :preview-src-list="[scope.row.fm_facility_check.url]">
                </el-image>
              </template>
              <template v-else-if="column.label==='保养记录'">
                <el-image
                  fit="contain"
                  style="height: 30px"
                  :src="scope.row.fm_facility_keep.url"
                  :preview-src-list="[scope.row.fm_facility_keep.url]">
                </el-image>
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
      <editItem
        :itemInfo="itemInfoActive"
        :visible.sync="editDialogVisible"
        @update-list="getTableList"
        v-if="editDialogVisible"
      ></editItem>
    </div>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { filterMenu } from './config/config'
import editItem from './components/editItem/index'

export default {
  components: {
    editItem
  },
  data () {
    return {
      isLoading: false,
      editDialogVisible: false,
      filterMenu: filterMenu,
      itemInfoActive: {},
      addBtnConfig: [],
      tableProps: [
        {
          label: '单位名称',
          prop: 'unit_name',
          width: 200
        },
        {
          label: '设施名称',
          prop: 'facility_name',
          width: 150
        },
        {
          label: '设备位置',
          prop: 'facility_locale'
        },
        {
          label: '检查记录',
          prop: 'check_record'
        },
        {
          label: '保养记录',
          prop: 'maintain_record'
        },
        {
          label: '操作',
          prop: 'showHistory',
          width: 315
        }],
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
        return state.fireEquipmentList.map(item => {
          return {
            ...item,
            fm_facility_check: item.image.find(img => img.type === 'fm_facility_check') || { url: '' },
            fm_facility_keep: item.image.find(img => img.type === 'fm_facility_keep') || { url: '' },
          }
        })
      }
    }),
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
      this.$store.dispatch('sinuo/fireControl/getFireEquipmentList', params).then(res => {
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
    deleteInfo (info) {
      this.$confirm('此操作将对其进行永远删除，是否继续？', '提示', {
        type: 'warning'
      }).then(() => {
        const params = {
          id: info.id
        }
        this.$store
          .dispatch('sinuo/fireControl/deleteFireEquipment', params)
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
    },
    setCellStyle ({ row, column, rowIndex, columnIndex }) {
      let style = ''
      const checkIndex = this.tableProps.findIndex(item => {
        return item.label === '检查记录'
      })
      const keepIndex = this.tableProps.findIndex(item => {
        return item.label === '保养记录'
      })
      switch (columnIndex) {
        case checkIndex: {
          style = 'text-align:center'
          break
        }
        case keepIndex: {
          style = 'text-align:center'
          break
        }
      }
      return style
    },
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
      .photo-item{
          display: flex;
          width:60px;
          height:40px;
          background-size: cover !important;
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
