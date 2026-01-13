<template>
  <div class="wrapper">
    <div class="container-header flex-justify-main">
      <div class="search-wrapper">
        <sn-form :formList="filterMenu" @submitForm="handleSearch"></sn-form>
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
          stripe
        >
          <el-table-column
            :key="column.prop||column.label"
            :label="column.label"
            :width="column.width||''"
            v-for="column of tableProps"
          >
            <template slot-scope="scope">
              <template v-if="column.label==='操作'">
<!--                <span-->
<!--                    @click="editUser(scope.row)"-->
<!--                  class="color-secondary cursor-pointer inline-flex-center operation-btn"-->
<!--                >-->
<!--               编辑-->
<!--                </span>-->
                <span
                  class="color-danger cursor-pointer inline-flex-center operation-btn"
                  @click="deleteUser(scope.row)"
                >
                删除
                </span>
              </template>
              <template v-else>{{scope.row[column.prop]?scope.row[column.prop]:'-'}}</template>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </div>
    <editItem
        :info="itemInfoActive"
        :visible.sync="editDialogVisible"
        @update-list="getTableList"
        v-if="editDialogVisible"
    ></editItem>
  </div>
</template>

<script>
import { mapState } from 'vuex'
import { filterMenu } from './config/config'
import editItem from "./components/editItem/index.vue";
import user from "@/request/modules/user";
import aeDevice from "@/request/modules/aeDevice";

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
      tableProps: [
        {
          label: '账户名称',
          prop: 'name',
          width: 150
        },
        {
          label: '账户类别',
          prop: 'role',
        },
        {
          label: '操作',
          prop: 'showHistory',
          width: 315
        }
      ],
      tableList: [],
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
  },
  methods: {
    handleSearch (res) {
      // 重置状态
      this.itemInfoActive = {};
      this.editDialogVisible = true;
    },
    getTableList () {
      const params = {}
      user.request({
        params
      }).then(res => {
        this.tableList = res.data
      })
    },
    deleteUser (item) {
      this.$confirm(`是否删除${item.name}此用户嘛?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        user.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getTableList();
        })
      })
    },
    editUser (item) {
      this.itemInfoActive = item;
      this.editDialogVisible = true;
    },
    setHeaderCellStyle ({ row, column, rowIndex, columnIndex }) {
      let style = ''
      const nameIndex = this.tableProps.findIndex(item => {
        return item.label === '操作'
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
