<template>
  <div class="dialog-wrapper">
    <sn-dialog
      :title="dialogHeader"
      :visible.sync="dialogVisible"
      borderType="secondary"
      headerType="secondary"
      width="450px"
    >
      <div class="dialog-content-wrapper flex-column flex-center">
        <sn-form
          :formList="stationConfig"
          :formValue="stationData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
          ref="form"
          v-model="stationData"
        ></sn-form>
        <hr class="halfing-line" />
        <div class="button-wrapper">
          <sn-form
            :formList="btnConfig"
            :formStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            :itemStyle="{
              'margin-bottom': '0px',
              'margin-top': '0px'
            }"
            @cancel-btn-click="closeDialog"
            @submitForm="submitForm"
          ></sn-form>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
export default {
  name: 'fireStationDialog',
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    stationInfo: {
      type: Object,
      default () {
        return {}
      }
    }
  },
  data () {
    return {
      stationData: {},
      dialogVisible: false,
      stationConfig: [
        {
          type: 'input',
          value: '',
          label: '消防器材',
          key: 'equipment_name'
        },
        {
          type: 'input-number',
          value: '1',
          label: '器材数量',
          key: 'equipment_amount',
          suffix: '',
          props: {
            min: 1
          }
        },
        {
          type: 'date-picker',
          label: '保质期限',
          key: 'guarantee_date',
          placeholder: '请选择日期',
          value: '',
          style: {
            display: 'inline-flex'
          }
        },
        {
          type: 'input',
          value: '',
          label: '责任人',
          key: 'guarantee_person'
        }
      ],
      btnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '取消',
              type: 'cancel'
            },
            {
              label: '确定',
              type: 'submit'
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
      return '消防器材'
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
        const isEdit = Object.keys(this.stationInfo).length !== 0
        if (isEdit) {
          const id = this.stationInfo.id
          this.$store
            .dispatch('sinuo/fireControl/updateFireStation', {
              ...this.stationData,
              id
            })
            .then(() => {
              this.closeDialog()
              this.emitUpdateList()
            })
        } else {
          this.$store
            .dispatch('sinuo/fireControl/createFireStation', this.stationData)
            .then(() => {
              this.closeDialog()
              this.emitUpdateList()
            })
        }
      })
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
    stationInfo: {
      immediate: true,
      handler (newVal) {
        this.stationData = newVal
      }
    }
  }
}
</script>

<style lang="scss" scoped>
@import '~@/assets/style/theme/register.scss';
.dialog-wrapper::v-deep {
  .el-dialog__body {
    padding: 40px 4px 0 !important;
  }
  .dialog-content-wrapper {
    justify-content: space-around;
    height: 100%;
  }
  .el-input__inner,
  .el-input-number {
    height: 36px !important;
    width: 300px !important;
  }
  .halfing-line {
    /* 逐步尝试值 */
    width: 435px;
    height: 1px;
    background-color: rgba(101, 226, 245, 0.1);
    border: none;
    margin: 60px 0 15px;
  }
  .button-wrapper {
    width: 435px;
    display: flex;
    justify-content: flex-end;
  }
  .el-date-editor--date {
    width: 300px !important;
    background-position-x: 272px !important;
  }
  .el-form-item__label {
    width: 60px !important;
  }
}
</style>
