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
          :formList="dialogConfig"
          :formValue="userData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
          ref="form"
          v-model="userData"
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
  name: 'controlRoomDialog',
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    userInfo: {
      type: Object,
      default () {
        return {}
      }
    }
  },
  data () {
    return {
      userData: {},
      dialogVisible: false,
      dialogConfig: [
        {
          type: 'input',
          value: '',
          label: '姓名',
          key: 'name',
          rule: {
            required: true
          }
        },
        {
          type: 'select',
          label: '性别',
          key: 'gender',
          value: '',
          options: [
            {
              label: '男',
              value: 0
            },
            {
              label: '女',
              value: 1
            },
          ],
          style: {
            display: 'inline-flex'
          },
          rule: {
            required: true
          }
        },
        {
          type: 'input',
          value: '',
          label: '联系电话',
          key: 'phone',
          rule: {
            required: true
          }
        },
        {
          type: 'input',
          value: '',
          label: '岗位',
          key: 'job',
          rule: {
            required: true
          }
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
      return '添加值班人员'
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
        const isEdit = Object.keys(this.userInfo).length !== 0
        if (isEdit) {
          const id = this.userInfo.id
          this.$store
            .dispatch('sinuo/fireControl/updateControlRoom', {
              ...this.userData,
              id
            })
            .then(() => {
              this.closeDialog()
              this.emitUpdateList()
            })
        } else {
          this.$store
            .dispatch('sinuo/fireControl/createControlRoom', this.userData)
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
    userInfo: {
      immediate: true,
      handler (newVal) {
        this.userData = newVal
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
  .form-wrapper{
    .el-form{
      .el-form-item.is-required:not(.is-no-asterisk) .el-form-item__label-wrap>.el-form-item__label::before,
      .el-form-item.is-required:not(.is-no-asterisk)>.el-form-item__label::before{
        content: '' !important;
        font-size: 0px !important;
      }
    }
  }
}
</style>
