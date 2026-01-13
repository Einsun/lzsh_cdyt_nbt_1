<template>
  <d2-container class="page" id="page-device">
      <div class="tab">
          <el-tabs v-model="tabIndex" editable @edit="handleTabsEdit" @tab-click="handleClick">
            <el-tab-pane :label="tab.name" :name="tab.name" :key="tab.name" v-for="tab in tableList">
              <div class="flex flex-column tabPane">
                <div class="flex-1" style="margin-bottom: 10px">
                  <el-row :gutter="10" style="margin: 0;height: 490px">
                    <el-col :span="12" style="height: 100%">
                      <dashCard title="产线说明" titleIcon="el-icon-edit-outline" @dash-clicked="productLineUpdate">
                        <div class="flex flex-justify-main" style="height:450px;align-items:center">
                          <div class="flex" style="height:360px">
                            <div class="line-content">产线名称: {{ tabInfo.name }}</div>
                            <div class="line-content">检测等级: {{ tabInfo.level }}</div>
                            <div class="line-content">产线位置: {{ tabInfo.location }}</div>
                            <div class="line-content">设备寿命: {{ tabInfo.life }}</div>
                            <div class="line-content">安装时间: {{ tabInfo.installTime }}</div>
                          </div>
                          <!-- <el-image class="flex-column " :src="getImage(tabInfo.image)" style="width: 400px; height: 400px; margin-top: 10px"> -->
                          <el-image class="flex-column " :src="getImage(tabInfo.image)" style="max-width: 600px; max-height: 300px;width:auto;height:auto; margin-top: 10px">
                            <div slot="error" class="flex-center image-slot" style="height: 400px; background: #dddddd">
                              <i class="el-icon-picture-outline" style="font-size: 30px"></i>
                            </div>
                          </el-image>
                        </div>
                      </dashCard>
                    </el-col>
                    <el-col :span="12" style="height: 100%">
                      <el-row :gutter="10" style="margin: 0;">
                        <el-col :span="24">
                          <dashCard :showHeader="false" style="height: 240px; margin-bottom: 10px;">
                            <div class="alarm-tabs">
                              <el-tabs v-model="rulesIndex" :addable="true" @edit="handleAlarmRuleEdit" @tab-click="handleClickAlarmRule">
                                <el-tab-pane :label="tab.name" :name="tab.name" :key="tab.name" v-for="tab in tableRules">
                                  <el-table :data="alarmRuleList" style="">
                                    <el-table-column
                                      v-for="column of alarmTableProps"
                                      :key="column.prop||column.label"
                                      :label="column.label"
                                      :width="column.width||''"
                                    >
                                      <template slot-scope="scope">
                                        <template v-if="column.label==='操作'">
                                          <span  class="color-secondary cursor-pointer handle-btn" @click="alarmRuleUpdate(scope.row)">编辑</span>
                                          |
                                          <span  class="color-secondary cursor-pointer handle-btn" @click="alarmRuleDelete(scope.row)">删除</span>
                                        </template>
                                        <template v-else-if="column.label==='报警上线'">
                                          {{ scope.row.max }}
                                        </template>
                                        <template v-else-if="column.label==='报警下线'">
                                          {{ scope.row.min }}
                                        </template>
                                        <template v-else>
                                          {{ scope.row[column.prop] ? scope.row[column.prop] : '-' }}
                                        </template>
                                      </template>
                                    </el-table-column>
                                  </el-table>
                                </el-tab-pane>
                              </el-tabs>
                            </div>
                          </dashCard>
                        </el-col>
                        <el-col :span="24">
                          <dashCard :showHeader="false" style="height: 240px">
                            <el-tabs v-model="gatherIndex" :addable="true" @edit="handleGatherRuleEdit" @tab-click="handleClickGatherRule">
                              <el-tab-pane :label="gather.name" :name="gather.name" :key="gather.name"
                                           v-for="gather in gatherRules">
                                <el-table :data="gatherRuleList" style="" v-if = "gatherRuleType !== 0">
                                  <el-table-column
                                    v-for="column of gatherTableProps"
                                    :key="column.prop||column.label"
                                    :label="column.label"
                                    :width="column.width||''"
                                  >
                                    <template slot-scope="scope">
                                      <template v-if="column.label==='操作'">
                                        <span  class="color-secondary cursor-pointer handle-btn" @click="gatherRuleUpdate(scope.row)">编辑</span>
                                        |
                                        <span  class="color-secondary cursor-pointer handle-btn" @click="gatherRuleDelete(scope.row)">删除</span>
                                      </template>
                                      <template v-else-if="column.label==='开始时间'">
                                        {{ scope.row.startTime }}
                                      </template>
                                      <template v-else-if="column.label==='结束时间'">
                                        {{ scope.row.endTime }}
                                      </template>
                                      <template v-else-if="column.label==='间隔时间'">
                                        {{ scope.row.interval }}
                                      </template>
                                      <template v-else-if="column.label==='次数'">
                                        {{ scope.row.count }}
                                      </template>
                                      <template v-else>
                                        {{ scope.row[column.prop] ? scope.row[column.prop] : '-' }}
                                      </template>
                                    </template>
                                  </el-table-column>
                                </el-table>
                                <el-table :data="gatherRuleList" style="" v-else>
                                  <el-table-column
                                    v-for="column of aeGatherProps"
                                    :key="column.prop||column.label"
                                    :label="column.label"
                                    :width="column.width||''"
                                  >
                                    <template slot-scope="scope">
                                      <template v-if="column.label==='操作'">
                                        <span  class="color-secondary cursor-pointer handle-btn" @click="gatherRuleUpdate(scope.row)">编辑</span>
                                        |
                                        <span  class="color-secondary cursor-pointer handle-btn" @click="aeRGatherRuleDelete(scope.row)">删除</span>
                                      </template>
                                      <template v-else-if="column.label==='定时采集类型'">
                                        {{ scope.row.timingConfig.ae_timing_type == 3 ? '间隔采集模式': '连续采集模式' }}
                                      </template>
                                      <template v-else-if="column.label==='采样间隔'">
                                        {{ scope.row.measureConfig.ae_measure_interval }}
                                      </template>
                                      <template v-else-if="column.label==='采样次数'">
                                        {{ scope.row.measureConfig.ae_measure_times }}
                                      </template>
                                      <template v-else>
                                        {{ scope.row[column.prop] ? scope.row[column.prop] : '-' }}
                                      </template>
                                    </template>
                                  </el-table-column>
                                </el-table>
                              </el-tab-pane>
                            </el-tabs>
                          </dashCard>
                        </el-col>
                      </el-row>
                    </el-col>
                  </el-row>
                </div>
                <div class="flex-1">
                  <el-row :gutter="10" style="margin: 0;height: 400px">
                    <el-col :span="8" style="height: 100%">
                      <dashCard title="传输设备" titleIcon="el-icon-circle-plus-outline" @dash-clicked="commDeviceAdd">
                        <el-table :data="commDevicesList" style="">
                          <el-table-column
                            v-for="column of comDeviceTableProps"
                            :key="column.prop||column.label"
                            :label="column.label"
                            :width="column.width||''"
                          >
                            <template slot-scope="scope">
                              <template v-if="column.label==='操作'">
                                <span  class="color-secondary cursor-pointer handle-btn" @click="commDeviceUpdate(scope.row)">编辑</span>
                                |
                                <span  class="color-secondary cursor-pointer handle-btn" @click="commDeviceDelete(scope.row)">删除</span>
                              </template>
                              <template v-else-if="column.label==='设备类型'">
                                {{ getComDeviceName(scope.row.type) }}
                              </template>
                              <template v-else-if="column.label==='设备名称'">
                                {{ scope.row.name }}
                              </template>
                              <template v-else-if="column.label==='SN'">
                                {{ scope.row.sn }}
                              </template>
                              <template v-else-if="column.label==='IP'">
                                {{ scope.row.ip }}
                              </template>
                              <template v-else-if="column.label==='ID'">
                                {{ scope.row.id }}
                              </template>
                              <template v-else>
                                {{ scope.row[column.prop] ? scope.row[column.prop] : '-' }}
                              </template>
                            </template>
                          </el-table-column>
                        </el-table>
                      </dashCard>
                    </el-col>
                    <el-col :span="16" style="height: 100%">
                      <dashCard :showHeader="false">
                        <el-tabs v-model="deviceIndex" :addable="true" @edit="handleAeDeviceEdit" @tab-click="handleClickAeDevice">
                          <el-tab-pane :label="tab.name" :name="tab.name" :key="tab.name" v-for="tab in deviceRules">
                            <el-table :data="devicesList" style="">
                              <el-table-column
                                v-for="column of deviceTableProps"
                                :key="column.prop||column.label"
                                :label="column.label"
                                :width="column.width||''"
                              >
                                <template slot-scope="scope">
                                  <template v-if="column.label==='操作'">
                                    <span  class="color-secondary cursor-pointer handle-btn" @click="aeDeviceUpdate(scope.row)">编辑</span>
                                    |
                                    <span  class="color-secondary cursor-pointer handle-btn" @click="aeDeviceDelete(scope.row)">删除</span>
                                  </template>
                                  <template v-else-if="column.label==='网关类型'">
                                    {{ scope.row.commDeviceId }}
                                  </template>
                                  <template v-else-if="column.label==='地址'">
                                    {{ scope.row.address }}
                                  </template>
                                  <template v-else-if="column.label==='网关ID'">
                                    {{ scope.row.commDeviceId }}
                                  </template>
                                  <template v-else-if="column.label==='通道'">
                                    {{ scope.row.pos }}
                                  </template>
                                  <template v-else-if="column.label==='设备类型'">
                                    {{ getDeviceName(scope.row.type) }}
                                  </template>
                                  <template v-else>
                                    {{ scope.row[column.prop] ? scope.row[column.prop] : '-' }}
                                  </template>
                                </template>
                              </el-table-column>
                            </el-table>
                          </el-tab-pane>
                        </el-tabs>
                      </dashCard>
                    </el-col>
                  </el-row>
                </div>
              </div>
            </el-tab-pane>
          </el-tabs>
        <div class="plan-record-dialog-wrapper">
          <product-line-dialog
            v-if="editProductLineDialogVisible"
            :visible.sync="editProductLineDialogVisible"
            @update-list="getTableList"
            :info="productLineInfo"
          ></product-line-dialog>
          <alarm-rule-dialog
            v-if="alarmRuleDialogVisible"
            :visible.sync="alarmRuleDialogVisible"
            @update-list="getAlarmRuleList"
            :info="alarmRuleInfo"
            :alarmType="alarmRuleType"
            :title="rulesIndex"
          ></alarm-rule-dialog>
          <gather-rule-dialog
            v-if="gatherRuleDialogVisible"
            :visible.sync="gatherRuleDialogVisible"
            @update-list="getGatherRuleList"
            :info="gatherRuleInfo"
            :alarmType="gatherRuleType"
            :title="gatherIndex"
          ></gather-rule-dialog>
          <ae-gather-rule-dialog
            v-if="aeGatherRuleDialogVisible"
            :visible.sync="aeGatherRuleDialogVisible"
            @update-list="getAeGatherRuleList"
            :info="aeGatherRuleInfo"
            :alarmType="gatherRuleType"
            :title="gatherIndex"
          ></ae-gather-rule-dialog>
          <comm-Device-dialog
            v-if="commDeviceDialogVisible"
            :visible.sync="commDeviceDialogVisible"
            @update-list="getCommDeviceList"
            :productLineId="productLineId"
            :info="commDeviceInfo"
          ></comm-Device-dialog>
          <device-dialog
            v-if="deviceDialogVisible"
            :visible.sync="deviceDialogVisible"
            @update-list="getAeDeviceProductList"
            :productLineId="productLineId"
            :aeType="aeType"
            :info="aeDeviceInfo"
          ></device-dialog>
          <ae-Device-dialog
            v-if="aeDeviceDialogVisible"
            :visible.sync="aeDeviceDialogVisible"
            @update-list="getAeDeviceProductList"
            :productLineId="productLineId"
            :aeType="aeType"
            :info="aeDeviceInfo"
          ></ae-Device-dialog>
        </div>
      </div>
  </d2-container>
