<template>
  <div class="wrapper flex-center">
    <div class="unit-select-wrapper">
      <sn-form :formList="filterMenu" v-model="params"></sn-form>
    </div>
    <div class="info-wrapper">
      <p class="title">基本信息</p>
      <div class="info-content">
        <div class="info">
          <p class="info-item flex-center-cross" v-for="item of infoList" :key="item.label">
            <label>{{item.label}}:</label>
            <span class="value color-secondary">{{userInfo[item.key]||'暂无'}}</span>
          </p>
        </div>
      </div>
    </div>
    <div class="photo-wrapper">
      <p class="title">企业图片展示</p>
      <div class="content">
          <div class="photo-content" >
              <div class="photo-item" v-for="item of photoList" :key="item.label" :style="{background:`url(${item && item.file&&item.file.url})`}">
                <label>{{item.name}}</label>
                <div class="hover-mask flex-center">
<!--                  <el-image :src="icon.edit" @click="editInfo(item)"></el-image>-->
<!--                  <el-image :src="icon.del" class="delete-icon" @click="deleteInfo(item)"></el-image>-->
                  <el-image :src="icon.zoom" :preview-src-list="[item.file.url]"></el-image>
                </div>
              </div>
          </div>
      </div>
    </div>
    <div class="plan-record-dialog-wrapper">
      <editItem
        :itemInfo="itemInfoActive"
        :visible.sync="editDialogVisible"
        @update-list="updateList"
        v-if="editDialogVisible"
      ></editItem>
    </div>
  </div>
</template>

<script>
import editItem from './components/editItem/index'
import { filterMenu } from './config/config'

const icon = {
  del: require('./images/icon_del.svg'),
  edit: require('./images/icon_edit.svg'),
  zoom: require('./images/icon_zoom.png'),
}
export default {
  components: {
    editItem
  },
  mounted () {
    this.getList()
  },
  data () {
    return {
      filterMenu,
      params: {},
      icon,
      addBtnConfig: [
        {
          type: 'button',
          options: [
            {
              label: '添加企业图片',
              type: 'submit'
            }
          ],
          style: {
            display: 'inline-flex'
          }
        }
      ],
      infoList: [
        {
          label: '公司名称',
          key: 'unitName'
        },
        {
          label: '联系人',
          key: 'managerName'
        },
        {
          label: '联系电话',
          key: 'managerPhone'
        },
        {
          label: '联系邮箱',
          key: 'unitMail'
        }, {
          label: '当前审核组织',
          key: ''
        }, {
          label: '当前抽查机构',
          key: ''
        }, {
          label: '消防主管部门',
          key: ''
        }, {
          label: '具体位置',
          key: 'address'
        },
      ],
      photoList: [],
      itemInfoActive: {},
      editDialogVisible: false,
      imagesPage: {
        page: 1,
        perPage: 10,
        total: 0
      }
    };
  },
  methods: {
    editInfo (info = {}) {
      this.itemInfoActive = info
      this.editDialogVisible = true
    },
    deleteInfo (info) {
      this.$confirm('此操作将永久删除该文件, 是否继续?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        this.$api.fireManage.companyImage({
          method: 'delete',
          data: {
            id: info.id
          }
        }).then(res => {
          const itemIndex = this.photoList.findIndex(item => {
            return item.id === info.id
          })
          this.photoList.splice(itemIndex, 1)
        })
      })
    },
    updateList (data) {
      if (data) {
        const itemIndex = this.photoList.findIndex(item => {
          return item.id === data.id
        })
        this.photoList.splice(itemIndex, 1, data)
      } else {
        this.getList()
      }
    },
    getList () {
      const { page, perPage } = this.imagesPage
      const params = {
        page,
        perPage,
        ...this.params
      }
      this.$api.fireManage.companyImage({ params }).then(res => {
        const list = [...this.photoList, ...res]
        const idList = list.map(item => {
          return item.id
        })
        const singleIdList = [...new Set(idList)]
        this.photoList = singleIdList.map(id => {
          return list.find(item => {
            return item.id === id
          })
        }).filter(item => item)
      })
    }
  },
  computed: {
    userInfo () {
      return this.$store.state.sinuo.user.companyList.find(item => {
        return item.unitID === this.params.unitID
      }) || {
        unitName: '请先选择一个公司'
      }
    }
  },
  watch: {
    params: {
      deep: true,
      handler () {
        this.getList()
      }
    }
  }
}
</script>

<style lang="scss" scoped>

  @import '~@/assets/style/theme/register.scss';
  .wrapper{
    position: relative;
    .unit-select-wrapper{
      position: absolute;
      top: 0;
      right: 30px;
      .add-field-btn::v-deep{
        button{
          width: 144px !important;
        }
      }
    }
    .info-wrapper{
      background: url("./images/info_bg.png");
      background-size: 100% 100%;

      width: 588px;
      height: 860px;
      margin-top: 13px;
      margin-right: 40px;

      padding: 0  0 0 49px;

      font-size: 16px;
      .title{
        margin-top: 15px;
      }
      .info-content{
        .info-item{
          margin: 30px 0;
          &:nth-of-type(1){
            margin-top: 70px;
          }
          label{
            display: inline-flex;
            min-width: 98px;
            font-size: 14px;
          }
        }
      }
    }
    .photo-wrapper{
      background: url("./images/photo_bg.png");
      background-size: 100% 100%;
      width: 1162px;
      height: 860px;

      padding: 0  0 0 49px;
      margin-top: 13px;
      .title{
        margin-top: 15px;
      }
      .photo-content{
        height: 100%;
        overflow-y: auto;
        padding: 30px 0;
        display: flex;
        flex-wrap: wrap;
        .photo-item{
          display: flex;
          width:320px;
          height:200px;
          margin: 30px 15px;
          background-size: cover !important;
          position: relative;
          label{
            align-self: flex-end;
            margin: 0 0 12px 12px;
          }
          .hover-mask{
            display: none;
            position: absolute;
            top: 0;
            left: 0;
          }
          &:hover{
            .hover-mask{
              display: flex;
              height: 100%;
              width: 100%;
              background: rgba(0,0,0,.6);
              .delete-icon{
                margin: 0 15px;
              }
              .zoom-picture-icon{
                /*模拟icon*/
                box-sizing: border-box;
                color: $color-secondary;
                border: 1px solid $color-secondary;
                width: 48px;
                height: 48px;
                border-radius: 50%;
                line-height: 46px;
                text-align-last: center;
                font-size: 26px;
                background: #003051;
                &::before{
                  display: inline-flex;
                }
              }
            }
          }
        }
      }
    }
  }

</style>
