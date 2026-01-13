<template>
  <div class="wrapper">
    <div class="container-header flex-justify-main">
      <sn-form
        :formList="alarmFilterMenu"
        @submitForm="formSubmit"
        @formReset="changeDevice"
      ></sn-form>
    </div>
    <div
      class="table-wrapper"
      v-loading.body.lock="isLoading"
      element-loading-text="拼命加载中"
      element-loading-spinner="el-icon-loading"
      element-loading-background="rgba(0, 0, 0, 0.5)"
    >
      <el-table
        :data="tableData"
        stripe
        :cell-style="setCellStyle"
        :header-cell-style="setHeaderCellStyle"
      >
        <el-table-column
          v-for="column of tableProps"
          :key="column.prop || column.label"
          :label="column.label"
          :width="column.width || ''"
        >
          <template slot-scope="scope">
            <template v-if="column.label === '操作'">
              <span
                v-if="scope.row.rating && scope.row.status === 1"
                class="color-secondary cursor-pointer handle-btn"
                @click="handlingFireAlarm(scope.row)"
                >待处理</span
              >
              <span
                v-else-if="scope.row.rating && scope.row.status === 0"
                class="color-secondary cursor-pointer handle-btn"
                >已处理</span
              >
              <span v-else>-</span>
            </template>
            <template v-else-if="column.label === '处理类型'">
              {{ alarmCategory[scope.row.alarm_category] }}
            </template>
            <template v-else-if="column.label === '报警类型'">
              {{ scope.row.deviceType }} - {{ scope.row[column.prop] }}
            </template>
            <template v-else-if="column.label === '用户编码'">
              {{
                scope.row.deviceTypeID == 1
                  ? scope.row["fireUserCode"]
                  : scope.row[column.prop]
              }}
            </template>
            <template v-else>
              {{ scope.row[column.prop] }}
            </template>
          </template>
        </el-table-column>
      </el-table>
    </div>
    <div class="pagination-wrapper flex-center">
      <el-pagination
        :page-size="pagination.perPage"
        :current-page.sync="pagination.page"
        layout="total, prev, pager, next, jumper"
        :total="pagination.total"
      >
      </el-pagination>
    </div>
    <div class="dialog-wrapper" v-if="handlingDialogVisible">
      <handlingFireAlarmDialog
        :handleType="handleType"
        :visible.sync="handlingDialogVisible"
        :itemInfo="activeAlarm"
        @update-list="getAlarmList"
      ></handlingFireAlarmDialog>
    </div>
  </div>
</template>

<script>
import { alarmFilterMenu, deviceTypeMap } from "../../config/config";
import handlingFireAlarmDialog from "../../../device/components/productLinedialog/index";
const perPage = 100;