</template>

<script>
import productLine from "@/request/modules/productLine";
import alarmRule from "@/request/modules/alarmRule";
import gatherRule from "@/request/modules/gatherRule";
import aeRule from "@/request/modules/aeRule";
import dashCard from './components/card/index.vue'
import commDevice from "@/request/modules/commDevice";
import aeDevice from "@/request/modules/aeDevice";
import productLineDialog from "./components/productLinedialog"
import alarmRuleDialog from "./components/alarmRuledialog"
import gatherRuleDialog from "./components/gatherRuledialog"
import aeGatherRuleDialog from "./components/aeGatherRuleDialog"
import commDeviceDialog from "./components/commDevicedialog"
import deviceDialog from "./components/devicedialog"
import aeDeviceDialog from "./components/aeDevicedialog"
import { tableRules, gatherRules, alarmTableProps, gatherTableProps, comDeviceTable, deviceTable, deviceTableProps, gatherTableProps1 } from "./config/tableConfig"

export default {
  components: {
    dashCard,
    productLineDialog,
    alarmRuleDialog,
    gatherRuleDialog,
    aeGatherRuleDialog,
    commDeviceDialog,
    deviceDialog,
    aeDeviceDialog
  },
  computed: {},
  data () {
    return {
      tableList: [],
      alarmRuleList: [],
      gatherRuleList: [],
      commDevicesList: [],
      devicesList: [],
      tableRules: tableRules,
      gatherRules: gatherRules,
      deviceRules: deviceTable,
      alarmTableProps: alarmTableProps,
      gatherTableProps: gatherTableProps,
      comDeviceTableProps: comDeviceTable,
      deviceTableProps: deviceTableProps,
      aeGatherProps: gatherTableProps1,
      rulesIndex: '温度报警规则',
      gatherIndex: '声发射采集规则',
      deviceIndex: '声发射',
      tabIndex: '',
      tabInfo: null,
      tableRulesInfo: null,
      editProductLineDialogVisible: false,
      alarmRuleDialogVisible: false,
      gatherRuleDialogVisible: false,
      aeGatherRuleDialogVisible: false,
      commDeviceDialogVisible: false,
      deviceDialogVisible: false,
      aeDeviceDialogVisible: false,
      productLineInfo: {},
      alarmRuleInfo: {},
      gatherRuleInfo: {},
      aeGatherRuleInfo: {},
      commDeviceInfo: {},
      aeDeviceInfo: {},
      productLineId: 0,
      alarmRuleType: 0,
      gatherRuleType: 0,
      aeType: 0
    }
  },
  mounted () {
    this.getTableList()
  },
  beforeDestroy () {

  },
  methods: {
    getImage (image) {
      return process.env.VUE_APP_API_HOST + image;
    },
    handleClick (tab, event) {
      this.tabInfo = this.tableList[tab.index]
      console.log(this.tabInfo,999);
      this.productLineId = this.tableList[tab.index].id

      this.alarmRuleList = []
      this.gatherRuleList = []
      this.commDevicesList = []
      this.commDevicesList = []

      this.getAlarmRuleList()
      this.getGatherRuleList()
      this.getCommDeviceList()
      this.getAeDeviceProductList()
    },
    handleClickAlarmRule (tab, event) {
      this.tableRulesInfo = this.tableRules[tab.index]
      this.alarmRuleType = this.tableRules[tab.index].id
      this.getAlarmRuleList()
    },
    handleClickGatherRule (tab, event) {
      this.gatherRuleType = this.tableRules[tab.index].id
      this.getGatherRuleList()
    },
    handleClickAeDevice (tab, event) {
      this.aeType = this.deviceRules[tab.index].id
      this.getAeDeviceProductList()
    },
    productLineUpdate () {
      console.log(111);
      this.productLineInfo = this.tabInfo;
      this.editProductLineDialogVisible = true
    },
    alarmRuleUpdate (item) {
      this.alarmRuleInfo = item;
      this.alarmRuleDialogVisible = true
    },
    alarmRuleDelete (item) {
      this.$confirm(`是否删除${item.name}此报警规则?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        alarmRule.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getAlarmRuleList();
        })
      })
    },
    gatherRuleUpdate (item) {
      if (this.gatherRuleType === 0) {
        this.aeGatherRuleInfo = item;
        this.aeGatherRuleDialogVisible = true
      } else {
        this.gatherRuleInfo = item;
        this.gatherRuleDialogVisible = true
      }
    },
    gatherRuleDelete (item) {
      this.$confirm(`是否删除${item.name}此采集规则?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        gatherRule.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getGatherRuleList();
        })
      })
    },
    commDeviceUpdate (item) {
      console.log(item);
      this.commDeviceInfo = item;
      this.commDeviceDialogVisible = true
    },
    commDeviceDelete (item) {
      this.$confirm(`是否删除${item.name}此报警规则?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        commDevice.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getCommDeviceList();
        })
      })
    },
    aeDeviceUpdate (item) {
      this.aeDeviceInfo = item;
      console.log(this.aeDeviceInfo);
      this.productLineId = this.tabInfo.id
      if (this.aeType === 0) {
        this.aeDeviceDialogVisible = true
      } else {
        this.deviceDialogVisible = true
      }
    },
    aeDeviceDelete (item) {
      this.$confirm(`是否删除${item.name}此采集设备嘛?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        aeDevice.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getAeDeviceProductList();
        })
      })
    },
    commDeviceAdd () {
      this.commDeviceInfo = {}
      this.commDeviceDialogVisible = true
    },
    getTableList () {
      const params = {}
      productLine.line({
        params
      }).then(res => {
        console.log(res);
        this.tableList = res.data
        this.tabIndex = res.data[0].name
        this.tabInfo = res.data[0]
        this.productLineId = res.data[0].id
        this.getCommDeviceList()
        this.getAlarmRuleList()
        this.getGatherRuleList()
        this.getAeDeviceProductList()
      })
    },
    getAlarmRuleList () {
      const params = {
        productLineId: this.tabInfo.id,
        type: this.alarmRuleType
      }
      alarmRule.requestTypeRules({
        params
      }).then(res => {
        this.alarmRuleList = res.data
      })
    },
    getGatherRuleList () {
      if (this.gatherRuleType === 0) {
        const params = {}
        aeRule.request({
          params
        }).then(res => {
          this.gatherRuleList = res.data
        })
      } else {
        const params = {
          productLineId: this.tabInfo.id,
          type: this.gatherRuleType
        }
        gatherRule.requestTypeRules({
          params
        }).then(res => {
          this.gatherRuleList = res.data
        })
      }
    },

    getAeGatherRuleList () {
      const params = {
        // productLineId: this.tabInfo.id,
        // type: this.gatherRuleType
      }
      aeRule.request({
        params
      }).then(res => {
        this.gatherRuleList = res.data
      })
    },
    //声发射采集规则删除
    aeRGatherRuleDelete (item) {
      this.$confirm(`是否删除${item.name}此声发射采集规则嘛?`, '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        const data = { "id": item.id }
        aeRule.request({
          method: 'DELETE',
          data
        }).then(res => {
          this.$message({
            message: '删除成功',
            type: 'success'
          });
        }).then(res => {
          this.getGatherRuleList();
        })
      })
    },
    getCommDeviceList () {
      const params = {
        id: this.tabInfo.id
      }
      commDevice.requestProductLine({
        params
      }).then(res => {
        this.commDevicesList = res.data
      })
    },
    getAeDeviceProductList () {
      const params = {
        id: this.tabInfo.id,
        type: this.aeType
      }
      aeDevice.requestProductLine({
        params
      }).then(res => {
        this.devicesList = res.data
      })
    },
    handleTabsEdit (targetName, action) {
      if (action === 'add') {
        this.productLineInfo = {}
        this.editProductLineDialogVisible = true
      }
      if (action === 'remove') {
        console.log(targetName)
        let id = 0;
        this.tableList.forEach((tab, index) => {
          if (tab.name === targetName) {
            id = tab.id
          }
        });
        console.log(id)
        this.$confirm(`是否删除${targetName}此产线?`, '提示', {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          const data = { "id": id }
          productLine.line({
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
      }
    },
    handleAlarmRuleEdit (targetName, action) {
      if (action === 'add') {
        this.alarmRuleInfo = {}
        this.alarmRuleDialogVisible = true
      }
    },
    handleGatherRuleEdit (targetName, action) {
      if (action === 'add') {
        if (this.gatherRuleType === 0) {
          this.aeGatherRuleInfo = {}
          this.aeGatherRuleDialogVisible = true
        } else {
          this.gatherRuleInfo = {}
          this.gatherRuleDialogVisible = true
        }
      }
    },
    handleAeDeviceEdit (targetName, action) {
          console.log(this.aeType)
      if (action === 'add') {
        if (this.aeType === 0) {
          this.aeDeviceInfo = {}
          this.aeDeviceDialogVisible = true
        } else {
          this.aeDeviceInfo = {}
          this.deviceDialogVisible = true
        }
      }
    },
    getComDeviceName (type) {
      if (type === 0) {
        return '采集网关'
      } else if (type === 1) {
        return '声发射'
      }
    },
    getDeviceName (type) {
      if (type === 0) {
        return '声发射'
      } else if (type === 1) {
        return '传感器'
      } else if (type === 2) {
        return '振动设备'
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '~@/assets/style/theme/register.scss';

.page.container-component {
  color: #fff;
  .tabPane {
    height: calc(100% - 150px);
  }

  .line-content {
    font-size: 16px;
    margin-top: 10px;
    margin-left: 10px;
  }
}

#page-device::v-deep {
  @include el-tabs-reset();
  @include el-table-reset-primary();

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
