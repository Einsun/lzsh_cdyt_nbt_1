<template>
  <div class="form-wrapper">
    <el-form
      ref="form"
      :model="formData"
      :style="formStyle"
      :rules="formRules"
      :inline="inline"
    >
      <el-form-item
        v-for="formItem of formList"
        :label="formItem.label ? formItem.label + ':' : ''"
        :key="formItem.key"
        :prop="formItem.key"
        :style="[itemStyle, formItem.style || {}]"
        :class="{ 'high-label': ['select', 'input'].includes(formItem.type) }"
        :form-type="formItem.type"
      >
        <template v-if="formItem.type === 'checkbox'">
          <el-checkbox-group v-model="formData[formItem.key]">
            <el-checkbox-button
              v-for="item in formItem.options"
              :label="item.value"
              :key="item.label"
              >{{ item.label }}</el-checkbox-button
            >
          </el-checkbox-group></template
        >
        <template v-if="formItem.type === 'radio'">
          <el-radio-group
            v-model="formData[formItem.key]"
            @change="formItem.onChange"
          >
            <el-radio-button
              v-for="item in formItem.options"
              :label="item.value"
              :key="item.label"
              >{{ item.label }}</el-radio-button
            >
          </el-radio-group>
        </template>
        <template v-if="formItem.type === 'date-picker'">
          <el-date-picker
            v-model="formData[formItem.key]"
            type="date"
            :placeholder="formItem.placeholder || '请选择' + formItem.label"
            format="yyyy 年 MM 月 dd 日"
            value-format="yyyy-MM-dd"
            :clearable="false"
          >
          </el-date-picker>
        </template>
        <template v-if="formItem.type === 'time-picker'">
          <el-time-select
            v-model="formData[formItem.key]"
            :picker-options="{
              start: '00:00',
              step: '00:30',
              end: '23:30',
            }"
            :placeholder="formItem.placeholder || '请选择' + formItem.label"
          >
          </el-time-select>
        </template>
        <template v-if="formItem.type === 'date-time-picker'">
          <el-date-picker
            v-model="formData[formItem.key]"
            type="datetime"
            :picker-options="{
              start: '00:00',
              step: '00:30',
              end: '23:30',
            }"
            :placeholder="formItem.placeholder || '请选择' + formItem.label"
            format="yyyy-MM-dd HH:mm"
            value-format="yyyy-MM-dd HH:mm:00"
            :clearable="false"
          >
          </el-date-picker>
        </template>
        <template v-if="formItem.type === 'datetimerange'">
          <el-date-picker
            v-model="formData[formItem.key]"
            type="datetimerange"
            range-separator="至"
            start-placeholder="开始日期"
            end-placeholder="结束日期"
            format="yyyy-MM-dd HH:mm"
            value-format="yyyy-MM-dd HH:mm:ss"
            :clearable="false"
          ></el-date-picker>
        </template>
        <template v-if="formItem.type === 'input'" class="input-group">
          <el-input
            v-bind="formItem.props"
            v-model="formData[formItem.key]"
            :placeholder="formItem.placeholder || '请输入' + formItem.label"
          ></el-input>
        </template>
        <template v-if="formItem.type === 'password'" class="input-group">
          <el-input
            type="password"
            v-bind="formItem.props"
            v-model="formData[formItem.key]"
            :placeholder="formItem.placeholder || '请输入' + formItem.label"
          ></el-input>
        </template>
        <template v-if="formItem.type === 'button'">
          <el-button
            v-for="item in formItem.options"
            :key="item.label"
            :type="item.type"
            @click="buttonOnClick(item)"
            >{{ item.label }}</el-button
          >
        </template>
        <template v-if="formItem.type === 'select'">
          <el-select
            v-bind="formItem.props || {}"
            class="item"
            trigger="click"
            :placeholder="formItem.placeholder || '请选择' + formItem.label"
            v-model="formData[formItem.key]"
          >
            <el-option
              class="el-select_dropdown__item__reset"
              v-for="item in formItem.options"
              :key="item.value"
              :value="item.value"
              :label="item.label"
              >{{ item.label }}</el-option
            >
          </el-select>
        </template>
        <template v-if="formItem.type === 'cascader'">
          <el-cascader-panel
            class="form-cascader"
            v-model="formData[formItem.key]"
            :props="formItem.props || {}"
            :options="formItem.options"
            @change="getpNode"
          ></el-cascader-panel>
        </template>
        <template v-if="formItem.type === 'upload'">
          <el-upload
            class="el-upload-reset"
            :file-list="formData[formItem.key]"
            :on-exceed="uploadExceedMethod(formItem.props.limit)"
            v-bind="formItem.props"
          >
            <el-button size="small" type="primary">点击上传</el-button>
            <div slot="tip" class="el-upload__tip">
              {{ formItem.describe ? formItem.describe : "" }}
              <a
                v-if="formItem.download"
                class="el-upload__tip"
                :href="formItem.download"
                target="_blank"
                >( 模板下载 )</a
              >
            </div>
          </el-upload>
        </template>
        <template v-if="formItem.type === 'slot'">
          <div
            class="solt-wrapper"
            :name="formItem.name"
            @change.stop="slotValueChange"
          ></div>
        </template>
        <template v-if="formItem.type === 'input-number'">
          <el-input-number
            v-model="formData[formItem.key]"
            v-bind="formItem.props || {}"
            @change="changeInputNumberSuffixOffset($event, formItem.key)"
          >
          </el-input-number>
          <span class="suffix" :ref="formItem.key + '-suffix'">{{
            formItem.suffix || ""
          }}</span>
        </template>
        <template v-if="formItem.type === 'title'">
          <div class="item-title" style="width: 100%">{{ formItem.title }}</div>
        </template>
      </el-form-item>
    </el-form>
  </div>
