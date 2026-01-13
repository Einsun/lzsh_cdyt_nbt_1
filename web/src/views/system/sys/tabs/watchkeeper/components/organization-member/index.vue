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
              <template v-if="column.label==='照片'">
                <el-image
                  style="height: 30px"
                  fit="contain"
                  :src="scope.row.image.url"
                  :preview-src-list="[scope.row.image.url]">
                </el-image>
              </template>
              <template v-else-if="column.label==='所属组织'">
                <span class="text-underline color-secondary">
                  {{scope.row.organization.organization_name}}
                </span>
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
              <template v-else>
                <span>{{scope.row[column.prop]?scope.row[column.prop]:'-'}}</span>
              </template>
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
          label: '组员名称',
          prop: 'member_name',
        },
        {
          label: '照片',
          prop: 'image'
        },
        {
          label: '职责',
          prop: 'duty'
        },
        {
          label: '岗位',
          prop: 'post'
        },
        {
          label: '手机号',
          prop: 'phone'
        },
        {
          label: '所属组织',
          prop: 'organization'
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
        return state.organizationMemberList
      }
    }),
    organization_id () {
      return this.$route.query.organization_id
    }
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
        organization_id: this.organization_id,
        ...this.searchParams
      }
      this.$store.dispatch('sinuo/fireControl/getOrganizationMemberList', params).then(res => {
        this.tablePage.total = res.total
      })
    },
    formSubmit (res) {
      console.log(res)
    },
    editInfo (info = {}) {
      this.itemInfoActive = {
        ...info,
        organization_id: this.organization_id
      }
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
          .dispatch('sinuo/fireControl/deleteOrganizationMember', params)
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