export default {
  mounted() {
    // this.changeDevice(1)
  },
  components: {
    handlingFireAlarmDialog,
  },
  computed: {
    companyListOptions() {
      // return this.$store.getters['sinuo/user/companyListOptions'];
    },
  },
  data() {
    const vm = this;
    return {
      handleType: "fireAlarm",
      deviceTypeID: 1,
      handlingDialogVisible: false,
      activeAlarm: {},
      isLoading: false,
      tableProps: [
        {
          label: "序号",
          prop: "orderNumber",
          width: 18 + 158,
        },
        {
          label: "单位",
          prop: "companyName",
        },
        {
          label: "报警位置",
          prop: "deviceAddress",
        },
        {
          label: "设备类型",
          prop: "detail",
        },
        {
          label: "用户编码",
          prop: "deviceNo",
        },
        {
          label: "报警类型",
          prop: "alarmType",
        },
        {
          label: "报警等级",
          prop: "ratingType",
        },
        {
          label: "处理类型",
          prop: "alarm_category",
        },
        {
          label: "处理结果",
          prop: "alarm_cause",
        },
        {
          label: "报警时间",
          prop: "time",
        },
        {
          label: "操作",
          prop: "",
          width: 18 + 158,
        },
      ],
      alarmCategory: [
        "真实火警",
        "施工扬尘",
        "环境恶劣",
        "维护检测",
        "吸烟",
        "餐饮活动",
        "设备故障",
        "其他",
      ],
      alarmFilter: [
        "真实火警",
        "施工扬尘",
        "环境恶劣",
        "维护检测",
        "吸烟",
        "餐饮活动",
        "设备故障",
        "其他",
      ],
      tableData: [],
      alarmFilterMenu: alarmFilterMenu.map((item) => {
        switch (item.key) {
          case "deviceTypeID": {
            item.onChange = (val) => {
              vm.changeDevice(val);
              const searchForm = {
                ...this.alarmData,
                deviceTypeID: val,
              };
              searchForm.alarmType = "";
              vm.formSubmit(searchForm);
            };
            break;
          }
          case "alarmType": {
            item.onChange = (val) => {
              const searchForm = {
                ...this.alarmData,
                alarmType: val,
              };
              vm.formSubmit(searchForm);
            };
            break;
          }
          case "unitID": {
            setTimeout(() => {
              item.options = vm.companyListOptions;
            }, 0);
            break;
          }
          case "alarmCategory": {
            setTimeout(() => {
              item.options = vm.alarmFilter.map((item, key) => {
                return {
                  label: item,
                  value: key,
                };
              });
            }, 0);
            break;
          }
        }
        return item;
      }),
      alarmData: (() => {
        const obj = {};
        for (let item of alarmFilterMenu) {
          if (item.key) {
            obj[item.key] = item.value;
          }
        }
        return obj;
      })(),
      checkbox_group: ["全部"],
      pagination: {
        page: 1,
        total: 0,
        perPage: perPage,
      },
      flag: false,
      onClick: false,
    };
  },
  watch: {
    "pagination.page"(newVal, oldVal) {
      // this.getAlarmList();
    },
    $route: {
      immediate: true,
      handler(to, from) {
        const searchForm = {
          ...this.alarmData,
          ...to.query,
        };
        let params = Object.keys(to.query).length
          ? searchForm
          : {
              deviceTypeID: 1,
              alarmType: "火警",
            };
        this.getRouteParams(params);
        this.formSubmit(searchForm);
      },
    },
  },
  methods: {
    getAlarmCategory(category) {
      // alarmCategory
    },
    getRouteParams(val) {
      this.alarmFilterMenu[0].value = val.deviceTypeID;
      this.alarmFilterMenu[1].value = val.alarmType;
    },
    formSubmit(res) {
      // console.log('******* 222 *******', res.deviceTypeID)
      this.alarmData = res;
      this.deviceTypeID = res.deviceTypeID;
      this.getAlarmList();
    },
    changeDevice(val = 1) {
      const list = deviceTypeMap[val];
      if (Array.isArray(list)) {
        this.alarmFilterMenu[1].options = list.map((item) => {
          return {
            label: item,
            value: item,
          };
        });
        this.alarmFilterMenu[1] = this.alarmFilterMenu[1];
      } else {
        this.alarmFilterMenu[1].options = [
          {
            label: "全部",
            value: "",
          },
        ];
      }
    },
    getAlarmList(params = {}) {
      this.tableData = [];
      const { page, perPage } = this.pagination;
      params = {
        page,
        perPage,
        ...this.alarmData,
        ...params,
      };
      // remove empty values
      for (let key in params) {
        if (!params[key] && key !== "alarmCategory") {
          delete params[key];
        }
      }
      // remove unitID #
      if (params.unitID) {
        params.unitID = params.unitID.replace("#", "");
      }
      this.isLoading = true;
      this.$api.alarm
        .getAlarmList({ params })
        .then((res) => {
          this.pagination.total = res.total;
          this.tableData = res.data.map((item, index) => {
            return {
              ...item,
              companyName: this.$store.getters["sinuo/user/companyName"](
                item.unitID
              ),
              orderNumber:
                index +
                1 +
                (this.pagination.page - 1) * this.pagination.perPage,
            };
          });
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    setHeaderCellStyle({ row, column, rowIndex, columnIndex }) {
      let style = "";
      const controlIndex = this.tableProps.findIndex((item) => {
        return item.label === "操作";
      });
      const IdIndex = this.tableProps.findIndex((item) => {
        return item.label === "ID";
      });
      switch (columnIndex - 1) {
        case controlIndex: {
          style = "text-align:left";
          break;
        }
        case IdIndex: {
          style = "text-align:left";
          break;
        }
      }
      return style;
    },
    setCellStyle({ row, column, rowIndex, columnIndex }) {
      let style = "";
      const alarmTypeIndex = this.tableProps.findIndex((item) => {
        return item.label === "报警类型";
      });
      const controlIndex = this.tableProps.findIndex((item) => {
        return item.label === "操作";
      });
      const IdIndex = this.tableProps.findIndex((item) => {
        return item.label === "ID";
      });
      switch (columnIndex - 1) {
        case alarmTypeIndex: {
          const colorRules = {
            火警系统: "#04C0C8",
            水压: "#EC3636",
            液位: "#3FBECA",
            无线烟感: "#0291FF",
            电气火灾: "#EFB45F",
          };
          // 报警类型颜色更改为使用设备而非报警类型来计算
          const prop = this.tableProps[alarmTypeIndex].prop;
          const key = row[prop === "alarmType" ? "deviceType" : prop];
          if (key) {
            switch (key) {
              case "火警系统": {
                let color = "#fff";
                switch (row.alarmType) {
                  case "火警":
                    color = "#EC3636";
                    break;
                  case "复位":
                    color = "#ADFF2F";
                    break;
                  case "故障":
                    color = "#EEE685";
                    break;
                  default:
                    break;
                }
                style = `color:${color}`;
                break;
              }
              default:
                style = `color:${colorRules[key]}`;
                break;
            }
          }
          break;
        }
        case controlIndex: {
          style = "text-align:left";
          break;
        }
        case IdIndex: {
          style = "text-align:left";
          break;
        }
      }
      return style;
    },
    watchDetail() {
      this.$message.warning(
        this.$store.state.sinuo.dashboard.VUE_APP_UNOPENED_PROMPT
      );
    },
    handlingFireAlarm(item) {
      this.activeAlarm = item;
      this.handlingDialogVisible = true;
      this.handleType = "fireAlarm";
    },
    handlingSmokeAlarm(item) {
      this.activeAlarm = item;
      this.handlingDialogVisible = true;
      this.handleType = "smokeAlarm";
    },
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.wrapper {
  .summary-wrapper {
    width: 71.2%;
    min-width: 1000px;
    flex-wrap: wrap;
    align-content: space-between;
    justify-content: space-between;
    .item {
      @include corner-line-borders(6px, 2px, #2ec0f3);
      width: 220px;
      height: 48px;
      background: rgba(6, 39, 91, 0.7);

      font-size: 16px;
      font-weight: 400;
      color: rgba(216, 216, 218, 0.7);
      padding: 0 18px;
      .value {
        .number {
          font-size: 36px;
          font-weight: 500;
          color: rgba(46, 192, 243, 1);
          margin-right: 8px;
        }
      }
    }
  }
  &::v-deep .table-wrapper {
    @include el-table-reset-primary();
  }
  .pagination-wrapper::v-deep {
    margin: 20px 0 0;
    @include el-pagination-reset();
  }
  .table-wrapper {
    @include styled-scrollbar;
    position: relative;
    max-height: 700px;
    overflow-y: auto;
  }
  .handle-btn {
    margin-right: 30px;
  }
}
</style>
