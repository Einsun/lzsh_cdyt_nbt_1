<template>
  <div class="dialog-wrapper">
    <sn-dialog :visible.sync="dialogVisible" :title="dialogHeader" width="450px" borderType="secondary" headerType="secondary">
      <div class="dialog-content-wrapper flex-column flex-center">
        <el-upload
            class="upload-demo"
            ref="upload"
            action="http://192.168.1.108:5000/api/ProductLine"
            :headers="headers"

            :file-list="fileList"
            :limit = 1
            accept = 'image/jpg,image/jpeg,image/png'
            :auto-upload="false">
          <el-button slot="trigger" size="small" type="primary">选取文件</el-button>
          <el-button style="margin-left: 10px;" size="small" type="success" @click="submitUpload">上传图片</el-button>
          <div slot="tip" class="el-upload__tip">只能上传jpg/png文件，且不超过500kb</div>
        </el-upload>
        <hr class="halfing-line"/>
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
import axios from 'axios'
import util from "@/libs/util";
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
      headers: {
        'Content-Type': 'multipart/form-data; boundary=${data._boundary}',
        'Authorization': 'Bearer ' + util.cookies.get('token')
      },
      imageFile: null,
      infoData: {},
      fileList: [],
      fileImage: null,
      dialogVisible: false,
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
    async submitUpload () {
      this.$refs.upload.submit();
      // console.log(this.infoData['name'])
      // console.log(this.infoData['name'])
      // const formData = new FormData();
      // formData.append('file', this.imageFile);
      // // formData.append('', this.infoData.)
      //
      // const response = await axios.post('你的API端点', formData, {
      //   headers: {
      //     'Content-Type': 'multipart/form-data', // 重要：此标头告诉服务器这是一个多部分表单数据请求
      //   },
      // });
    },
    beforeUpload (file) {
      // 获取选取的文件信息
      console.log('选取的文件信息：', file);
      this.imageFile = file;
      // 自定义上传前的逻辑，例如限制文件类型和大小
      const isJPG = file.type === 'image/jpeg' || file.type === 'image/png';
      if (!isJPG) {
        this.$message.error('只能上传jpg/png文件！');
        return false;
      }
      const isLt500KB = file.size / 1024 < 500;
      if (!isLt500KB) {
        this.$message.error('文件大小不能超过500KB！');
        return false;
      }
      return true; // 返回 true 允许上传
    },
    submitForm () {
      const isEdit = Object.keys(this.info).length !== 0
      if (isEdit) {
        const id = this.info.id
        const data = { ...this.infoData, id }
        productLine.updateLine({
          method: 'POST',
          data
        }).then(res => {
          this.closeDialog()
          this.emitUpdateList()
        })
      } else {
        const data = { ...this.infoData }
        productLine.line({
          method: 'POST',
          data
        }).then(res => {
          this.closeDialog()
          this.emitUpdateList()
        })
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
