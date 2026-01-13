<template>
  <div class="wrapper">
    <div class="container-header flex-justify-main">
      <div class="search-wrapper" style="margin-left: 30px">
        <sn-form :formList="filterMenu" @submitForm="handleSearch"></sn-form>
      </div>
    </div>
    <div class="exportBtnDiv">
      <el-button @click="exportInfo">导出</el-button>
    </div>
    <div class="container-table" style="margin-top: 30px">
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
          ref="table"
          :data="tableData"
          :header-cell-style="setHeaderCellStyle"
          @sort-change="detailTableSort"
          stripe
        >
          <el-table-column
            :key="column.prop || column.label"
            :label="column.label"
            :width="column.width || ''"
            v-for="column of tableProps"
          >
            <template slot-scope="scope">
              <template v-if="column.label === '报警数值'">
                <div class="color-secondary cursor-pointer inline-flex-center">
                  {{ scope.row.value }}
                </div>
              </template>
              <template v-else-if="column.label === '操作'">
                <span
                  @click="editInfo(scope.row)"
                  class="color-secondary cursor-pointer inline-flex-center"
                >
                  <sn-icon name="edit"></sn-icon>编辑状态
                </span>
              </template>
              <template v-else>{{
                scope.row[column.prop] ? scope.row[column.prop] : "-"
              }}</template>
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
    <div class="plan-record-dialog-wrapper"></div>
  </div>
</template>

<script>
import { mapState } from "vuex";
import { filterMenu } from "./config/config";
import history from "@/request/modules/histroyData";
import productLine from "@/request/modules/productLine";
import XLSX from "xlsx"; // Import the xlsx library or use a similar library for Excel manipulation
import excel from "@/export/libs/excel";

export default {
  components: {},
  data() {
    return {
      isLoading: false,
      editDialogVisible: false,
      filterMenu: filterMenu,
      itemInfoActive: {},
      addBtnConfig: [],
      tableProps: [
        {
          label: "deviceID",
          prop: "commdeviceid",
        },
        {
          label: "设备名称",
          prop: "devicename",
        },
        {
          label: "产线",
          prop: "productLineName",
        },
        {
          label: "传输设备",
          prop: "commDeviceName",
        },
        {
          label: "设备类型",
          prop: "devectTypeName",
        },
        {
          label: "报警原因",
          prop: "reason",
        },
        {
          label: "报警时间",
          prop: "time",
        },
        {
          label: "报警数值",
          prop: "value",
        },
        // {
        //   label: '操作',
        //   prop: 'showHistory',
        //   width: 160
        // }
      ],
      tableData: [],
      tablePage: {
        page: 1,
        total: 0,
        // 不开启
        finish: true,
      },
      searchParams: {},
    };
  },
  mounted() {
    this.getTableList();
    this.getLine();
    this.getDevice();
  },
  computed: {
    ...mapState("sinuo/fireControl", {
      tableList: (state) => {
        return state.dangerRectificationList;
      },
    }),
  },
  watch: {
    "tablePage.page"(newVal, oldVal) {
      this.getTableList();
    },
  },
  methods: {
    // 获取传输设备数据
    getDevice() {
      console.log(111);
      history.getDevice().then((res) => {
        let list = res.data.devlist || [];
        this.filterMenu[2].options = list
          .map((item) => {
            return item.datas.map((v) => {
              return {
                label: v.name,
                value: v.id,
              };
            });
          })
          .flat();
      });
    },
    // 获取产线数据
    getLine() {
      productLine.line().then((res) => {
        if (res.data && res.data.length) {
          filterMenu[1].options = res.data.map((v) => {
            return {
              label: v.name,
              value: v.id,
            };
          });
        }
      });
    },
    getUnitName(unitID) {
      return this.$store.getters["sinuo/user/companyName"](unitID);
    },
    handleSearch(res) {
      console.log(res);
      if (res.timeselect) {
        this.searchParams = {
          commdeviceid: res.commdeviceid,
          gathertype: res.gathertype,
          name: res.name,
          productLine: res.productLine,
          type: res.type,
          starttime: res.timeselect[0],
          endtime: res.timeselect[1],
        };
      } else {
        this.searchParams = res;
      }
      // 重置状态
      this.tablePage = {
        page: 1,
        total: 0,
        finish: true,
      };
      this.getTableList();
    },
    getTableList() {
      const { page } = this.tablePage;
      const params = {
        page,
        ...this.searchParams,
      };
      history
        .request({
          params,
        })
        .then((res) => {
          console.log(res);
          // this.tableData = res.data.map
          let deviceTypeData = this.filterMenu[3].options;
          this.tableData = res.data.map((v) => {
            // 获取设备类型名称
            let devectTypeName = deviceTypeData[v.deviceType + 1].label;
            return {
              ...v,
              devectTypeName,
            };
          });
          this.tablePage.total = res.count;
        });
    },
    formSubmit(res) {
      console.log(res);
    },
    viewImgHandle(url) {
      this.$refs.viewImageDialog.imgUrl = url;
      this.$refs.viewImageDialog.dialogVisible = true;
    },
    editInfo(info = {}) {
      this.itemInfoActive = info;
      this.editDialogVisible = true;
    },
    deleteInfo(info) {
      this.$confirm("此操作将对其进行永远删除，是否继续？", "提示", {
        type: "warning",
      }).then(() => {
        const params = {
          id: info.id,
        };
        this.$store
          .dispatch("sinuo/fireControl/deleteDangerRectification", params)
          .then((res) => {
            this.getTableList();
          });
      });
    },
    detailTableSort({ column, prop, order }) {
      if (order === "descending") {
        switch (prop) {
          case "time": {
            this.$set(this.deviceActive, "time", "desc");
            this.getDeviceDetail();
            break;
          }
          default:
            break;
        }
      } else if (order === "ascending") {
        switch (prop) {
          case "time": {
            this.$set(this.deviceActive, "time", "asc");
            this.getDeviceDetail();
            break;
          }
          default:
            break;
        }
      }
    },
    setHeaderCellStyle({ row, column, rowIndex, columnIndex }) {
      let style = "";
      const nameIndex = this.tableProps.findIndex((item) => {
        return item.label === "器材";
      });
      switch (columnIndex) {
        case nameIndex: {
          style = "text-align:center";
          break;
        }
        default: {
          break;
        }
      }
      return style;
    },
    exportInfo() {
      console.log(this.tableData);
      const params = {
        title: ["deviceID", "设备名称", "产线", "传输设备", "设备类型", "报警原因", "报警时间", "报警数值"],
        key: ["commdeviceid", "devicename", "productLineName", "commDeviceName", "devectTypeName", "reason", "time", "value"],
        data: this.tableData, // 数据源
        autoWidth: true, //autoWidth等于true，那么列的宽度会适应那一列最长的值
        filename: "历史数据",
      };
      excel.exportArrayToExcel(params);
    },
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
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

.exportBtnDiv {
  display: flex;
  justify-content: flex-end;
  margin-right: 53px;
  .el-button {
    width: 91px;
    height: 32px;
    font-size: 14px;
    font-weight: 500;
    color: rgba(101, 226, 245, 1);
    padding: 0 0;
    background: rgba(0, 21, 53, 1);
    margin: 0 15px 0 0;
    border-width: 2px;
    border-style: solid;
    border-image-source: url("../../../components/sn-form/images/border.png");
    border-image-repeat: round;
    border-image-slice: 2 2 2 2 fill;
    transition: 0s all;
    &:hover {
      background-image: url("../../../components/sn-form/images/button-background.png");
      background-size: 100% 100%;
      border: none;
      border-radius: 0;
    }
  }
}
</style>
