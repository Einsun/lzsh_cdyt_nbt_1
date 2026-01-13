<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="450px" borderType="secondary" headerType="secondary">
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
import productLine from "@/request/modules/productLine";
export default {
  name: 'editPlanDialog',
  props: {
    visible: {
      type: Boolean,
      default () {
        return false
      }
    },
    info: {
      type: Object,
      default () {
        return {};
      }
    }
  },
  data () {
    const vm = this
    return {
      infoData: {},
      fileImage: null,
      dialogVisible: false,
      planConfig: [
        {
          type: 'input',
          value: '',
          label: '产线名称',
          key: 'name',
          rule: {
            required: true,
          },
        },
        {
          type: 'input',
          label: '检测等级',
          key: 'level',
        },
        {
          type: 'input',
          label: '产线位置',
          key: 'location',
          rule: {
            required: true,
          },
        },
        {
          type: 'input',
          label: '设备寿命',
          key: 'life',
        },
        {
          type: 'date-time-picker',
          label: '安装时间',
          key: 'installTime',
          value: '',
          rule: {
            required: true,
          }
        },
        {
          type: 'upload',
          label: '上传照片',
          key: 'File',
          value: '',
          describe: '仅支持图片',
          props: {
            action: '',
            multiple: false,
            'show-file-list': true,
            limit: 1,
            accept: 'image/png,image/jpg,image/jpeg',
            'on-change': (file, fileList) => {
              console.log(file)
              this.fileImage = file.raw;
            }
          },
          rule: {
          }
        },
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
      return (Object.keys(this.info).length === 0 ? '添加' : '修改') + '产线'
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
      console.log(111);
      this.$refs.form.submitForm().then(() => {
        const isEdit = Object.keys(this.info).length !== 0
        const File = this.fileImage;
        if (isEdit) {
          const id = this.info.id
          const data = { ...this.infoData, id, File, Image }
      console.log(222,data);
          productLine.updateLine({
            method: 'POST',
            data
          }).then(res => {
            this.closeDialog()
            this.emitUpdateList()
          })
        } else {
          const data = { ...this.infoData,File }
          productLine.line({
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
        this.infoData = newVal
      }
    }
  }
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
  .el-input__inner,.el-input-number{
    height: 36px !important;
    width: 300px !important;
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
    width: 80px !important;
  }
}
</style>
