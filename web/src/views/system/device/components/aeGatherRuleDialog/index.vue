<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="600px" borderType="secondary" headerType="secondary">
      <div class="dialog-content-wrapper flex-column flex-center">
        <sn-form
          :formList="planConfig"
          ref="form"
          v-model="infoData"
          :formValue="infoData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
        ></sn-form>
<!--        <hr class="halfing-line"/>-->
        <div class="button-wrapper">
          <sn-form
            :formList="btnConfig"
            @cancel-btn-click="closeDialog"
            @submitForm="submitForm"
            :itemStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            :formStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
          ></sn-form>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
import aeRules from "@/request/modules/aeRule";
export default {
  name: 'editPlanDialog',
  props: {
    title: {
      type: String,
      default: '采集规则'
    },
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    alarmType: {
      type: Number,
      default () {
        return 0
      }
    },
    info: {
      type: Object,
      default () {
        return {};
      }
    }
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler (val) {
        this.dialogVisible = val;
      }
    },
    dialogVisible (newVal) {
      this.$emit('update:visible', newVal)
    },
    info: {
      immediate: true,
      handler (newVal) {
        this.infoData.name = newVal.name
        this.infoData.ae_measure_mode = newVal.measureConfig.ae_measure_mode;
        this.infoData.ae_timing_type = newVal.timingConfig.ae_timing_type;
        this.infoData.ae_timing_length = newVal.timingConfig.ae_timing_length;
        this.infoData.ae_timing_sleep = newVal.timingConfig.ae_timing_sleep;
      
        this.infoData.ae_measure_times = newVal.measureConfig.ae_measure_times;
        this.infoData.ae_measure_interval = newVal.measureConfig.ae_measure_interval;
        this.infoData.ae_threshold = newVal.measureConfig.ae_threshold;
        this.infoData.ae_measure_speed = newVal.measureConfig.ae_measure_speed;
        this.infoData.ae_eet = newVal.measureConfig.ae_eet;
        this.infoData.ae_hdt = newVal.measureConfig.ae_hdt;
        this.infoData.ae_hlt = newVal.measureConfig.ae_hlt;
        this.infoData.ae_measure_length = newVal.measureConfig.ae_measure_length;
        console.log(this.infoData)
      }
    }
  },
  data () {
    return {
      infoData: {},
      dialogVisible: false,
      planConfig: [
        {
          type: 'input',
          value: '',
          label: '规则名称',
          key: 'name',
          rule: {
            required: true,
          },
        },
        {
          type: 'select',
          value: 1,
          label: '采集模式',
          key: 'ae_measure_mode',
          options: [
            {
              label: '包络采集',
              value: 1
            },
            {
              label: '连续采集',
              value: 2
            }
          ],
          rule: {
            required: true,
          },
        },
        {
          type: 'select',
          value: 3,
          label: '定时采集类型',
          key: 'ae_timing_type',
          options: [
            {
              label: '连续采集模式',
              value: 1
            },
            {
              label: '间隔采集模式',
              value: 3
            },
          ],
          rule: {
            required: true,
          },
        },
        {
          type: 'input',
          value: 10,
          label: '定时采集时长',
          key: 'ae_timing_length',
          
        },{
          type: 'input',
          value: 10,
          label: '定时采集休眠时间',
          key: 'ae_timing_sleep',
          
        },
        {
          type: 'input',
          value: '',
          label: '采样次数',
          key: 'ae_measure_times',
          rule: {
            required: true,
          },
        },
        {
          type: 'input',
          value: 60,
          label: '采样间隔',
          key: 'ae_measure_interval',
          rule: {
            required: true,
          },
        },
        {
          type: 'input',
          value: 30,
          label: '采集门限',
          key: 'ae_threshold',
        },
        {
          type: 'input',
          value: 800,
          label: '采集速率',
          key: 'ae_measure_speed',
        },
        {
          type: 'input',
          value: 12500,
          label: '波形截止时间(eet)',
          key: 'ae_eet',
        },
        {
          type: 'input',
          value: 1250,
          label: '波形定义时间(hdt)',
          key: 'ae_hdt',
        },
        {
          type: 'input',
          value: 18,
          label: '采集速率(hlt)',
          key: 'ae_hlt',
        },
        {
          type: 'input',
          value: 3,
          label: '采样长度',
          key: 'ae_measure_length',
        },

        // {
        //   type: 'select',
        //   value: '',
        //   label: '参数发送使能',
        //   key: 'ae_para_enable',
        //   options: [
        //     {
        //       label: '是',
        //       value: true
        //     },
        //     {
        //       label: '否',
        //       value: false
        //     }
        //   ],
        // },
        // {
        //   type: 'select',
        //   value: '',
        //   label: '波形发送使能',
        //   key: 'ae_wave_enable',
        //   options: [
        //     {
        //       label: '是',
        //       value: true
        //     },
        //     {
        //       label: '否',
        //       value: false
        //     }
        //   ],
        // },
        // {
        //   type: 'input',
        //   value: '',
        //   label: '系统当前时间',
        //   key: 'ae_system_time',
        // },
        // {
        //   type: 'select',
        //   value: '',
        //   label: '系统采集状态',
        //   key: 'ae_measure_state',
        //   options: [
        //     {
        //       label: '停止',
        //       value: false
        //     },
        //     {
        //       label: '启动',
        //       value: true
        //     }
        //   ],
        // },
        // {
        //   type: 'title',
        //   value: '',
        //   label: '',
        //   title: '---------------- 声发射评级设置 ----------------',
        //   key: '',
        // },
        // {
        //   type: 'select',
        //   value: '',
        //   label: '评级功能使能',
        //   key: 'ae_level_enable',
        //   options: [
        //     {
        //       label: '使能',
        //       value: true
        //     },
        //     {
        //       label: '禁止',
        //       value: false
        //     }
        //   ],
        // },
        // {
        //   type: 'input',
        //   value: '',
        //   label: '评级统计时间',
        //   key: 'ae_level_cal_time',
        // },
        // {
        //   type: 'select',
        //   value: '',
        //   label: '实时强度类型',
        //   key: 'ae_level_strength_report_type',
        //   options: [
        //     {
        //       label: '不上报',
        //       value: 0
        //     },
        //     {
        //       label: '上报1级强度',
        //       value: 1
        //     },
        //     {
        //       label: '上报2级强度',
        //       value: 2
        //     },
        //     {
        //       label: '上报3级强度',
        //       value: 3
        //     },
        //   ],
        // },
        // {
        //   type: 'input',
        //   value: '',
        //   label: '实时强度上报最小间隔',
        //   key: 'ae_level_strength_report_interval',
        // },
        // {
        //   type: 'title',
        //   value: '',
        //   label: '',
        //   title: '---------------- 声发射定时设置 ----------------',
        //   key: '',
        // },
        // {
        //   type: 'select',
        //   value: 1,
        //   label: '定时采集类型',
        //   key: 'ae_timing_type',
        //   options: [
        //     {
        //       label: '连续采集模式',
        //       value: 1
        //     },
        //     {
        //       label: '间隔采集模式',
        //       value: 3
        //     },
        //   ],
        // },
        // {
        //   type: 'input',
        //   value: '',
        //   label: '定时采集时长',
        //   key: 'ae_timing_length',
        // },
        // {
        //   type: 'input',
        //   value: '',
        //   label: '定时采集休眠时间',
        //   key: 'ae_timing_sleep',
        // },
      ],
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '取消',
              type: 'cancel',
            },
            {
              label: '确定',
              type: 'submit',
            }
          ],
          style: {
            display: 'inline-flex'
          }
        }
      ]
    }
  },
  computed: {
    dialogHeader () {
      return (Object.keys(this.info).length === 0 ? '添加' : '修改') + this.title
    }
  },
  methods: {
    closeDialog () {
      this.dialogVisible = false
    },
    emitUpdateList () {
      this.$emit('update-list')
    },
    submitForm () {
      this.$refs.form.submitForm().then(() => {
        const isEdit = Object.keys(this.info).length !== 0
        console.log(this.infoData)
        const name = this.infoData.name
        const measureConfig = {
          ae_threshold: this.infoData.ae_threshold,
          ae_measure_speed: this.infoData.ae_measure_speed,
          ae_measure_mode: this.infoData.ae_measure_mode,
          ae_eet: this.infoData.ae_eet,
          ae_hdt: this.infoData.ae_hdt,
          ae_hlt: this.infoData.ae_hlt,
          ae_measure_length: this.infoData.ae_measure_length,
          ae_measure_times: this.infoData.ae_measure_times,
          ae_measure_interval: this.infoData.ae_measure_interval,
          ae_para_enable: this.infoData.ae_para_enable,
          ae_system_time: this.infoData.ae_system_time,
          ae_measure_state: this.infoData.ae_measure_state,
        }
        const levelConfig = {
          ae_level_enable: this.infoData.ae_level_enable,
          ae_level_cal_time: this.infoData.ae_level_cal_time,
          ae_level_strength_report_type: this.infoData.ae_level_strength_report_type,
          ae_level_strength_report_interval: this.infoData.ae_level_strength_report_interval,
        }
        if(this.infoData.ae_timing_type == 3 && (!this.infoData.ae_timing_length || !this.infoData.ae_timing_sleep)) {
          return this.$message({ message: '间隔采集模式请补充定时采集时长和定时采集休眠时间', type: 'error'});
        }
        const timingConfig = {
          ae_timing_enable: true,
          ae_timing_type: this.infoData.ae_timing_type,
          ae_timing_length: this.infoData.ae_timing_type == 3 ? this.infoData.ae_timing_length : 0,
          ae_timing_sleep: this.infoData.ae_timing_type == 3 ? this.infoData.ae_timing_sleep : 0,
        }
        if (isEdit) {
          const id = this.info.id
          const data = { name, measureConfig, levelConfig, timingConfig, id }
          aeRules.update({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        } else {
          const data = { name, measureConfig, levelConfig, timingConfig }
          console.log(data)
          aeRules.request({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        }
      })
    }
  },
}
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.dialog-wrapper::v-deep{
  .el-dialog__body{
    padding: 40px 4px 0 !important;
  }
  .dialog-content-wrapper{
    justify-content: space-around;
    height: 100%;
  }
  input::-webkit-input-placeholder {
    color: $color-secondary;
  }
  .el-input__inner,.el-input-number{
    height: 36px !important;
    width: 300px !important;
  }
  .el-input__inner{
    font-size: 12px;
    font-family:PingFangSC-Medium ;
    color:rgba(101,226,245,1);
    width: 180px;
    height: 32px;
    background:linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%);
    line-height: 32px;
    padding-left: 12px;
    border-radius: 0;
    border:1px solid rgba(101,226,245,.5);
    &::placeholder{
      opacity: .5;
    }
    &+.el-input__prefix{
      display: none;
    }
    &+.el-input__suffix{
      display: none;
    }
  }
  .halfing-line{
    /* 逐步尝试值 */
    width: 435px;
    height: 1px;
    background-color: rgba(101, 226, 245, .1);
    border: none;
    margin: 302px 0 15px;
  }
  .button-wrapper{
    width: 435px;
    display: flex;
    justify-content: flex-end;
  }
  .el-date-editor--date {
    width: 300px !important;
    background-position-x: 272px !important;
  }
  .el-form-item__label {
    width: 150px !important;
  }
}
.form-wrapper::v-deep{
  font-size: 0;
  input::-webkit-input-placeholder {
    color: $color-secondary;
  }
  .el-date-picker {
    .el-picker-panel__footer {
      border-top: 1px solid rgba(101,226,245,1);
      background-color: transparent;
    }
  }

  .el-form-item__label{
    padding:0;
    font-size: 12px;
    margin-right: 8px;
    text-align: start;
    font-family:PingFangSC-Medium;
    height: 32px;
    line-height: 32px;
    font-weight:500;
    color:rgba(101,226,245,1);
  }
  .el-form-item{
    display: flex;
    align-items: center;
    .el-form-item__content{
      line-height: 24px;
      >.form-cascader {
        border: 1px solid rgba(101,226,245,.5) !important;
        background: linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%) !important;
        .el-scrollbar {
          border-right: 1px solid rgba(101,226,245,.5) !important;
          .el-cascader-menu__wrap {
            .el-cascader-menu__list {
              li {
                background: rgba(8,40,79,0.2) !important;
                >label {
                  .el-checkbox__input {
                    .el-checkbox__inner {
                      background: linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%) !important;
                      border-color: rgba(101,226,245,1) !important;
                    }
                  }
                  .el-checkbox__input.is-checked {
                    .el-checkbox__inner {
                      background: linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%) !important;
                      border-color: rgba(101,226,245,1) !important;
                    }
                  }
                  .is-indeterminate {
                    .el-checkbox__inner {
                      background: linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%) !important;
                      border-color: rgba(101,226,245,1) !important;
                    }
                  }
                }
                >span {
                  color:rgba(101,226,245,1) !important;
                }
              }
              >.el-cascader-node.in-active-path {
                color: rgba(101,226,245,1) !important;
              }
            }
          }
        }
      }
      .el-radio-group{
        vertical-align: top;
      }
      .el-radio-button{
        padding-right: 16px;
        .el-radio-button__inner{
          background:transparent;
          padding: 0 12px;
          color:rgba(255,255,255,0.5);
          height:24px;
          line-height: 24px;
          border-radius: 0;
          border-width: 0;
          transition: all 0s;
        }
      }
      .is-active{
        .el-radio-button__inner {
          border-width: 2px;
          border-style: solid;
          //border-image-source: url('images/border.png');
          border-image-repeat: round;
          line-height: 20px;
          height: 24px;
          border-image-slice: 2 2 2 2 fill;
          color:rgba(101,226,245,1);
          box-shadow: 0 0 0 0;
        }
      }
      .el-input__inner{
        font-size: 12px;
        font-family:PingFangSC-Medium ;
        color:rgba(101,226,245,1);
        width: 200px;
        height: 32px;
        background:linear-gradient(180deg,rgba(8,40,79,0.2) 0%,rgba(16,86,117,0.3) 100%);
        line-height: 32px;
        padding-left: 12px;
        border-radius: 0;
        border:1px solid rgba(101,226,245,.5);
        &::placeholder{
          opacity: .5;
        }
        &+.el-input__prefix{
          display: none;
        }
        &+.el-input__suffix{
          display: none;
        }
      }
      /*  日期选择器*/
      .el-date-editor--date{
        //background-image: url("images/icon-date.png");
        background-repeat: no-repeat;
        background-position-y: 8px;
        background-position-x: 172px;
      }
      .el-date-editor.el-input, .el-date-editor.el-input__inner{
        width: 200px;
      }
      /*  button*/
      .el-button{
        width: 91px;
        height: 32px;
        font-size:14px;
        font-weight:500;
        color:rgba(101,226,245,1);
        padding: 0 0 ;
        background:rgba(0,21,53,1);
        margin: 0 15px 0 0;
        border-width: 2px;
        border-style: solid;
        //border-image-source: url('images/border.png');
        border-image-repeat: round;
        border-image-slice: 2 2 2 2 fill;
        transition: 0s all;
        &:hover{
          //background-image: url('images/button-background.png');
          background-size: 100% 100%;
          border: none;
          border-radius: 0;
        }
      }
      .el-dropdown-option{
        @extend .el-input__inner;
        display: inline-flex;
        box-sizing: border-box;
        justify-content: center;
        align-items: center;
      }
      span{
        font-size: 12px;
        font-family:PingFangSC-Medium;
      }
    }
    /* 上传 upload */
    .el-upload-reset{
      .el-upload__tip{
        line-height: 1em;
        margin: 10px 3px;
        font-weight:500;
        color: $color-secondary;
      }
      .el-upload-list{
        maring-top: 0px;
        .el-upload-list__item{
          .el-upload-list__item-name {
            width: 220px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
          margin: 0;
          border-radius: 0;
          &:hover{
            background: rgba(16,86,117,0.8);
          }
          .el-upload-list__item-name{
            color: $color-secondary;
            &:hover{
              color: $color-secondary;
            }
          }
          /*直接重置所有图标颜色*/
          i{
            color: $color-secondary;
          }
        }
      }
    }
    /* inputNumber   */
    .el-input-number{
      width: 200px;
      span[role='button']{
        box-sizing: border-box;
        width: 36px;
        height: 36px;
        background-color: transparent;
        border: 0;
        color: transparent;
        &:hover{
          color: transparent;
        }
        &::before{
          content: '';
          display: inline-flex;
          position: absolute;
          top: 0;
          left: 0;
          width: 100%;
          height: 100%;
        }
      }
      .el-input-number__decrease{
        left: 0;
        &::before{
          //background-image: url("images/icon-decrease.svg");
        }
      }
      .el-input-number__increase{
        right: 0;
        &::before{
          //background-image: url("images/icon-increase.svg");
        }
      }
      .el-input__inner{
        width: 100%;
        padding: 0 36px;
      }
    }
    .suffix{
      position: absolute;
      right: calc( 50% - 1.4em - .6em);
      top: 0;
      font-size: 12px;
      line-height: 40px;
      color: rgba(101, 226, 245, 1);
    }
    /* select */
    .el-select{
      .el-select__tags{
        .el-tag.el-tag--info{
          color: $color-secondary;
          background-color: transparent;
          /* background-image的拉伸不能总是合适，所以使用slice代替 */
          //border-image: url("./images/el-tag-background.png");
          border-image-slice: 2 fill;
          border-radius: 0;
          padding: 0 10px;
          .el-select__tags-text{
            margin-right: 24px;
          }
          .el-tag__close{
            background: transparent;
            color: $color-secondary;
            font-size: 14px; // 把图标放大
          }
        }
      }
    }
  }
}
</style>
