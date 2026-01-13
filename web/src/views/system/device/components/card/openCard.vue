<template>
  <div class="openCard">
    <div class="header flex-center-cross" v-if="showHeader">
      <div class="icon-wrapper">
        <sn-icon name="title-icon" :size="16" path="homepage/card" :rotate="true"></sn-icon>
      </div>
      <div class="title">
        {{title}}
        <slot name="title"></slot>
      </div>
    </div>
    <div class="content-wrapper">
      <slot></slot>
    </div>
  </div>
</template>

<script>
export default {
  name: 'index',
  props: {
    showHeader: {
      type: Boolean,
      default () {
        return true
      }
    },
    title: String,
    decorationNumber: {
      type: Number,
      default () {
        return 20
      }
    }
  }
}
</script>

<style lang="scss" scoped>
.openCard{
  width: 100%;
  height: 100%;
  // background:linear-gradient(180deg,rgba(15,29,55,0.5) 50%,rgba(23,74,127,1) 100%);
  // box-shadow:0px 1px 10px 0px rgba(50,123,225,0.5);
  border-radius:4px;
  // border:1px solid rgba(50,123,225,1);
  padding: 15px 14px;
  .header{
    background:linear-gradient(270deg,rgba(14,29,52,0) 0%,rgba(16,59,101,0.5) 100%);
    height: 35px;
    font-size:14px;
    font-family:PingFangSC-Medium,PingFangSC;
    font-weight:500;
    color:rgba(46,192,243,0.5);
    .icon-wrapper{
      padding-left: 10px;
    }
  }
  .content-wrapper{
    height: calc(100% - 5px);
    overflow-y: scroll;
    -ms-overflow-style: none;
    &::-webkit-scrollbar {
      width: 0 !important
    }
  }
}
</style>
