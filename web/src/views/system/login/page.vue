<template>
  <div class="page-login">
    <!-- 表单 -->
    <div class="title-wrapper">
      <!-- <p class="title title-primary">智慧消防应急管理系统-管理版</p> -->
    </div>
    <div class="page-login-form">
      <p class="login-title">用户登录</p>
      <el-form
        ref="loginForm"
        label-position="top"
        :rules="rules"
        :model="formLogin"
        size="default"
      >
        <el-form-item prop="username">
          <el-input
            type="text"
            v-model="formLogin.username"
            placeholder="用户名"
            @keypress.enter.native="submit"
          >
            <span
              slot="prepend"
              class="icon icon-user"
            ></span>
          </el-input>
        </el-form-item>
        <el-form-item prop="password">
          <el-input
            type="password"
            v-model="formLogin.password"
            placeholder="密码"
            @keypress.enter.native="submit"
          >
            <span
              slot="prepend"
              class="icon icon-password"
            ></span>
          </el-input>
        </el-form-item>
        <el-form-item class="el-form-item-rememberPassword">
          <el-checkbox
            @change="changeRememberPassword"
            :value="formLogin.remember_token===1"
          >记住密码</el-checkbox>
        </el-form-item>
        <el-button
          size="default"
          @click="submit"
          type="primary"
          class="button-login"
          :loading="loading"
        >登录</el-button>
      </el-form>
    </div>
    <div class="page-login-content-footer">
      <!-- <p class="page-login--content-footer-copyright">
             Copyright © 2019 ssgk.net 盛世高科
          </p> -->
    </div>
    <el-dialog
      title="快速选择用户"
      :visible.sync="dialogVisible"
      width="400px"
    >
      <el-row
        :gutter="10"
        style="margin: -20px 0px -10px 0px;"
      >
        <el-col
          v-for="(user, index) in users"
          :key="index"
          :span="8"
        >
          <div
            class="page-login--quick-user"
            @click="handleUserBtnClick(user)"
          >
            <d2-icon name="user-circle-o" />
            <span>{{user.name}}</span>
          </div>
        </el-col>
      </el-row>
    </el-dialog>
  </div>
</template>

<script>
import dayjs from 'dayjs'
import { mapActions } from 'vuex'
import util from '@/libs/util.js'

export default {
  data() {
    return {
      timeInterval: null,
      time: dayjs().format('HH:mm:ss'),
      // 快速选择用户
      dialogVisible: false,
      loading: false,
      users: [
        {
          name: '管理员',
          username: 'admin',
          password: 'admin'
        },
        {
          name: '编辑',
          username: 'editor',
          password: 'editor'
        },
        {
          name: '用户1',
          username: 'user1',
          password: 'user1'
        }
      ],
      // 表单
      formLogin: {
        username: '',
        password: '',
        remember_token: 0,
        code: 'v9am'
      },
      // 校验
      rules: {
        username: [
          { required: true, message: '请输入用户名', trigger: 'blur' }
        ],
        password: [
          { required: true, message: '请输入密码', trigger: 'blur' }
        ],
        code: [
          { required: true, message: '请输入验证码', trigger: 'blur' }
        ]
      },
    }
  },
  mounted() {
    this.clearUserInfo()
  },
  activated() {
    this.clearUserInfo()
  },
  beforeDestroy() {
    clearInterval(this.timeInterval)
  },
  methods: {
    clearUserInfo() {
      localStorage.clear()
      util.cookies.remove('uuid')
      util.cookies.remove('token')
    },
    ...mapActions('d2admin/account', [
      'login'
    ]),
    /**
       * @description 接收选择一个用户快速登录的事件
       * @param {Object} user 用户信息
       */
    handleUserBtnClick(user) {
      this.formLogin.username = user.username
      this.formLogin.password = user.password
      this.submit()
    },
    // 修改记住密码
    changeRememberPassword(val) {
      if (val) {
        this.formLogin.remember_token = 1
      } else {
        this.formLogin.remember_token = 0
      }
    },
    /**
       * @description 提交表单
       */
    // 提交登录信息
    submit() {
      // this.$router.replace(this.$route.query.redirect || '/')
      this.$refs.loginForm.validate((valid) => {
        if (valid) {
          // 登录
          // 注意 这里的演示没有传验证码
          // 具体需要传递的数据请自行修改代码
          this.loading = true
          const shutLoading = () => {
            this.loading = false
          }
          this.login({
            username: this.formLogin.username,
            password: this.formLogin.password
          })
            .then(() => {
              // 重定向对象不存在则返回顶层路径
              this.$router.replace(this.$route.query.redirect || '/', shutLoading, shutLoading)
              this.$store.dispatch('sinuo/user/getCompanyList')
            }).catch(() => {
              shutLoading()
            })
        } else {
          // 登录表单校验失败
          this.$message.error('请输入密码或用户名')
        }
      })
    },
  }
}
</script>

