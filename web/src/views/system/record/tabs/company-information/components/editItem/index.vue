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
          :formValue="itemData"
          :itemStyle="{
            'margin-bottom': '40px'
          }"
          ref="form"
          v-model="itemData"
          class="open-height-form"
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
  name: 'editItemDialog',
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    itemInfo: {
      type: Object,
      default () {
        return {}
      }
    }
  },
  data () {
    const vm = this
    return {
      itemData: {},
      dialogVisible: false,
      dialogConfig: [
        {
          type: 'input',
          value: '',
          label: '图片名称',
          key: 'name',
          rule: {
            required: true,
          },
        },
        {
          type: 'upload',
          value: [],
          label: '上传照片',
          key: 'upload_id',
          style: {
            'align-items': 'flex-start',
          },
          describe: '照片支持png、jpg格式',
          props: {
            action: '',
            multiple: false,
            'show-file-list': true,
            limit: 1,
            accept: 'image/png,image/jpg,image/jpeg',
            httpRequest: (param) => {
              vm.uploadFile(param, 'upload_id')
            },
            'on-remove': (file, fileList) => {
              vm.removeFile(file, 'upload_id')
            }
          },
          rule: {
            required: true,
          },
        },
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
      return Object.keys(this.itemInfo).length > 0 ? '修改图片' : '添加图片'
    }
  },
  methods: {
    closeDialog () {
      this.dialogVisible = false
    },
    emitUpdateList (updateData) {
      this.$emit('update-list', updateData)
    },
    uploadFile (params, targetKey) {
      const file = params.file
      const data = new FormData()
      data.append('file', file)
      data.append('type', 'fm_company_pic')
      this.$api.fireManage.uploadFile({ data }).then(res => {
        this.itemData[targetKey].push(res)
        this.$refs.form.clearValidate(targetKey)
        params.onSuccess()
      }).catch(() => {
        params.onError('上传文件失败')
        // throw new Error('上传文件失败')
      })
    },
    removeFile (file, target) {
      const fileIndex = this.itemData[target].findIndex(item => {
        return item.id === file.id
      })
      this.itemData[target].splice(fileIndex, 1)
    },
    submitForm () {
      this.$refs.form.submitForm().then(() => {
        const formData = {
          ...this.itemData,
          upload_id: this.itemData.upload_id[0].id
        }
        const isEdit = Object.keys(this.itemInfo).length !== 0
        if (isEdit) {
          const id = this.itemInfo.id
          const data = {
            ...formData,
            id
          }
          const updateData = {
            ...this.itemInfo,
            ...this.itemData,
            file: this.itemData.upload_id[0]
          }
          this.$api.fireManage.companyImage({ data, method: 'put' })
            .then(() => {
              this.closeDialog()
              this.emitUpdateList(updateData)
            })
        } else {
          const data = {
            ...formData,
          }
          this.$api.fireManage.companyImage({ data, method: 'post' })
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
    itemInfo: {
      immediate: true,
      handler (newVal) {
        if (newVal.file) {
          this.$nextTick().then(() => {
            this.itemData = {
              name: newVal.name
            }
            this.dialogConfig.find(item => item.key === 'upload_id').value.push(newVal.file)
          })
        }
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
    .open-height-form{
      height: 372px;
    }
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
