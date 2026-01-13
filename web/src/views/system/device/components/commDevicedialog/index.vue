<template>
  <div class="dialog-wrapper">
    <sn-dialog
      :visible.sync="dialogVisible"
      :title="dialogHeader"
      width="450px"
      borderType="secondary"
      headerType="secondary"
    >
      <div class="dialog-content-wrapper flex-column flex-center">
        <div class="form-wrapper">
          <el-form ref="form" :model="ruleForm" :rules="rules">
            <el-form-item label="设备名称" prop="name">
              <el-input
                v-model="ruleForm.name"
                placeholder="请输入设备名称"
              ></el-input>
            </el-form-item>
            <el-form-item label="设备类型" prop="type">
              <el-select
                class="item"
                trigger="click"
                placeholder="请选择设备类型"
                v-model="ruleForm.type"
                @change="selectChange"
              >
                <el-option
                  class="el-select_dropdown__item__reset"
                  v-for="item in selectOptions"
                  :key="item.value"
                  :value="item.value"
                  :label="item.label"
                  >{{ item.label }}</el-option
                >
              </el-select>
            </el-form-item>
            <el-form-item label="设备编号">
              <el-input
                v-model="ruleForm.sn"
                placeholder="请输入设备编号"
              ></el-input>
            </el-form-item>
            <el-form-item
              label="采集规则"
              v-show="ruleForm.type == 0"
              :prop="ruleForm.type == 0 ? 'gatherRuleId' : ''"
            >
              <el-select
                class="item"
                trigger="click"
                placeholder="请选择采集规则"
                v-model="ruleForm.gatherRuleId"
              >
                <el-option
                  class="el-select_dropdown__item__reset"
                  v-for="item in selectGatherRuleOptions"
                  :key="item.value"
                  :value="item.value"
                  :label="item.label"
                  >{{ item.label }}</el-option
                >
              </el-select>
            </el-form-item>
          </el-form>
        </div>
        <hr class="halfing-line" />
        <div class="button-wrapper">
          <div class="form-wrapper">
            <el-form>
              <el-form-item>
                <el-button key="取消" type="cancel" @click="closeDialog()"
                  >取消</el-button
                >
                <el-button key="确定" type="submit" @click="submitForm()"
                  >确定</el-button
                >
              </el-form-item>
            </el-form>
          </div>
        </div>
      </div>
    </sn-dialog>
  </div>
</template>