</template>

<script>
import Vue from "vue";
import _ from "lodash";

export default {
  name: "sn-form",
  model: {
    event: "formChange",
    prop: "formValue",
  },
  computed: {
    slotItem() {
      return this.formList.filter((item) => {
        return item.type === "slot";
      });
    },
    /**
     * return form rules
     * @return {}.
     */
    formRules() {
      let rules = {};
      for (const item of this.formList) {
        if (item.rule) {
          // 对rule进行一段处理
          if (!item.rule.message && !item.rule.validator) {
            item.rule.message = `请${
              item.type.includes("input") ? "输入" : "选择"
            }${item.label}`;
          }
          rules[item.key] = item.rule;
        }
      }
      return rules;
    },
  },
  watch: {
    formValue: {
      immediate: true,
      // 因为绑定的是对象，修改内部值是不会进行触发的，所以所有的formValue的值都是想覆盖formData的值
      handler(newVal) {
        for (let key in newVal) {
          this.formData[key] = newVal[key];
        }
      },
    },
    formData: {
      deep: true,
      handler(newVal) {
        this.$emit("formChange", newVal);
      },
    },
    slotItem: {
      immediate: true,
      handler(newVal) {
        // 将js中的组件实例挂载到template中
        // 需要nextTick因为dom还未渲染
        this.$nextTick().then(() => {
          for (const item of newVal) {
            const MyComponent = Vue.extend(item.component);
            new MyComponent().$mount(`div[name=${item.name}]`);
          }
        });
      },
    },
    /**
     * 使formData 跟随formList变化
     **/
    formList: {
      immediate: true,
      handler(newVal) {
        this.formData = (() => {
          const obj = {};
          // 用于判断upload中添加的文件是否为允许文件
          const isNotAllowFileType = (file, accept) => {
            const allowList = accept.split(",");
            const fileType = file.name.split(".").splice(-1)[0];
            let result = allowList.some((item) => {
              return item.includes(fileType);
            });
            return result;
          };
          for (let item of this.formList) {
            if (item.key) {
              // 需要保留已有值，已有值有可能为false、0 这些被类型转换的值，所以在此仅判断空字符串
              obj[item.key] =
                this.formData[item.key] !== ""
                  ? this.formData[item.key]
                  : item.value;
            }
            switch (item.type) {
              case "upload": {
                item.props["before-upload"] = (file) => {
                  if (!file) {
                    return;
                  }
                  let result = true;
                  const isAllowFile = isNotAllowFileType(
                    file,
                    item.props.accept
                  );
                  if (isAllowFile) {
                    const oldMethod = item.props["before-upload"];
                    if (oldMethod) {
                      result = oldMethod();
                    }
                  } else {
                    // const index = fileList.find(item => {
                    //   return item === file
                    // })
                    // fileList.splice(index, 1)
                    this.$message.warning(
                      "您选择的文件不符合上传类型，请重新选择！"
                    );
                    result = isAllowFile;
                  }
                  return result;
                };
                break;
              }
              case "input": {
                const oldRules = item.rule || {};
                item.rule = {
                  validator(rule, value, callback) {
                    if (oldRules.required) {
                      if (
                        value !== undefined &&
                        value !== null &&
                        value.toString().replace(/\s+/g, "")
                      ) {
                        callback();
                      } else {
                        callback(
                          new Error(
                            `请${
                              item.type.includes("input") ? "输入" : "选择"
                            }${item.label}`
                          )
                        );
                      }
                    } else {
                      callback();
                    }
                  },
                  ...oldRules,
                };
                break;
              }
              default:
                break;
            }
          }
          return obj;
        })();
      },
    },
  },
  data() {
    return {
      formData: (() => {
        const obj = {};
        for (let item of this.formList) {
          if (item.key) {
            obj[item.key] = item.value;
          }
        }
        return obj;
      })(),
      choiceP: [],
      checkedRow: [],
    };
  },
  methods: {
    getpNode(node) {
      // console.log('****** getpNode ******', node)
      let arr = [];
      _.forEach(this.formData["permission"], (val) => {
        arr.push.apply(arr, val);
      });
      this.choiceP = _.uniq(arr);
      // console.log('****** getpNode 11 ******', this.choiceP)
    },
    slotValueChange(key, value) {
      this.$set(this.formData, key, value);
    },
    buttonOnClick(item) {
      if (item.type === "submit") {
        this.submitForm();
      } else if (item.type === "reset") {
        this.resetForm();
      } else {
        this.$emit(`${item.type}-btn-click`);
      }
    },
    clearValidate(params) {
      this.$refs["form"].clearValidate(params);
    },
    /**
     * @param formName
     * @return Promise
     */
    submitForm() {
      return new Promise((resolve, reject) => {
        this.$refs["form"].validate((valid) => {
          if (valid) {
            this.$emit("submitForm", this.formData);
            resolve(this.formData);
            // this.page.p = 1
            // this.getAlarmList()
          } else {
            reject(new Error("校验未通过"));
            return false;
          }
        });
      });
    },
    resetForm() {
      this.formData = (() => {
        const obj = {};
        for (let item of this.formList) {
          if (item.key) {
            obj[item.key] = item.value;
          }
        }
        return obj;
      })();
      this.$refs["form"].resetFields();
      this.$emit("submitForm", this.formData);
    },
    // 用于改变inputNumber的内容展示位置，通过refs获取vue组件后获取dom直接修改dom对应样式
    // todo change事件并不能输入即更新，所以最好直接使用input事件来监听
    changeInputNumberSuffixOffset(newVal, key) {
      const suffixNode = this.$refs[key + "-suffix"][0];
      if (newVal > 9) {
        const suffixFontSize = window.getComputedStyle(suffixNode).fontSize;

        const virtualNode = document.createElement("span");
        suffixNode.appendChild(virtualNode);
        virtualNode.innerText = String(newVal);
        // display:none 会导致offsetWidth获取不到宽度
        virtualNode.style = `font-size: ${suffixFontSize}; color: transparent;`;

        const numberWidth = virtualNode.offsetWidth;
        suffixNode.removeChild(virtualNode);
        const halfNumberWidth = parseInt(numberWidth) / 2 + "px";
        suffixNode.style = `margin-right: -${halfNumberWidth}`;
      } else {
        suffixNode.style = `margin-right: 0px}`;
      }
    },
    uploadExceedMethod() {
      return (fileList) => {
        const max = fileList.length;
        this.$message(`最多只允许上传${max}个文件，请移除文件后重试`);
      };
    },
  },
  props: {
    formList: Array,
    formValue: {
      type: Object,
      default() {
        return {};
      },
    },
    itemStyle: {
      type: Object,
      default() {
        return {
          "margin-bottom": "14px",
          "margin-right": "20px",
        };
      },
    },
    formStyle: {
      type: Object,
      default() {
        return {
          "margin-bottom": "6px",
          "margin-top": "20px",
        };
      },
    },
    inline: {
      type: Boolean,
      default: false,
    },
  },
};
</script>

<style lang="scss" scoped>
@import "~@/assets/style/theme/register.scss";
.form-wrapper::v-deep .el-form {
  font-size: 0;
  input::-webkit-input-placeholder {
    color: $color-secondary;
  }
  .el-date-picker  {
    .el-picker-panel__footer {
      border-top: 1px solid rgba(101, 226, 245, 1);
      background-color: transparent;
    }
  }
  .el-date-range-picker  {
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
          border-image-source: url("images/border.png");
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
        background-image: url("images/icon-date.png");
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
        border-image-source: url("images/border.png");
        border-image-repeat: round;
        border-image-slice: 2 2 2 2 fill;
        transition: 0s all;
        &:hover {
          background-image: url("images/button-background.png");
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
          background-image: url("images/icon-decrease.svg");
        }
      }
      .el-input-number__increase {
        right: 0;
        &::before {
          background-image: url("images/icon-increase.svg");
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
          border-image: url("./images/el-tag-background.png");
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