<style lang="scss" scoped>
.page-login {
  background-image: url("./image/login_backgroung.jpg");
  background-size: cover;
  width: 100%;
  height: 100%;
  padding-top: calc(280vh / 1080 * 100);
  box-sizing: border-box;
  .title-wrapper {
    .title {
      text-align: center;
      margin: 0 auto;
    }
    .title-primary {
      margin-bottom: 16px;
      width: 15em;
      line-height: 40px;
      height: 40px;
      font-size: 40px;
      font-family: FZZZHONGJW--GB1-0;
      font-weight: normal;
      color: rgba(101, 226, 245, 1);
      line-height: 40px;
      background: linear-gradient(
        270deg,
        rgba(0, 120, 215, 1) 0%,
        rgba(58, 171, 212, 1) 100%
      );
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
    .title-secondary {
      width: 288px;
      text-align-last: justify;
      line-height: 16px;
      height: 16px;
      font-size: 16px;
      font-family: FZZZHONGJW--GB1-0;
      font-weight: normal;
      color: rgba(101, 226, 245, 1);
      line-height: 16px;
      background: linear-gradient(
        270deg,
        rgba(0, 120, 215, 1) 0%,
        rgba(58, 171, 212, 1) 100%
      );
      -webkit-background-clip: text;
      -webkit-text-fill-color: transparent;
    }
  }
  .page-login-form {
    margin: 0 auto;
    margin-top: 30px;
    width: 540px;
    height: 374px;
    background-image: url("./image/login_form_background.png");
    background-repeat: no-repeat;
    background-size: contain;
    padding-top: 38px;
    .login-title {
      text-align-last: justify;
      width: 128px;
      margin: 0 auto 32px;
      font-size: 22px;
      font-family: PingFangSC-Regular;
      font-weight: 400;
      color: rgba(101, 226, 245, 1);
      line-height: 22px;
    }
    &::v-deep .el-form {
      .el-form-item {
        width: 440px;
        margin: 0 auto 24px;
        .el-input {
          width: 440px;
          height: 46px;
          background: rgba(5, 101, 159, 0.5);
          border: 2px solid rgba(5, 101, 159, 1);
          .el-input-group__prepend {
            border: none;
            background: transparent !important;
            width: 57px;
            box-sizing: border-box;
          }
          .icon {
            width: 18px;
            height: 18px;
            display: inline-block;
            background-size: contain;
            background-position: center center;
            background-repeat: no-repeat;
          }
          .icon-user {
            background-image: url("./image/icon_user.svg");
          }
          .icon-password {
            background-image: url("./image/icon_password.svg");
          }
          input,
          input:-webkit-autofill {
            padding: 0 !important;
            font-size: 16px !important;
            height: 100% !important;
            background: transparent !important;
            color: rgba(238, 238, 238, 1) !important;
            line-height: 100% !important;
            border: none !important;
            box-shadow: #1cae77 inset;
          }
          input:-webkit-autofill {
            -webkit-animation: autofill-fix 1s infinite;
            -webkit-text-fill-color: rgba(238, 238, 238, 1);
          }

          @-webkit-keyframes autofill-fix {
            from {
              background-color: transparent;
            }
            to {
              background-color: transparent;
            }
          }
        }
      }
      .el-form-item-rememberPassword {
        height: 16px;
        .el-form-item__content {
          line-height: 16px;
          .el-checkbox__inner {
            width: 18px;
            height: 18px;
            background: rgba(5, 101, 159, 0.5);
            border: 2px solid rgba(5, 101, 159, 1);
            &::after {
              /*border:2px solid #ddd;*/
              border-width: 2px;
            }
          }
          .el-checkbox__label {
            padding-left: 12px;
            width: 80px;
            font-size: 16px;
            font-weight: 400;
            line-height: 18px;
            color: rgba(238, 238, 238, 1);
            text-align-last: justify;
          }
        }
      }
    }
    .button-login {
      margin: 0 auto;
      width: 440px;
      height: 48px;
      background: rgba(58, 171, 212, 1);
      display: block;
    }
  }
  .page-login-content-footer {
    width: 100%;
    position: fixed;
    bottom: 40px;
    text-align: center;
    p {
      font-size: 12px;
      font-weight: 500;
      color: rgba(101, 226, 245, 1);
      line-height: 12px;
    }
  }
}
</style>