<script>
import aeRules from "@/request/modules/aeRule";
import commDevice from "@/request/modules/commDevice";
export default {
  name: "editPlanDialog",
  props: {
    visible: {
      type: Boolean,
      default() {
        return false;
      },
    },
    productLineId: {
      type: Number,
      default() {
        return 0;
      },
    },
    info: {
      type: Object,
      default() {
        return {};
      },
    },
  },
  data() {
    return {
      infoData: {},
      ruleForm: {
        name: "",
        type: "",
        sn: "",
        gatherRuleId: "",
      },
      selectGatherRuleOptions: [],
      selectOptions: [
        {
          label: "采集网关",
          value: 0,
        },
        {
          label: "声发射",
          value: 1,
        },
      ],
      showRuleSelct: false,
      dialogVisible: false,
      rules: {
        name: [{ required: true, message: "请输入设备名称", trigger: "blur" }],
        type: [{ required: true, message: "请选择活动资源", trigger: "click" }],
        gatherRuleId: [
          { required: true, message: "请选择采集规则", trigger: "click" },
        ],
      },
      dialogHeader: "",
    };
  },
  created() {
    if (Object.keys(this.info).length != 0) {
      aeRules
        .request({
          method: "GET",
        })
        .then((res) => {
          console.log(res.data);
          if (res.data) {
            console.log(res.data.count);
            this.selectGatherRuleOptions = [];
            res.data.forEach((element) => {
              this.selectGatherRuleOptions.push({
                value: element.id,
                label: element.name,
              });
            });
          }
        });
    }
    this.dialogHeader =
      (Object.keys(this.info).length === 0 ? "添加" : "修改") + "传输设备";
  },
  // computed: {
  //   dialogHeader() {
  //     return (
  //       (Object.keys(this.info).length === 0 ? "添加" : "修改") + "传输设备"
  //     );
  //   },
  // },
  methods: {
    selectChange(e) {
      console.log(e);
      if (e === 0) {
        aeRules
          .request({
            method: "GET",
          })
          .then((res) => {
            console.log(res.data);
            if (res.data) {
              console.log(res.data.count);
              this.selectGatherRuleOptions = [];
              res.data.forEach((element) => {
                this.selectGatherRuleOptions.push({
                  value: element.id,
                  label: element.name,
                });
              });
            }
          });
      }
    },
    closeDialog() {
      this.dialogVisible = false;
    },
    emitUpdateList() {
      this.$emit("update-list");
    },
    submitForm() {
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.infoData = {
            name: this.ruleForm.name,
            type: this.ruleForm.type,
            sn: this.ruleForm.sn,
            gatherRuleId: this.ruleForm.gatherRuleId,
          };
          const isEdit = Object.keys(this.info).length !== 0;
          const productLineId = this.productLineId;
          if (isEdit) {
            const id = this.info.id;
            const data = { ...this.infoData, id, productLineId };
            commDevice
              .update({
                method: "POST",
                data,
              })
              .then((res) => {
                this.closeDialog();
                this.emitUpdateList();
              });
          } else {
            const data = { ...this.infoData, productLineId };
            commDevice
              .request({
                method: "POST",
                data,
              })
              .then((res) => {
                this.closeDialog();
                this.emitUpdateList();
              });
          }
        }
      });
    },
  },
  watch: {
    visible: {
      immediate: true, // 解决watch props的问题
      handler(val) {
        this.dialogVisible = val;
      },
    },
    dialogVisible(newVal) {
      this.$emit("update:visible", newVal);
    },
    info: {
      immediate: true,
      handler(newVal) {
        this.infoData = newVal;
        this.ruleForm = newVal;
      },
    },
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
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
    margin: 302px 0 15px;
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
    width: 80px !important;
  }
}
.form-wrapper::v-deep .el-form {
  font-size: 0;
  input::-webkit-input-placeholder {
    color: $color-secondary;
  }
  .el-date-picker {
    .el-picker-panel__footer {
      border-top: 1px solid rgba(101, 226, 245, 1);
      background-color: transparent;
    }
  }

  .el-form-item__label {
    padding: 0;
    font-size: 12px;
    margin-right: 8px;
    text-align: start;
    font-family: PingFangSC-Medium;
    height: 32px;
    line-height: 32px;
    font-weight: 500;
    color: rgba(101, 226, 245, 1);
  }
  .item-title {
    padding: 0;
    font-size: 20px;
    margin-right: 8px;
    text-align: center;
    height: 32px;
    line-height: 32px;
    color: rgba(101, 226, 245, 1);
  }

  .el-form-item {
    display: flex;
    align-items: center;
    .el-form-item__content {
      line-height: 24px;
      > .form-cascader {
        border: 1px solid rgba(101, 226, 245, 0.5) !important;
        background: linear-gradient(
          180deg,
          rgba(8, 40, 79, 0.2) 0%,
          rgba(16, 86, 117, 0.3) 100%
        ) !important;
        .el-scrollbar {
          border-right: 1px solid rgba(101, 226, 245, 0.5) !important;
          .el-cascader-menu__wrap {
            .el-cascader-menu__list {
              li {
                background: rgba(8, 40, 79, 0.2) !important;
                > label {
                  .el-checkbox__input {
                    .el-checkbox__inner {
                      background: linear-gradient(
                        180deg,
                        rgba(8, 40, 79, 0.2) 0%,
                        rgba(16, 86, 117, 0.3) 100%
                      ) !important;
                      border-color: rgba(101, 226, 245, 1) !important;
                    }
                  }
                  .el-checkbox__input.is-checked {
                    .el-checkbox__inner {
                      background: linear-gradient(
                        180deg,
                        rgba(8, 40, 79, 0.2) 0%,
                        rgba(16, 86, 117, 0.3) 100%
                      ) !important;
                      border-color: rgba(101, 226, 245, 1) !important;
                    }
                  }
                  .is-indeterminate {
                    .el-checkbox__inner {
                      background: linear-gradient(
                        180deg,
                        rgba(8, 40, 79, 0.2) 0%,
                        rgba(16, 86, 117, 0.3) 100%
                      ) !important;
                      border-color: rgba(101, 226, 245, 1) !important;
                    }
                  }
                }
                > span {
                  color: rgba(101, 226, 245, 1) !important;
                }
              }
              > .el-cascader-node.in-active-path {
                color: rgba(101, 226, 245, 1) !important;
              }
            }
          }
        }
      }
      .el-radio-group {
        vertical-align: top;
      }
      .el-radio-button {
        padding-right: 16px;
        .el-radio-button__inner {
          background: transparent;
          padding: 0 12px;
          color: rgba(255, 255, 255, 0.5);
          height: 24px;
          line-height: 24px;
          border-radius: 0;
          border-width: 0;
          transition: all 0s;
        }
      }
      .is-active {
        .el-radio-button__inner {
          border-width: 2px;
          border-style: solid;
          border-image-source: url("../../../../../components/sn-form/images/border.png");
          border-image-repeat: round;
          line-height: 20px;
          height: 24px;
          border-image-slice: 2 2 2 2 fill;
          color: rgba(101, 226, 245, 1);
          box-shadow: 0 0 0 0;
        }
      }
      .el-input__inner {
        font-size: 12px;
        font-family: PingFangSC-Medium;
        color: rgba(101, 226, 245, 1);
        width: 200px;
        height: 32px;
        background: linear-gradient(
          180deg,
          rgba(8, 40, 79, 0.2) 0%,
          rgba(16, 86, 117, 0.3) 100%
        );
        line-height: 32px;
        padding-left: 12px;
        border-radius: 0;
        border: 1px solid rgba(101, 226, 245, 0.5);
        &::placeholder {
          opacity: 0.5;
        }
        & + .el-input__prefix {
          display: none;
        }
        & + .el-input__suffix {
          display: none;
        }
      }
      /*  日期选择器*/
      .el-date-editor--date {
        background-image: url("../../../../../components/sn-form/images/icon-date.png");
        background-repeat: no-repeat;
        background-position-y: 8px;
        background-position-x: 172px;
      }
      .el-date-editor.el-input,
      .el-date-editor.el-input__inner {
        width: 200px;
      }
      /*  button*/
      .el-button {
        width: 91px;
        height: 32px;
        font-size: 14px;
        font-weight: 500;
        color: rgba(101, 226, 245, 1);
        padding: 0 0;
        background: rgba(0, 21, 53, 1);
        margin: 0 15px 0 0;
        border-width: 2px;
        border-style: solid;
        border-image-source: url("../../../../../components/sn-form/images/border.png");
        border-image-repeat: round;
        border-image-slice: 2 2 2 2 fill;
        transition: 0s all;
        &:hover {
          background-image: url("../../../../../components/sn-form/images/button-background.png");
          background-size: 100% 100%;
          border: none;
          border-radius: 0;
        }
      }
      .el-dropdown-option {
        @extend .el-input__inner;
        display: inline-flex;
        box-sizing: border-box;
        justify-content: center;
        align-items: center;
      }
      span {
        font-size: 12px;
        font-family: PingFangSC-Medium;
      }
    }
    /* 上传 upload */
    .el-upload-reset {
      .el-upload__tip {
        line-height: 1em;
        margin: 10px 3px;
        font-weight: 500;
        color: $color-secondary;
      }
      .el-upload-list {
        maring-top: 0px;
        .el-upload-list__item {
          .el-upload-list__item-name {
            width: 220px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
          }
          margin: 0;
          border-radius: 0;
          &:hover {
            background: rgba(16, 86, 117, 0.8);
          }
          .el-upload-list__item-name {
            color: $color-secondary;
            &:hover {
              color: $color-secondary;
            }
          }
          /*直接重置所有图标颜色*/
          i {
            color: $color-secondary;
          }
        }
      }
    }
    /* inputNumber   */
    .el-input-number {
      width: 200px;
      span[role="button"] {
        box-sizing: border-box;
        width: 36px;
        height: 36px;
        background-color: transparent;
        border: 0;
        color: transparent;
        &:hover {
          color: transparent;
        }
        &::before {
          content: "";
          display: inline-flex;
          position: absolute;
          top: 0;
          left: 0;
          width: 100%;
          height: 100%;
        }
      }
      .el-input-number__decrease {
        left: 0;
        &::before {
          background-image: url("../../../../../components/sn-form/images/icon-decrease.svg");
        }
      }
      .el-input-number__increase {
        right: 0;
        &::before {
          background-image: url("../../../../../components/sn-form/images/icon-increase.svg");
        }
      }
      .el-input__inner {
        width: 100%;
        padding: 0 36px;
      }
    }
    .suffix {
      position: absolute;
      right: calc(50% - 1.4em - 0.6em);
      top: 0;
      font-size: 12px;
      line-height: 40px;
      color: rgba(101, 226, 245, 1);
    }
    /* select */
    .el-select {
      .el-select__tags {
        .el-tag.el-tag--info {
          color: $color-secondary;
          background-color: transparent;
          /* background-image的拉伸不能总是合适，所以使用slice代替 */
          border-image: url("../../../../../components/sn-form/images/el-tag-background.png");
          border-image-slice: 2 fill;
          border-radius: 0;
          padding: 0 10px;
          .el-select__tags-text {
            margin-right: 24px;
          }
          .el-tag__close {
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
